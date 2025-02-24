using System;

// ReSharper disable InconsistentNaming
namespace Examples.CoreHaptics.Plugins.iOS
{
    /// <summary>
    /// <see href="https://developer.apple.com/documentation/corehaptics/chhapticparametercurve">CHHapticParameterCurve</see> の実装
    /// </summary>
    [Serializable]
    public sealed class CHHapticParameterCurve
    {
        /// <summary>
        /// <see href="https://developer.apple.com/documentation/corehaptics/chhapticparametercurve/controlpoint">CHHapticParameterCurve.ControlPoint</see> の実装
        /// </summary>
        [Serializable]
        public struct ControlPoint
        {
            // value
            public float ParameterValue;

            // relativeTime
            public float Time;

            public ControlPoint(float parameterValue, float time)
            {
                ParameterValue = parameterValue;
                Time = time;
            }
        }

        // parameterID
        public string ParameterID;

        // controlPoints
        public ControlPoint[] ParameterCurveControlPoints;

        // relativeTime
        public float Time;

        public CHHapticParameterCurve(CHHapticDynamicParameter.ID parameterID, ControlPoint[] controlPoints, float relativeTime)
        {
            ParameterID = parameterID.ToString();
            ParameterCurveControlPoints = controlPoints;
            Time = relativeTime;
        }
    }
}
