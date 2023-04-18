﻿using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Data.Core.Plugins;
using Avalonia.Reactive;

namespace Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings
{
    internal class ObservableStreamPlugin<T> : IStreamPlugin
    {
        [RequiresUnreferencedCode(TrimmingMessages.StreamPluginRequiresUnreferencedCodeMessage)]
        public bool Match(WeakReference<object?> reference)
        {
            return reference.TryGetTarget(out var target) && target is IObservable<T>;
        }

        [RequiresUnreferencedCode(TrimmingMessages.StreamPluginRequiresUnreferencedCodeMessage)]
        public IObservable<object?> Start(WeakReference<object?> reference)
        {
            if (!(reference.TryGetTarget(out var target) && target is IObservable<T> obs))
            {
                return Observable.Empty<object?>();
            }
            else if (target is IObservable<object?> obj)
            {
                return obj;
            }

            return obs.Select(x => (object?)x);
        }
    }
}
