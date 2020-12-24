using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Weapon
{
    private int ammo;
    private const float BulletSpeed = 40f;
    private const float Angular = 10f;
    public override void Init(ItemGrade grade)
    {
        base.Init(grade);
        HitEffect = GameManager.Instance.Effect.HitEffect;
        ammo = GameData.GetWeaponExtensionStat(WeaponType.Shotgun, grade).Item2;
    }
    public override bool Attack()
    {
        throw new System.NotImplementedException();
    }
}
