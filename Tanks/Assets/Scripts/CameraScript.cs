using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public TankController tankController;
    Vector3 offset;

    // Update is called once per frame
    private void Start()
    {
        SetUpCameraPosition(tankController.transform);
    }

    void LateUpdate()
    {
        var tankControllerTransform = tankController.transform;

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

    private void SetUpCameraPosition(Transform tankControllerTransform)
    {
        transform.position = new Vector3
        {
            x = tankControllerTransform.position.x,
            y = tankControllerTransform.position.y + tankController.cameraDistance * tankController.cameraPositionYRatio,
            z = tankControllerTransform.position.z - tankController.cameraDistance *  tankController.cameraPositionZRatio
        };

        offset = tankControllerTransform.transform.position - transform.position;
    }
}
