using System.Windows.Forms;

namespace KameliaRecycleSystem.Utilities.Extensions;

public static class ControlExtensions
{
    public static void ToggleEnabled(this Control control, bool enabled) => control.Enabled = enabled;
}
