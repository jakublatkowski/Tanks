using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{
    public GameObject tank;
    public GameObject specialPrefab;
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
        CreateSpecials();
    }

    // Update is called once per frame
    private void CreatePlayer()
    {
        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        var index = Random.Range(0, spawnPoints.Length);
        var spawnPoint = spawnPoints[index];

        tank = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "SuperTank"), spawnPoint.transform.position, spawnPoint.transform.rotation);
    }

    private void CreateSpecials()
    {
        var specialPoints = GameObject.FindGameObjectsWithTag("SpecialPoint");

        var i = 1;
        foreach (var specialPoint in specialPoints)
        {
            var createdSpecial = Instantiate(specialPrefab, specialPoint.transform.position, specialPoint.transform.rotation);
            createdSpecial.name += i++;
        }
    }
}
