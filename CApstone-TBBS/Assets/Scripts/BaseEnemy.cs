using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BaseEnemy
{
    public string name;

    public enum Type
    {
        GRASS,
        FIRE,
        WATER,
        ELECTRIC
    }

    public enum Rarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        SUPERRARE
    }

    public Type EnemyType;
    public Rarity rarity;

    public float baseHP;
    public float currHP;

    public float baseMP;
    public float currMP;

    public float baseATK;
    public float currATK;
    public float baseDEF;
    public float currDEF;
}
