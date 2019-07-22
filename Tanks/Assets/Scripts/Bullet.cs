using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float hitPoints;

    [SerializeField]
    private TankController owner;

    public float HitPoints { get { return hitPoints; } }
    public TankController Owner { get { return owner; } set { owner = value; } }
}
