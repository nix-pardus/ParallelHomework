using ParallelHomework;
using ParallelHomework.ArrayCalculators;
using ParallelHomework.Table;

const int COUNT_THREADS = 6;

Console.WriteLine(ComputerInfo.GetSummaryInfo());
Console.WriteLine();

int[] arr1 = CreateArray(100_000);
int[] arr2 = CreateArray(1_000_000);
int[] arr3 = CreateArray(10_000_000);

var calculator = new ArrayCalculatorContext(new ArrayCalculator());

TableDrawer table = new TableDrawer();
string content = table.AddCell("")
    .AddCell("Синхронный метод")
    .AddCell($"С использованием Thread (потоков: {COUNT_THREADS})")
    .AddCell("С использованием PLINQ")
    .MoveToNewRow()
    .AddCell("Array length: 100 000")
    .AddCell(calculator.SetCalculator(new ArrayCalculator()).CalculateSum(arr1).sw.ElapsedMilliseconds.ToString())
    .AddCell(calculator.SetCalculator(new ArrayCalculatorWithThreads(COUNT_THREADS)).CalculateSum(arr1).sw.ElapsedMilliseconds.ToString())
    .AddCell(calculator.SetCalculator(new ArrayCalculatorWithPlinq()).CalculateSum(arr1).sw.ElapsedMilliseconds.ToString())
    .MoveToNewRow()
    .AddCell("Array length: 1 000 000")
    .AddCell(calculator.SetCalculator(new ArrayCalculator()).CalculateSum(arr2).sw.ElapsedMilliseconds.ToString())
    .AddCell(calculator.SetCalculator(new ArrayCalculatorWithThreads(4)).CalculateSum(arr2).sw.ElapsedMilliseconds.ToString())
    .AddCell(calculator.SetCalculator(new ArrayCalculatorWithPlinq()).CalculateSum(arr2).sw.ElapsedMilliseconds.ToString())
    .MoveToNewRow()
    .AddCell("Array length: 10 000 000")
    .AddCell(calculator.SetCalculator(new ArrayCalculator()).CalculateSum(arr3).sw.ElapsedMilliseconds.ToString())
    .AddCell(calculator.SetCalculator(new ArrayCalculatorWithThreads(4)).CalculateSum(arr3).sw.ElapsedMilliseconds.ToString())
    .AddCell(calculator.SetCalculator(new ArrayCalculatorWithPlinq()).CalculateSum(arr3).sw.ElapsedMilliseconds.ToString())
    .Draw();
Console.WriteLine(content);

int[] CreateArray(int length)
{
    int[] arr = new int[length];

    for (int i = 0; i < length; i++)
    {
        arr[i] = i;
    }

    return arr;
}
