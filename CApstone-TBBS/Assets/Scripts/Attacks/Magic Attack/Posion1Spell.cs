using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posion1Spell : BaseAttack
{
    public Posion1Spell()
    {
        AttackName = "Posion";
        AttackDesc = "Basic Posion Spell which poisons enemy over time";
        Damage = 5f;
        ManaCost = 5f;
        ismagic = true;
    }
}
