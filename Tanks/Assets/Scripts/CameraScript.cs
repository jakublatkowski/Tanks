using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject tank;
    Vector3 offset;

    // Update is called once per frame
    private void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.GetPhotonView().IsMine)
            {
                tank = player;
                break;
            }
        }
        SetUpCameraPosition(tank.transform);
    }

    void LateUpdate()
    {
        var tankControllerTransform = tank.transform;

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
        TankController tankController = tank.GetComponent<TankController>();
        transform.position = new Vector3
        {
            x = tankTransform.position.x,
            y = tankTransform.position.y + tankController.cameraDistance * tankController.cameraPositionYRatio,
            z = tankTransform.position.z - tankController.cameraDistance * tankController.cameraPositionZRatio
        };

        offset = tankTransform.transform.position - transform.position;
    }
}
