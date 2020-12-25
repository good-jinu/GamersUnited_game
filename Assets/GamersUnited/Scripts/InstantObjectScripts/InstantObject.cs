using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantObject : MonoBehaviour
{
    protected delegate void Method();
    private Method updateMethod;
    private Method fixedUpdateMethod;
    private Method destoryMethod;
    private Vector3 startpos;
    private Rigidbody rigid;
    public enum IncreaseScaleMode { WithoutYAxis, AllAxis }
    public enum TimerAction { Destory }

    public Vector3 Startpos { get => startpos; set => startpos = value; }
    protected Method UpdateMethod { get => updateMethod; set => updateMethod = value; }
    protected Method FixedUpdateMethod { get => fixedUpdateMethod; set => fixedUpdateMethod = value; }
    protected Method DestoryMethod { get => destoryMethod; set => destoryMethod = value; }

    protected virtual void Update()
    {
        if (updateMethod != null)
            updateMethod();
    }
    protected virtual void FixedUpdate()
    {
        if (fixedUpdateMethod != null)
            fixedUpdateMethod();
    }

    protected void DestoryThis()
    {
        if (destoryMethod != null)
            destoryMethod();
        Destroy(this.gameObject);
    }

    //외부 호출용 public 함수들

    //BulletFire : 지정한 거리만큼 지정한 속도로 빠르게 이동한다.
    //FixedUpdate를 사용하며, 다른 FixedUpdate 사용 Method와 중복 사용할 시 어떻게 될 지 예측할 수 없음
    public void BulletFire(float speed, float range, bool destroyAfterFire = true)
    {
        if (startpos == null) startpos = transform.position;
        if(rigid == null)
            rigid = GetComponent<Rigidbody>();
        fixedUpdateMethod = BulletMoving(speed, range, destroyAfterFire);
    }

    //SetTimer : 지정한 시간 이후 TimerMethod로 지정한 작업을 수행한다.
    //현재는 Destory만 지원
    public void SetTimer(float seconds, TimerAction action)
    {
        Method timerMethod = null;
        switch (action)
        {
            case TimerAction.Destory:
                timerMethod = DestoryThis;
                break;
        }
        StartCoroutine(Timer(seconds, timerMethod));
    }

    //IncreaseScale : Object의 Scale을 (startScale ~ targetScale) 범위에 걸쳐 변화시킨다.
    //변화 속도는 speed로 지정하고, mode 매개변수로 Y축을 Scale 변화에 포함시킬지 여부를 결정한다.
    //destoryTime 지정시 해당 시간 이후 Destory한다.
    //Update를 사용하며, 다른 Update 사용 Method와 중복 사용할 시 어떻게 될 지 예측할 수 없음
    public void IncreaseScale(float speed, float startScale, float targetScale, IncreaseScaleMode mode = IncreaseScaleMode.WithoutYAxis)
    {
        Vector3 scale = Vector3.one * startScale;
        if (mode == IncreaseScaleMode.WithoutYAxis)
            scale.y = transform.localScale.y;
        transform.localScale = scale;
        updateMethod = ScaleUp(speed, targetScale, mode);
    }
    public void IncreaseScale(float speed, float startScale, float targetScale, float destroyTime, IncreaseScaleMode mode = IncreaseScaleMode.WithoutYAxis)
    {
        IncreaseScale(speed, startScale, targetScale, mode);
        SetTimer(destroyTime, TimerAction.Destory);
    }

    //SetAttackWhenDestory : 이 오브젝트가 사라질 때 공격판정을 가지도록 한다.
    //공격 판정이 생기는 위치는 오브젝트의 transform과 동일함, 판정 범위는 Capsule 형태이며 scale에 비례한다.
    //이 Method를 수행하더라도, 외부에서 Destroy() 등을 통해 오브젝트를 파괴하면 공격 판정이 발동하지 않는다.
    //다른 Public Method로 오브젝트 내부에서 Destory 작업을 수행하도록 유도해야만 공격 판정이 발동한다.(ex : SetTimer Method 사용(TimerAction.Destory로 지정))
    public void SetAttackWhenDestory(float scale, float damage, string targetTag, GameUnit caster, EffectManager.EffectMethod effect = null)
    {
        if (targetTag == null||caster == null)
            throw new System.ArgumentNullException();
        destoryMethod += DeathAttack(scale,damage, targetTag, caster, effect);
    }

    //Delegate 또는 Coroutine용 함수들
    private Method BulletMoving(float speed, float range, bool destroyAfterFire)
    {
        return () => 
        {
            Vector3 diff = startpos - transform.position;
            float distance = Mathf.Sqrt(Mathf.Pow(diff.x, 2) + Mathf.Pow(diff.z, 2));
            if (distance < range)
            {
                rigid.velocity = transform.forward * speed;
            }
            else
            {
                if (destroyAfterFire)
                {
                    DestoryThis();
                }
                else
                {
                    rigid.velocity = Vector3.zero;
                    fixedUpdateMethod = null;
                }
            }
        };
    }
    private Method ScaleUp(float speed, float targetScale, IncreaseScaleMode mode)
    {
        return () =>
        {
            Vector3 scale = Vector3.one * speed * Time.deltaTime;
            if(mode == IncreaseScaleMode.WithoutYAxis)
                scale.y = 0;
            transform.localScale += scale;
            if (transform.localScale.x >= targetScale)
            {
                scale = Vector3.one * targetScale;
                if (mode == IncreaseScaleMode.WithoutYAxis) 
                    scale.y = transform.localScale.y;
                transform.localScale = scale;
                updateMethod = null;
            }
        };
    }
    
    private IEnumerator Timer(float seconds, Method method)
    {
        yield return new WaitForSeconds(seconds);
        method();
    }
    private Method DeathAttack(float scale, float damage, string targetTag, GameUnit caster, EffectManager.EffectMethod effect)
    {
        return () =>
        {
            var instant = Instantiate(GameData.PrefabCapsuleAttackArea, transform.position, transform.rotation);
            var script = instant.GetComponent<AttackObject>();
            instant.transform.localScale = Vector3.one * scale;
            script.Init(damage, targetTag, instant.transform.position, caster, int.MaxValue, effect);
            script.SetTimer(0.25f, TimerAction.Destory);
        };
    }
}
