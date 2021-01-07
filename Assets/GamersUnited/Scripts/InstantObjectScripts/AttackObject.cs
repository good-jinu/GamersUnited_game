using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AttackObject : InstantObject
{
    private AttackInfo attackInfo;
    private int hitCount;
    private HashSet<GameObject> hitSet;
    private bool isActive = false;
    private IgnoreType ignore;

    public enum IgnoreType { None, IgnoreWall, IgnoreFloor, IgnoreWallAndFloor }

    //HitSet은 공격체가 동일한 대상을 여러번 공격하지 않도록 체크하는 역할을 수행하는 HashSet으로,
    //HitSet을 초기화 함으로써 동일한 대상을 여러번 공격하거나
    //HitSet을 여러개의 AttackObject가 공유하게 함으로써 AttackObject들이 동일한 공격체로 인식되게 처리 할 수 있다.
    public HashSet<GameObject> HitSet { set => hitSet = value; }

    public void SetAttackInfo(AttackInfo attackInfo,IgnoreType ignore = IgnoreType.IgnoreFloor)
    {
        if (hitSet == null)
            hitSet = new HashSet<GameObject>();
        this.attackInfo = attackInfo;
        this.ignore = ignore;
        hitCount = 0;
        isActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Init() 메소드 호출 전일시 충돌을 모두 무시한다.
        if (!isActive) return;
        //충돌체가 공격 대상이고, 아직 공격하지 않았어야 함
        if (attackInfo.TargetTag.Equals(other.tag) && !hitSet.Contains(other.gameObject))
        {
            hitSet.Add(other.gameObject);
            var hitscript = GameManager.Instance.Units[other.gameObject.name];
            var hitInfo = hitscript.HitbyAttack(attackInfo);
            //적용된 데미지가 0 초과일시 데미지 프린트
            if (hitInfo.Damage > 0.0f)
            {
                //데미지 이펙트
                GameManager.Instance.UI.PrintDamage(hitInfo.Damage, hitscript.transform.position);
            }
            //공격 성공 메소드 있을시 수행
            if(attackInfo.AttackSuccess != null)
            {
                attackInfo.AttackSuccess(hitInfo);
            }
            //최대 타격수 만족시 파괴
            if (++hitCount >= attackInfo.EnableHitCount)
            {
                DestoryThis();
            }
        }
        else if (IsHitWallOrFloor(other.tag))
        {
            DestoryThis();
        }
    }
    private bool IsHitWallOrFloor(string tag)
    {
        if (tag.Equals("Floor"))
        {
            return (ignore == IgnoreType.IgnoreWall || ignore == IgnoreType.None);
        }
        else if (tag.Equals("Wall"))
        {
            return (ignore == IgnoreType.IgnoreFloor || ignore == IgnoreType.None);
        }
        else return false;
    }
    public override void StopActions()
    {
        base.StopActions();
        isActive = false;
    }
    protected override void DestoryThis()
    {
        if(attackInfo != null)
        {
            if(attackInfo.AttackFailed != null)
            {
                attackInfo.AttackFailed(transform.position);
            }
        }
        base.DestoryThis();
    }
}
