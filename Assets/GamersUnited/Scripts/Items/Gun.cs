using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    private int ammo;
    private int damage;
    private float cooldown;
    private int range;
    private const float BulletSpeed = 30f;
    public override void Init(EffectManager.EffectMethod hitEffect,ItemGrade grade)
    {
        base.Init(hitEffect, grade);
        var stat = GameData.GetWeaponStat(WeaponType.Gun, grade);
        damage = stat.Item1;
        cooldown = stat.Item2;
        range = stat.Item3;
        ammo = stat.Item4;
    }
    public override bool Attack()
    {
        Debug.Log("Call Gun Attack()");
        if(ammo > 0 && IsCanAttack())
        {
            StartCoroutine(BulletFire());
            CooldownEndTime = System.DateTime.Now.AddSeconds(cooldown);
            return true;
        }
        return false;
    }

    IEnumerator BulletFire()
    {
        Debug.Log("Call Gun BulletFire()");
        //Player Animation 호출
        //bullet instance 생성, 2/3번째 매개변수인 pos와 dir는 플레이어 Prefab과 무기 위치를 보고 변경..
        var bullet = Instantiate(GameManager.Instance.prefabGunBullet, Unit.transform.position, Unit.transform.rotation);
        var script = bullet.GetComponent<AttackObject>();
        script.Init(damage, "Enemy", bullet.transform.position, this, 1);
        script.StartBullet(BulletSpeed, range);
        yield break;
    }
}
