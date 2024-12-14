using System;
using System.Runtime.InteropServices;

namespace Examples.UIFeedbackGenerator.Plugins.iOS
{
    /// <summary>
    /// `UINotificationFeedbackGenerator` を C# から扱えるようにしたクラス
    /// </summary>
    /// <remarks>
    /// https://developer.apple.com/documentation/uikit/uinotificationfeedbackgenerator
    /// </remarks>
    public sealed class UINotificationFeedbackGenerator : IDisposable
    {
        // ネイティブ側と数値は合わせておくこと
        // https://developer.apple.com/documentation/uikit/uinotificationfeedbackgenerator/feedbacktype
        public enum FeedbackType
        {
            Success = 0,
            Warning = 1,
            Error = 2,
        }

        private readonly IntPtr _instance;

        public UINotificationFeedbackGenerator()
        {
            _instance = NativeMethod();

            [DllImport("__Internal", EntryPoint = "createUINotificationFeedbackGenerator")]
            static extern IntPtr NativeMethod();
        }

        public void Dispose()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "releaseUINotificationFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance);
        }

        public void Prepare()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "prepareUINotificationFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance);
        }

        public void NotificationOccurred(FeedbackType feedbackType)
        {
            NativeMethod(_instance, feedbackType);

            [DllImport("__Internal", EntryPoint = "notificationOccurredUINotificationFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance, FeedbackType feedbackType);
        }
    }
}
