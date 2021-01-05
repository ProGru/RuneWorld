using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Player : NetworkBehaviour {
    RaycastHit hit;
    
    [SerializeField]
    Camera playerCamera;
    PlayerStats playerStats;

    private void OnEnable()
    {
        playerStats = transform.GetComponent<PlayerStats>();
    }



    // Update is called once per frame
    void Update () {
        if (hasAuthority)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                suckEnergy();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                checkEnergy();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                useRune(playerStats.rune1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                useRune(playerStats.rune2);
            }
        }
    }
    [Client]
    public void addDmg(int dmg, int dmgType)
    {
        playerStats.HP -= dmg - (dmg *(playerStats.DEF[dmgType])/100);
        if (playerStats.HP < 1)
        {
            Debug.Log("Dead");
            Death();
        }
    }
    [Client]
    void suckEnergy()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10))
        {
            PlaneStats hiten = hit.transform.GetComponentInChildren<PlaneStats>();
            for (int i=0; i < playerStats.howMuchEnergySuck.Length;i++)
            {
                if (playerStats.howMuchEnergySuck[i] != 0)
                {
                    if ((hiten.suckEnergy(i, playerStats.howMuchEnergySuck[i]) + playerStats.energyList[i]) < playerStats.maxEnergyList[i])
                    {
                        playerStats.energyList[i] += hiten.suckEnergy(i, playerStats.howMuchEnergySuck[i]);
                    }else
                    {
                        playerStats.energyList[i] = playerStats.maxEnergyList[i];
                    }
                }
            }
        }
    }
    [Client]
    void checkEnergy()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10))
        {
            Debug.Log(hit.transform.GetComponentInChildren<PlaneStats>().ToString());
        }
    }
    [Client]
    float disctance(Vector3 firstPos, Vector3 second)
    {
        Vector3 secondPos = second;
        float dx = firstPos.x - secondPos.x, dy = firstPos.y - secondPos.y, dz = firstPos.z - secondPos.z;
        return Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
    }
    [Client]
    void useRune(Rune rune)
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (disctance(hit.transform.position, this.transform.position) < playerStats.playerRange)
            {

                if (rune.remove)
                {
                    remove(hit);
                }else if (rune.placeObject)
                {
                    placeObject(hit, rune);
                }
                

            }
        }
    }
    [Client]
    public void remove(RaycastHit hiten)
    {
        if (hiten.transform.tag == "Spell")
        {
            CmdRemove(hit.transform.GetComponent<NetworkIdentity>().netId);
        }else if (hiten.transform.tag == "Monster")
        {
            Debug.Log("Can't Remove Monster");
        }
        else
        {
            CmdRemoveComponentInChild(hit.transform.GetComponent<NetworkIdentity>().netId);
        }
    }

    [Command]
    public void CmdRemoveComponentInChild(NetworkInstanceId ID)
    {
        GameObject obj = NetworkServer.FindLocalObject(ID);
        LifeTime lifeObject = obj.transform.GetComponentInChildren<LifeTime>();
        if (lifeObject != null)
        {
            CmdRemove(lifeObject.GetComponent<NetworkIdentity>().netId);
        }

    }

    [Command]
    public void CmdRemove(NetworkInstanceId ID)
    {
        GameObject obj = NetworkServer.FindLocalObject(ID);
        Destroy(obj);
        NetworkServer.Destroy(obj);

    }

    [Client]
    public void placeObject(RaycastHit hiten, Rune rune)
    {
        if (hiten.transform.tag == "Map" || hiten.transform.tag == "Spell")
        {
            int prefabIndex = NetworkManager.singleton.spawnPrefabs.IndexOf(rune.objectToCreate);
            if (rune.canBeStack)
            {
                if (rune.canUseOnMonster)
                {
                    CmdCreateStackObject(hiten.transform.GetComponent<NetworkIdentity>().netId, rune.maxStack, prefabIndex);
                }
                else
                {
                    CmdCreateStackOnMonster(hiten.transform.GetComponent<NetworkIdentity>().netId, rune.maxStack, prefabIndex);
                }
            }
            else
            {
                if (rune.canUseOnMonster)
                {
                    CmdCreateNonStackObject(hiten.transform.GetComponent<NetworkIdentity>().netId, prefabIndex);
                }
                else
                {
                    CmdCreateOnMonster(hiten.transform.GetComponent<NetworkIdentity>().netId, prefabIndex);
                }

            }
        }
    }

    [Client]
    public void createObject(Rune rune, RaycastHit hiten)
    {
        PlaneStats hitenObject = hiten.transform.GetComponentInParent<PlaneStats>();
        int prefabIndex = NetworkManager.singleton.spawnPrefabs.IndexOf(rune.objectToCreate);
        CmdInstantinate(prefabIndex, hitenObject.GetComponent<NetworkIdentity>().netId);
    }

    [Command]
    public void CmdCreateStackObject(NetworkInstanceId hitenObjectID,int maxStack, int objectTospawn)
    {
        GameObject obj = NetworkServer.FindLocalObject(hitenObjectID);

        PlaneStats hitenObject = obj.transform.GetComponentInParent<PlaneStats>();
        LifeTime[] life = hitenObject.transform.GetComponentsInChildren<LifeTime>();
        if (life.Length < maxStack)
        {
            CmdInstantinate(objectTospawn, hitenObject.GetComponent<NetworkIdentity>().netId);
        }
    }

    [Command]
    public void CmdCreateNonStackObject(NetworkInstanceId hitenObjectID, int objectToSpawn)
    {
        GameObject obj = NetworkServer.FindLocalObject(hitenObjectID);

        LifeTime life = obj.transform.GetComponentInChildren<LifeTime>();
        if (life == null)
        {
            CmdInstantinate(objectToSpawn, hitenObjectID);
        }
    }

    [Command]
    public void CmdCreateOnMonster(NetworkInstanceId hitenObjectID, int objectToSpawn)
    {
        GameObject obj = NetworkServer.FindLocalObject(hitenObjectID);

        Monster monster = obj.transform.GetComponentInChildren<Monster>();
        LifeTime life = obj.transform.GetComponentInChildren<LifeTime>();
        if (life == null && monster == null)
        {
            CmdInstantinate(objectToSpawn, hitenObjectID);
        }

    }

    [Command]
    public void CmdCreateStackOnMonster(NetworkInstanceId hitenObjectID, int maxStack, int objectTospawn)
    {
        GameObject obj = NetworkServer.FindLocalObject(hitenObjectID);

        PlaneStats hitenObject = obj.transform.GetComponentInParent<PlaneStats>();
        LifeTime[] life = hitenObject.transform.GetComponentsInChildren<LifeTime>();
        Monster monster = obj.transform.GetComponentInChildren<Monster>();

        if (life.Length < maxStack && monster == null)
        {
            CmdInstantinate(objectTospawn, hitenObject.GetComponent<NetworkIdentity>().netId);
        }

    }

    [Command]
    public void CmdInstantinate(int ob, NetworkInstanceId ID)
    {
        GameObject createdObject;
        GameObject obj = NetworkServer.FindLocalObject(ID);

        if (obj != null)
        {
            GameObject prefabToSpawn = NetworkManager.singleton.spawnPrefabs[ob];
            createdObject = Instantiate(prefabToSpawn, obj.transform.position + prefabToSpawn.transform.position, new Quaternion(0, 0, 0, 0), obj.transform);
            createdObject.GetComponent<Owner>().ownerPlayer = this.gameObject.GetComponent<NetworkIdentity>().netId;
            NetworkServer.Spawn(createdObject);
        }
        
    }

    //Actions after death.
    public void Death()
    {
        Destroy(this.gameObject);
    }

    public void AddEXP(int amond)
    {
        playerStats.PlayerExp += amond;
    }
}
