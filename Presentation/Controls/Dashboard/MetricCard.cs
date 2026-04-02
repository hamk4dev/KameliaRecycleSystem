using System.Drawing;
using System.Windows.Forms;

namespace KameliaRecycleSystem.Presentation.Controls.Dashboard;

public class MetricCard : UserControl
{
    public MetricCard()
    {
        Name = nameof(MetricCard);
        Size = new Size(320, 120);
        BackColor = Color.WhiteSmoke;
    }
}
