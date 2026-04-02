using System.Data;

namespace KameliaRecycleSystem.Utilities.Extensions;

public static class DataTableExtensions
{
    public static bool HasRows(this DataTable table) => table != null && table.Rows.Count > 0;
}
