using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseProtag: BaseClass
{

    public float stamina;
    public float intellect;
    public float dexterity;
    public float agility;

    public List<BaseAttack> MagicAttacks = new List<BaseAttack>();
}
