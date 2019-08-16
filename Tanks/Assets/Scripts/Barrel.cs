using UnityEngine;

public class Barrel : MonoBehaviour
{
    public float raisingSpeed = 1;
    public float loweringSpeed = 3;
    public float maxAngle = 70;
    public float minAngle = 0;
    public float angleOffset = 1;
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

        var angle = Quaternion.Angle(gameObject.transform.rotation, rotationPoint.transform.rotation);
        if(angle < maxAngle - angleOffset)
        {
            transform.RotateAround(rotationPoint.position, -transform.right, currentRaisingSpeed * Time.deltaTime);
        }
    }

    public void LowerDownToNormal()
    {
        currentRaisingSpeed = raisingSpeed;
        currentLoweringSpeed += 0.02f * currentLoweringSpeed;

        var angle = Quaternion.Angle(gameObject.transform.rotation, rotationPoint.transform.rotation);
        if (angle > minAngle + angleOffset) 
        {
            transform.RotateAround(rotationPoint.position, transform.right, currentLoweringSpeed * Time.deltaTime);
        }
    }
}
