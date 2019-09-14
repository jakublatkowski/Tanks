using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float hitPoints;

    [SerializeField]
    private TankController owner;

    public float HitPoints { get { return hitPoints; } }
    public TankController Owner { get { return owner; } set { owner = value; } }

    public void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.GetPhotonView().IsMine)
            {
                owner = player.GetComponent<TankController>();
                break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "SmallExplosion"), this.transform.position, this.transform.rotation);
        }
        else
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "TinyExplosion"), this.transform.position, this.transform.rotation);
        }
        PhotonNetwork.Destroy(this.gameObject); 
    }
}
