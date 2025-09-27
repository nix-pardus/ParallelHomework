using System.Diagnostics;

namespace ParallelHomework.ArrayCalculators;

public class ArrayCalculatorContext
{
    public IArrayCalculator Calculator { private get; set; }

    public ArrayCalculatorContext(IArrayCalculator calculator)
    {
        Calculator = calculator;
    }

    public ArrayCalculatorContext SetCalculator(IArrayCalculator calculator)
    {
        Calculator = calculator;
        return this;
    }

    public (long sum, Stopwatch sw) CalculateSum(int[] array)
    {
        if (Calculator == null)
        {
            throw new InvalidOperationException("Calculator is not set.");
        }
        var stopwatch = Stopwatch.StartNew();
        var sum = Calculator.CalculateSum(array);
        stopwatch.Stop();
        return (sum, stopwatch);
    }
}
