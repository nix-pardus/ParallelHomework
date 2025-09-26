using ParallelHomework;
using System.Diagnostics;

Console.WriteLine("Тест на 100 000");
PrintResults(CreateArray(100_000));
Console.WriteLine("Тест на 1 000 000");
PrintResults(CreateArray(1_000_000));
Console.WriteLine("Тест на 10 000 000");
PrintResults(CreateArray(10_000_000));


int[] CreateArray(int length)
{
    int[] arr = new int[length];

    for (int i = 0; i < length; i++)
    {
        arr[i] = i;
    }

    return arr;
}

void PrintResults(int[] arr)
{
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();
    long sum = ArrayHelper.Sum(arr);
    stopwatch.Stop();
    Console.WriteLine($"Sum: {sum}, Time taken (Single Thread): {stopwatch.ElapsedMilliseconds} ms");

    stopwatch.Restart();
    long sumWithListThreads = ArrayHelper.SumWithListThreads(arr, 4);
    stopwatch.Stop();
    Console.WriteLine($"Sum: {sumWithListThreads}, Time taken (4 List Threads): {stopwatch.ElapsedMilliseconds} ms");

    stopwatch.Restart();
    long sumWithPLINQ = ArrayHelper.SumWithPLINQ(arr);
    stopwatch.Stop();
    Console.WriteLine($"Sum: {sumWithPLINQ}, Time taken (PLINQ): {stopwatch.ElapsedMilliseconds} ms");
}