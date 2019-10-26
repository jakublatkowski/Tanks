using UnityEngine;
using Color = Assets.Scripts.Color;

public class PointsController : MonoBehaviour
{
    public int RedPoints { get; private set; }
    public int BluePoints { get; private set; }
    public int GreenPoints { get; private set; }
    public int YellowPoints { get; private set; }
    public int WhitePoints { get; private set; }
    public int BlackPoints { get; private set; }
    public int MagentaPoints { get; private set; }
    public int PurplePoints { get; private set; }

    public void AddPoints(string color, int points)
    {
        switch (color)
        {
            case Color.Red:
                RedPoints += points;
                break;

            case Color.Blue:
                BluePoints += points;
                break;

            case Color.Green:
                GreenPoints += points;
                break;

            case Color.Yellow:
                YellowPoints += points;
                break;

            case Color.White:
                WhitePoints += points;
                break;

            case Color.Black:
                BlackPoints += points;
                break;

            case Color.Magenta:
                MagentaPoints += points;
                break;

            case Color.Purple:
                PurplePoints += points;
                break;

            default:
                Debug.LogError($"Not supported color: {color}");
                break;
        }
    }
}