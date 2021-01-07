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
        Ani.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.2f);
        var area = Instantiate(GameData.PrefabMonsterMeleeAttackArea, transform);
        var script = area.GetComponent<AttackObject>();
        var attackInfo = new AttackInfo(this, Atk * AssaultDamage, 5f, "Player", transform.position);
        script.SetAttackInfo(attackInfo, AttackObject.IgnoreType.IgnoreWallAndFloor);
        Rigid.AddForce(transform.forward * AssaultForce, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        Rigid.velocity = Vector3.zero;
        Destroy(script.gameObject);
        yield return new WaitForSeconds(0.8f);
        //공격 후딜레이
        yield return new WaitForSeconds(0.25f);
        Ani.SetBool("isAttack", false);
        IsAttack = false;
        IsChase = true;
    }
}
