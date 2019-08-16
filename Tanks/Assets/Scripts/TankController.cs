using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;


public class TankController : MonoBehaviour
{
    #region Variables
    [Header("Required Components")]
    public Transform bulletGenerator;
    public Barrel barrel;
    public UIController ui;

    [Header("Prefabs")]
    public GameObject bulletPrefab;

    [Header("Control Properties")]
    public float rotationSpeed = 1.0f;
    public float accelerationForce = 1.0f;
    public float shotForce = 10f;

    [Header("Health Properties")]
    public float healthPoints = 100;

    [Header("Special Properties")]
    public int maxSpecialBulletCount = 50;
    public float maxTimeSpecialActive = 50;

    // activators
    private bool _isShootingActive;
    private bool _isSpecialActive;

    // timestamps
    private float _timeToActivateShooting;
    private float timeSpecialActivated;

    // forces
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

    public float HealthPoints => healthPoints;

    public bool IsBarrelRaising { get; set; }
    #endregion

    void Start()
    {
        tankRb = GetComponent<Rigidbody>();
        ui.SetHealthBarValue(1);
        ui.SetSpecialBarValue(0);
        IsBarrelRaising = false;
        _isShootingActive = true;
        _isSpecialActive = false;
        _timeToActivateShooting = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position;
        var rotation = new Vector3(0, (_leftForce - _rightForce) * rotationSpeed * Time.deltaTime);

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

        if(!_isShootingActive && _timeToActivateShooting <= Time.time)
        {
            _isShootingActive = true;
        }

        if (_isSpecialActive)
        {
            var specialBarValue = (Time.time - timeSpecialActivated) / maxTimeSpecialActive;
            ui.SetSpecialBarValue(1 - specialBarValue);

            if (specialBarValue >= 1) _isSpecialActive = false;
        }
    }

    public void Shot()
    {
        if (!_isShootingActive) return;

        StartCoroutine(SpawnAndStartBullet(_isSpecialActive ? maxSpecialBulletCount : 1));

        ui.PlayShootingDelayAnimation();
        _timeToActivateShooting = Time.time + 1;
        _isShootingActive = false;
    }

    private IEnumerator SpawnAndStartBullet(int bulletsToSpawn)
    {
        for (var i = 0; i < bulletsToSpawn; i++)
        {
            //tworzenie pocisku
            GameObject bullet = Instantiate(bulletPrefab, bulletGenerator.position, bulletGenerator.rotation);
            Destroy(bullet, 5f); //jeżeli pocisk w nic nie trafi zniknie po 5 sekundach

            bullet.GetComponent<Rigidbody>().AddForce(shotForce * bulletGenerator.forward, ForceMode.Impulse);
            tankRb.AddForce(-shotForce * bulletGenerator.forward, ForceMode.Impulse);

            yield return new WaitForSeconds(.1f);
        }
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

        if (collision.gameObject.tag.Equals("Special"))
        {
            Destroy(collision.gameObject);

            ui.SetSpecialBarValue(1f);

            _isSpecialActive = true;
            timeSpecialActivated = Time.time;
        }
    }
}
