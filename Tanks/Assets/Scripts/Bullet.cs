using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float hitPoints;

    public TankController Owner { get; set; }

    void OnCollisionEnter(Collision collision)
    {
        var hitTank = collision.gameObject.GetComponent<TankController>();

        if (hitTank != null && hitTank != Owner)
        {
            hitTank.AddDamage(hitPoints, Owner);
            Destroy(this.gameObject);
        }
    }
}
