using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    private int ammo;
    private const float BulletSpeed = 30f;
    public override void Init(ItemGrade grade)
    {
        base.Init(grade);
        HitEffect = GameManager.Instance.Effect.HitEffect;
        ammo = GameData.GetWeaponExtensionStat(WeaponType.Gun, grade).Item2;
    }
    public override bool Attack()
    {
        if(ammo > 0 && IsCanAttack())
        {
            var stat = GameData.GetWeaponStat(WeaponType.Gun, Grade);
            StartCoroutine(BulletFire(stat.Item1 * Unit.Atk));
            CooldownEndTime = System.DateTime.Now.AddSeconds(stat.Item2);
            return true;
        }
        return false;
    }

    IEnumerator BulletFire(float damage)
    {
        //Player Animation 호출
        //bullet instance 생성, 2/3번째 매개변수인 pos와 dir는 플레이어 Prefab과 무기 위치를 보고 변경..
        var bullet = Instantiate(GameData.PrefabGunBullet, Unit.transform.position, Unit.transform.rotation);
        var script = bullet.GetComponent<AttackObject>();
        var bulletstat = GameData.GetWeaponExtensionStat(WeaponType.Gun, Grade);
        script.Init(damage, "Enemy", bullet.transform.position, Unit, bulletstat.Item3,HitEffect);
        script.StartBullet(BulletSpeed, bulletstat.Item1);
        yield break;
    }
}
