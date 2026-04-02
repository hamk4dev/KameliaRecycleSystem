using System.Drawing;
using System.Windows.Forms;

namespace KameliaRecycleSystem.Presentation.Forms.Utilities;

public class PrintPreviewForm : Form
{
    public PrintPreviewForm()
    {
        Text = "PrintPreviewForm";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(960, 640);
        MinimumSize = new Size(720, 480);
    }
}
