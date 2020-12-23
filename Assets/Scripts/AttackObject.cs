using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//이 클래스는 이름이 바뀌거나 삭제될 수 있음
public class AttackObject : MonoBehaviour
{
    public delegate void ObjectMoveMethod();
    private ObjectMoveMethod updateMethod;
    private float damage;
    private string targetTag;
    private Vector3 startpos;
    private Weapon caller;

    public ObjectMoveMethod UpdateMethod { set => updateMethod = value; }

    public void init(float damage, string targetTag, Vector3 pos, Weapon caller)
    {
        this.damage = damage;
        this.targetTag = targetTag;
        this.startpos = pos;
        this.caller = caller;
    }
    public void DoMethod(ObjectMoveMethod method)
    {
        method();
    }

    void Update()
    {
        if (updateMethod != null)
            updateMethod();
    }
    private void OnTriggerEnter(Collider other)
    {
        //충돌체가 공격 대상일시
        if (targetTag.Equals(other.tag))
        {
            var hitscript = GameManager.Instance.Units[other.gameObject.name];
            var validDamage = hitscript.hitbyAttack(damage, startpos);
            //적용된 데미지가 0 초과일시 공격 성공으로 판정
            if (validDamage > 0.0f)
            {
                //타격 이펙트
                //hitpos 식을 타격 위치로 얻어오도록 변경..
                var hitpos = Vector3.zero;
         
                var hitdir = hitpos - startpos;
                hitdir.y = 0;
                hitdir = hitdir.normalized;
                caller.HitEffect(hitpos, hitdir);
                //데미지 이펙트
                GameManager.Instance.UI.PrintDamage(validDamage, hitscript.transform.position);
                //그외 공격 성공 후 처리 필요할 시 작성
            }
            Destroy(this.gameObject);
        }
    }
}
