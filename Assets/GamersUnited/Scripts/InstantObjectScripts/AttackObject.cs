using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AttackObject : InstantObject
{
    private float damage;
    private string targetTag;
    private GameUnit caster;
    private int enableHitCount;
    private EffectManager.EffectMethod hitEffect;
    private HashSet<GameObject> hitSet;
    private bool isActive = false;
    private IgnoreType ignore;
    private float pushPower;

    public enum IgnoreType { None, IgnoreWall, IgnoreFloor, IgnoreWallAndFloor }

    //HitSet은 공격체가 동일한 대상을 여러번 공격하지 않도록 체크하는 역할을 수행하는 HashSet으로,
    //HitSet을 초기화 함으로써 동일한 대상을 여러번 공격하거나
    //HitSet을 여러개의 AttackObject가 공유하게 함으로써 AttackObject들이 동일한 공격체로 인식되게 처리 할 수 있다.
    public HashSet<GameObject> HitSet { set => hitSet = value; }

    public void Init(float damage,
                     string targetTag,
                     float pushPower,
                     Vector3 pos,
                     GameUnit caster,
                     int enableHitCount,
                     EffectManager.EffectMethod effect,
                     IgnoreType ignore = IgnoreType.IgnoreFloor)
    {
        Startpos = pos;
        this.damage = damage;
        this.targetTag = targetTag;
        this.caster = caster;
        this.enableHitCount = enableHitCount;
        hitEffect = effect;
        this.ignore = ignore;
        this.pushPower = pushPower;
        if (hitSet == null)
            hitSet = new HashSet<GameObject>();
        isActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Init() 메소드 호출 전일시 충돌을 모두 무시한다.
        if (!isActive) return;
        //충돌체가 공격 대상이고, 아직 공격하지 않았어야 함
        if (targetTag.Equals(other.tag) && !hitSet.Contains(other.gameObject))
        {
            hitSet.Add(other.gameObject);
            var hitscript = GameManager.Instance.Units[other.gameObject.name];
            var validDamage = hitscript.HitbyAttack(damage, Startpos, pushPower);
            //적용된 데미지가 0 초과일시 공격 성공으로 판정
            if (validDamage > 0.0f)
            {
                //타격 이펙트
                //hitpos 식을 타격 위치로 얻어오도록 변경..
                if (hitEffect != null)
                {
                    var hitpos = Vector3.zero;

                    var hitdir = hitpos - Startpos;
                    hitdir.y = 0;
                    hitdir = hitdir.normalized;
                    hitEffect(hitpos, hitdir);
                }
                //데미지 이펙트
                GameManager.Instance.UI.PrintDamage(validDamage, hitscript.transform.position);
                //그외 공격 성공 후 처리 필요할 시 작성
            }
            if (--enableHitCount <= 0)
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
}
