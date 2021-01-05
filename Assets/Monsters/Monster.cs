using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Monster : NetworkBehaviour {

    public void addDmg(int dmg, int dmgType, NetworkInstanceId killer)
    {
        MonsterStats monsterStats = this.transform.GetComponent<MonsterStats>();
        monsterStats.HP -= dmg - (dmg * (monsterStats.DEF[dmgType]) / 100);
        if (monsterStats.HP <= 0)
        {

            Death(killer);
        }
    }

    //actions after monster death
    public void Death(NetworkInstanceId killer)
    {
        Destroy(this.gameObject);
        CmdAddExpToPlayer(killer);
    }

    [Client]
    public void CmdAddExpToPlayer(NetworkInstanceId killer)
    {
        GameObject obj = NetworkServer.FindLocalObject(killer);
        obj.GetComponent<Player>().AddEXP(this.transform.GetComponent<MonsterStats>().expPerKill);
    }

}
