using System;
using System.Collections.Generic;
using Examples.CoreHaptics.Plugins.iOS;
using Misc;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.CoreHaptics
{
    internal sealed class CoreHapticsExample : MonoBehaviour
    {
        [Header("Transient")]
        [SerializeField] private Button playTransient;
        [SerializeField] private SliderAndLabel transientIntensity;
        [SerializeField] private SliderAndLabel transientSharpness;

        [Header("Continuous")]
        [SerializeField] private Button playContinuous;
        [SerializeField] private Button continuousSendParameters;
        [SerializeField] private SliderAndLabel continuousIntensity;
        [SerializeField] private SliderAndLabel continuousSharpness;
        [SerializeField] private SliderAndLabel continuousDuration;
        [SerializeField] private SliderAndLabel continuousAttack;
        [SerializeField] private Toggle continuousSustained;
        [SerializeField] private SliderAndLabel continuousDecay;
        [SerializeField] private SliderAndLabel continuousRelease;

        [Header("Pattern Samples")]
        [SerializeField] private Button playSample1;
        [SerializeField] private Button playSample2;
        [SerializeField] private Button playSample3;

        [Header("AHAP Samples")]
        [SerializeField] private Transform ahapButtonsParent;
        [SerializeField] private Button ahapButtonPrefab;
        [SerializeField] private List<TextAsset> ahapFiles;

        private IHapticEngine _hapticEngine;
        private CHHapticPatternPlayer _latestContinuousPlayer;

        private void Start()
        {
            if (CHHapticEngine.IsSupportsHaptics)
            {
                _hapticEngine = new HapticEngine();
            }
            else
            {
                Debug.LogWarning("Not Supported CHHapticEngine.");
                _hapticEngine = new DummyHapticEngine();
            }

            _hapticEngine.StartHapticEngine();

            SetEventTransient();
            SetEventContinuous();
            SetEventPatternSamples();
            SetEventAhapSamples();
        }

        private void OnDestroy()
        {
            _hapticEngine.StopHapticEngine();
            _hapticEngine.Dispose();
            _latestContinuousPlayer?.Dispose();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _hapticEngine?.StopHapticEngine();
            }
            else
            {
                _hapticEngine?.StartHapticEngine();
            }
        }

        private void SetEventTransient()
        {
            // Transition の再生
            playTransient.onClick.AddListener(() =>
            {
                var intensity = transientIntensity.Value;
                var sharpness = transientSharpness.Value;

                var hapticEvent = new CHHapticEvent(
                    CHHapticEvent.Type.HapticTransient,
                    new[]
                    {
                        new CHHapticEventParameter(CHHapticEventParameter.ID.HapticIntensity, intensity),
                        new CHHapticEventParameter(CHHapticEventParameter.ID.HapticSharpness, sharpness),
                    },
                    relativeTime: 0f);

                var pattern = new CHHapticPattern(new[] { hapticEvent });
                _hapticEngine.PlayPattern(pattern);
            });
        }

        private void SetEventContinuous()
        {
            // Continuous の再生
            playContinuous.onClick.AddListener(() =>
            {
                if (_latestContinuousPlayer != null)
                {
                    _latestContinuousPlayer.Stop();
                    _latestContinuousPlayer.Dispose();
                    _latestContinuousPlayer = null;
                }

                var intensity = continuousIntensity.Value;
                var sharpness = continuousSharpness.Value;
                var attackTime = continuousAttack.Value;
                var decayTime = continuousDecay.Value;
                var releaseTime = continuousRelease.Value;
                var sustained = continuousSustained.isOn;
                var duration = continuousDuration.Value;

                var hapticEvent = new CHHapticEvent(
                    CHHapticEvent.Type.HapticContinuous,
                    new[]
                    {
                        new CHHapticEventParameter(CHHapticEventParameter.ID.HapticIntensity, intensity),
                        new CHHapticEventParameter(CHHapticEventParameter.ID.HapticSharpness, sharpness),
                        new CHHapticEventParameter(CHHapticEventParameter.ID.AttackTime, attackTime),
                        new CHHapticEventParameter(CHHapticEventParameter.ID.DecayTime, decayTime),
                        new CHHapticEventParameter(CHHapticEventParameter.ID.ReleaseTime, releaseTime),
                        // NOTE: Sustained は float で渡す必要がある
                        new CHHapticEventParameter(CHHapticEventParameter.ID.Sustained, sustained ? 1.0f : 0.0f),
                    },
                    relativeTime: 0f,
                    duration: duration);

                // Continuous の場合には再生中のパラメータを動的に変更出来るようにプレイヤーを保持しておく
                var pattern = new CHHapticPattern(new[] { hapticEvent });
                var player = _hapticEngine.MakePlayer(pattern);
                player.OnError += error => Debug.LogError($"CHHapticPatternPlayer.Start Error: {error}");
                player.Start();
                _latestContinuousPlayer = player;
            });

            // 再生中の Continuous パターンのパラメータを変更
            continuousSendParameters.onClick.AddListener(() =>
            {
                if (_latestContinuousPlayer == null)
                {
                    Debug.LogWarning("No player to send parameters.");
                    return;
                }

                var intensity = continuousIntensity.Value;
                var sharpness = continuousSharpness.Value;
                var attackTime = continuousAttack.Value;
                var decayTime = continuousDecay.Value;
                var releaseTime = continuousRelease.Value;

                var parameters = new[]
                {
                    new CHHapticDynamicParameter(CHHapticDynamicParameter.ID.HapticIntensityControl, intensity, 0f),
                    new CHHapticDynamicParameter(CHHapticDynamicParameter.ID.HapticSharpnessControl, sharpness, 0f),
                    new CHHapticDynamicParameter(CHHapticDynamicParameter.ID.HapticAttackTimeControl, attackTime, 0f),
                    new CHHapticDynamicParameter(CHHapticDynamicParameter.ID.HapticDecayTimeControl, decayTime, 0f),
                    new CHHapticDynamicParameter(CHHapticDynamicParameter.ID.HapticReleaseTimeControl, releaseTime, 0f),
                };

                _latestContinuousPlayer.SendParameters(parameters);
            });
        }

        private void SetEventPatternSamples()
        {
            playSample1.onClick.AddListener(() =>
            {
                _hapticEngine.PlayPattern(PatternSamples.Sample1());
            });

            // TODO: 他のサンプルを作る
            playSample2.onClick.AddListener(() =>
            {
                _hapticEngine.PlayPattern(PatternSamples.Sample1());
            });

            playSample3.onClick.AddListener(() =>
            {
                _hapticEngine.PlayPattern(PatternSamples.Sample1());
            });
        }

        private void SetEventAhapSamples()
        {
            foreach (var ahap in Enum.GetValues(typeof(AhapFileName)))
            {
                var button = Instantiate(ahapButtonPrefab, ahapButtonsParent);
                button.GetComponentInChildren<Text>().text = ahap.ToString();
                button.onClick.AddListener(() =>
                {
                    var ahapAsset = ahapFiles.Find(asset => asset.name == ahap.ToString());
                    _hapticEngine.PlayPattern(ahapAsset.text);
                });
                button.gameObject.SetActive(true);
            }

            ahapButtonPrefab.gameObject.SetActive(false);
        }
    }
}
