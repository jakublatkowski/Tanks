using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankColorDropDownScript : MonoBehaviourPun
{
    [SerializeField]
    private GameObject LeftArrow;

    [SerializeField]
    private GameObject RightArrow;

    [SerializeField]
    private GameObject Label;
    
    private int _currentIndex;
    private int CurrentIndex
    {
        get { return _currentIndex; }
        set
        {
            int oldValue = _currentIndex;
            _currentIndex = value;
            OnCurrentIndexChanged(oldValue);
        }
    }
    public void Click(GameObject button)
    {
        if (button == RightArrow)
        {
            CurrentIndex++;
        }
        else if (button == LeftArrow)
        {
            CurrentIndex--;
        }
    }

    private void OnCurrentIndexChanged(int oldValue)
    {
        string currentItem = Label.GetComponent<Text>().text;
        if (currentItem != "")
        {
            ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
            for (int i = 0; i < 8; i++)
            {
                if (i == oldValue)
                    table.Add(i.ToString(), currentItem);
                else
                    table.Add(i.ToString(), PhotonNetwork.CurrentRoom.CustomProperties[i.ToString()].ToString());
            }
            PhotonNetwork.CurrentRoom.SetCustomProperties(table);
        }
        bool foundItem = false;
        while (!foundItem)
        {
            string item = PhotonNetwork.CurrentRoom.CustomProperties[CurrentIndex.ToString()]?.ToString();
            if (item != null && item != "")
            {
                foundItem = true;
                Label.GetComponent<Text>().text = item;

                ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
                for (int i = 0; i < 8; i++)
                {
                    if (i == CurrentIndex)
                        table.Add(i.ToString(), "");
                    else
                        table.Add(i.ToString(), PhotonNetwork.CurrentRoom.CustomProperties[i.ToString()].ToString());
                }
                PhotonNetwork.CurrentRoom.SetCustomProperties(table);
            }
            else
            {
                if (_currentIndex > 7) _currentIndex = 0;
                else if (_currentIndex < 0) _currentIndex = 7;
                else if ((oldValue - CurrentIndex) != 0) _currentIndex += (CurrentIndex - oldValue);
                else
                    _currentIndex++; 
            }
        }
    }
    private void Start()
    {
        CurrentIndex = 0;
    }
}
