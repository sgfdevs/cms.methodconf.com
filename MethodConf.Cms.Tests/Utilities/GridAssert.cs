using System.Text;

namespace MethodConf.Cms.Tests.Utilities;

using NUnit.Framework;

public static class GridAssert
{
    public static void ThatGridsAreEqual<T>(T[,]? actual, T[,]? expected)
    {
        if (actual is null && expected is null)
        {
            return;
        }

        if (actual is null || expected is null)
        {
            Assert.Fail("One of the grids is null while the other is not.");
            return;
        }

        var actualRows = actual.GetLength(0);
        var actualCols = actual.GetLength(1);
        var expectedRows = expected.GetLength(0);
        var expectedCols = expected.GetLength(1);

        if (actualRows != expectedRows || actualCols != expectedCols)
        {
            Assert.Fail($"Grid dimensions do not match. Expected dimensions: [{expectedRows}, {expectedCols}], Actual dimensions: [{actualRows}, {actualCols}]");
        }

        // Compare each element and collect differences
        var differences = new StringBuilder();
        var areEqual = true;

        for (var i = 0; i < actualRows; i++)
        {
            for (var j = 0; j < actualCols; j++)
            {
                if (!Equals(actual[i, j], expected[i, j]))
                {
                    areEqual = false;
                    differences.AppendLine($"Difference at position [{i}, {j}]: Expected='{expected[i, j]}', Actual='{actual[i, j]}'");
                }
            }
        }

        if (!areEqual)
        {
            var message = new StringBuilder();
            message.AppendLine("Grids are not equal.");
            message.AppendLine("Expected Grid:");
            message.AppendLine(GridToString(expected));
            message.AppendLine("Actual Grid:");
            message.AppendLine(GridToString(actual));
            message.AppendLine("Differences:");
            message.AppendLine(differences.ToString());

            Assert.Fail(message.ToString());
        }
    }

    private static string GridToString<T>(T[,] grid)
    {
        var rows = grid.GetLength(0);
        var cols = grid.GetLength(1);

        // Calculate the maximum width for each column
        var maxColWidths = new int[cols];
        for (var j = 0; j < cols; j++)
        {
            var maxWidth = 0;
            for (var i = 0; i < rows; i++)
            {
                var cellValue = grid[i, j]?.ToString() ?? string.Empty;
                if (cellValue.Length > maxWidth)
                {
                    maxWidth = cellValue.Length;
                }
            }
            maxColWidths[j] = maxWidth;
        }

        // Build the string representation of the grid with padded columns
        var sb = new StringBuilder();
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                var cellValue = grid[i, j]?.ToString() ?? string.Empty;
                sb.Append(cellValue.PadRight(maxColWidths[j] + 1)); // Add 1 space for separation
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
}

