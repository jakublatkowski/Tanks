using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class TankController : MonoBehaviour
{
    #region Variables
    public Transform bulletGenerator;
    public Barrel barrel;

    public GameObject bulletPrefab;

    public float rotationSpeed = 1.0f;
    public float barrelRotationSpeed = 1.0f;
<<<<<<< HEAD

    public float accelerationForce = 1.0f;

    public float shotForce = 10f;

    private float _leftForce;
    private float _rightForce;
    private Rigidbody tankRb;

    public float healthPoints = 100;
    public UIController ui;
=======

    public float accelerationForce = 1.0f;

    public float shotForce = 10f;

    private float _leftForce;
    private float _rightForce;
    private Rigidbody tankRb;

    public float healthPoints = 100;
    public UIController ui;

    private bool _isShootingActive;
    private float _timeToActivateShooting;
>>>>>>> parent of 3c47ef2... Add special shooting mechanic
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

    public float HealthPoints => healthPoints;

    public bool IsBarrelRaising { get; set; }
    #endregion

    void Start()
    {
        tankRb = GetComponent<Rigidbody>();
        ui.SetHealthBarValue(1);
        IsBarrelRaising = false;
<<<<<<< HEAD
=======
        _isShootingActive = true;
        _timeToActivateShooting = 0;
>>>>>>> parent of 3c47ef2... Add special shooting mechanic
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
       
        if(IsBarrelRaising)
        {
            barrel.Raise();
        }
        else
        {
            barrel.LowerDownToNormal();
        }
<<<<<<< HEAD
=======

        if(!_isShootingActive && _timeToActivateShooting <= Time.time)
        {
            _isShootingActive = true;
        }
>>>>>>> parent of 3c47ef2... Add special shooting mechanic
    }

    public void Shot()
    {
<<<<<<< HEAD
=======
        if (!_isShootingActive) return;

>>>>>>> parent of 3c47ef2... Add special shooting mechanic
        //tworzenie pocisku
        GameObject bullet = Instantiate(bulletPrefab, bulletGenerator.position, bulletGenerator.rotation);
        Destroy(bullet, 5f); //jeżeli pocisk w nic nie trafi zniknie po 5 sekundach

        bullet.GetComponent<Rigidbody>().AddForce(shotForce * bulletGenerator.forward, ForceMode.Impulse);
        tankRb.AddForce(-shotForce * bulletGenerator.forward, ForceMode.Impulse);
<<<<<<< HEAD
=======

        ui.PlayShootingDelayAnimation();
        _timeToActivateShooting = Time.time + 1;
        _isShootingActive = false;
>>>>>>> parent of 3c47ef2... Add special shooting mechanic
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            Destroy(collision.collider);

            var hitPoints = collision.gameObject.GetComponent<Bullet>().HitPoints;
            healthPoints -= hitPoints;

            ui.SetHealthBarValue(healthPoints / 100f);
          
            if(healthPoints <= 0)
            {
                //do sth 
                Debug.Log("You Are Dead Man!");
            }

            Destroy(collision.gameObject);
        }
    }
}
