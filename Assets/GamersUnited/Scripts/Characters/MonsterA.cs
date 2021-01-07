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
    protected override void Targeting()
    {
        var hits = Physics.SphereCastAll(transform.position, 1.5f, transform.forward, 3f, LayerMask.GetMask("Player"));
        if (hits.Length > 0 && !IsAttack && IsChase)
        {
            StartCoroutine(MeleeAttack());
        }
    }

    private IEnumerator MeleeAttack()
    {
        IsAttack = true;
        IsChase = false;
        Ani.SetBool("isAttack",true);
        yield return new WaitForSeconds(0.15f);
        var area = Instantiate(GameData.PrefabMonsterMeleeAttackArea, transform);
        var script = area.GetComponent<AttackObject>();
        var attackInfo = new AttackInfo(this, Atk * MeleeAttackDamage, 0, "Player", transform.position);
        script.SetAttackInfo(attackInfo, AttackObject.IgnoreType.IgnoreWallAndFloor);
        script.SetTimer(0.25f, InstantObject.TimerAction.Destory);
        yield return new WaitForSeconds(1.85f);
        //공격 후딜레이
        yield return new WaitForSeconds(0.25f);
        Ani.SetBool("isAttack", false);
        IsAttack = false;
        IsChase = true;
    }
}
