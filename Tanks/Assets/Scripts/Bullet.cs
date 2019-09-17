using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float hitPoints;
    
    public float HitPoints => hitPoints;

    void OnCollisionEnter(Collision collision)
    {
        //Find player that shooted bullet
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.GetPhotonView().Owner == this.GetComponentInParent<PhotonView>().Owner)
            {
                //if collison happened with another player then add damage
                if (collision.gameObject.tag.Equals("Player"))
                    collision.gameObject.GetPhotonView().RPC("AddDamage", RpcTarget.All, hitPoints, player.GetPhotonView().Owner);

                //destroy bullet
                player.GetComponent<TankController>().DestroyMyBullet(this.gameObject);
                break;
            }
        }
    }
}
