using System.Drawing;
using System.Windows.Forms;

namespace KameliaRecycleSystem.Presentation.Controls.DataGrids;

public class EditableGrid : UserControl
{
    public EditableGrid()
    {
        Name = nameof(EditableGrid);
        Size = new Size(320, 120);
        BackColor = Color.WhiteSmoke;
    }
}
