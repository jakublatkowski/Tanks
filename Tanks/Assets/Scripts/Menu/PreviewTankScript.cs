using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewTankScript : MonoBehaviour
{
    public static GameObject instance;
    private void Start()
    {
        instance = this.gameObject;
    }
    void Update()
    {
        instance.transform.Rotate(0, 1, 0);
    }
    public static void ChangeTankColor(Color color)
    {
        TankController.SetTankColor(instance, color);
    }
}
