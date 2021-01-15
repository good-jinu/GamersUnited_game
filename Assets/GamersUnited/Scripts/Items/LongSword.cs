using System.Collections;
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
        var area = Instantiate(GameData.PrefabLongSwordAttackArea, Unit.transform);
        var script = area.GetComponent<AttackObject>();
        var info = new AttackInfo(Unit, damage, 0, "Enemy", Unit.transform.position, int.MaxValue,
            (HitInfo hitInfo) => { GameManager.Instance.Effect.HitEffect(hitInfo.HitUnit.transform.position); });
        script.SetAttackInfo(info, AttackObject.IgnoreType.IgnoreWallAndFloor);
        script.SetTimer(0.25f, InstantObject.TimerAction.Destory);
        yield break;
    }
}
