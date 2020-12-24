using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Weapon
{
    private int ammo;
    private int damage;
    private float cooldown;
    private int range;
    private const float BulletSpeed = 40f;
    private const float Angular = 10f;
    public override void Init(EffectManager.EffectMethod hitEffect, ItemGrade grade)
    {
        base.Init(hitEffect, grade);
        var stat = GameData.GetWeaponStat(WeaponType.Shotgun, grade);
        damage = stat.Item1;
        cooldown = stat.Item2;
        range = stat.Item3;
        ammo = stat.Item4;
    }
    public override bool Attack()
    {
        throw new System.NotImplementedException();
    }
}
