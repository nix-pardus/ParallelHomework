using System.Text;
using System.Threading.Tasks;

namespace ParallelHomework.Table;

public class TableDrawer
{
    private StringBuilder _table;
    private List<List<string>> rows;

    public TableDrawer()
    {
        _table = new StringBuilder();
        rows = new List<List<string>>();
        rows.Add(new List<string>());
    }

    public TableDrawer AddCell(string cell)
    {
        rows.Last().Add(cell);
        return this;
    }

    public TableDrawer MoveToNewRow()
    {
        rows.Add(new List<string>());
        return this;
    }

    public string Draw()
    {
        if (rows.Count == 0 || rows.All(r => r.Count == 0))
            return "Таблица пуста";

        int columnsCount = rows.Max(r => r.Count);

        // Вычисляем ширины столбцов
        int[] columnWidths = new int[columnsCount];
        for (int col = 0; col < columnsCount; col++)
        {
            int maxWidth = 0;
            for (int row = 0; row < rows.Count; row++)
            {
                if (col < rows[row].Count)
                {
                    maxWidth = Math.Max(maxWidth, rows[row][col].Length);
                }
            }
            columnWidths[col] = maxWidth;
        }

        _table.Clear();

        // Верхняя граница
        DrawHorizontalBorder(columnWidths, "┌", "┬", "┐");
        _table.AppendLine();

        // Содержимое таблицы
        for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            _table.Append("│");

            for (int colIndex = 0; colIndex < columnsCount; colIndex++)
            {
                string cellContent = colIndex < rows[rowIndex].Count ? rows[rowIndex][colIndex] : "";
                _table.Append($" {cellContent.PadRight(columnWidths[colIndex])} │");
            }
            _table.AppendLine();

            // Разделитель между строками или нижняя граница
            if (rowIndex < rows.Count - 1)
            {
                DrawHorizontalBorder(columnWidths, "├", "┼", "┤");
            }
            else
            {
                DrawHorizontalBorder(columnWidths, "└", "┴", "┘");
            }
            _table.AppendLine();
        }

        return _table.ToString();
    }


    private void DrawHorizontalBorder(int[] columnWidths, string left, string middle, string right)
    {
        _table.Append(left);

        for (int i = 0; i < columnWidths.Length; i++)
        {
            _table.Append(new string('─', columnWidths[i] + 2));

            if (i < columnWidths.Length - 1)
                _table.Append(middle);
        }

        _table.Append(right);
    }
}
