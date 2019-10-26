using UnityEngine;

namespace Assets.Scripts
{
    public static class Color
    {
        public const string Red = "Red";
        public const string Blue = "Blue";
        public const string Green = "Green";
        public const string Yellow = "Yellow";
        public const string White = "White";
        public const string Black = "Black";
        public const string Magenta = "Magenta";
        public const string Purple = "Purple";

        public static string ToColor(this string color)
        {
            switch (color)
            {
                case "Red":
                    return Color.Red;
                    break;

                case "Blue":
                    return Color.Blue;
                    break;

                case "Green":
                    return Color.Green;
                    break;

                case "Yellow":
                    return Color.Yellow;
                    break;

                case "White":
                    return Color.White;
                    break;

                case "Black":
                    return Color.Black;
                    break;

                case "Magenta":
                    return Color.Magenta;
                    break;

                case "Purple":
                    return Color.Purple;
                    break;

                default:
                    Debug.LogError($"Not supported color: {color}");
                    return "NONE";
                    break;
            }
        }
    }
}
