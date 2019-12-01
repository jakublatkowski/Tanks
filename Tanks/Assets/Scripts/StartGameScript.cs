using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{
    public GameObject tank;
    public GameObject specialPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        var canvas = GameObject.Find("Canvas");
        var computerCanvas = GameObject.Find("ComputerCanvas");

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            Destroy(computerCanvas.gameObject);
            CreatePlayer();
        }
        else
        {
            Destroy(canvas.gameObject);
            Camera.main.GetComponent<CameraScript>().SetUpComputerCamera();
        }

        CreateSpecials();
    }

    // Update is called once per frame
    private void CreatePlayer()
    {
        var players = PhotonNetwork.PlayerList.ToList();
        var playerIndex = players.IndexOf(players.Single(player => player.IsLocal == true));

        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        
        var spawnPoint = spawnPoints[playerIndex % spawnPoints.Length];
        
        tank = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "SuperTank"), spawnPoint.transform.position, spawnPoint.transform.rotation);
    }

    private void CreateSpecials()
    {
        var specialPoints = GameObject.FindGameObjectsWithTag("SpecialPoint");

        var i = 1;
        foreach (var specialPoint in specialPoints)
        {
            var createdSpecial = Instantiate(specialPrefab, specialPoint.transform.position, specialPoint.transform.rotation);
            createdSpecial.GetComponent<SphereCollider>().isTrigger = true;
            createdSpecial.name += i++;
        }
    }
}
