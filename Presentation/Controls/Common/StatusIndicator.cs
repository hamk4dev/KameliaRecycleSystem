using System.Drawing;
using System.Windows.Forms;

namespace KameliaRecycleSystem.Presentation.Controls.Common;

public class StatusIndicator : UserControl
{
    public StatusIndicator()
    {
        Name = nameof(StatusIndicator);
        Size = new Size(320, 120);
        BackColor = Color.WhiteSmoke;
    }
}
