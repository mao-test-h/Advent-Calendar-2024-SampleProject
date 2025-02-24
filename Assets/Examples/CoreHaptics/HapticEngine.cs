using System;
using Examples.CoreHaptics.Plugins.iOS;
using UnityEngine;

namespace Examples.CoreHaptics
{
    /// <summary>
    /// CHHapticEngine のラッパークラス
    /// </summary>
    public sealed class HapticEngine : IHapticEngine, CHHapticEngine.IEventHandler
    {
        private readonly CHHapticEngine _hapticEngine;
        private bool _isNeedsStart;

        public HapticEngine()
        {
            _hapticEngine = new CHHapticEngine();
            _hapticEngine.SetEventHandler(this);
        }

        public void Dispose()
        {
            _hapticEngine.Dispose();
        }

        public void StartHapticEngine()
        {
            _hapticEngine.Start();
            _isNeedsStart = false;
        }

        public void StopHapticEngine()
        {
            _hapticEngine.Stop();
            _isNeedsStart = true;
        }

        public void PlayPattern(CHHapticPattern pattern)
        {
            StartHapticEngineIfNeeded();
            _hapticEngine.PlayPattern(pattern);
        }

        public void PlayPattern(string patternJson)
        {
            StartHapticEngineIfNeeded();
            _hapticEngine.PlayPattern(patternJson);
        }

        public CHHapticPatternPlayer MakePlayer(CHHapticPattern pattern)
        {
            StartHapticEngineIfNeeded();
            return _hapticEngine.MakePlayer(pattern);
        }

        private void StartHapticEngineIfNeeded()
        {
            if (_isNeedsStart)
            {
                StartHapticEngine();
            }
        }

        void CHHapticEngine.IEventHandler.OnReset()
        {
            Debug.Log("OnReset");
            _isNeedsStart = true;
        }

        void CHHapticEngine.IEventHandler.OnStopped(CHHapticEngine.StoppedReason reason)
        {
            Debug.Log($"OnStopped: {reason}");

            switch (reason)
            {
                case CHHapticEngine.StoppedReason.AudioSessionInterrupt:
                    Debug.Log("Audio session interrupt.");
                    break;
                case CHHapticEngine.StoppedReason.ApplicationSuspended:
                    Debug.Log("Application suspended.");
                    break;
                case CHHapticEngine.StoppedReason.IdleTimeout:
                    Debug.Log("Idle timeout.");
                    break;
                case CHHapticEngine.StoppedReason.NotifyWhenFinished:
                    Debug.Log("Finished.");
                    break;
                case CHHapticEngine.StoppedReason.EngineDestroyed:
                    Debug.Log("Engine destroyed.");
                    break;
                case CHHapticEngine.StoppedReason.GameControllerDisconnect:
                    Debug.Log("Controller disconnected.");
                    break;
                case CHHapticEngine.StoppedReason.SystemError:
                    Debug.Log("System error.");
                    break;
                default:
                    Debug.Log("Unknown error");
                    break;
            }

            _isNeedsStart = true;
        }

        void CHHapticEngine.IEventHandler.OnErrorWithInit(string message)
            => Debug.LogError($"CHHapticEngine.Init Error: {message}");

        void CHHapticEngine.IEventHandler.OnErrorWithStart(string message)
            => Debug.LogError($"CHHapticEngine.Start Error: {message}");

        void CHHapticEngine.IEventHandler.OnErrorWithStop(string message)
            => Debug.LogError($"CHHapticEngine.Stop Error: {message}");

        void CHHapticEngine.IEventHandler.OnErrorWithMakePlayer(string message)
            => Debug.LogError($"CHHapticPatternPlayer.MakePlayer Error: {message}");

        void CHHapticEngine.IEventHandler.OnErrorWithPlayPattern(string message)
            => Debug.LogError($"CHHapticPatternPlayer.PlayPattern Error: {message}");
    }

    public interface IHapticEngine : IDisposable
    {
        public void StartHapticEngine();
        public void StopHapticEngine();
        public void PlayPattern(CHHapticPattern pattern);
        public void PlayPattern(string patternJson);
        public CHHapticPatternPlayer MakePlayer(CHHapticPattern pattern);
    }

    /// <summary>
    /// 非対応プラットフォーム用のダミークラス
    /// </summary>
    public sealed class DummyHapticEngine : IHapticEngine
    {
        public void Dispose()
        {
        }

        public void StartHapticEngine()
        {
        }

        public void StopHapticEngine()
        {
        }

        public void PlayPattern(CHHapticPattern pattern)
        {
        }

        public void PlayPattern(string patternJson)
        {
        }

        public CHHapticPatternPlayer MakePlayer(CHHapticPattern pattern)
        {
            throw new System.NotImplementedException();
        }
    }
}
