using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special : MonoBehaviour
{
    public float movingSpeed = 1.0f;
    public float offset = 1.0f;
    public float rotatingSpeed = 1.0f;

    private float _baseYPosition;
    private float _currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _baseYPosition = gameObject.transform.position.y;
        _currentSpeed = movingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y >= _baseYPosition + offset)
        {
            _currentSpeed = -movingSpeed;
        }

        if (gameObject.transform.position.y <= _baseYPosition - offset)
        {
            _currentSpeed = movingSpeed;
        }

        var translation = new Vector3(0f, (_currentSpeed * Time.deltaTime), 0f);
        gameObject.transform.Translate(translation, Space.Self);

        var eulers = new Vector3(0f, rotatingSpeed * Time.deltaTime, 0f);
        gameObject.transform.Rotate(eulers, Space.Self);
    }
}
