using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MetX.Standard.Primary.Host;
using MetX.Standard.Strings;
using MetX.Standard.Strings.Extensions;

namespace MetX.Windows.Library;

public class WinFormGenerationHost<T> : GenerationHost where T : Form
{
    public T Form {get; set; }

    public object SyncRoot { get; } = new();

    private Rectangle _boundary;
    public override Rectangle Boundary => Form.Bounds;

    public WinFormGenerationHost(T form, Func<string> getTextForProcessing)
    {
        Form = form;
        _boundary = Form.Bounds;
        MessageBox = new WinFormMessageBoxHost<T>(Form, this);
        GetTextForProcessing = getTextForProcessing;
    }

    public override MessageBoxResult InputBox(string title, string description, ref string itemName)
    {
        var dialog = new AskForStringDialog
        {
            ValueToReturnOnCancel = "~|~|"
        };
        var response = dialog.Ask(Form.Top + 50, Form.Left + 50, promptText: description, title: title, defaultValue: itemName, height: 140, width: 700);

        if (response.IsEmpty() || response == dialog.ValueToReturnOnCancel)
            return MessageBoxResult.Cancel;
        itemName = response;
        return dialog.MessageBoxResult;
    }

    public override void WaitFor(Action action)
    {
        if (!Monitor.TryEnter(SyncRoot)) return;

        try
        {
            Cursor.Current = Cursors.WaitCursor;
            action();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.ToString());
        }
        finally
        {
            Monitor.Exit(SyncRoot);
            Cursor.Current = Cursors.Default;
        }
    }
}