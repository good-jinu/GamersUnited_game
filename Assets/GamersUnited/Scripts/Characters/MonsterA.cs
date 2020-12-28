using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterA : Monster
{
    private const float MeleeAttackDamage = 10f;

    protected override void Awake()
    {
        base.Awake();
        Type = GameUnitList.MonsterA;
    }

    private IEnumerator MeleeAttack()
    {
        //TODO : 공격상태 확인 방법 확인하고 그거에 맞게 적용할 것
        Ani.SetBool("isAttack",true);
        yield return new WaitForSeconds(0.15f);
        var area = Instantiate(GameData.PrefabMonsterMeleeAttackArea, transform.position + transform.forward * 2, transform.rotation, transform);
        var script = area.GetComponent<AttackObject>();
        script.Init(Atk * MeleeAttackDamage, "Player", 0, transform.position, this, int.MaxValue, null, AttackObject.IgnoreType.IgnoreWallAndFloor);
        script.SetTimer(0.25f, InstantObject.TimerAction.Destory);
        yield return new WaitForSeconds(1.85f);
        Ani.SetBool("isAttack", false);
        //공격 후딜레이
        yield return new WaitForSeconds(0.25f);
        //TODO : 공격 끝
    }
}
