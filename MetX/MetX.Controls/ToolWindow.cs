using System;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Gui;
using MetX.Standard.Primary;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Windows.Controls
{
    public interface IToolWindow
    {
        IGenerationHost Host { get; set; }
        void SetFocus(string controlName);
        void Show(QuickScriptOutput quickScriptOutput);
        void Hide();
    }

    public partial class ToolWindow : Form, IToolWindow
    {

        public IGenerationHost Host { get; set; }

        /*
        public static IGenerationHost HostInstance { get; set; }

        public static GuiContext SharedContext
        {
            get
            {
                if (ContextBase.Default != null)
                {
                    return ContextBase.Default as GuiContext;
                }
                ContextBase.Default = new GuiContext(HostInstance);
                return (GuiContext)ContextBase.Default;
            }
            set => ContextBase.Default = value;
        }
        */

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
                    Controls[controlName]?.Focus();
            }
            catch (Exception)
            {
                // Ignore
            }
        }

        public void Show(QuickScriptOutput quickScriptOutput)
        {
            quickScriptOutput.Show(this);
            quickScriptOutput.BringToFront();

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
                else if (component is TextArea)
                {
                    ChangeTheme(scheme, component.Controls);
                    component.BackColor = scheme.TextBoxBG;
                    component.ForeColor = scheme.TextBoxFG;
                    if (component.Controls.Count > 0)
                    {
                        component.Controls[0].BackColor = scheme.TextBoxBG;
                        component.Controls[0].ForeColor = scheme.TextBoxFG;
                    }

                    var textArea = (TextArea) component;
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
