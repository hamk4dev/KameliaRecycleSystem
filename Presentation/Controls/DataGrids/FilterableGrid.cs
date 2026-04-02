using System.Drawing;
using System.Windows.Forms;

namespace KameliaRecycleSystem.Presentation.Controls.DataGrids;

public class FilterableGrid : UserControl
{
    public FilterableGrid()
    {
        Name = nameof(FilterableGrid);
        Size = new Size(320, 120);
        BackColor = Color.WhiteSmoke;
    }
}
