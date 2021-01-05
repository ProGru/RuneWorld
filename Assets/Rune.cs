using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Rune", menuName = "Runes")]
public class Rune : ScriptableObject {

    public GameObject objectToCreate;
    public bool remove;
    public bool placeObject;
    public bool canBeStack;
    public bool canUseOnMonster;
    public int maxStack;

}
