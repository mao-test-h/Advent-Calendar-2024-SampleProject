using Examples.CoreHaptics.Plugins.iOS;

namespace Examples.CoreHaptics
{
    public static class PatternSamples
    {
        public static CHHapticPattern Sample1()
        {
            // Sharpness は 1 固定
            const float sharpness = 1f;

            var hapticEvents = new[]
            {
                // 0.1 秒後に [Intensity:1] で Transient を再生
                new CHHapticEvent(
                    CHHapticEvent.Type.HapticTransient,
                    new[]
                    {
                        new CHHapticEventParameter(CHHapticEventParameter.ID.HapticIntensity, 1f),
                        new CHHapticEventParameter(CHHapticEventParameter.ID.HapticSharpness, sharpness),
                    },
                    relativeTime: 0.1f),

                // 0.2 秒後に [Intensity:0.7] で Transient を再生
                new CHHapticEvent(
                    CHHapticEvent.Type.HapticTransient,
                    new[]
                    {
                        new CHHapticEventParameter(CHHapticEventParameter.ID.HapticIntensity, 0.7f),
                        new CHHapticEventParameter(CHHapticEventParameter.ID.HapticSharpness, sharpness),
                    },
                    relativeTime: 0.2f),

                // 0.3 秒後に [Intensity:0.6] で Transient を再生
                new CHHapticEvent(
                    CHHapticEvent.Type.HapticTransient,
                    new[]
                    {
                        new CHHapticEventParameter(CHHapticEventParameter.ID.HapticIntensity, 0.6f),
                        new CHHapticEventParameter(CHHapticEventParameter.ID.HapticSharpness, sharpness),
                    },
                    relativeTime: 0.3f),

                // 0.4 秒後に [Intensity:0.5] で Continuous を [0.4秒間] 再生
                new CHHapticEvent(
                    CHHapticEvent.Type.HapticContinuous,
                    new[]
                    {
                        new CHHapticEventParameter(CHHapticEventParameter.ID.HapticIntensity, 0.5f),
                        new CHHapticEventParameter(CHHapticEventParameter.ID.HapticSharpness, sharpness),
                    },
                    duration: 0.4f,
                    relativeTime: 0.4f),

                // 0.9 秒後に [Intensity:0.7] で Transient を再生
                new CHHapticEvent(
                    CHHapticEvent.Type.HapticTransient,
                    new[]
                    {
                        new CHHapticEventParameter(CHHapticEventParameter.ID.HapticIntensity, 0.7f),
                        new CHHapticEventParameter(CHHapticEventParameter.ID.HapticSharpness, sharpness),
                    },
                    relativeTime: 0.9f),
            };

            return new CHHapticPattern(hapticEvents);
        }

        public static CHHapticPattern Sample2()
        {
            var hapticEvents = new CHHapticEvent[6];
            for (var i = 0; i < hapticEvents.Length; ++i)
            {
                var value = i * 0.2f;
                hapticEvents[i] =
                    new CHHapticEvent(
                        CHHapticEvent.Type.HapticTransient,
                        new[]
                        {
                            new CHHapticEventParameter(CHHapticEventParameter.ID.HapticIntensity, 1f),
                            new CHHapticEventParameter(CHHapticEventParameter.ID.HapticSharpness, value),
                        },
                        relativeTime: value);
            }

            return new CHHapticPattern(hapticEvents);
        }

        public static CHHapticPattern Sample3()
        {
            // 検証用
            return new CHHapticPattern(null);
        }
    }
}
