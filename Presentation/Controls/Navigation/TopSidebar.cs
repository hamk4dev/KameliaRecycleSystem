using System.Drawing;
using System.Windows.Forms;

namespace KameliaRecycleSystem.Presentation.Controls.Navigation;

public class TopSidebar : UserControl
{
    public TopSidebar()
    {
        Name = nameof(TopSidebar);
        Size = new Size(320, 120);
        BackColor = Color.WhiteSmoke;
    }
}
