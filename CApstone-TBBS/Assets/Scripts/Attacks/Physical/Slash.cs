using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : BaseAttack
{
    public Slash()
    {
        AttackName = "Slash";
        AttackDesc = "A basic slash with a sword, dagger, or alike weapon.";
        ManaCost = 0.0f;
        Damage = 10.0f;
        ismagic = false;
    }
}
