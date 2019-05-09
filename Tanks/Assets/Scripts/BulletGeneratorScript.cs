using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGeneratorScript : MonoBehaviour
{
    public GameObject obj;
    public GameObject bulletGenerator;
    
    public void Spawn()
    {
        GameObject tmp = Instantiate(obj, bulletGenerator.transform.position, bulletGenerator.transform.rotation);
        Rigidbody bullet = tmp.GetComponent<Rigidbody>();
        bullet.velocity = bulletGenerator.transform.forward * 50;
        Rigidbody tank = bulletGenerator.GetComponentInParent<Rigidbody>();
        tank.velocity += bulletGenerator.transform.forward * -5;


        Debug.Log("Spawn");
    }
}
