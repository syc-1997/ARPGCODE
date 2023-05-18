using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseThirdPersonCamera : MonoBehaviour
{
    public Transform target; 
    public GameObject PlayerHandle;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    public float distanceMin = .5f;
    public float distanceMax = 15f;
    private float x = 1.0f;
    private float y = 1.0f;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;
    private GameObject model;

    void Awake()
    {
        
        x = transform.eulerAngles.y;
        y = transform.eulerAngles.x;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;
        rotation = transform.rotation;
        model = PlayerHandle.GetComponent<ActorController>().model;
    }

    private void Start()
    {

    }

    void FixedUpdate()
    {
        if (target)
        {
            Vector3 tempModelEuler = model.transform.eulerAngles;
            Cursor.visible = false;
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;            
            y = ClampAngle(y, yMinLimit, yMaxLimit);            
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

            Quaternion rotationYAxis = Quaternion.Euler(0, x, 0);
            Quaternion rotationXAxis = Quaternion.Euler(y, 0, 0);

            desiredRotation = rotationYAxis * rotationXAxis;
            currentRotation = transform.rotation;

            rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * 5f);
            transform.rotation = rotation;
           
            position = target.position - (rotation * Vector3.forward * distance + new Vector3(0, -1f, 0));
            transform.position = position;
            PlayerHandle.transform.rotation = Quaternion.Euler(0, x, 0);
            model.transform.eulerAngles = tempModelEuler;
        }
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < 0)
            angle += 0;
        if (angle > 100)
            angle -= 100;
        return Mathf.Clamp(angle, min, max);
    }
}