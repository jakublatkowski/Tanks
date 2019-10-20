﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;


public class TankController : MonoBehaviour
{
    #region Variables
    [Header("Required Components")]
    public Transform bulletGenerator;
    public Barrel barrel;

    [Header("Control Properties")]
    public float rotationSpeed = 1.0f;
    public float accelerationForce = 1.0f;
    public float shotForce = 10f;
    public float maxSpeed = 20.0f;

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
    private UIController ui;

    #endregion

    #region Properties
    public float HealthPoints => healthPoints;

    public bool IsBarrelRaising { get; set; }
    #endregion

    #region UnityMethodsOverride

    void Start()
    {
        ui = GameObject.Find("Canvas").GetComponent<UIController>();

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
        _leftForce = ui.GetLeftJoystickVertical();
        _rightForce = ui.GetRightJoystickVertical();

        if (_isGravityActive) Moving();

        MoveBarrel();

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
        // Disable Unitys gravity
        tankRb.useGravity = false;
        // And enable Better Gravity
        tankRb.AddForce(Physics.gravity * gravityForce * gravityForce);

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

        //if (collision.gameObject.tag.Equals("Bullet"))
        //{
        //    var bullet = collision.gameObject.GetComponent<Bullet>();
        //    var attacker = collision.gameObject.GetPhotonView().Owner;
        //    AddDamage(bullet.HitPoints, attacker);
        //}
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

        var speed = tankRb.velocity.magnitude;

        if (speed < maxSpeed)
            tankRb.AddForce(force, ForceMode.VelocityChange);
    }

    private void MoveBarrel()
    {
        if (!gameObject.GetComponent<PhotonView>().IsMine) return;

        if (IsBarrelRaising) barrel.Raise();
        else barrel.LowerDown();
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
            bullet.GetComponent<Rigidbody>().AddForce(shotForce * bulletGenerator.forward, ForceMode.Impulse);
            tankRb.AddForce(-shotForce * bulletGenerator.forward, ForceMode.Impulse);

            yield return new WaitForSeconds(.1f);
        }
    }

    [PunRPC]
    public void AddDamage(float value, Player attacker)
    {
        if (!gameObject.GetPhotonView().IsMine)
        {
            return;
        }

        healthPoints -= value;
        ui.SetHealthBarValue(healthPoints / 100f);

        if (healthPoints <= 0)
        {
            StartCoroutine(GameController.instance.RespawnTank(this, attacker));
        }
    }
    public void DestroyMyBullet(GameObject bullet)
    {
        PhotonNetwork.Destroy(bullet);
    }
    public void ResetTank(float waitingTime)
    {
        if (gameObject.GetPhotonView().IsMine)
        {
            healthPoints = 100f;
            _isSpecialActive = false;
            timeSpecialActivated = Time.time;

            ui.SetHealthBarValue(1);
            ui.SetSpecialBarValue(0);

            var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            var index = Random.Range(0, spawnPoints.Length);

            gameObject.transform.position = spawnPoints[index].transform.position;
            gameObject.transform.rotation = spawnPoints[index].transform.rotation;
        }
    }
}
