using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
public class BaseClass{
    public string thename;

    public float baseHP;
    public float currHP;

    public float baseMP;
    public float currMP;

    public float baseATK;
    public float currATK;

    public float baseDEF;
    public float currDEF;

    public List<BaseAttack> MeleeAttacks = new List<BaseAttack>();
    public Vector2 GUIPosition;
    
}
