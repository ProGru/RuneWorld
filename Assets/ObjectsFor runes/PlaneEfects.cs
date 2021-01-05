using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneEfects : MonoBehaviour {

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
            Instantiate(playerEfect, other.transform.position, transform.rotation, other.gameObject.transform);
        }
    }
}
