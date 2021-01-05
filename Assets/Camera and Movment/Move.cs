using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent(typeof(Rigidbody))]
public class Move : NetworkBehaviour {

    Rigidbody rigidbody;
    public float speed = 10;
    public float jumpPower = 0.5f;
    public float timeBetwenJumps = 1;
    public bool groundJump = true;
    float jumpTime = -1;

    void Start () {
        rigidbody = GetComponent<Rigidbody>();
	}
	
	void Update () {

        if (isLocalPlayer)
        {
            move();

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }



    }

    void move()
    {
        float vertical = Input.GetAxis("Vertical") * speed;
        float horizontal = Input.GetAxis("Horizontal") * speed;

        vertical *= Time.deltaTime;
        horizontal *= Time.deltaTime;

        transform.Translate(horizontal, 0, vertical);
    }

    void Jump()
    {
        if (groundJump)
        {
            if (Physics.Raycast(transform.position, Vector3.down, 1f))
            {
                rigidbody.AddForce(Vector3.up * jumpPower * 100, ForceMode.Impulse);
            }
        }
        else
        {
            if (jumpTime < Time.time - timeBetwenJumps)
            {
                jumpTime = Time.time;
                rigidbody.AddForce(Vector3.up * jumpPower * 100, ForceMode.Impulse);
            }
        }

    }
}
