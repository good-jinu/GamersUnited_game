using System.Collections;
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
            AttackObject bullet = GameManager.Instance.Pooling.GetAttackObject(PoolManager.AttackObjectList.Bullet);
            bullet.transform.position = Unit.transform.position;
            bullet.transform.rotation = Unit.transform.rotation;
            bullet.transform.Rotate(new Vector3(0, (i - 2) * Angle, 0));
            var bulletstat = GameData.GetWeaponExtensionStat(WeaponType.Shotgun, Grade);
            var info = new AttackInfo(Unit, damage, 0, "Enemy", bullet.transform.position, bulletstat.Item3);
            bullet.SetAttackInfo(info);
            bullet.BulletFire(BulletSpeed, bulletstat.Item1);
        }
        yield break;
    }
}
