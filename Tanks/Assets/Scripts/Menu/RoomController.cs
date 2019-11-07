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
    private GameObject gameMode;
    [SerializeField]
    private GameObject gameTime;

    [SerializeField]
    private Text roomName;

    [SerializeField]
    private List<GameObject> components;

    [SerializeField]
    private GameObject colorDropDown;

    void ClearPlayerListings()
    {
        for (int i = playersContainer.childCount -1; i>=0; i--)
        {
            Destroy(playersContainer.GetChild(i).gameObject);
        }
    } //done
    void ActivatePanels()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (GameObject comp in components)
                comp.SetActive(true);
        }
        else
        {
            foreach (GameObject comp in components)
                comp.SetActive(false);
        }
    } //done
    void ListPlayers()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject tmpListing = Instantiate(playerListingPrefab, playersContainer);
            Text tmpText = tmpListing.transform.GetChild(0).GetComponent<Text>();
            tmpText.text = player.NickName;
        }
    } //done
    public override void OnJoinedRoom()
    {
        colorDropDown.SetActive(true);
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        ActivatePanels();
        ClearPlayerListings();
        ListPlayers();
    } //done
    public void OnGameModeChanged()
    {
        string mode = gameMode.GetComponent<Dropdown>().options[gameMode.GetComponent<Dropdown>().value].text;

        PlayerPrefs.SetString("Mode", mode);
        PhotonNetwork.CurrentRoom.CustomProperties["Mode"] = mode;
    } //done
    public void OnGameTimeChanged()
    {
        PlayerPrefs.SetString("Time", gameTime.GetComponent<Dropdown>().options[gameTime.GetComponent<Dropdown>().value].text);
    } //done

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ClearPlayerListings();
        ListPlayers();
    } //done
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ClearPlayerListings();
        ListPlayers();
        ActivatePanels();
    } //done

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerPrefs.SetString("Type", "Serwer");
            PhotonNetwork.CurrentRoom.IsOpen = true; // tylko dla debuggingu
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

    IEnumerator rejoinLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    } //done
    public void BackOnClick()
    {
        colorDropDown.SetActive(false);
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());
    } //done
}
