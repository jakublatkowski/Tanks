using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField]
    public GameObject tank;

    public float rotationSpeed = 1.0f;

    public float accelerationForce = 1.0f;


    UIController uiController;

    private void Start()
    {
        uiController = GetComponentInChildren<UIController>();
    }


    // Update is called once per frame
    void Update()
    {
        if (tank.GetComponent<PhotonView>().IsMine == true)
        {
            //nie mój czołg nie poruszam
            return;
        }

        float leftForce = uiController.GetLeftSrollBarValue() - 0.5f;
        float rightForce = uiController.GetRightSrollBarValue() - 0.5f;
        Debug.Log("Lewa : " + leftForce);
        Debug.Log("Prwa : " + rightForce);

        Vector3 position = tank.transform.forward * (leftForce + rightForce);
        Vector3 rotation = new Vector3(0, (leftForce - rightForce) * rotationSpeed * Time.deltaTime);

        tank.transform.Rotate(rotation,Space.Self);
        tank.GetComponent<Rigidbody>().AddForce(position * accelerationForce * Time.deltaTime, ForceMode.VelocityChange);
    }
}
