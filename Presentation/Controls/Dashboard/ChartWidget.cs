using System.Drawing;
using System.Windows.Forms;

namespace KameliaRecycleSystem.Presentation.Controls.Dashboard;

public class ChartWidget : UserControl
{
    public ChartWidget()
    {
        Name = nameof(ChartWidget);
        Size = new Size(320, 120);
        BackColor = Color.WhiteSmoke;
    }
}
