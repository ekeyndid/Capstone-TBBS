using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice1Spell : BaseAttack
{
    public Ice1Spell()
    {
        AttackName = "Ice";
        AttackDesc = "Basic Ice Spell which freezes enemy";
        Damage = 15f;
        ManaCost = 2f;
        ismagic = true;
        Type = "Magic";
    }
}
