using System.Drawing;
using System.Windows.Forms;

namespace KameliaRecycleSystem.Presentation.Controls.Navigation;

public class BreadcrumbNav : UserControl
{
    public BreadcrumbNav()
    {
        Name = nameof(BreadcrumbNav);
        Size = new Size(320, 120);
        BackColor = Color.WhiteSmoke;
    }
}
