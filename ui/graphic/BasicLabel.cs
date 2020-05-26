using System.Windows.Forms;

namespace new_GenericDBMS.project.ui.graphic {
    public sealed class BasicLabel : Label{
        public BasicLabel(string text) {
            this.Text = text ?? "Basic Label";
            this.Padding = new Padding(3);
            this.Margin = new Padding(3);
            this.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            this.Dock = DockStyle.Fill;
        }
    }
}