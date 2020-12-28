﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSword : Weapon
{
    private void Awake()
    {
        Type = WeaponType.Longsword;
    }
    public override void Init(ItemGrade grade)
    {
        base.Init(grade);
        HitEffect = GameManager.Instance.Effect.HitEffect;
    }
    public override bool Attack()
    {
        if (IsCanAttack())
        {
            var stat = GameData.GetWeaponStat(WeaponType.Longsword, Grade);
            StartCoroutine(Slash(stat.Item1 * Unit.Atk));
            CooldownEndTime = System.DateTime.Now.AddSeconds(stat.Item2);
            return true;
        }
        return false;
    }
    private IEnumerator Slash(float damage)
    {
        var area = Instantiate(GameData.PrefabLongSwordAttackArea, transform);
        var script = area.GetComponent<AttackObject>();
        script.Init(damage, "Enemy", 0, Unit.transform.position, Unit, int.MaxValue, HitEffect, AttackObject.IgnoreType.IgnoreWallAndFloor);
        script.SetTimer(0.25f, InstantObject.TimerAction.Destory);
        yield break;
    }
}
