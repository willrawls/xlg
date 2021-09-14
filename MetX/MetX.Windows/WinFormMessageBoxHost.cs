using System.Windows.Forms;
using MetX.Standard.Interfaces;
using MetX.Standard.Pipelines;
using MetX.Windows;

namespace MetX.Controls
{
    public class WinFormMessageBoxHost<TForm> : IMessageBox where TForm : Form
    {
        public TForm Parent { get; set; }
        public IGenerationHost Host { get; set; }

        public WinFormMessageBoxHost(TForm parent, IGenerationHost host)
        {
            Host = host;
            Parent = parent;
        }

        public MessageBoxResult Show(string message)
        {
            
            return MessageBox.Show(Parent, message).As<MessageBoxResult>();
        }

        public MessageBoxResult Show(string message, string title)
        {
            return MessageBox.Show(Parent, message, title).As<MessageBoxResult>();
        }

        public MessageBoxResult Show(string message, string title, MessageBoxChoices choices)
        {
            return MessageBox.Show(Parent, message, title, choices.As<MessageBoxButtons>()).As<MessageBoxResult>();
        }

        public MessageBoxResult Show(string message, string title, MessageBoxChoices choices, MessageBoxStatus status,
            MessageBoxDefault @default)
        {
            return MessageBox.Show(
                    Parent, message, title, 
                    choices
                        .As<MessageBoxButtons>(), status
                        .As<MessageBoxIcon>(), @default
                        .As<MessageBoxDefaultButton>())
                .As<MessageBoxResult>();
        }
    }
}