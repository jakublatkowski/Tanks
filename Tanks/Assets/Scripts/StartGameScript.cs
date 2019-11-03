using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{
    public GameObject tank;
    private TankColorDropDownScript tankColorDropDownScript;
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
        tankColorDropDownScript = FindObjectOfType<TankColorDropDownScript>();
    }

    // Update is called once per frame
    private void CreatePlayer()
    {
        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        var index = Random.Range(0, spawnPoints.Length);
        var spawnPoint = spawnPoints[index];

        tank = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "SuperTank"), spawnPoint.transform.position, spawnPoint.transform.rotation);
        TankController.SetTankColor(tank, new Color(1.0f, 1.0f, 1.0f));
    }
}
