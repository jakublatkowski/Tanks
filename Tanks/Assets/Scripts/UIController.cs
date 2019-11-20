using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Tooltip("UI Text to display Player's Name")]
    [SerializeField]
    private Text playerNameText;

    [Tooltip("UI Slider to display Player's Health")]
    [SerializeField]
    private Slider playerHealthSlider;

    [Header("GameOver")]
    public GameObject gameOverPanel;

    [Header("Time Panel")]
    public GameObject timePanel;

    [Header("Colors-Points Panels")]
    public GameObject redPanel;
    public GameObject bluePanel;
    public GameObject greenPanel;
    public GameObject yellowPanel;
    public GameObject whitePanel;
    public GameObject blackPanel;
    public GameObject magentaPanel;
    public GameObject purplePanel;

    [Header("Controls")]
    public Joystick leftJoystick;
    public Joystick rightJoystick;
    public Image healthBarImage;
    public Image specialBarImage;
    public GameObject shootingLagPanel;

    private PointsController pointsController;
    private float maxTimeSeconds;

    public void SetHealthBarValue(float val)
    {
        if (val <= 1 && val >= 0)
            healthBarImage.fillAmount = val;
        else if (val < 0)
        {
            Debug.Log("HealthBar value < 0");
            healthBarImage.fillAmount = 0;
        }
        else
        {
            Debug.Log("HealthBar Value > 1");
            healthBarImage.fillAmount = 1;
        }

    }

    public void SetSpecialBarValue(float val)
    {
        if (val <= 1 && val >= 0)
            specialBarImage.fillAmount = val;
        else if (val < 0)
        {
            Debug.Log("SpecialBar Value < 0");
            specialBarImage.fillAmount = 0;
        }
        else
        {
            Debug.Log("SpecialBar Value > 1");
            specialBarImage.fillAmount = 1;
        }
    }

    public float GetLeftJoystickVertical()
    {
        return leftJoystick.Vertical;
    }

    public float GetRightJoystickVertical()
    {
        return rightJoystick.Vertical;
    }

    public void PlayShootingDelayAnimation()
    {
        shootingLagPanel.SetActive(false);
        shootingLagPanel.SetActive(true);
    }

    private void ShowPoints()
    {
        redPanel.GetComponentInChildren<Text>().text = pointsController.RedPoints.ToString();
        bluePanel.GetComponentInChildren<Text>().text = pointsController.BluePoints.ToString();
        greenPanel.GetComponentInChildren<Text>().text = pointsController.GreenPoints.ToString();
        yellowPanel.GetComponentInChildren<Text>().text = pointsController.YellowPoints.ToString();
        whitePanel.GetComponentInChildren<Text>().text = pointsController.WhitePoints.ToString();
        blackPanel.GetComponentInChildren<Text>().text = pointsController.BlackPoints.ToString();
        magentaPanel.GetComponentInChildren<Text>().text = pointsController.MagentaPoints.ToString();
        purplePanel.GetComponentInChildren<Text>().text = pointsController.PurplePoints.ToString();
    }

    void Start()
    {
        pointsController = GameObject.Find(nameof(PointsController)).GetComponent<PointsController>();
        var minutes = PhotonNetwork.CurrentRoom.CustomProperties["Time"].ToString();

        switch (minutes)
        {
            case "1 min":
            {
                maxTimeSeconds = 60;
                break;
            }

            case "2 min":
            {
                maxTimeSeconds = 2 * 60;
                break;
            }

            case "5 min":
            {
                maxTimeSeconds = 5 * 60;
                break;
            }

            case "10 min":
            {
                maxTimeSeconds = 10 * 60;
                break;
            }

            default:
            {
                throw new NotSupportedException("Not supported time range");
            }
        }
    }


    public void GameOver()
    {
        timePanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void Update()
    {
        ShowPoints();
        var seconds = (int)(maxTimeSeconds -= Time.deltaTime);
        var minutes = seconds / 60;
        seconds -= minutes * 60;
        timePanel.GetComponentInChildren<Text>().text = seconds < 10 ? $"{minutes}:0{seconds}" : $"{minutes}:{seconds}";

        if(minutes == 0 && seconds == 0) GameOver();
    }
}