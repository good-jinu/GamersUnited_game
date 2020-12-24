using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public override void Init(ItemGrade grade)
    {
        base.Init(grade);
        HitEffect = GameManager.Instance.Effect.HitEffect;
    }
    public override bool Attack()
    {
        if (IsCanAttack())
        {
            var stat = GameData.GetWeaponStat(WeaponType.Sword, Grade);
            StartCoroutine(Slash(stat.Item1 * Unit.Atk));
            CooldownEndTime = System.DateTime.Now.AddSeconds(stat.Item2);
            return true;
        }
        return false;
    }
    private IEnumerator Slash(float damage)
    {
        //Player Animation 호출
        //AttackArea instance 생성, 2/3번째 매개변수인 pos와 dir는 플레이어 Prefab과 무기 위치를 보고 변경..
        var area = Instantiate(GameData.PrefabSwordAttackArea, Unit.transform.position, Unit.transform.rotation);
        var script = area.GetComponent<AttackObject>();
        script.Init(damage, "Enemy", Unit.transform.position, Unit, int.MaxValue, HitEffect);
        StartCoroutine(script.SetTimer(0.4f));
        yield break;
    }
}
