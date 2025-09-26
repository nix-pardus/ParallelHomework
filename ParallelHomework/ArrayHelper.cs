namespace ParallelHomework;

public static class ArrayHelper
{
    public static long Sum(int[] array)
    {
        long sum = 0;
        foreach (var item in array)
        {
            sum += item;
        }
        return sum;
    }

    public static long SumWithListThreads(int[] array, int numberOfThreads)
    {
        if (numberOfThreads <= 0)
        {
            throw new ArgumentException("Number of threads must be greater than zero.");
        }
        int length = array.Length;
        int chunkSize = (int)Math.Ceiling((double)length / numberOfThreads);
        long[] sums = new long[numberOfThreads];
        List<Thread> threads = new List<Thread>();
        for (int i = 0; i < numberOfThreads; i++)
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

    public static long SumWithPLINQ(int[] array)
    {
        return array.AsParallel().Sum(x => (long)x);
    }   
}
