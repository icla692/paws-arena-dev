using UnityEngine;

namespace Anura.Globals
{
    public static class Constants
    {
        public static Color accentColor;

        static Constants()
        {
            ColorUtility.TryParseHtmlString("#74006F",out accentColor);
        }
    }
}
