using System;
using System.Runtime.InteropServices;

namespace Examples.UIFeedbackGenerator.Plugins.iOS
{
    /// <summary>
    /// <see href="https://developer.apple.com/documentation/uikit/uiimpactfeedbackgenerator">UIImpactFeedbackGenerator</see> の実装
    /// </summary>
    public sealed class UIImpactFeedbackGenerator : IDisposable
    {
        /// <summary>
        /// <see href="https://developer.apple.com/documentation/uikit/uiimpactfeedbackgenerator/feedbackstyle">UIImpactFeedbackGenerator.FeedbackStyle</see>
        /// </summary>
        public enum FeedbackStyle : Int32
        {
            // 数値はネイティブ側と合わせておく必要がある
            Light = 0,
            Medium = 1,
            Heavy = 2,
            Soft = 3,
            Rigid = 4,
        }

        private readonly IntPtr _instance;

        public UIImpactFeedbackGenerator(FeedbackStyle style)
        {
            _instance = NativeMethod(style);

            [DllImport("__Internal", EntryPoint = "init_UIImpactFeedbackGenerator")]
            static extern IntPtr NativeMethod(FeedbackStyle style);
        }

        public void Dispose()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "release_UIImpactFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance);
        }

        public void Prepare()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "prepare_UIImpactFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance);
        }

        public void ImpactOccurred()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "impactOccurred_UIImpactFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance);
        }

        // NOTE: intensity の指定は iOS 13 以降より利用可能
        public void ImpactOccurred(float intensity)
        {
            NativeMethod(_instance, intensity);

            [DllImport("__Internal", EntryPoint = "impactOccurred_UIImpactFeedbackGeneratorWithIntensity")]
            static extern void NativeMethod(IntPtr instance, float intensity);
        }
    }
}
