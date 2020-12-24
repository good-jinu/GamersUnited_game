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
    private HashSet<GameObject> hitSet = new HashSet<GameObject>();
    private bool isActive = false;

    public void Init(float damage, string targetTag, Vector3 pos, GameUnit caster, int enableHitCount, EffectManager.EffectMethod effect)
    {
        Startpos = pos;
        this.damage = damage;
        this.targetTag = targetTag;
        this.caster = caster;
        this.enableHitCount = enableHitCount;
        hitEffect = effect;
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
            var validDamage = hitscript.HitbyAttack(damage, Startpos);
            //적용된 데미지가 0 초과일시 공격 성공으로 판정
            if (validDamage > 0.0f)
            {
                //타격 이펙트
                //hitpos 식을 타격 위치로 얻어오도록 변경..
                var hitpos = Vector3.zero;
         
                var hitdir = hitpos - Startpos;
                hitdir.y = 0;
                hitdir = hitdir.normalized;
                if (hitEffect != null)
                    hitEffect(hitpos,hitdir);
                //데미지 이펙트
                GameManager.Instance.UI.PrintDamage(validDamage, hitscript.transform.position);
                //그외 공격 성공 후 처리 필요할 시 작성
            }
            if (--enableHitCount <= 0)
            {
                DestoryThis();
            }
        }
        //벽에 부딪힐 시
        else if (false)
        {
            DestoryThis();
        }
    }

}
