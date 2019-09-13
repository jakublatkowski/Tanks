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
    private GameObject lobbyPanel;
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private InputField playerName;

    private string roomName;
    private int roomSize;

    [SerializeField]
    private List<RoomInfo> roomListings;
    [SerializeField]
    private Transform roomContainer;
    [SerializeField]
    private GameObject roomListingPrefab;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        lobbyConnectButton.SetActive(true);
        roomListings = new List<RoomInfo>();
        
        if (PlayerPrefs.HasKey("NickName"))
        {
            if (playerName.text == "")
            {
                PhotonNetwork.NickName = "Player " + Random.Range(0, 10000);
            }
            else
            {
                PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
            }
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
    public override void OnJoinedLobby()
    {
        roomCreateButton.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var item in roomListings)
        {
            roomListings.Remove(item);
        }
        for (int i = 0; i < roomContainer.childCount; i++)
        {
            Destroy(roomContainer.GetChild(i).gameObject);
        }

        foreach(RoomInfo room in roomList)
        {
            if (room.PlayerCount > 0)
            {
                roomListings.Add(room);
                ListRoom(room);
            }
        }
    }

    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }
    void ListRoom(RoomInfo room)
    {
        GameObject tmpListing = Instantiate(roomListingPrefab, roomContainer);
        RoomButton tmpButton = tmpListing.GetComponent<RoomButton>();
        tmpButton.SetRoom(room.Name, room.MaxPlayers, room.PlayerCount);
    }

    public void OnRoomNameChanged(string nameIn)
    {
        roomName = nameIn;
    }
    public void OnRoomSizeChanged(string sizeIn)
    {
        roomSize = int.Parse(sizeIn);
    }
    public void CreateRoom()
    {
        if (roomSize == 0)
            roomSize = 2;
        if (roomName == null)
            roomName = PhotonNetwork.NickName + "'s Room";
        RoomOptions roomOpt = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom(roomName, roomOpt);
    }
    public void MatchingCancel()
    {
        mainPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        PhotonNetwork.LeaveLobby();
    }
}