using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public int maxHP = 100;
    public int HP = 100;

    public int[] maxEnergyList = new int[8];
    public int[] energyList = new int[8];

    public int[] DEF = new int[8];

    public int playerRange = 5;

    public float[] howMuchEnergySuck = new float[8];

    public Rune rune1;
    public Rune rune2;

    public int PlayerExp = 0;
}
