using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSword : Weapon
{
    private int damage;
    private float cooldown;
    public override void Init(EffectManager.EffectMethod hitEffect, ItemGrade grade)
    {
        base.Init(hitEffect, grade);
        var stat = GameData.GetWeaponStat(WeaponType.Longsword, grade);
        damage = stat.Item1;
        cooldown = stat.Item2;
    }
    public override bool Attack()
    {
        throw new System.NotImplementedException();
    }
}
