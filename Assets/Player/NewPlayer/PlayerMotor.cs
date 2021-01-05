using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraRotation = Vector3.zero;
    private Vector3 trusterForce = Vector3.zero;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
    }

    public void LookAtObject(GameObject gameObject)
    {
        cam.transform.LookAt(gameObject.transform);
    }


    public void RotateCamera(Vector3 _cameraRotation)
    {
        cameraRotation = _cameraRotation;
    }

    public void ApplyTruster(Vector3 _trusterFroce)
    {
        trusterForce = _trusterFroce;
    }

    private void FixedUpdate()
    {
        PerformMovment();
        PerformRotation();
    }

    void PerformMovment()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if (trusterForce != Vector3.zero)
        {
            if (Physics.Raycast(transform.position, Vector3.down, 1f))
            {
                rb.AddForce(trusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
            }
        }
    }

    void PerformRotation()
    {


        if (cam != null)
        {
            cam.transform.Rotate(-cameraRotation);
        }
    }


}
