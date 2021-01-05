using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ExplosionForce : MonoBehaviour {

    public float radius = 5.0F;
    public int power = 10;

    public GameObject particleSystem;

    void Start()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        particleSystem.GetComponent<ParticleSystem>().startSize = radius;
        Instantiate(particleSystem, transform.position, transform.rotation);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (hit.tag == "Player") {
                if (hit.gameObject.GetComponent<NetworkIdentity>().netId != this.GetComponent<Owner>().ownerPlayer)
                hit.GetComponent<Player>().addDmg(power ,1);
            }else if (hit.tag == "Monster")
            {
                hit.GetComponent<Monster>().addDmg(power, 1, this.GetComponent<Owner>().ownerPlayer);
            }

        }
    }
}
