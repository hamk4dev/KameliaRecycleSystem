using System.Drawing;
using System.Windows.Forms;

namespace KameliaRecycleSystem.Presentation.Controls.Navigation;

public class HeaderStatusBar : UserControl
{
    public HeaderStatusBar()
    {
        Name = nameof(HeaderStatusBar);
        Size = new Size(320, 120);
        BackColor = Color.WhiteSmoke;
    }
}
