using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posion1Spell : BaseAttack
{
    public Posion1Spell()
    {
        AttackName = "Poison";
        AttackDesc = "Basic Poison Spell which poisons enemy over time";
        Damage = 5f;
        ManaCost = 5f;
        ismagic = true;
        Type = "Magic";
    }
}
