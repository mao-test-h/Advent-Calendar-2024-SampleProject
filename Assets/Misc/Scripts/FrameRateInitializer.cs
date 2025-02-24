using UnityEngine;

namespace Misc
{
    public static class FrameRateInitializer
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            Application.targetFrameRate = 60;
        }
    }
}
