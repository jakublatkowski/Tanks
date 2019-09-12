using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject tank;
    Vector3 offset;

    // Update is called once per frame
    private void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.GetPhotonView().IsMine)
            {
                tank = player;
                transform.position = new Vector3(tank.transform.position.x, tank.transform.position.y + 2, tank.transform.position.z - 3);
                transform.rotation = new Quaternion(tank.transform.rotation.x, tank.transform.rotation.y, tank.transform.rotation.z, tank.transform.rotation.w);
                offset = tank.transform.position - transform.position;
            }
        }
    }
    void LateUpdate()
    {
        float angle = tank.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = tank.transform.position - (rotation * offset);
        Vector3 targetPosition = new Vector3(tank.transform.position.x, transform.position.y, tank.transform.position.z);
        transform.LookAt(targetPosition);
    }
}
