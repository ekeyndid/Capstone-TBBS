using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cure1Spell : BaseAttack
{
    public Cure1Spell()
    {
        AttackName = "Cure";
        AttackDesc = "Basic Cure spell that heals allies";
        Damage = -20f;
        ManaCost = 4f;
        ismagic = true;
        Type = "Magic";
    }
}
