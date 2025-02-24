using System;

// ReSharper disable InconsistentNaming
namespace Examples.CoreHaptics.Plugins.iOS
{
    /// <summary>
    /// <see href="https://developer.apple.com/documentation/corehaptics/chhapticeventparameter">CHHapticEventParameter</see> の実装
    /// </summary>
    [Serializable]
    public sealed class CHHapticEventParameter
    {
        // parameterID
        public string ParameterID;

        // value
        public float ParameterValue;

        public CHHapticEventParameter(ID parameterID, float value)
        {
            ParameterID = parameterID.ToString();
            ParameterValue = value;
        }

        /// <summary>
        /// <see href="https://developer.apple.com/documentation/corehaptics/chhapticevent/parameterid">CHHapticEvent.ParameterID</see>
        /// </summary>
        public enum ID
        {
            HapticIntensity = 0,
            HapticSharpness,
            AttackTime,
            DecayTime,
            ReleaseTime,
            Sustained,
        }
    }
}
