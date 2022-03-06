using System.Drawing;
using System.Windows.Forms;

namespace MetX.Controls;

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

