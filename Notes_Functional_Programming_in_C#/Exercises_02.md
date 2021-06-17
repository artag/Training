1. Write a console app that calculates a user’s Body Mass Index (BMI):

  * Prompt the user for their height in meters and weight in kilograms.
  * Calculate the BMI as `weight / (height ^ 2)`.
  * Output a message: underweight (BMI < 18.5), overweight (BMI >= 25), or healthy.
  * Structure your code so that pure and impure parts are separate.

2. Unit test the pure parts.

3. Unit test the overall workflow using the function-based approach to abstract
away the reading from and writing to the console.

```csharp
public enum BmiRange
{
    Underweight,
    Healthy,
    Overweight
}

public static class Bmi
{
    public static void Run() =>
        Run(Console.ReadLine, Console.Write);

    internal static void Run(Func<string> readFunc, Action<string> writeFunc)
    {
        void Write(string text) => WriteWithFunc(writeFunc, text);
        string Read() => ReadWithFunc(readFunc);

        Write("Enter your height (in m): ");
        var height = Read().ToDouble();

        Write("Enter your weight (in kg): ");
        var weight = Read().ToDouble();

        var range = CalculateBmi(height, weight).ToBmiRange();
        var message = ToMessage(range);

        Write(message);
    }

    // Pure
    internal static void WriteWithFunc(Action<string> printer, string text) =>
        printer(text);

    // Pure
    internal static string ReadWithFunc(Func<string> input) =>
        input();

    // Pure
    internal static double ToDouble(this string input)
    {
        var format = new NumberFormatInfo();
        format.NumberDecimalSeparator = ",";
        var input2 = input.Replace('.', ',');
        return double.Parse(input2, format);
    }

    // Pure
    internal static double CalculateBmi(double height, double weight) =>
        Round(weight / (height * height), 2);

    // Pure
    internal static BmiRange ToBmiRange(this double bmi)
    {
        if (bmi < 18.5)
            return BmiRange.Underweight;

        if (bmi < 25)
            return BmiRange.Healthy;

        return BmiRange.Overweight;
    }

    // Pure
    internal static string ToMessage(this BmiRange range)
    {
        if (range == BmiRange.Underweight)
            return "Underweight.";

        if (range == BmiRange.Healthy)
            return "Healthy.";

        return "Overweight.";
    }
}

public class BmiExcercisesTests
{
    [TestCase("20", ExpectedResult = 20.0)]
    [TestCase("20,1", ExpectedResult = 20.1)]
    [TestCase("20.1", ExpectedResult = 20.1)]
    public double Test_ToDouble(string input) =>
        Bmi.ToDouble(input);

    [TestCase(2.00, 67, ExpectedResult = 16.75)]
    [TestCase(1.80, 77, ExpectedResult = 23.77)]
    [TestCase(1.60, 77, ExpectedResult = 30.08)]
    public double Test_CalculateBmi(double height, double weight) =>
        Bmi.CalculateBmi(height, weight);

    [TestCase(2.00, 67, ExpectedResult = BmiRange.Underweight)]
    [TestCase(1.80, 77, ExpectedResult = BmiRange.Healthy)]
    [TestCase(1.60, 77, ExpectedResult = BmiRange.Overweight)]
    public BmiRange Test_ToBmiRange(double height, double weight) =>
        Bmi.CalculateBmi(height, weight).ToBmiRange();

    [TestCase(2.00, 67, ExpectedResult = "Underweight.")]
    [TestCase(1.80, 77, ExpectedResult = "Healthy.")]
    [TestCase(1.60, 77, ExpectedResult = "Overweight.")]
    public string Test_ToMessage(double height, double weight) =>
        Bmi.CalculateBmi(height, weight).ToBmiRange().ToMessage();

    [TestCase("2.00", "67", "Underweight.")]
    [TestCase("1.80", "77", "Healthy.")]
    [TestCase("1.60", "77", "Overweight.")]
    public void Test_Run(string height, string weight, string expected)
    {
        Bmi.Run(
            () => MockReadFunc(height, weight),
            actual => MockWriteFunc(expected, actual)
        );
    }

    private static string MockReadFunc(string height, string weight)
    {
        var i = 0;
        switch (i)
        {
            case 0:
                i++;
                return height;
            default:
                return weight;
        }
    }

    private static void MockWriteFunc(string expected, string actual)
    {
        var i = 0;
        switch (i)
        {
            case 0:
            case 1:
                i++;
                break;
            default:
                Assert.AreEqual(expected, actual);
                break;
        }
    }
}
```
