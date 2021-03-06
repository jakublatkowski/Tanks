﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Vector3 offset;
    private bool computer = false;

    public GameObject WatchedTank { get; set; }

    // Update is called once per frame
    private void Start()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var player in players)
        {
            if (player.GetPhotonView().IsMine)
            {
                WatchedTank = player;
                break;
            }
        }

        SetUpCameraPosition(WatchedTank.transform);
    }

    void LateUpdate()
    {
        if (computer)
        {
            transform.LookAt(new Vector3(0, 0));
            transform.RotateAround(Vector3.zero, Vector3.up, 10 * Time.deltaTime);
        };

        if (WatchedTank == null) return;

        var tankControllerTransform = WatchedTank.transform;

        SetUpCameraPosition(tankControllerTransform);

        var angle = tankControllerTransform.transform.eulerAngles.y;

        var rotation = Quaternion.Euler(0, angle, 0);
        transform.position = tankControllerTransform.transform.position - (rotation * offset);

        var targetPosition = new Vector3
        {
            x = tankControllerTransform.position.x,
            y = transform.position.y,
            z = tankControllerTransform.position.z
        };

        transform.LookAt(targetPosition);
    }

    private void SetUpCameraPosition(Transform tankTransform)
    {
        TankController tankController = WatchedTank.GetComponent<TankController>();
        transform.position = new Vector3
        {
            x = tankTransform.position.x,
            y = tankTransform.position.y + tankController.cameraDistance * tankController.cameraPositionYRatio,
            z = tankTransform.position.z - tankController.cameraDistance * tankController.cameraPositionZRatio
        };

        offset = tankTransform.transform.position - transform.position;
    }

    public void SetUpComputerCamera()
    {
        computer = true;
        ////gameObject.transform.position = new Vector3(0,100,-8);
        ////gameObject.transform.rotation = new Quaternion(.7f,0,0,.7f);

        //var comera = gameObject.GetComponent<Camera>();
        ////comera.orthographic = true;
        ////comera.orthographicSize = 270;
    }
}
