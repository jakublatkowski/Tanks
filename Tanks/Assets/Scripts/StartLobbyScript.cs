using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.IO;

public class StartLobbyScript : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        if (PlayerPrefs.GetString("Type") == "Klient")
        {
            CreatePlayer();
        }
        else
        {
            camera.SetActive(true);
        }
    }

    private void CreatePlayer()
    {
        Debug.Log("Tworze czołg");
        PhotonNetwork.Instantiate(Path.Combine("Prefabs/Multi", "Klient"), Vector3.zero, Quaternion.identity);
    }
}
