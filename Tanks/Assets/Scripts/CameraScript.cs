using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform tank;
    Vector3 offset;

    // Update is called once per frame
    private void Start()
    {
        transform.position = new Vector3(tank.position.x, tank.position.y + 2, tank.position.z - 3);
        transform.rotation = new Quaternion(tank.rotation.x, tank.rotation.y, tank.rotation.z, tank.rotation.w);
        offset = tank.transform.position - transform.position;

    }
    void Update()
    {
        float angle = tank.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        transform.position = tank.transform.position - (rotation * offset);
        Vector3 targetPosition = new Vector3(tank.position.x, transform.position.y, tank.position.z);
        transform.LookAt(targetPosition);
    }
}
