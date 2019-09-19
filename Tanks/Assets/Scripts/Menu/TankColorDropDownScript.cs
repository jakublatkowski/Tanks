using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankColorDropDownScript : MonoBehaviour
{
    [SerializeField]
    private GameObject LeftArrow;

    [SerializeField]
    private GameObject RightArrow;

    [SerializeField]
    private GameObject Label;

    [SerializeField]
    private List<string> ListOfItems;

    private int _currentIndex;
    private int CurrentIndex
    {
        get { return _currentIndex; }
        set
        {
            if (value >= ListOfItems.Count)
            {
                value = 0;
            }
            if (value < 0)
            {
                value = ListOfItems.Count - 1;
            }
            _currentIndex = value;
            OnCurrentIndexChanged();
        }
    }

    public void Click(GameObject button)
    {
        if (ListOfItems.Count == 0) return;
        if (button == RightArrow)
        {
            CurrentIndex++;
        }
        else if (button == LeftArrow)
        {
            CurrentIndex--;
        }
    }
    private void OnCurrentIndexChanged()
    {
        Label.GetComponent<Text>().text = ListOfItems[CurrentIndex];
    }
    private void Start()
    {
        if (ListOfItems.Count != 0)
            CurrentIndex = 0;
    }
}
