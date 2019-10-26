using Photon.Pun;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Color = Assets.Scripts.Color;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float hitPoints;
    
    private Rigidbody m_Rigidbody;
    [SerializeField]
    private AudioClip explosionSoundEffect;

    private PointsController pointsController;

    public float HitPoints { get { return hitPoints; } }
    public int colorPoints = 5;
    
    private void Start()
    {
        pointsController = GameObject.Find(nameof(PointsController)).GetComponent<PointsController>();

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
            // Add points
            HandlePoints();

            collision.gameObject.GetPhotonView().RPC(nameof(TankController.AddDamage), RpcTarget.All, hitPoints, attackingPlayer.GetPhotonView().Owner);
        }
        
        //Make Explosion
        if (collision.gameObject.tag.Equals("Player"))
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "SmallExplosion"), this.transform.position, this.transform.rotation);
        else
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "TinyExplosion"), this.transform.position, this.transform.rotation);

        //Play Sound effect
        this.gameObject.GetPhotonView().RPC("PlaySound", RpcTarget.All);

        //Destroy bullet
        attackingPlayer.GetComponent<TankController>().DestroyMyBullet(this.gameObject);
    }

    private void HandlePoints()
    {
        var color = PlayerPrefs.GetString("Color");
        
        /* TODO HANDLE FRIENDLY FIRE
         * if(bulletsOwner.GetColor == collision Get Color)
         * {
         *      pointsController.AddPoints(color, -ColorPoints);
         * }
         * else
         */
        pointsController.gameObject.GetPhotonView()
            .RPC(nameof(PointsController.AddPoints), RpcTarget.All, color, colorPoints);
    }

    [PunRPC]
    private void PlaySound()
    {
        AudioSource.PlayClipAtPoint(explosionSoundEffect, this.transform.position);
    }
}
