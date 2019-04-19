using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public GameObject tank;

    public float rotationSpeed = 1.0f;

    public float accelerationForce = 1.0f;

    // Update is called once per frame
    void Update()
    {
        UIController ui = FindObjectOfType<Canvas>().GetComponent<UIController>();

        float leftForce = ui.GetLeftSrollBarValue() - 0.5f;
        float rightForce = ui.GetRightSrollBarValue() - 0.5f;

        Vector3 position = tank.transform.forward * (leftForce + rightForce);
        Vector3 rotation = new Vector3(0, (leftForce - rightForce) * rotationSpeed * Time.deltaTime);

        tank.transform.Rotate(rotation,Space.Self);
        tank.GetComponent<Rigidbody>().AddForce(position * accelerationForce * Time.deltaTime, ForceMode.Acceleration);

        if (Input.touches.Length == 0)
            ui.SetScrollbarsDefault();
    }
}
