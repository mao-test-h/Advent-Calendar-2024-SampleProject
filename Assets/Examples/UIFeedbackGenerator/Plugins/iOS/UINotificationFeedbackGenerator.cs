using System;
using System.Runtime.InteropServices;

namespace Examples.UIFeedbackGenerator.Plugins.iOS
{
    /// <summary>
    /// <see href="https://developer.apple.com/documentation/uikit/uinotificationfeedbackgenerator">UINotificationFeedbackGenerator</see> の実装
    /// </summary>
    public sealed class UINotificationFeedbackGenerator : IDisposable
    {
        /// <summary>
        /// <see href="https://developer.apple.com/documentation/uikit/uinotificationfeedbackgenerator/feedbacktype">UINotificationFeedbackGenerator.FeedbackType</see>
        /// </summary>
        public enum FeedbackType
        {
            // 数値はネイティブ側と合わせておく必要がある
            Success = 0,
            Warning = 1,
            Error = 2,
        }

        private readonly IntPtr _instance;

        public UINotificationFeedbackGenerator()
        {
            _instance = NativeMethod();

            [DllImport("__Internal", EntryPoint = "init_UINotificationFeedbackGenerator")]
            static extern IntPtr NativeMethod();
        }

        public void Dispose()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "release_UINotificationFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance);
        }

        public void Prepare()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "prepare_UINotificationFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance);
        }

        public void NotificationOccurred(FeedbackType feedbackType)
        {
            NativeMethod(_instance, feedbackType);

            [DllImport("__Internal", EntryPoint = "notificationOccurred_UINotificationFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance, FeedbackType feedbackType);
        }
    }
}
