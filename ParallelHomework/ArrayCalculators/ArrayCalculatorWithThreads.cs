namespace ParallelHomework.ArrayCalculators;

public class ArrayCalculatorWithThreads : IArrayCalculator
{
    private readonly int _numberOfThreads;
    public ArrayCalculatorWithThreads(int numberOfThreads)
    {
        if (numberOfThreads <= 0)
        {
            throw new ArgumentException("Number of threads must be greater than zero.");
        }
        _numberOfThreads = numberOfThreads;
    }
    public long CalculateSum(int[] array)
    {
        int length = array.Length;
        int chunkSize = (int)Math.Ceiling((double)length / _numberOfThreads);
        long[] sums = new long[_numberOfThreads];
        List<Thread> threads = new List<Thread>();
        for (int i = 0; i < _numberOfThreads; i++)
        {
            int threadIndex = i;
            var thread = new Thread(() =>
            {
                int start = threadIndex * chunkSize;
                int end = Math.Min(start + chunkSize, length);
                for (int j = start; j < end; j++)
                {
                    sums[threadIndex] += array[j];
                }
            });
            threads.Add(thread);
            thread.Start();
        }
        foreach (var thread in threads)
        {
            thread.Join();
        }
        long totalSum = 0;
        foreach (var partialSum in sums)
        {
            totalSum += partialSum;
        }
        return totalSum;
    }

}
