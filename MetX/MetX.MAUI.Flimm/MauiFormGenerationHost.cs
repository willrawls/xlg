using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using MetX.Standard.Primary.Host;
using MetX.Standard.Strings;

namespace MetX.Windows.Library;

public class MauiFormGenerationHost<T> : GenerationHost where T : ContentPage
{
    public T ContentPage {get; set; }

    public object SyncRoot { get; } = new();

    private Rectangle _boundary;
    public sealed override Rectangle Boundary => new Rectangle((int) ContentPage.Bounds.X, (int) ContentPage.Bounds.Y, (int) ContentPage.Bounds.Width, (int) ContentPage.Bounds.Height);

    public MauiFormGenerationHost(T contentPage, Func<string> getTextForProcessing)
    {
        ContentPage = contentPage;
        _boundary = Boundary;
        MessageBox = new MauiFormMessageBoxHost<T>(ContentPage, this);
        GetTextForProcessing = getTextForProcessing;
    }

    public override MessageBoxResult InputBox(string title, string description, ref string itemName)
    {
        var dialog = new AskForStringDialog
        {
            ValueToReturnOnCancel = "~|~|"
        };
        var response = dialog.Ask((int) (ContentPage.X + 50), (int) (ContentPage.Y + 50), promptText: description, title: title, defaultValue: itemName, height: 140, width: 700);

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