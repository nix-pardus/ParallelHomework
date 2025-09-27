namespace ParallelHomework.ArrayCalculators;

public class ArrayCalculator : IArrayCalculator
{
    public long CalculateSum(int[] array)
    {
        long sum = 0;
        foreach (var item in array)
        {
            sum += item;
        }
        return sum;
    }
}
