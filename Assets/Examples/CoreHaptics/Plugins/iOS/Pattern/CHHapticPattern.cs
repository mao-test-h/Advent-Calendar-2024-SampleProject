using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Examples.CoreHaptics.Plugins.iOS
{
    /// <summary>
    /// <see href="https://developer.apple.com/documentation/corehaptics/chhapticpattern">CHHapticPattern</see> の実装
    /// </summary>
    public sealed class CHHapticPattern
    {
        private readonly List<CHHapticEvent> _events;
        private readonly List<CHHapticDynamicParameter> _dynamicParameters;
        private readonly List<CHHapticParameterCurve> _parameterCurves;

        public CHHapticPattern(CHHapticEvent[] events)
        {
            _events = new List<CHHapticEvent>(events);
        }

        public CHHapticPattern(CHHapticEvent[] events, CHHapticDynamicParameter[] parameters)
        {
            _events = new List<CHHapticEvent>(events);
            _dynamicParameters = new List<CHHapticDynamicParameter>(parameters);
        }

        public CHHapticPattern(CHHapticEvent[] events, CHHapticParameterCurve[] parameterCurves)
        {
            _events = new List<CHHapticEvent>(events);
            _parameterCurves = new List<CHHapticParameterCurve>(parameterCurves);
        }

        /// <summary>
        /// AHAP 形式に変換
        /// </summary>
        public string ToAhap()
        {
            // 各パラメータを AHAP に準拠した JSON 形式に変換して返す。

            // NOTE:
            // このサンプルでは AHAP へのシリアライズを行う際には JsonUtility を用いており、
            // 一部の箇所はその制約からそのままシリアライズを行うことが出来ないので、手動で文字列を組み立てることで解決している。
            using var _ = ListPool<string>.Get(out var pattern);

            if (_events != null)
            {
                pattern.AddRange(_events.Select(JsonUtility.ToJson)
                    .Select(str => $"{{ \"Event\": {str} }}"));
            }

            if (_dynamicParameters != null)
            {
                pattern.AddRange(_dynamicParameters.Select(JsonUtility.ToJson)
                    .Select(str => $"{{ \"Parameter\": {str} }}"));
            }

            if (_parameterCurves != null)
            {
                pattern.AddRange(_parameterCurves.Select(JsonUtility.ToJson)
                    .Select(str => $"{{ \"ParameterCurve\": {str} }}"));
            }

            // NOTE: サンプルなのでバージョンは `1.0` 固定
            return $"{{\"Version\":1.0,\"Pattern\":[{string.Join(",", pattern)}]}}";
        }
    }
}
