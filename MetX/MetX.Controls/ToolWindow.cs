using MetX.Standard.Library;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using MetX.Standard;
using MetX.Standard.Interfaces;
using MetX.Standard.Pipelines;

namespace MetX.Controls
{
    public partial class ToolWindow : Form //: DockContent
    {

        public IGenerationHost Host { get; set; }
        public static IGenerationHost HostInstance { get; set; }

        public static Context SharedContext
        {
            get
            {
                if (ContextBase.Default != null)
                {
                    return ContextBase.Default as Context;
                }
                ContextBase.Default = new Context(HostInstance);
                return (Context)ContextBase.Default;
            }
            set => ContextBase.Default = value;
        }

        public ToolWindow()
        {
            InitializeComponent();
            ChangeTheme(ColorScheme.DarkThemeOne, Controls);
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

        public static bool EnableThemes = false;
        public static void ChangeTheme(ColorScheme scheme, Control.ControlCollection container)
        {
            if (!EnableThemes)
                return;

            foreach (Control component in container)
            {
                if (component is Panel)
                {
                    ChangeTheme(scheme, component.Controls);
                    component.BackColor = scheme.PanelBG;
                    component.ForeColor = scheme.PanelFG;
                }
                else if (component is Button)
                {
                    component.BackColor = scheme.ButtonBG;
                    component.ForeColor = scheme.ButtonFG;
                }
                else if (component is TextBox)
                {
                    component.BackColor = scheme.TextBoxBG;
                    component.ForeColor = scheme.TextBoxFG;
                }
                else if (component is ToolStripButton)
                {
                    component.BackColor = scheme.TextBoxBG;
                    component.ForeColor = scheme.TextBoxFG;
                }
                else if (component is ToolStripComboBox)
                {
                    component.BackColor = scheme.TextBoxBG;
                    component.ForeColor = scheme.TextBoxFG;
                }
                else if (component is ToolStripLabel)
                {
                    component.BackColor = scheme.TextBoxBG;
                    component.ForeColor = scheme.TextBoxFG;
                }
                else if (component is ToolStrip or MenuStrip)
                {
                    ChangeTheme(scheme, component.Controls);
                    component.BackColor = scheme.TextBoxBG;
                    component.ForeColor = scheme.TextBoxFG;
                }
                else if (component is QuickScriptControl)
                {
                    ChangeTheme(scheme, component.Controls);
                    component.BackColor = scheme.TextBoxBG;
                    component.ForeColor = scheme.TextBoxFG;
                    component.Controls[0].BackColor = scheme.TextBoxBG;
                    component.Controls[0].ForeColor = scheme.TextBoxFG;
                }
                else if (component is ICSharpCode.TextEditor.TextArea)
                {
                    ChangeTheme(scheme, component.Controls);
                    component.BackColor = scheme.TextBoxBG;
                    component.ForeColor = scheme.TextBoxFG;
                    if (component.Controls.Count > 0)
                    {
                        component.Controls[0].BackColor = scheme.TextBoxBG;
                        component.Controls[0].ForeColor = scheme.TextBoxFG;
                    }

                    var textArea = (ICSharpCode.TextEditor.TextArea) component;
                    textArea.BackColor = scheme.TextBoxBG;
                    textArea.ForeColor = scheme.TextBoxFG;
                }
                else
                {
                    component.BackColor = scheme.PanelBG;
                    component.ForeColor = scheme.PanelFG;
                    ChangeTheme(scheme, component.Controls);
                }
            }
        }
    }
}
