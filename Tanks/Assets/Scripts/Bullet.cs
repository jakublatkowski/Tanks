using Photon.Pun;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float hitPoints;

    public TankController Owner { get; set; }
    public float HitPoints => hitPoints;

    void OnCollisionEnter(Collision collision)
    {
        PhotonNetwork.Destroy(this.gameObject);
    }
}
