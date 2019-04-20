using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResetBarValuesScript : MonoBehaviour, IPointerExitHandler
{

    public Scrollbar scrollbar;

    public void OnPointerExit(PointerEventData eventData)
    {
        scrollbar.value = 0.5f;
    }
}
