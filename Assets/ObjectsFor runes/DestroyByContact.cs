using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DestroyByContact : NetworkBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public int power = 10;
    public int dmgType = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(!(this.gameObject.GetComponent<Owner>().ownerPlayer == other.GetComponent<NetworkIdentity>().netId))
            {
                if (hasAuthority)
                {
                    Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                    Instantiate(explosion, transform.position, transform.rotation);
                    other.GetComponent<Player>().addDmg(power, dmgType);
                    Destroy(this.gameObject);
                }
            }

        }else if (other.tag == "Monster")
        {
            other.GetComponent<Monster>().addDmg(power, dmgType, this.GetComponent<Owner>().ownerPlayer);
            Instantiate(explosion, transform.position, transform.rotation);
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
