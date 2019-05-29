using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    // Update is called once per frame
    private void CreatePlayer()
    {
        Debug.Log("Tworze czolg");
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Tank"), Vector3.zero, Quaternion.identity);
    }
}
