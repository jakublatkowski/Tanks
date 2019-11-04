using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float hitPoints;
    
    private Rigidbody m_Rigidbody;
    [SerializeField]
    private AudioClip explosionSoundEffect;

    public float HitPoints { get { return hitPoints; } }

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        var bulletsOwner = this.GetComponentInParent<PhotonView>().Owner;
        //Find player that shot bullet
        var attackingPlayer = players.Single(player => player.GetPhotonView().Owner == bulletsOwner);
        
        //if collison happened with another player then add damage
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetPhotonView().RPC(nameof(TankController.AddDamage), RpcTarget.All, hitPoints, attackingPlayer.GetPhotonView().Owner);
        }
        
        //Make Explosion
        if (collision.gameObject.tag.Equals("Player"))
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "SmallExplosion"), this.transform.position, this.transform.rotation);
        else
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "TinyExplosion"), this.transform.position, this.transform.rotation);

        //Play Sound effect
        this.gameObject.GetPhotonView().RPC(nameof(PlaySound), RpcTarget.All);

        //Destroy bullet
        attackingPlayer.GetComponent<TankController>().DestroyMyBullet(this.gameObject);
    }
    
    [PunRPC]
    private void PlaySound()
    {
        AudioSource.PlayClipAtPoint(explosionSoundEffect, this.transform.position);
    }
}
