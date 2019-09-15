using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float hitPoints;

    public TankController Owner { get; set; }

    void OnCollisionEnter(Collision collision)
    {
        var hitTank = collision.gameObject.GetComponent<TankController>();
        Debug.Log($"Collision: {collision.gameObject.GetPhotonView()?.Owner.NickName}  HP: {collision.gameObject.GetComponent<TankController>().healthPoints}");
        Debug.Log($"Owner: {Owner.gameObject.GetPhotonView().Owner.NickName}  HP: {Owner.healthPoints}");

        if (hitTank == Owner) return;

        if (hitTank != null) hitTank.AddDamage(hitPoints, Owner);
        
        PhotonNetwork.Destroy(this.gameObject);
    }
}
