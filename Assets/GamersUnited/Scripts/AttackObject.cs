using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//이 클래스는 이름이 바뀌거나 삭제될 수 있음
public class AttackObject : MonoBehaviour
{
    private delegate void Method();
    private Method updateMethod;
    private Method fixedUpdateMethod;

    private float damage;
    private string targetTag;
    private Vector3 startpos;
    private GameUnit caster;
    private int enableHitCount;
    private EffectManager.EffectMethod hitEffect;
    private HashSet<GameObject> hitSet = new HashSet<GameObject>();

    //특정 Object에만 사용할 변수들..
    private Rigidbody rigid;
    private float range;
    private float speed;

    public Vector3 Startpos { get => startpos; set => startpos = value; }

    public void Init(float damage, string targetTag, Vector3 pos, GameUnit caster, int enableHitCount, EffectManager.EffectMethod effect)
    {
        this.damage = damage;
        this.targetTag = targetTag;
        this.startpos = pos;
        this.caster = caster;
        this.enableHitCount = enableHitCount;
        hitEffect = effect;
    }

    void Update()
    {
        if (updateMethod != null)
            updateMethod();
    }
    void FixedUpdate()
    {
        if (fixedUpdateMethod != null)
            fixedUpdateMethod();
    }
    private void OnTriggerEnter(Collider other)
    {
        //충돌체가 공격 대상이고, 아직 공격하지 않았어야 함
        if (targetTag.Equals(other.tag) && !hitSet.Contains(other.gameObject))
        {
            hitSet.Add(other.gameObject);
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
                if (hitEffect != null)
                    hitEffect(hitpos,hitdir);
                //데미지 이펙트
                GameManager.Instance.UI.PrintDamage(validDamage, hitscript.transform.position);
                //그외 공격 성공 후 처리 필요할 시 작성
            }
            if (--enableHitCount <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        //벽에 부딪힐 시
        else if (false)
        {
            Destroy(this.gameObject);
        }
    }

    //외부 호출용 public 함수들

    public void BulletFire(float speed, float range)
    {
        this.speed = speed;
        this.range = range;
        rigid = GetComponent<Rigidbody>();
        fixedUpdateMethod = BulletMoving;
    }

    public IEnumerator SetTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }

    //Delegate 또는 Coroutine용 함수들
    private void BulletMoving()
    {
        Vector3 diff = startpos - transform.position;
        float distance = Mathf.Sqrt(Mathf.Pow(diff.x, 2) + Mathf.Pow(diff.z, 2));
        if(distance < range)
        {
            rigid.velocity = transform.forward * speed;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
