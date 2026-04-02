using System.Drawing;
using System.Windows.Forms;

namespace KameliaRecycleSystem.Presentation.Controls.Dashboard;

public class RecentActivity : UserControl
{
    public RecentActivity()
    {
        Name = nameof(RecentActivity);
        Size = new Size(320, 120);
        BackColor = Color.WhiteSmoke;
    }
}
