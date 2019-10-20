using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewTankScript : MonoBehaviour
{
    void Update()
    {
        this.gameObject.transform.Rotate(0, 1, 0);
    }
}
