﻿using MetX.Standard.Interfaces;
using MetX.Standard.Pipelines;

namespace MetX.Standard.Generation
{
    public class DoNothingMessageBoxHost : IMessageBox
    {
        public IGenerationHost Host { get; set; }

        public DoNothingMessageBoxHost(IGenerationHost host)
        {
            Host = host;
        }

        public MessageBoxResult Show(string message)
        {

            return MessageBoxResult.No;
        }

        public MessageBoxResult Show(string message, string title)
        {
            return MessageBoxResult.No;
        }

        public MessageBoxResult Show(string message, string title, MessageBoxChoices choices)
        {
            return MessageBoxResult.No;
        }

        public MessageBoxResult Show(string message, string title, MessageBoxChoices choices, MessageBoxStatus status, MessageBoxDefault @default)
        {
            return MessageBoxResult.No;
        }
    }
}