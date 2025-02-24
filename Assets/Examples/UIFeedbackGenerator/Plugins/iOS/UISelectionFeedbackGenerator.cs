using System;
using System.Runtime.InteropServices;

namespace Examples.UIFeedbackGenerator.Plugins.iOS
{
    /// <summary>
    /// <see href="https://developer.apple.com/documentation/uikit/uiselectionfeedbackgenerator">UISelectionFeedbackGenerator</see> の実装
    /// </summary>
    public sealed class UISelectionFeedbackGenerator : IDisposable
    {
        private readonly IntPtr _instance;

        public UISelectionFeedbackGenerator()
        {
            _instance = NativeMethod();

            [DllImport("__Internal", EntryPoint = "init_UISelectionFeedbackGenerator")]
            static extern IntPtr NativeMethod();
        }

        public void Dispose()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "release_UISelectionFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance);
        }

        public void Prepare()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "prepare_UISelectionFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance);
        }

        public void SelectionChanged()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "selectionChanged_UISelectionFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance);
        }
    }
}
