using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneStats : MonoBehaviour
{
    public int earth;
    public int water;
    public int fire;
    public int wind;
    public int ice;
    public int energy;
    public int death;
    public int holy;

    public int speedUp;
    public int HealtRegeneration;

    public int[] restoreList = new int[8];
    public int maxValue = 1000;


    public override string ToString()
    {
        return "Earth: " + earth.ToString() + "\n" +
            "Water: " + water.ToString() + "\n" +
            "fire: " + fire.ToString() + "\n" +
            "Wind: " + wind.ToString() + "\n" +
            "Ice: " + ice.ToString() + "\n" +
            "Enegry: " + energy.ToString() + "\n" +
            "Death: " + death.ToString() + "\n" +
            "Holy: " + holy.ToString();
    }

    public int suckEnergy(int whith, float procent)
    {
        int count = 0;
        switch (whith)
        {
            case 0:
                count = (int)(earth * procent);
                earth -= count;
                break;
            case 1:
                count = (int)(water * procent);
                water -= count;
                break;
            case 2:
                count = (int)(fire * procent);
                fire -= count;
                break;
            case 3:
                count = (int)(wind * procent);
                wind -= (int)(wind * procent);
                break;
            case 4:
                count = (int)(ice * procent);
                ice -= (int)(ice * procent);
                break;

            case 5:
                count = (int)(energy * procent);
                energy -= (int)(energy * procent);
                break;
            case 6:
                count = (int)(death * procent);
                death -= count;
                break;
            case 7:
                count = (int)(holy * procent);
                holy -= count;
                break;

        }
        return count;
    }

    public void RestoreEnergy()
    {
        if(earth<maxValue)
            earth += restoreList[0];
        if (water < maxValue)
            water += restoreList[1];
        if (fire < maxValue)
            fire += restoreList[2];
        if (wind < maxValue)
            wind += restoreList[3];
        if (ice < maxValue)
            ice += restoreList[4];
        if (energy < maxValue)
            energy += restoreList[5];
        if (death < maxValue)
            death += restoreList[6];
        if (holy < maxValue)
            holy += restoreList[7];
    }

    private float nextActionTime = 0.0f;
    public float period = 0.1f;

    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime = (Time.time +period);
            RestoreEnergy();
        }
    }
}

