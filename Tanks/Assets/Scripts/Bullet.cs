using System.Linq;
using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float hitPoints;
    
    public float HitPoints => hitPoints;

    void OnCollisionEnter(Collision collision)
    {
        //if collison happened with another player then add damage
        if (collision.gameObject.tag.Equals("Player"))
        {
            var bulletsOwner = this.GetComponentInParent<PhotonView>().Owner;

            //Find player that shot bullet
            var players = GameObject.FindGameObjectsWithTag("Player");
            var attackingPlayer = players.Single(player => player.GetPhotonView().Owner == bulletsOwner)
                .GetPhotonView().Owner;

            collision.gameObject.GetPhotonView()
                .RPC(nameof(TankController.AddDamage), RpcTarget.All, hitPoints, attackingPlayer);
        }

        //destroy bullet
        PhotonNetwork.Destroy(this.gameObject);
    }
}
