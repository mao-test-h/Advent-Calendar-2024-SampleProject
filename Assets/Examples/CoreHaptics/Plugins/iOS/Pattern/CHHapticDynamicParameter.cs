using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
namespace Examples.CoreHaptics.Plugins.iOS
{
    /// <summary>
    /// <see href="https://developer.apple.com/documentation/corehaptics/chhapticdynamicparameter">CHHapticDynamicParameter</see> の実装
    /// </summary>
    [Serializable]
    public sealed class CHHapticDynamicParameter
    {
        /// <summary>
        /// <see href="https://developer.apple.com/documentation/corehaptics/chhapticdynamicparameter/id">CHHapticDynamicParameter.ID</see>
        /// </summary>
        public enum ID : Int32
        {
            HapticIntensityControl = 0,
            HapticSharpnessControl,
            HapticAttackTimeControl,
            HapticDecayTimeControl,
            HapticReleaseTimeControl,
        }

        // parameterID
        public string ParameterID;

        // value
        public float ParameterValue;

        // relativeTime
        public float Time;

        private ID _parameterID;

        public CHHapticDynamicParameter(ID parameterID, float value, float relativeTime)
        {
            _parameterID = parameterID;
            ParameterID = parameterID.ToString();
            ParameterValue = value;
            Time = relativeTime;
        }

        internal BlittableDynamicParameter ToBlittableDynamicParameter()
        {
            return new BlittableDynamicParameter((Int32)_parameterID, Time, ParameterValue);
        }

        // NOTE: P/Invoke 用に Blittable 型で定義した CHHapticDynamicParameter
        [StructLayout(LayoutKind.Sequential)]
        internal struct BlittableDynamicParameter
        {
            public Int32 ParameterID;
            public float Time;
            public float ParameterValue;

            public BlittableDynamicParameter(Int32 paramId, float time, float paramValue)
            {
                ParameterID = paramId;
                Time = time;
                ParameterValue = paramValue;
            }
        }
    }
}
