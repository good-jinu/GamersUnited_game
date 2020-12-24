using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterA : Monster
{
    private const float MeleeAttackDamage = 10f;

    private Pattern meleeAttack;

    protected override void Start()
    {
        base.Start();
        meleeAttack = new Pattern(MeleeAttack, 1f, 1);
    }

    private IEnumerator MeleeAttack()
    {
        //animation 작업
        //attack
        var area = Instantiate(GameData.PrefabMonsterMeleeAttackArea, transform.position, transform.rotation);
        var script = area.GetComponent<AttackObject>();
        script.Init(Atk * MeleeAttackDamage, "Player", transform.position, this, int.MaxValue, null);
        StartCoroutine(script.SetTimer(0.5f));
        yield break;
    }
}
