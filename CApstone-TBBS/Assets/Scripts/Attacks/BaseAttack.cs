﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAttack: MonoBehaviour
{
    public string AttackName;
    public string AttackDesc;
    public float Damage;
    public float ManaCost; //if spell
    public bool ismagic;
    public string Type;
}
