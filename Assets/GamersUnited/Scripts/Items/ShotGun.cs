﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Weapon
{
    private int ammo;
    private const float BulletSpeed = 75f;
    private const float Angle = 11.25f;

    public int Ammo { get => ammo; }

    private void Awake()
    {
        Type = WeaponType.Shotgun;
    }
    public override void Init(ItemGrade grade)
    {
        base.Init(grade);
        HitEffect = GameManager.Instance.Effect.HitEffect;
        ammo = GameData.GetWeaponExtensionStat(WeaponType.Shotgun, grade).Item2;
    }
    public override bool Attack()
    {
        if (ammo > 0 && IsCanAttack())
        {
            var stat = GameData.GetWeaponStat(WeaponType.Shotgun, Grade);
            ammo--;
            StartCoroutine(BulletFire(stat.Item1 * Unit.Atk));
            CooldownEndTime = System.DateTime.Now.AddSeconds(stat.Item2);
            return true;
        }
        return false;
    }

    IEnumerator BulletFire(float damage)
    {
        for(int i = 0; i < 5; ++i)
        { 
            var bullet = Instantiate(GameData.PrefabShotGunBullet, Unit.transform.position, Unit.transform.rotation);
            bullet.transform.Rotate(new Vector3(0, (i - 2) * Angle, 0));
            var script = bullet.GetComponent<AttackObject>();
            var bulletstat = GameData.GetWeaponExtensionStat(WeaponType.Shotgun, Grade);
            script.Init(damage, "Enemy", 0, bullet.transform.position, Unit, bulletstat.Item3, HitEffect);
            script.BulletFire(BulletSpeed, bulletstat.Item1);
        }
        yield break;
    }
}
