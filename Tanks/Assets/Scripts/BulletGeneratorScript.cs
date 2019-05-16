using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BulletGeneratorScript : MonoBehaviour
{
    [SerializeField]
    public GameObject tank;

    public void Spawn()
    {
        Debug.Log("Probuje strzelic");
        if (tank.GetComponent<PhotonView>().IsMine == false)
        {
            Debug.Log("return przedwczesny");
            return;
        }
        GameObject bulletGenerator = tank.GetComponent("BulletGenerator").gameObject;
        GameObject tmp = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Bullet"), bulletGenerator.transform.position, bulletGenerator.transform.rotation);
        Rigidbody bullet = tmp.GetComponent<Rigidbody>();
        bullet.velocity = bulletGenerator.transform.forward * 50;
        tank.GetComponent<Rigidbody>().velocity = tank.GetComponent<Rigidbody>().velocity - bulletGenerator.transform.forward * 5;


        Debug.Log("Spawn");
    }
}
