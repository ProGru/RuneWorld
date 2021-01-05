using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3;
    [SerializeField]
    private float trusterForce = 1000f;

    private PlayerMotor motor;
    bool state = true;


    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

        motor.Move(_velocity);



        Vector3 _trusterForce = Vector3.zero;

        if (Input.GetButton("Jump"))
        {
            _trusterForce = Vector3.up * trusterForce;
        }

        motor.ApplyTruster(_trusterForce);

        if (Input.GetMouseButtonDown(1))
        {
            state = false;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            state = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (state)
        {
            motor.LookAtObject(this.gameObject);

        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            float _yRot = Input.GetAxisRaw("Mouse X");
            Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

            motor.Rotate(_rotation);

            float _xRot = Input.GetAxisRaw("Mouse Y");
            Vector3 _cameraRotation = new Vector3(_xRot, 0f, 0f) * lookSensitivity;

            motor.RotateCamera(_cameraRotation);
        }

    }
}
