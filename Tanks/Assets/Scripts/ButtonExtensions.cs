using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonExtensions : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
{
    public GameController gameController;


    public void OnPointerDown(PointerEventData eventData)
    {
        gameController.TankRaiseBarrel = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameController.TankShot();
        gameController.TankRaiseBarrel = false;
    }
}
