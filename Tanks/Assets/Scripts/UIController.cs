using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    [Tooltip("UI Text to display Player's Name")]
    [SerializeField]
    private Text playerNameText;
    
    [Tooltip("UI Slider to display Player's Health")]
    [SerializeField]
    private Slider playerHealthSlider;

    [SerializeField]
    private Scrollbar leftScrollbar;

    [SerializeField]
    private Scrollbar rightScrollbar;

    [SerializeField]
    private Image healthBarImage;

    [SerializeField]
    private Image specialBarImage;

    [SerializeField]
    private GameObject shootingLagPanel;

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

    public float GetLeftSrollBarValue()
    {
        return leftScrollbar.value;
    }

    public float GetRightSrollBarValue()
    {
        return rightScrollbar.value;
    }

    public void PlayShootingDelayAnimation()
    {
        shootingLagPanel.SetActive(false);
        shootingLagPanel.SetActive(true);
    }

    public void SetScrollbarsDefault()
    {
        rightScrollbar.value = 0.5f;
        leftScrollbar.value = 0.5f;
    }
    public void Update()
    {
        //tylko dla testu
        //SetHealthBarValue(GetRightSrollBarValue());
        SetSpecialBarValue(GetRightSrollBarValue());

        //if (playerHealthSlider != null)
        //{
        //    playerHealthSlider.value = target.Health;
        //}
    }


}