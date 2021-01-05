using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CollisionEfect : NetworkBehaviour {

    public GameObject objectEfect;
    public GameObject playerEfect;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            Instantiate(objectEfect, transform.position, other.transform.rotation, other.gameObject.transform);
        }
        if (other.tag == "Player")
        {
            if (!(this.gameObject.GetComponent<Owner>().ownerPlayer == other.GetComponent<NetworkIdentity>().netId))
            {
                if (hasAuthority)
                {
                    Instantiate(objectEfect, transform.position, other.transform.rotation, other.gameObject.transform);
                }
            }
        }
    }

}
