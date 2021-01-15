using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterB : Monster
{
    private const float AssaultDamage = 12f;
    private const float AssaultForce = 20f;
    protected override void Awake()
    {
        base.Awake();
        Type = GameUnitList.MonsterB;
    }
    protected override void Targeting()
    {
        var hits = Physics.SphereCastAll(transform.position, 1f, transform.forward, 12f, LayerMask.GetMask("Player"));
        if (hits.Length > 0 && !IsAttack && IsChase)
        {
            StartCoroutine(Assault());
        }
    }
    private IEnumerator Assault()
    {
        IsAttack = true;
        IsChase = false;
        yield return new WaitForSeconds(0.3f);
        Ani.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.2f);
        AttackObject attack = GameManager.Instance.Pooling.GetAttackObject(PoolManager.AttackObjectList.MonsterMeleeAttack);
        attack.transform.SetParent(transform);
        attack.transform.localPosition = Vector3.zero;
        attack.transform.rotation = transform.rotation;
        var attackInfo = new AttackInfo(this, Atk * AssaultDamage, 10f, "Player", transform.position, int.MaxValue,
            (HitInfo info) => { GameManager.Instance.Effect.HitEffect(info.HitUnit.transform.position); });
        attack.SetAttackInfo(attackInfo, AttackObject.IgnoreType.IgnoreWallAndFloor);
        attack.SetTimer(1f, InstantObject.TimerAction.Destory);
        Rigid.AddForce(transform.forward * AssaultForce, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        Rigid.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.8f);
        //공격 후딜레이
        yield return new WaitForSeconds(0.25f);
        Ani.SetBool("isAttack", false);
        IsAttack = false;
        IsChase = true;
    }
}
