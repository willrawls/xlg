using MetX.Library;
using System;
using System.Windows.Forms;

namespace MetX.Controls
{
    public partial class ToolWindow : Form //: DockContent
    {
        public static Context Context
        {
            get
            {
                if (ContextBase.Default != null)
                {
                    return ContextBase.Default as Context;
                }
                ContextBase.Default = new Context();
                return (Context)ContextBase.Default;
            }
            set { ContextBase.Default = value; }
        }

        public ToolWindow()
        {
            InitializeComponent();
        }

        public void SetFocus(string controlName)
        {
            try
            {
                if (Controls.ContainsKey(controlName))
                    Controls[controlName].Focus();
            }
            catch (Exception)
            {
                // Ignore
            }
        }
    }
}
