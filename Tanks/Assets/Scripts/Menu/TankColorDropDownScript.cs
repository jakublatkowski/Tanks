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
    
    private int direction = 1;
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
            direction = 1;
        }
        else if (button == LeftArrow)
        {
            CurrentIndex--;
            direction = -1;
        }
    }

    private void OnCurrentIndexChanged(int oldValue)
    {
        string mode = PhotonNetwork.CurrentRoom.CustomProperties["Mode"].ToString();
        if (mode == "Deathmatch")
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
        }

        while (true)
        {
            string item = PhotonNetwork.CurrentRoom.CustomProperties[CurrentIndex.ToString()]?.ToString();
            if (item != null && item != "")
            {
                Label.GetComponent<Text>().text = item;
                PlayerPrefs.SetString("Color", item);

                //xxxxxxx
                PreviewTankScript.ChangeTankColor(GetColorFromName(item));

                if (mode == "Deathmatch")
                {
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
                break;
            }
            else
            {
                if (_currentIndex > 7) _currentIndex = 0;
                else if (_currentIndex < 0)
                {
                    if (mode == "TeamDeathmatch")
                        _currentIndex = 1;
                    else if (mode == "Deathmatch")
                        _currentIndex = 7;
                }
                else
                    _currentIndex += direction; 
            }
        }
    }
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        Label.GetComponent<Text>().text = "";
        CurrentIndex = 0;
    }

    [PunRPC]
    public void ChangedModeChangeColor(int indeks)
    {
        CurrentIndex = indeks;
    }

    public static Color GetColorFromName(string name)
    {
        switch (name)
        {
            case "Red":
                return new Color(0.8f, 0.0f, 0.0f);
            case "Blue":
                return new Color(0.0f, 0.0f, 0.8f);
            case "Green":
                return new Color(0.0f, 0.8f, 0.0f);
            case "Yellow":
                return new Color(0.8f, 0.8f, 0.0f);
            case "White":
                return new Color(0.8f, 0.8f, 0.8f);
            case "Black":
                return new Color(0.0f, 0.0f, 0.0f);
            case "Magenta":
                return new Color(0.8f, 0.0f, 0.8f);
            case "Purple":
                return new Color(0.0f, 0.8f, 0.8f);
            default:
                return new Color(0.0f, 0.0f, 0.0f);
        }
    }
}
