using System;
using System.Runtime.InteropServices;

namespace Examples.UIFeedbackGenerator.Plugins.iOS
{
    /// <summary>
    /// `UISelectionFeedbackGenerator` を C# から扱えるようにしたクラス
    /// </summary>
    /// <remarks>
    /// https://developer.apple.com/documentation/uikit/uiselectionfeedbackgenerator
    /// </remarks>
    public sealed class UISelectionFeedbackGenerator : IDisposable
    {
        private readonly IntPtr _instance;

        public UISelectionFeedbackGenerator()
        {
            _instance = NativeMethod();

            [DllImport("__Internal", EntryPoint = "createUISelectionFeedbackGenerator")]
            static extern IntPtr NativeMethod();
        }

        public void Dispose()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "releaseUISelectionFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance);
        }

        public void Prepare()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "prepareUISelectionFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance);
        }

        public void SelectionChanged()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "selectionChangedUISelectionFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance);
        }
    }
}
