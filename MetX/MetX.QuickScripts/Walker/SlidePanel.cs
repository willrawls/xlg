using System.Windows.Forms;

namespace XLG.QuickScripts.Walker;

public class SlidePanel<TControl, TState> : SlidePanelBase 
    where TControl : Control
    where TState : new()
{
    public TControl Child => (TControl) base.ChildControl;
    public TState State { get; set; }

    public SlidePanel(TState state = default)
    {
        State = state;
    }
}

