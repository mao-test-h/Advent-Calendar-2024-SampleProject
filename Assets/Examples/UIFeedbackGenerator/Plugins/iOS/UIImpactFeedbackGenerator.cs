using System;
using System.Runtime.InteropServices;

namespace Examples.UIFeedbackGenerator.Plugins.iOS
{
    /// <summary>
    /// `UIImpactFeedbackGenerator` を C# から扱えるようにしたクラス
    /// </summary>
    /// <remarks>
    /// https://developer.apple.com/documentation/uikit/uiimpactfeedbackgenerator
    /// </remarks>
    public sealed class UIImpactFeedbackGenerator : IDisposable
    {
        // ネイティブ側と数値は合わせておくこと
        // https://developer.apple.com/documentation/uikit/uiimpactfeedbackgenerator/feedbackstyle
        public enum FeedbackStyle : Int32
        {
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

            [DllImport("__Internal", EntryPoint = "createUIImpactFeedbackGenerator")]
            static extern IntPtr NativeMethod(FeedbackStyle style);
        }

        public void Dispose()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "releaseUIImpactFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance);
        }

        public void Prepare()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "prepareUIImpactFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance);
        }

        public void ImpactOccurred()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "impactOccurredUIImpactFeedbackGenerator")]
            static extern void NativeMethod(IntPtr instance);
        }

        // NOTE: intensity の指定は iOS 13 以降より利用可能
        public void ImpactOccurred(float intensity)
        {
            NativeMethod(_instance, intensity);

            [DllImport("__Internal", EntryPoint = "impactOccurredUIImpactFeedbackGeneratorWithIntensity")]
            static extern void NativeMethod(IntPtr instance, float intensity);
        }
    }
}
