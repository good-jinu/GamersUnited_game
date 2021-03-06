﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    private int ammo;
    private const float BulletSpeed = 40f;

    public int Ammo { get => ammo; }
    private void Awake()
    {
        Type = WeaponType.Gun;
    }

    public override void Init(ItemGrade grade)
    {
        base.Init(grade);
        ammo = GameData.GetWeaponExtensionStat(WeaponType.Gun, grade).Item2;
    }
    public override bool Attack()
    {
        if(ammo > 0 && IsCanAttack())
        {
            var stat = GameData.GetWeaponStat(WeaponType.Gun, Grade);
            ammo--;
            StartCoroutine(BulletFire(stat.Item1 * Unit.Atk));
            CooldownEndTime = System.DateTime.Now.AddSeconds(stat.Item2);
            return true;
        }
        return false;
    }

    IEnumerator BulletFire(float damage)
    {
        AttackObject bullet = GameManager.Instance.Pooling.GetAttackObject(PoolManager.AttackObjectList.Bullet);
        bullet.transform.position = Unit.transform.position;
        bullet.transform.rotation = Unit.transform.rotation;
        var bulletstat = GameData.GetWeaponExtensionStat(WeaponType.Gun, Grade);
        var info = new AttackInfo(Unit, damage, 0, "Enemy", bullet.transform.position, bulletstat.Item3);
        bullet.SetAttackInfo(info);
        bullet.BulletFire(BulletSpeed, bulletstat.Item1);
        yield break;
    }
}
