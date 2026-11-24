using System.Diagnostics;
using System.Text.RegularExpressions;

// 1. Укажите здесь пути к вашим основным .sln файлам по умолчанию
var defaultSlns = new[]
{
    @"D:\Projects\OrdersService\Orders.sln",
    @"D:\Projects\BillingService\Billing.sln",
    @"D:\Projects\SharedContracts\Contracts.sln"
};

// Можно переопределить через аргумент --sln "путь1,путь2"
var slnArg = GetArg(args, "--sln");
var slnPaths = !string.IsNullOrEmpty(slnArg)
    ? slnArg.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    : defaultSlns;

var projectFilter = GetArg(args, "--project") ?? ".*";
var nugetFilter = GetArg(args, "--nuget") ?? "Kafka|MediatR|EntityFramework|FluentValidation";

var validSlns = slnPaths.Select(Path.GetFullPath).Where(File.Exists).ToList();
if (validSlns.Count == 0)
{
    Console.Error.WriteLine("Ошибка: ни один из указанных .sln файлов не найден! Проверьте пути в defaultSlns или аргументе --sln.");
    return;
}

// 2. Вычисляем общую родительскую директорию на диске для всех переданных .sln
var commonRoot = GetCommonRoot(validSlns);
Console.WriteLine($"Общая рабочая директория: {commonRoot}");

var allPackages = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
var foldersToPack = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
var nugetRegex = new Regex(nugetFilter, RegexOptions.IgnoreCase);
var projRegex = new Regex(projectFilter, RegexOptions.IgnoreCase);

foreach (var slnPath in validSlns)
{
    Console.WriteLine($"Обработка {Path.GetFileName(slnPath)}...");

    // Собираем версии NuGet-пакетов
    var packageOutput = RunCommand("dotnet", $"list \"{slnPath}\" package --include-transitive");
    foreach (var line in packageOutput.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
    {
        if (nugetRegex.IsMatch(line)) allPackages.Add(line);
    }

    // Собираем проекты (.csproj), исключая тесты
    var slnListOutput = RunCommand("dotnet", $"sln \"{slnPath}\" list");
    var slnDir = Path.GetDirectoryName(slnPath)!;

    var projects = slnListOutput
        .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Where(line => line.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
        .Where(line => !line.Contains("Tests", StringComparison.OrdinalIgnoreCase))
        .Where(line => projRegex.IsMatch(line));

    foreach (var relPath in projects)
    {
        var fullProjPath = Path.GetFullPath(Path.Combine(slnDir, relPath));
        if (File.Exists(fullProjPath))
        {
            var projDir = Path.GetDirectoryName(fullProjPath)!;
            var relDir = Path.GetRelativePath(commonRoot, projDir).Replace('\\', '/');
            foldersToPack.Add($"{relDir}/**/*.cs");
        }
    }
}

if (foldersToPack.Count == 0)
{
    Console.WriteLine($"Ни один проект не подошёл под фильтр '{projectFilter}'");
    return;
}

// Временные файлы создаются в системной папке Temp и удаляются после работы
var tempId = Guid.NewGuid().ToString("N");
var rulesFile = Path.Combine(Path.GetTempPath(), $"ai-rules-{tempId}.md");
var tempOutput = Path.Combine(Path.GetTempPath(), $"repomix-{tempId}.xml");

var rulesText = $"""
    ### Project Stack & Rules (.NET 10)
    - Use C# 14 (Primary Constructors, file-scoped namespaces, records for DTOs).
    - Output ONLY the modified methods or classes; do not rewrite entire files unless necessary.
    - Do not hallucinate non-existent methods on the provided interfaces.
    - Strictly rely on these installed NuGet package versions:
    {(allPackages.Count > 0 ? string.Join(Environment.NewLine, allPackages.Order()) : "No specific packages matched.")}
    """;

await File.WriteAllTextAsync(rulesFile, rulesText);

Console.WriteLine($"Итого уникальных проектов для упаковки: {foldersToPack.Count}");
var includePattern = string.Join(",", foldersToPack);
var ignorePattern = "**/Migrations/**,**/bin/**,**/obj/**,**/*.g.cs,**/*.Designer.cs";

// 3. Запуск глобально установленного repomix из общей родительской директории
var repomixArgs = $"-o \"{tempOutput}\" --compress --remove-comments --remove-empty-lines --no-directory-structure --no-file-summary --include \"{includePattern}\" --ignore \"{ignorePattern}\" --instruction-file-path \"{rulesFile}\" --copy";

Console.WriteLine("Запуск Repomix...");
try
{
    RunCommand("repomix", repomixArgs, workingDir: commonRoot, forwardOutput: true);
}
finally
{
    if (File.Exists(rulesFile)) File.Delete(rulesFile);
    if (File.Exists(tempOutput)) File.Delete(tempOutput);
}

// --- Вспомогательные методы ---

static string? GetArg(string[] args, string name)
{
    var idx = Array.IndexOf(args, name);
    return idx >= 0 && idx + 1 < args.Length ? args[idx + 1] : null;
}

static string GetCommonRoot(List<string> paths)
{
    var splitPaths = paths
        .Select(p => Path.GetDirectoryName(p)!.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))
        .ToList();

    var minLen = splitPaths.Min(p => p.Length);
    var commonParts = new List<string>();

    for (int i = 0; i < minLen; i++)
    {
        var part = splitPaths[0][i]; // Берём i-й сегмент из первого пути
        if (splitPaths.All(p => p[i].Equals(part, StringComparison.OrdinalIgnoreCase)))
            commonParts.Add(part);
        else
            break;
    }

    var root = string.Join(Path.DirectorySeparatorChar, commonParts);
    return root.EndsWith(':') ? root + Path.DirectorySeparatorChar : root;
}

static string RunCommand(string fileName, string arguments, string? workingDir = null, bool forwardOutput = false)
{
    var isWindows = OperatingSystem.IsWindows();
    var useCmd = isWindows && fileName != "dotnet";

    var psi = new ProcessStartInfo
    {
        FileName = useCmd ? "cmd.exe" : fileName,
        Arguments = useCmd ? $"/c {fileName} {arguments}" : arguments,
        WorkingDirectory = workingDir ?? Directory.GetCurrentDirectory(),
        RedirectStandardOutput = !forwardOutput,
        RedirectStandardError = !forwardOutput,
        UseShellExecute = false,
        CreateNoWindow = !forwardOutput
    };

    using var process = Process.Start(psi)!;
    var output = forwardOutput ? string.Empty : process.StandardOutput.ReadToEnd();
    process.WaitForExit();
    return output;
}
