using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : MonoBehaviour
{
    public delegate void ObjectMoveMethod();
    private ObjectMoveMethod method;
    private float damage;
    private string targetTag;
    private Vector3 startpos;
    private Weapon caller;

    public void init(ObjectMoveMethod method, float damage, string targetTag, Vector3 pos, Weapon caller)
    {
        this.method = method;
        this.damage = damage;
        this.targetTag = targetTag;
        this.startpos = pos;
        this.caller = caller;
    }

    void FixedUpdate()
    {
        if (method != null)
            method();
    }
    private void OnCollisionEnter(Collision collision)
    {
        //충돌체가 공격 대상일시
        if (targetTag.Equals(collision.gameObject.tag))
        {
            var hitscript = GameManager.Instance.Units[collision.gameObject.name];
            var validDamage = hitscript.hitbyAttack(damage, startpos);
            //적용된 데미지가 0 초과일시 공격 성공으로 판정
            if(validDamage > 0.0f)
            {
                //타격 이펙트
                var hitpos = collision.GetContact(0).point;
                var hitdir = hitpos - startpos;
                hitdir.y = 0;
                hitdir = hitdir.normalized;
                caller.HitEffect(hitpos,hitdir);
                //데미지 이펙트
                GameManager.Instance.UI.PrintDamage(validDamage, hitscript.transform.position);
                //그외 공격 성공 후 처리 필요할 시 작성
            }
            //TODO : AttackObject 삭제 또는 비활성화 처리
        }
    }
}
