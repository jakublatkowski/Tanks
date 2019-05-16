using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.IO;

public class StartGameScript : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject camera;
    [SerializeField]
    private GameObject uiClient;
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        Debug.Log("Gra: ");
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log("Gracz: " + player.NickName);
        }

        if (PlayerPrefs.GetString("Type") == "Klient" || SystemInfo.deviceType == DeviceType.Handheld)
        {
            CreatePlayer();
        }
        else
        {
            camera.SetActive(true);
            uiClient.SetActive(false);
        }
    }

    private void CreatePlayer()
    {
        Vector3 tmp = new Vector3(Random.Range(-1, 1),0,0 );
        tmp *= 20;
        Debug.Log("Tworze czołg");
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Tank"), tmp, Quaternion.identity);
    }
}
