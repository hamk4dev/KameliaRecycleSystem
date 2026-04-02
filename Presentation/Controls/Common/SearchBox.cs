using System.Drawing;
using System.Windows.Forms;

namespace KameliaRecycleSystem.Presentation.Controls.Common;

public class SearchBox : UserControl
{
    public SearchBox()
    {
        Name = nameof(SearchBox);
        Size = new Size(320, 120);
        BackColor = Color.WhiteSmoke;
    }
}
