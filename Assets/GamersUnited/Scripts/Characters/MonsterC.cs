using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterC : Monster
{
    const float MissileDamage = 15f;
    const float MissileSpeed = 40f;
    const float MissileRange = 40f;
    protected override void Awake()
    {
        base.Awake();
        Type = GameUnitList.MonsterC;
    }
    protected override void Targeting()
    {
        var hits = Physics.SphereCastAll(transform.position, 0.5f, transform.forward, 30f, LayerMask.GetMask("Player"));
        if (hits.Length > 0 && !IsAttack)
        {
            StartCoroutine(Shot());
        }
    }
    private IEnumerator Shot()
    {
        IsAttack = true;
        IsChase = false;
        Ani.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.4f);
        var missile = Instantiate(GameData.PrefabMissile, transform.position, transform.rotation);
        var scripts = missile.GetComponentsInChildren<InstantObject>();
        foreach (InstantObject script in scripts)
        {
            if (script as AttackObject)
            {
                ((AttackObject)script).Init(Atk * MissileDamage, "Player", 0, transform.position, this, 1, null);
                script.BulletFire(MissileSpeed, MissileRange, true);
            }
            else
            {
                script.SelfRotate(90f, true, false, false);
            }
        }
        yield return new WaitForSeconds(0.7f);
        //공격 후딜레이
        yield return new WaitForSeconds(1f);
        Ani.SetBool("isAttack", false);
        IsAttack = false;
        IsChase = true;
    }
}
