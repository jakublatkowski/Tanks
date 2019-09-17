﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float hitPoints;
    [SerializeField]
    public AudioClip explosionSoundEffect;

    public float HitPoints { get { return hitPoints; } }
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collison");
        //Find player that shooted bullet
        var players = GameObject.FindGameObjectsWithTag("Player");
        TankController owner = null;
        foreach (var player in players)
        {
            if (player.GetPhotonView().Owner == this.GetComponentInParent<PhotonView>().Owner)
            {
                //if collison happened with another player then add damage
                if (collision.gameObject.tag.Equals("Player"))
                    collision.gameObject.GetPhotonView().RPC("AddDamage", RpcTarget.All, hitPoints, player.GetPhotonView().Owner);

                //destroy bullet
                owner = player.GetComponent<TankController>();
                break;
            }
        }
        Debug.Log("make explotion");
        if (collision.gameObject.tag.Equals("Player"))
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "SmallExplosion"), this.transform.position, this.transform.rotation);
        else
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "TinyExplosion"), this.transform.position, this.transform.rotation);
        AudioSource.PlayClipAtPoint(explosionSoundEffect, this.transform.position);

        Debug.Log("destroy bullet");
        owner.DestroyMyBullet(this.gameObject);
    }
}
