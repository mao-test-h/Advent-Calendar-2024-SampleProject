using System;

// ReSharper disable InconsistentNaming
namespace Examples.CoreHaptics.Plugins.iOS
{
    /// <summary>
    /// <see href="https://developer.apple.com/documentation/corehaptics/chhapticevent">CHHapticEvent</see> の実装
    /// </summary>
    [Serializable]
    public sealed class CHHapticEvent
    {
        // type
        public string EventType;

        // eventParameters
        public CHHapticEventParameter[] EventParameters;

        // relativeTime
        public float Time;

        // duration
        public float EventDuration;

        public CHHapticEvent(
            Type eventType,
            CHHapticEventParameter[] eventParameters,
            float relativeTime,
            float duration = 0f)
        {
            EventType = eventType.ToString();
            EventParameters = eventParameters;
            Time = relativeTime;
            EventDuration = duration;
        }

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/corehaptics/chhapticevent/eventtype">CHHapticEvent.EventType</see>
        /// </summary>
        public enum Type
        {
            HapticTransient = 0,
            HapticContinuous,
        }
    }
}
