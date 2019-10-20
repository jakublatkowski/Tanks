using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class StartGameScript : MonoBehaviour
{
    public GameObject tank;
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    // Update is called once per frame
    private void CreatePlayer()
    {
        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        var index = Random.Range(0, spawnPoints.Length);
        var spawnPoint = spawnPoints[index];
        tank = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "SuperTank"), spawnPoint.transform.position, spawnPoint.transform.rotation);
        SetTankColor(tank, new Color(0.25f, 0.0f, 0.25f));
    }

    private void SetTankColor(GameObject tank, Color color)
    {
        Component[] tank_parts = tank.GetComponentsInChildren<Component>(true);
        foreach (var mat in from Component part in tank_parts
                            let mats = part.GetComponent<Renderer>().materials
                            from Material mat in mats
                            where mat.name.Contains("Primary")
                            select mat)
        {
            mat.SetColor("_Color", color);
        }
    }
}