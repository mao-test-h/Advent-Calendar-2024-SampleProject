using System;
using System.Runtime.InteropServices;
using AOT;

namespace Examples.CoreHaptics.Plugins.iOS
{
    /// <summary>
    /// <see href="https://developer.apple.com/documentation/corehaptics/chhapticengine">CHHapticEngine</see> の実装
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class CHHapticEngine : IDisposable
    {
        /// <summary>
        /// 停止した理由
        /// </summary>
        /// <seealso href="https://developer.apple.com/documentation/corehaptics/chhapticengine/stoppedreason"/>
        public enum StoppedReason
        {
            AudioSessionInterrupt = 1,
            ApplicationSuspended = 2,
            IdleTimeout = 3,
            NotifyWhenFinished = 4,
            EngineDestroyed = 5,
            GameControllerDisconnect = 6,
            SystemError = -1
        }

        public interface IEventHandler
        {
            /// <summary>
            /// <see cref="CHHapticEngine"/> が停止したときに呼ばれるイベント
            /// </summary>
            /// <seealso href="https://developer.apple.com/documentation/corehaptics/chhapticengine/stoppedhandler-swift.property"/>
            void OnStopped(StoppedReason reason);

            /// <summary>
            /// <see cref="CHHapticEngine"/> がリセットされたときに呼ばれるイベント
            /// </summary>
            /// <seealso href="https://developer.apple.com/documentation/corehaptics/chhapticengine/resethandler-swift.property"/>
            void OnReset();

            // 各メソッドごとのエラーイベント
            void OnErrorWithInit(string message);
            void OnErrorWithStart(string message);
            void OnErrorWithStop(string message);
            void OnErrorWithMakePlayer(string message);
            void OnErrorWithPlayPattern(string message);
        }

        private static IEventHandler _eventHandler;
        private readonly IntPtr _instance;

        /// <summary>
        /// デバイスがサポートしているか？
        /// </summary>
        /// <seealso href="https://developer.apple.com/documentation/corehaptics/chhapticdevicecapability/supportshaptics"/>
        public static bool IsSupportsHaptics
        {
            get
            {
#if UNITY_EDITOR || !UNITY_IOS
                return false;
#else
                return NativeMethod() == 1;

                [DllImport("__Internal", EntryPoint = "supportsHaptics_CHHapticEngine")]
                static extern byte NativeMethod();
#endif
            }
        }

        public void SetEventHandler(IEventHandler eventHandler) => _eventHandler = eventHandler;

        public CHHapticEngine()
        {
            // エンジンを生成してインスタンスを受け取る
            // その際に各種イベントのコールバックも渡しておく
            _instance = NativeMethod(OnError, OnStopped, OnReset);

            [DllImport("__Internal", EntryPoint = "init_CHHapticEngine")]
            static extern IntPtr NativeMethod(
                [MarshalAs(UnmanagedType.FunctionPtr)] OnErrorDelegate onCreateError,
                [MarshalAs(UnmanagedType.FunctionPtr)] OnStoppedDelegate onStopped,
                [MarshalAs(UnmanagedType.FunctionPtr)] OnResetDelegate onReset);

            // エラー発生時のコールバック
            [MonoPInvokeCallback(typeof(OnErrorDelegate))]
            static void OnError(string error) => _eventHandler?.OnErrorWithInit(error);

            // エンジンが停止した際のコールバック
            [MonoPInvokeCallback(typeof(OnStoppedDelegate))]
            static void OnStopped(Int32 code) => _eventHandler?.OnStopped((StoppedReason)code);

            // エンジンがリセットされた際のコールバック
            [MonoPInvokeCallback(typeof(OnResetDelegate))]
            static void OnReset() => _eventHandler?.OnReset();
        }

        public void Dispose()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "release_CHHapticEngine")]
            static extern void NativeMethod(IntPtr instance);
        }

        /// <summary>
        /// エンジンの起動
        /// </summary>
        public void Start()
        {
            NativeMethod(_instance, OnError);

            [DllImport("__Internal", EntryPoint = "start_CHHapticEngine")]
            static extern void NativeMethod(IntPtr instance, [MarshalAs(UnmanagedType.FunctionPtr)] OnErrorDelegate onError);

            [MonoPInvokeCallback(typeof(OnErrorDelegate))]
            static void OnError(string error) => _eventHandler?.OnErrorWithStart(error);
        }

        /// <summary>
        /// エンジンの停止
        /// </summary>
        public void Stop()
        {
            NativeMethod(_instance, OnError);

            [DllImport("__Internal", EntryPoint = "stop_CHHapticEngine")]
            static extern void NativeMethod(IntPtr instance, [MarshalAs(UnmanagedType.FunctionPtr)] OnErrorDelegate onError);

            [MonoPInvokeCallback(typeof(OnErrorDelegate))]
            static void OnError(string error) => _eventHandler?.OnErrorWithStop(error);
        }

        /// <summary>
        /// <see cref="CHHapticPatternPlayer"/> の生成
        /// </summary>
        /// <param name="pattern">再生する <see cref="CHHapticPattern"/></param>
        public CHHapticPatternPlayer MakePlayer(CHHapticPattern pattern)
        {
            return MakePlayer(pattern.ToAhap());
        }

        /// <summary>
        /// <see cref="CHHapticPatternPlayer"/> の生成
        /// </summary>
        /// <param name="ahapStr">再生する AHAP 形式の文字列</param>
        public CHHapticPatternPlayer MakePlayer(string ahapStr)
        {
            var instance = NativeMethod(_instance, ahapStr, OnError);
            return new CHHapticPatternPlayer(instance);

            [DllImport("__Internal", EntryPoint = "makePlayer_CHHapticEngine")]
            static extern IntPtr NativeMethod(IntPtr instance, string patternJson, [MarshalAs(UnmanagedType.FunctionPtr)] OnErrorDelegate onError);

            [MonoPInvokeCallback(typeof(OnErrorDelegate))]
            static void OnError(string error) => _eventHandler?.OnErrorWithMakePlayer(error);
        }

        /// <summary>
        /// パターンの再生
        /// </summary>
        /// <param name="pattern">再生する <see cref="CHHapticPattern"/></param>
        public void PlayPattern(CHHapticPattern pattern)
        {
            PlayPattern(pattern.ToAhap());
        }

        /// <summary>
        /// パターンの再生
        /// </summary>
        /// <param name="ahapStr">再生する AHAP 形式の文字列</param>
        public void PlayPattern(string ahapStr)
        {
            NativeMethod(_instance, ahapStr, OnError);

            [DllImport("__Internal", EntryPoint = "playPattern_CHHapticEngine")]
            static extern void NativeMethod(IntPtr instance, string patternJson, [MarshalAs(UnmanagedType.FunctionPtr)] OnErrorDelegate onError);

            [MonoPInvokeCallback(typeof(OnErrorDelegate))]
            static void OnError(string error) => _eventHandler?.OnErrorWithPlayPattern(error);
        }

        private delegate void OnStoppedDelegate(Int32 code);

        private delegate void OnResetDelegate();

        private delegate void OnErrorDelegate(string error);
    }
}
