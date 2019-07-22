using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public float raisingSpeed = 1;
    public float loweringSpeed = 3;
    public float maxAngle = 70;
    public float minAngle = 90;
    public Transform rotationPoint;

    private float currentRaisingSpeed;
    private float currentLoweringSpeed;

    void Start()
    {
        currentRaisingSpeed = raisingSpeed;
        currentLoweringSpeed = loweringSpeed;
    }

    public void Raise()
    {
        currentLoweringSpeed = loweringSpeed;
        currentRaisingSpeed += 0.02f * currentRaisingSpeed;

        transform.rotation.ToAngleAxis(out var angle, out var axis);
        if(angle >= maxAngle)
        {
            transform.RotateAround(rotationPoint.position, - transform.right, currentRaisingSpeed * Time.deltaTime);
        }
    }

    public void LowerDownToNormal()
    {
        currentRaisingSpeed = raisingSpeed;
        currentLoweringSpeed += 0.02f * currentLoweringSpeed;
      
        transform.rotation.ToAngleAxis(out var angle, out var axis);
        if (angle <= minAngle)
        {
            transform.RotateAround(rotationPoint.position, transform.right, currentLoweringSpeed * Time.deltaTime);
        }
    }
}
