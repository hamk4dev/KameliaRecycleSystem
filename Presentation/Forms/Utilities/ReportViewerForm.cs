using System.Drawing;
using System.Windows.Forms;

namespace KameliaRecycleSystem.Presentation.Forms.Utilities;

public class ReportViewerForm : Form
{
    public ReportViewerForm()
    {
        Text = "ReportViewerForm";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(960, 640);
        MinimumSize = new Size(720, 480);
    }
}
