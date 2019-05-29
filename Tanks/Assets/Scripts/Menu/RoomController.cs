﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject lobbyPanel;
    [SerializeField]
    private GameObject roomPanel;

    [SerializeField]
    private GameObject startButton;

    [SerializeField]
    private Transform playersContainer;
    [SerializeField]
    private GameObject playerListingPrefab;

    [SerializeField]
    private Text roomName;

    void ClearPlayerListings()
    {
        for (int i = playersContainer.childCount -1; i>=0; i--)
        {
            Destroy(playersContainer.GetChild(i).gameObject);
        }
    }
    void ListPlayers()
    {
        Debug.Log("Room: ");
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject tmpListing = Instantiate(playerListingPrefab, playersContainer);
            Text tmpText = tmpListing.transform.GetChild(0).GetComponent<Text>();
            tmpText.text = player.NickName;
            Debug.Log("Gracz: "+ player.NickName);
        }
    }
    public override void OnJoinedRoom()
    {
        PlayerPrefs.SetString("Type", "Klient");
        PlayerPrefs.SetString("Name", PhotonNetwork.NickName);
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
        ClearPlayerListings();
        ListPlayers();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ClearPlayerListings();
        ListPlayers();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ClearPlayerListings();
        ListPlayers();
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
    }
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerPrefs.SetString("Type", "Serwer");
            PhotonNetwork.CurrentRoom.IsOpen = true; // tylko dla debuggingu
            //PhotonNetwork.CurrentRoom.IsOpen = false; 
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

    IEnumerator rejoinLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }

    public void BackOnClick()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());
    }
}
