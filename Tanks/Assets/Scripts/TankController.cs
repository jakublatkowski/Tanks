using Photon.Pun;
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


    [Header("Control Properties")]
    public float rotationSpeed = 1.0f;
    public float accelerationForce = 1.0f;
    public float shotForce = 10f;

    [Header("Gravity Settings")]
    public float gravityForce = 10f;
    public Vector3 centerOfMassOffset = new Vector3(0,-0.5f,0);

    [Header("Camera Properties")]
    public float cameraDistance = 10f;
    [Range(0f, 1f)]
    public float cameraPositionYRatio = .5f;
    [Range(0f, 1f)]
    public float cameraPositionZRatio = .5f;

    [Header("Health Properties")]
    public float healthPoints = 100;

    [Header("Special Properties")]
    public int maxSpecialBulletCount = 50;
    public float maxTimeSpecialActive = 50;

    // activators
    private bool _isShootingActive;
    private bool _isSpecialActive;
    private bool _isGravityActive;

    // timestamps
    private float _timeToActivateShooting;
    private float timeSpecialActivated;

    // forces
    private float _leftForce;
    private float _rightForce;

    private Rigidbody tankRb;
    private Vector3 baseCenterOfMass;
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

    #region UnityMethodsOverride
    void Start()
    {
        tankRb = GetComponent<Rigidbody>();
        baseCenterOfMass = tankRb.centerOfMass;
        tankRb.centerOfMass += centerOfMassOffset;

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
        if (_isGravityActive) Moving();

        if (IsBarrelRaising)
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
    void FixedUpdate()
    {
        // Calculate better gravity, when we don't touch ground.
        if (!_isGravityActive)
        {
            // Disable Unitys gravity
            tankRb.useGravity = false;
            // And enable Better Gravity
            tankRb.AddForce(Physics.gravity * gravityForce * gravityForce);
        }
        else
        {
            tankRb.useGravity = true;
        }

        tankRb.centerOfMass = baseCenterOfMass + centerOfMassOffset;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Special"))
        {
            Destroy(collision.gameObject);

            ui.SetSpecialBarValue(1f);

            _isSpecialActive = true;
            timeSpecialActivated = Time.time;
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Map"))
        {
            _isGravityActive = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Map"))
        {
            _isGravityActive = false;
        }
    }
    #endregion

    private void Moving()
    {
        if (!tankRb.GetComponent<PhotonView>().IsMine) return;
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
        var force = position * accelerationForce * Time.deltaTime;

        tankRb.AddForce(force, ForceMode.VelocityChange);
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
            GameObject bullet = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Bullet"), bulletGenerator.position, bulletGenerator.rotation);
            Destroy(bullet, 5f); //jeżeli pocisk w nic nie trafi zniknie po 5 sekundach

            bullet.GetComponent<Bullet>().Owner = this;
            bullet.GetComponent<Rigidbody>().AddForce(shotForce * bulletGenerator.forward, ForceMode.Impulse);
            tankRb.AddForce(-shotForce * bulletGenerator.forward, ForceMode.Impulse);

            yield return new WaitForSeconds(.1f);
        }
    }

    public void AddDamage(float value)
    {
        healthPoints -= value;

        ui.SetHealthBarValue(healthPoints / 100f);

        if (healthPoints <= 0)
        {
            //do sth
            Debug.Log("You Are Dead Man!");
        }
    }
}
