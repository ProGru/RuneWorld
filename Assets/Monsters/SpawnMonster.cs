using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnMonster : NetworkBehaviour {

    public GameObject monsterToSpawn;
    public int spawnTime = 100;
    GameObject created = null;

    private float nextActionTime = 0.0f;

    void Update()
    {
        if (created != null)
        {
            nextActionTime = (Time.time + spawnTime);
        }
        else
        {
            if (Time.time > nextActionTime)
            {
                nextActionTime = (Time.time + spawnTime);
                CmdSpawn();
            }
        }
    }

    [Command]
    void CmdSpawn()
    {
        GameObject create = Instantiate(monsterToSpawn, this.gameObject.transform.position + monsterToSpawn.transform.position, new Quaternion(0, 0, 0, 0), this.gameObject.transform);
        NetworkServer.Spawn(create);
    }
}

