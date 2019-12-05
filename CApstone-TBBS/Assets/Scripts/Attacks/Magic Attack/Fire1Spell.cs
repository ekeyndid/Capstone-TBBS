using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire1Spell : BaseAttack
{
public Fire1Spell()
    {
        AttackName = "Fire";
        AttackDesc = "Basic Fire Spell which burns enemy";
        Damage = 20f;
        ManaCost = 10f;
        ismagic = true;
    }
}
