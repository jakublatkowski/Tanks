using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class TankController : MonoBehaviour
{
    #region Variables
    public Transform bulletGenerator;

    public GameObject bulletPrefab;

    public float rotationSpeed = 1.0f;

    public float accelerationForce = 1.0f;

    public float shotForce = 10f;

    private float _leftForce;
    private float _rightForce;
    private Rigidbody tankRb;
    #endregion

    #region Properties
    public float LeftForce
    {
        set
        {
            if (value > 1) _leftForce = 1;
            else if (value < 0) _leftForce = 0;
            else
            {
                _leftForce = value - .5f;
            }
        }
    }

    public float RightForce
    {
        set
        {
            if (value > 1) _rightForce = 1;
            else if (value < 0) _rightForce = 0;
            else
            {
                _rightForce = value - .5f;
            }
        }
    }
    #endregion

    void Start()
    {
        tankRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position;
        Vector3 rotation = new Vector3(0, (_leftForce - _rightForce) * rotationSpeed * Time.deltaTime);

        if (_leftForce > 0.1 && _rightForce > 0.1 || _leftForce < -0.1 && _rightForce < -0.1)
        {
            position = gameObject.transform.forward * (_leftForce + _rightForce);
        }
        else
        {
            position = new Vector3();
        }

        gameObject.transform.Rotate(rotation, Space.Self);
        tankRb.AddForce(position * accelerationForce * Time.deltaTime, ForceMode.VelocityChange);
    }

    public void Shot()
    {
        //tworzenie pocisku
        GameObject bullet = Instantiate(bulletPrefab, bulletGenerator.position, bulletGenerator.rotation);
        Destroy(bullet, 5f); //jeżeli pocisk w nic nie trafi zniknie po 5 sekundach

        bullet.GetComponent<Rigidbody>().AddForce(shotForce * bulletGenerator.forward, ForceMode.Impulse);
        tankRb.AddForce(-shotForce * bulletGenerator.forward, ForceMode.Impulse);
    }
}

