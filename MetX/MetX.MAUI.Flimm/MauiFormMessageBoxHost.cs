using System.Windows.Forms;
using MetX.Standard.Primary.Host;
using MetX.Standard.Primary.Interfaces;

namespace MetX.Windows
{
    public class MauiFormMessageBoxHost<TContentPage> : IMessageBox where TContentPage : ContentPage
    {
        public TContentPage Parent { get; set; }
        public IGenerationHost Host { get; set; }

    
        public MauiFormMessageBoxHost(TContentPage parent, IGenerationHost host)
        {
            Host = host;
            Parent = parent;
        }

        public MessageBoxResult Show(string message)
        {
            return MessageBox.Show(message).As<MessageBoxResult>();
        }

        public MessageBoxResult Show(string message, string title)
        {
            return MessageBox.Show(message, title).As<MessageBoxResult>();
        }

        public MessageBoxResult Show(string message, string title, MessageBoxChoices choices)
        {
            return MessageBox.Show(message, title, choices.As<MessageBoxButtons>()).As<MessageBoxResult>();
        }

        public MessageBoxResult Show(string message, string title, MessageBoxChoices choices, MessageBoxStatus status,
            MessageBoxDefault @default)
        {
            return MessageBox.Show(
                    message, title, 
                    choices
                        .As<MessageBoxButtons>(), status
                        .As<MessageBoxIcon>(), @default
                        .As<MessageBoxDefaultButton>())
                .As<MessageBoxResult>();
        }
    }
}