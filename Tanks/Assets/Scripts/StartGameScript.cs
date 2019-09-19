using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{
    public GameObject tank;
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
        SetArenaTeamColors(new Color(0.5f, 0.0f, 0.5f), new Color(0.0f, 0.5f, 0.5f));
    }

    // Update is called once per frame
    private void CreatePlayer()
    {
        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        var index = Random.Range(0, spawnPoints.Length);
        var spawnPoint = spawnPoints[index];
        var test = Path.Combine("Prefabs", "WOJSKOOOO");
        tank = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Tank"), spawnPoint.transform.position, spawnPoint.transform.rotation);

        Material[] keks = tank.GetComponent<Renderer>().materials;
        foreach(Material kek in keks)
        {
            kek.SetColor("_Color", new Color(1.0f, 0.0f, 0.0f));
        }
    }

    private void SetArenaTeamColors(Color redTeamColor, Color blueTeamColor)
    {
        Dictionary<string, int> intensityDict = new Dictionary<string, int>
        {
            { "tint", 5 },
            { "light", 4 },
            { "medium", 3 },
            { "hard", 2 },
            { "extreme", 1 },
            { "wall", 0 }
        };

        GameObject terrain = GameObject.Find("TDM_Battlefield");
        Material[] materials = terrain.GetComponent<Renderer>().materials;

        int maxIntensity = intensityDict.Count - 1;
        foreach (Material material in materials)
        {
            foreach (string key in intensityDict.Keys)
            {
                if (material.name.Contains(key))
                {
                    float multiplier = 1.0f / maxIntensity * intensityDict[key];
                    // przyjęta konwencja domyślnych kolorów red i blue
                    if (material.name.Contains("Red"))
                    {
                        material.SetColor("_Color", new Color(
                            redTeamColor.r + (1 - redTeamColor.r) * multiplier,
                            redTeamColor.g + (1 - redTeamColor.g) * multiplier,
                            redTeamColor.b + (1 - redTeamColor.b) * multiplier));
                    }
                    else
                    {
                        material.SetColor("_Color", new Color(
                            blueTeamColor.r + (1 - blueTeamColor.r) * multiplier,
                            blueTeamColor.g + (1 - blueTeamColor.g) * multiplier,
                            blueTeamColor.b + (1 - blueTeamColor.b) * multiplier));
                    }
                }
            }
        }
    }
}
