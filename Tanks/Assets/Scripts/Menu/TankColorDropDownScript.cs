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

    [SerializeField]
    private List<string> ListOfItems = new List<string>();

    private int _currentIndex;
    private int CurrentIndex
    {
        get { return _currentIndex; }
        set
        {
            if (value > ListOfItems.Count)
            {
                value = 0;
            }
            if (value < 0)
            {
                value = ListOfItems.Count;
            }
            int oldValue = _currentIndex;
            _currentIndex = value;
            PlayerPrefs.SetString("Color", value.ToString());
            OnCurrentIndexChanged(oldValue);
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

    private void OnCurrentIndexChanged(int oldValue)
    {
        ListOfItems.Clear();
        for (int j = 0; j < 8; j++)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties[j.ToString()].ToString() != "")
                ListOfItems.Add(PhotonNetwork.CurrentRoom.CustomProperties[j.ToString()].ToString());
        }
        
        GetComponentInParent<PhotonView>().RPC(nameof(this.AddToList), RpcTarget.All, Label.GetComponent<Text>().text, oldValue);


        Label.GetComponent<Text>().text = ListOfItems[CurrentIndex];
        
        GetComponentInParent<PhotonView>().RPC(nameof(this.RemoveFromList), RpcTarget.All, CurrentIndex);
        
        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
        int i = 0;
        for (; i< ListOfItems.Count; i++)
        {
            table.Add(i.ToString(), ListOfItems[i]);
        }
        for (; i< 8; i++)
        {
            table.Add(i.ToString(), "");
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(table);
    }
    private void Start()
    {
        CurrentIndex = 0;
    }
    [PunRPC]
    private void RemoveFromList(int indeks)
    {
        string component = ListOfItems[indeks];
        ListOfItems.RemoveAt(indeks);
    }
    [PunRPC]
    private void AddToList(string component, int indeks)
    {
        if (component == "") return;
        ListOfItems.Insert(indeks, component);
    }
}
