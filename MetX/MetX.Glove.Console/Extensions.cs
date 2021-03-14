using System;
using System.Windows.Forms;
using MetX.Standard.Pipelines;

namespace XLG.Pipeliner
{
    public static class Extensions
    {
        public static TEnumOut As<TEnumOut>(this Enum target) where TEnumOut : struct
        {
            if (Enum.TryParse(typeof(TEnumOut), target.ToString(), true, out object? translated))
            {
                return translated == null 
                    ? default(TEnumOut)
                    : (TEnumOut) translated;
            }

            return default(TEnumOut);
        }

        /*public static MessageBoxResult AsMessageBoxResult(this DialogResult dialogResult)
        {
            if (Enum.TryParse(typeof(MessageBoxResult), dialogResult.ToString(), true, out object? messageBoxResult))
            {
                return messageBoxResult == null ? MessageBoxResult.None
                    : (MessageBoxResult) messageBoxResult;
            }

            return MessageBoxResult.None;
        }

        public static MessageBoxButtons AsMessageBoxButtons(this MessageBoxChoices messageBoxChoices)
        {
            if (Enum.TryParse(typeof(MessageBoxButtons), messageBoxChoices.ToString(), true, out object? messageBoxButtons))
            {
                return messageBoxButtons == null 
                    ? MessageBoxButtons.OKCancel
                    : (MessageBoxButtons) messageBoxButtons;
            }

            return MessageBoxButtons.OKCancel;
        }

        public static MessageBoxDefault AsMessageBoxDefault(this MessageBoxDefaultButton messageBoxDefaultButton)
        {
            if (Enum.TryParse(typeof(MessageBoxDefault), messageBoxDefaultButton.ToString(), true, out object? messageBoxDefault))
            {
                return messageBoxDefault == null 
                    ? MessageBoxButtons.OKCancel
                    : (MessageBoxButtons) messageBoxDefault;
            }

            return MessageBoxButtons.OKCancel;
        }*/
    }
}