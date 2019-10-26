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

    private void SetPoints()
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
    }

    public void FixedUpdate()
    {
        SetPoints();
    }
}