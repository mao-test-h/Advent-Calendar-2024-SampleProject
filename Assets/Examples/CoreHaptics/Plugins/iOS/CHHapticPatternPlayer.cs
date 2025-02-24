using System;
using System.Linq;
using System.Runtime.InteropServices;
using AOT;

namespace Examples.CoreHaptics.Plugins.iOS
{
    /// <summary>
    /// <see href="https://developer.apple.com/documentation/corehaptics/chhapticpatternplayer">CHHapticPatternPlayer</see> の実装
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class CHHapticPatternPlayer : IDisposable
    {
        /// <summary>
        /// エラー発生時のイベント
        /// </summary>
        /// <remarks>
        /// NOTE:
        /// - こちらは複数のインスタンスが生成される想定があるので、event 構文で登録できるようにしている
        /// - このサンプルでは簡略化のために全 API で共通のイベントを呼び出しているが、ちゃんとやるならメソッドごとに分けるなりしても良いかも
        /// </remarks>
        public event Action<string> OnError
        {
            add => OnErrorInternal += value;
            remove => OnErrorInternal -= value;
        }

        private static event Action<string> OnErrorInternal;
        private readonly IntPtr _instance;

        internal CHHapticPatternPlayer(IntPtr instance)
        {
            _instance = instance;
        }

        public void Dispose()
        {
            NativeMethod(_instance);

            [DllImport("__Internal", EntryPoint = "release_CHHapticPatternPlayer")]
            static extern void NativeMethod(IntPtr instance);
        }

        /// <summary>
        /// 再生
        /// </summary>
        public void Start(float atTime = 0f)
        {
            NativeMethod(_instance, atTime, OnError);

            [DllImport("__Internal", EntryPoint = "start_CHHapticPatternPlayer")]
            static extern void NativeMethod(IntPtr instance, float atTime, [MarshalAs(UnmanagedType.FunctionPtr)] OnErrorDelegate onError);

            [MonoPInvokeCallback(typeof(OnErrorDelegate))]
            static void OnError(string error) => OnErrorInternal?.Invoke(error);
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop(float atTime = 0f)
        {
            NativeMethod(_instance, atTime, OnError);

            [DllImport("__Internal", EntryPoint = "stop_CHHapticPatternPlayer")]
            static extern void NativeMethod(IntPtr instance, float atTime, [MarshalAs(UnmanagedType.FunctionPtr)] OnErrorDelegate onError);

            [MonoPInvokeCallback(typeof(OnErrorDelegate))]
            static void OnError(string error) => OnErrorInternal?.Invoke(error);
        }

        /// <summary>
        /// キャンセル
        /// </summary>
        public void Cancel()
        {
            NativeMethod(_instance, OnError);

            [DllImport("__Internal", EntryPoint = "cancel_CHHapticPatternPlayer")]
            static extern void NativeMethod(IntPtr instance, [MarshalAs(UnmanagedType.FunctionPtr)] OnErrorDelegate onError);

            [MonoPInvokeCallback(typeof(OnErrorDelegate))]
            static void OnError(string error) => OnErrorInternal?.Invoke(error);
        }

        /// <summary>
        /// パラメータの設定
        /// </summary>
        public void SendParameters(CHHapticDynamicParameter[] parameters, float atTime = 0f)
        {
            if (parameters is not { Length: > 0 })
            {
                throw new ArgumentException("No CHHapticDynamicParameters were provided for sending.");
            }

            var sendParams = parameters.Select(p => p.ToBlittableDynamicParameter()).ToArray();
            var handle = GCHandle.Alloc(sendParams, GCHandleType.Pinned);
            NativeMethod(_instance, handle.AddrOfPinnedObject(), parameters.Length, atTime, OnError);
            handle.Free();

            [DllImport("__Internal", EntryPoint = "sendParameters_CHHapticPatternPlayer")]
            static extern void NativeMethod(
                IntPtr instance,
                IntPtr parameters,
                Int32 parametersLength,
                float atTime,
                [MarshalAs(UnmanagedType.FunctionPtr)] OnErrorDelegate onError);

            [MonoPInvokeCallback(typeof(OnErrorDelegate))]
            static void OnError(string error) => OnErrorInternal?.Invoke(error);
        }

        private delegate void OnErrorDelegate(string error);
    }
}
