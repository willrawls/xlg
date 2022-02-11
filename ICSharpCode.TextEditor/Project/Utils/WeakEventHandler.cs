﻿using System;
using System.Diagnostics;
using System.Reflection;

namespace ICSharpCode.TextEditor.Utils
{
    /// <summary>
    /// http://paulstovell.com/blog/weakevents
    /// </summary>
    /// <typeparam name="TEventArgs">The type of the event args.</typeparam>
    [DebuggerNonUserCode]
    public sealed class WeakEventHandler<TEventArgs> where TEventArgs : EventArgs
    {
        private readonly WeakReference _targetReference;
        private readonly MethodInfo _method;

        public WeakEventHandler(EventHandler<TEventArgs> callback)
        {
            _method = callback.Method;
            _targetReference = new WeakReference(callback.Target, true);
        }

        [DebuggerNonUserCode]
        public void Handler(object sender, TEventArgs e)
        {
            var target = _targetReference.Target;
            if (target != null)
            {
                var callback = (Action<object, TEventArgs>)Delegate.CreateDelegate(typeof(Action<object, TEventArgs>), target, _method, true);
                if (callback != null)
                {
                    callback(sender, e);
                }
            }
        }
    }
}
