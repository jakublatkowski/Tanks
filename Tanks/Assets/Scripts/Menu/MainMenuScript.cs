using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject lobbyConnectButton;
    [SerializeField]
    private GameObject roomCreateButton;
    [SerializeField]
    private GameObject roomJoinButton;
    [SerializeField]
    private GameObject lobbyPanel;
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private InputField playerName;
    [SerializeField]
    private InputField gamePIN;
    [SerializeField]
    private GameObject errorText;

    private string PIN = "";

    public override void OnConnectedToMaster()
    {
        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
        if (SystemInfo.deviceType != DeviceType.Handheld)
            table.Add("isHandHeld", "false");
        else
            table.Add("isHandHeld", "true");
        PhotonNetwork.LocalPlayer.SetCustomProperties(table);


        PhotonNetwork.AutomaticallySyncScene = true;
        lobbyConnectButton.SetActive(true);
        PlayerPrefs.SetString("Type", "Klient");

        if (PlayerPrefs.HasKey("NickName"))
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
        }
        else
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 10000);
        }
        playerName.text = PhotonNetwork.NickName;
    }
    public void PlayerNameUpdate(string name)
    {
        PhotonNetwork.NickName = name;
        PlayerPrefs.SetString("NickName", name);
    }
    public void JoinLobbyOnClick()
    {
        mainPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        PhotonNetwork.JoinLobby();
    }
    public void OnPINChanged()
    {
        PIN = gamePIN.text;
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(PIN);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        StartCoroutine("FlashError");
        errorText.GetComponent<Text>().text = "There is no room with this PIN. Check if typed correctly!";
        Debug.Log(PhotonNetwork.CountOfRooms);
        Debug.Log(PhotonNetwork.CloudRegion);
        Debug.Log(message);
    }
    private System.Collections.IEnumerator FlashError()
    {
        errorText.SetActive(true);
        yield return new WaitForSeconds(5);
        errorText.SetActive(false);
    }
    public void CreateRoom()
    {
        int roomSize = 8;
        if (SystemInfo.deviceType != DeviceType.Handheld)
            roomSize++;
        string roomName = Random.Range(0, 99999).ToString();
        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable()
            { { "0", "Red" },
            { "1", "Blue" },
            { "2", "Green" },
            { "3", "Yellow" },
            { "4", "White" },
            { "5", "Black" },
            { "6", "Magenta" },
            { "7", "Purple" },
            { "Mode", "Deathmatch" },
            { "Time", "1 min" } };
        RoomOptions roomOpt = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize, CustomRoomProperties = table };
        PhotonNetwork.CreateRoom(roomName, roomOpt);
    }
    public void MatchingCancel()
    {
        mainPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        PhotonNetwork.LeaveLobby();
    }
}