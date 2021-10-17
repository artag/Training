# Lesson 26. Working with nuget packages

## Using NuGet with F#

В проектах F# работа с NuGet такая же как и в проектах C#. Пример:

```fsharp
module Library1
open Newtonsoft.Json

// Defining an F# record
type Person = { Name : string; Age : int }

let getPerson() =
    // Sample JSON text that matches your record structure
    let text = """{ "Name" : "Sam", "Age" : 18 }"""
    // Using Sample JSON text that matches your record structure Newtonsoft.Json
    // to deserialize the object
    let person = JsonConvert.DeserializeObject<Person>(text)
    printfn "Name is %s with age %d." person.Name person.Age
    person
```

* Usage triple-quoted strings (`"""`) allow you to use single quotes within the string.
* Newtonsoft.Json works out of the box with F# record types. It'll automatically
map JSON fields to F# record fields, as with C# class properties.

### Working with NuGet with F# scripts

1. Add the NuGet package to the project.

2. Create or open script file *.fsx.

3. In Solution Explorer get properties of the NuGet DLL, copy the entire path into the clipboard.
`#r` directive using to reference NuGet package.

4. Code in script:

```fsharp
#r @"<path to Humanizer.dll>"   // Referencing an assembly by using #r
open Humanizer
"ScriptsAreAGreatWayToExplorePackages".Humanize()
```

### Loading source files in scripts

```fsharp
// Referencing the Newtonsoft.Json assembly
#r @"<path to Newtonsoft.Json.dll>"
// Loading the Sample.fs source file into the REPL
#load "Library1.fs"
// Executing code from the Sample module
Library1.getPerson()
```

### Loading a source file into a script with a NuGet dependency

```fsharp
// Add the “..\packages\” folder to the search list by using a relative path.
#I @"..\packages\"
// Simplified NuGet package reference
#r @"Humanizer.Core.2.1.0\lib\netstandard1.0\Humanizer.dll"
#r @"Newtonsoft.Json.9.0.1\lib\net45\Newtonsoft.Json.dll"
```

### NuGet and project references

Для более новой версии F# для загрузки NuGet пакетов в скрипты можно использовать команду
`#nuget`.

```fsharp
// If a version is not specified, the highest available non-preview package is taken.
#r "nuget: Newtonsoft.Json"
open Newtonsoft.Json

let data = {| Name = "Don Syme"; Occupation = "F# Creator" |}
JsonConvert.SerializeObject(data)
```

```fsharp
// To reference a specific version, introduce the version via a comma.
#r "nuget: DiffSharp-lite, 1.0.0-preview-328097867"
open DiffSharp
// ...
```

### Specifying a package source

This will tell the resolution engine under the covers to also take into account the remote
and/or local sources added to a script.

```fsharp
#i "nuget: https://my-remote-package-source/index.json"
#i """nuget: C:\path\to\my\local\source"""
```

## Paket

*Paket* - open source, flexible, and powerful dependency management client for .NET.
It's backward-compatible with the NuGet service.

Paket is fully compatible with C#, VB .NET, and F# projects and solutions.

### Issues with the NuGet

* Invalid references across projects (можно добавить в два разных проекта NuGet пакет разных
версий).

* Updates project file on upgrade.

* Hard to reference from scripts (обновление пакета приведет к неработоспособности скрипта).

* Difficulty managing (on large solutions or multiple solution-sharing projects).

### Benefits of Paket

* *Dependency resolver* - Paket understands your dependencies across all projects in
your solution (or repository), and will keep all your dependencies stable across
all projects.

* *Easy to reason about* - Paket управляет дочерними зависимостями.

* *Source code dependencies* - You can have a dependency on, for example, a specific
commit of a GitHub file.

* *Fast*, *Lightweight* - Paket is a command-line-first tool, the configuration files are plain
text.

### Get Started with Paket

*From https://fsprojects.github.io/Paket/get-started.html*

1. Install .NET Core 3.0 or higher

2. Install and restore Paket as a local tool in the root of your codebase:

```text
dotnet new tool-manifest
dotnet tool install paket
dotnet tool restore
```

This will create a `.config/dotnet-tools.json` file in the root of your codebase.
It must be **checked into source control**.

3. Initialize Paket by creating a dependencies file.

```text
dotnet paket init
```

If you have a build.sh/build.cmd build script, also make sure you add the last two commands
before you execute your build:

```text
dotnet tool restore
dotnet paket restore
# Your call to build comes after the restore calls, possibly with FAKE: https://fake.build/
```

This will ensure Paket works in any .NET Core build environment.

4. Make sure to add the following entries to your `.gitignore`:

```text
#Paket dependency manager
.paket/
paket-files/
```

### Paket core concepts

*From https://fsprojects.github.io/Paket/learn-how-to-use-paket.html*

Paket manages your dependencies with three core file types:

* `paket.dependencies` - specify your dependencies and their versions for your entire codebase.

* `paket.references` - a file that specifies a subset of your dependencies for every project
in a solution.
  
* `paket.lock` - a lock file that Paket generates when it runs.
When you check it into source control, you get reproducible builds.

You edit the `paket.dependencies` and `paket.references` files by hand as needed.
When you run a paket command, it will generate the `paket.lock` file.

All three file types **must be committed to source control**.

### Important paket commands

The most frequently used Paket commands are:

`paket install` - Run this after adding or removing packages from the `paket.dependencies`
file. It will update any affected parts of the lock file that were affected by the changes
in the `paket.dependencies` file, and then refresh all projects in your codebase that
specify paket dependencies to import references.

`paket update` - Run this to update your codebase to the latest versions of all dependent
packages. It will update the `paket.lock` file to reference the most recent versions
permitted by the restrictions in `paket.dependencies`, then apply these changes to all
projects in your codebase.

`paket restore` - Run this after cloning the repository or switching branches.
It will take the current `paket.lock` file and update all projects in your codebase so that
they are referencing the correct versions of NuGet packages.
It should be called by your build script in your codebase, so you should not need to run it
manually.

### Walkthrough

1. Create a `paket.dependencies` file in your solution's root
(you can also create it by hand):

```text
dotnet paket init
```

2. Installing dependencies. For every project in your codebase,
create a `paket.references` file that specifies the dependencies
you want to pull in for that project.

Once you have a `paket.references` file alongside every project in your codebase,
install all dependencies with this command:

```text
dotnet paket install
```

This will automatically generate the `paket.lock` file.

3. Updating packages

To check if your dependencies have updates you can run command:

```text
dotnet paket outdated
```

To update all packages you can use:

```text
dotnet paket update
```

### Common Paket commands

* `(dotnet) paket convert-from-nuget` - converts the solution from NuGet tooling to Paket.

* `(dotnet) paket simplify` - parses the dependencies and strips out any packages
from the `paket.dependencies` file that aren’t top-level ones.
The `paket.lock` file still maintains the full tree of dependencies.

* `(dotnet) paket update` - updates your packages with the latest versions from NuGet.

* `(dotnet) paket restore` - brings down the current version of all dependencies specified in
the lock file (to ensure repeatable builds in CI).

* `(dotnet) paket add` - add a new NuGet package to the overall set of dependencies

  * Example: `dotnet paket add nuget Automapper project NugetFSharp`

* `paket generate-load-scripts` - generates a set of .fsx files that call `#r` on
all assemblies in a package and their dependencies.
