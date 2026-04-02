using System.Drawing;
using System.Windows.Forms;

namespace KameliaRecycleSystem.Presentation.Forms.Management;

public class KeuanganForm : Form
{
    public KeuanganForm()
    {
        Text = "KeuanganForm";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(960, 640);
        MinimumSize = new Size(720, 480);
    }
}
