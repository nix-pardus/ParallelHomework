namespace ParallelHomework.ArrayCalculators;

public class ArrayCalculatorWithPlinq : IArrayCalculator
{
    public long CalculateSum(int[] array)
    {
        return array.AsParallel().Sum(x => (long)x);
    }
}
