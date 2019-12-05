using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : BaseAttack
{
    public Hammer()
    {
        AttackName = "Hammer Swing";
        AttackDesc = "A Hammer Swing with a large hammer, axe, or mallet.";
        ManaCost = 0;
        Damage = 15.0f;
        ismagic = false;
    }
}
