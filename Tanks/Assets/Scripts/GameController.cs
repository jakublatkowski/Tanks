using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public static TankController tank;

    public float TankLeftForce
    {
        set => tank.LeftForce = value;
    }

    public float TankRightForce
    {
        set => tank.RightForce = value;
    }

    public bool TankRaiseBarrel
    {
        set => tank.IsBarrelRaising = value;
    }

    public void TankShot()
    {
        tank.Shot();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tank = GameObject.FindObjectOfType<TankController>();
    }
    void Update()
    {
        if (tank == null)
            tank = GameObject.FindObjectOfType<TankController>();
    }
}
