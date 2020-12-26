using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantObject : MonoBehaviour
{
    protected delegate void Method();
    public delegate void SignalMethod(Transform objTransform);
    private Method updateMethod;
    private Method fixedUpdateMethod;
    private SignalMethod destoryMethod;
    private Vector3 startpos;
    private Rigidbody rigid;
    public enum IncreaseScaleMode { WithoutYAxis, AllAxis }
    public enum TimerAction { Destory, Stop }

    public Vector3 Startpos { get => startpos; set => startpos = value; }

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
            destoryMethod(transform);
        Destroy(this.gameObject);
    }

    //외부 호출용 public 함수들

    //BulletFire : 지정한 거리만큼 지정한 속도로 빠르게 이동한다.
    //reservationDestory = true 일시 지정한 거리만큼 이동 후 Destory한다.
    //이 함수 사용 시, 이동을 시작할 떄 startPos가 오브젝트 위치로 갱신된다. AttackObject/startPos 사용시 주의
    //FixedUpdate를 사용하며, 다른 FixedUpdate 사용 Method와 중복 사용할 시 어떻게 될 지 예측할 수 없음
    public void BulletFire(float speed, float range, bool reservationDestory = true)
    {
        if (startpos == null) startpos = transform.position;
        if(rigid == null)
            rigid = GetComponent<Rigidbody>();
        startpos = transform.position;
        fixedUpdateMethod = BulletMoving(speed, range, reservationDestory);
    }

    //ChaseBulletFire : target을 목표로 지정한 이동속도/회전속도에 기반하여 계속 이동한다.
    //time 매개변수를 사용 시 지정한 시간 경과 후 Destory한다.
    //이 함수 사용시 매 FixedUpdate마다 startPos가 오브젝트 위치로 갱신된다. AttackObject/startPos 사용시 주의
    //FixedUpdate를 사용하며, 다른 FixedUpdate 사용 Method와 중복 사용할 시 어떻게 될 지 예측할 수 없음
    public void ChaseBulletFire(float moveSpeed, float rotateSpeed, Transform target)
    {
        if (moveSpeed < 0f || rotateSpeed < 0f || target == null)
            throw new System.ArgumentException();
        if (rigid == null)
            rigid = GetComponent<Rigidbody>();
        fixedUpdateMethod = ChaserMoving(moveSpeed, rotateSpeed, target);
    }
    public void ChaseBulletFire(float moveSpeed, float rotateSpeed, float time, Transform target)
    {
        ChaseBulletFire(moveSpeed, rotateSpeed, target);
        SetTimer(time, TimerAction.Destory);
    }

    //SetTimer : 지정한 시간 이후 TimerAction 으로 지정한 작업을 수행한다.
    //Destory : 이 오브젝트를 파괴한다.
    //Stop : 오브젝트에 설정된 모든 동작들을 중지 또는 삭제 한다. AttackObject의 경우 충돌 이벤트도 포함된다.
    //       주의 : 동작에 의해 이미 일어난 변화를 초기화 시키지 않음
    public void SetTimer(float seconds, TimerAction action)
    {
        Method timerMethod = null;
        switch (action)
        {
            case TimerAction.Destory:
                timerMethod = DestoryThis;
                break;
            case TimerAction.Stop:
                timerMethod = StopActions;
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

    //SelfRotate : speed에 기반하여 지정한 축들을 계속 회전시킨다.
    //Update를 사용하며, 다른 Update 사용 Method와 중복 사용할 시 어떻게 될 지 예측할 수 없음
    public void SelfRotate(float speed, bool xAxis, bool yAxis, bool zAxis)
    {
        updateMethod = Rotating(speed, new Vector3(xAxis ? 1 : 0, yAxis ? 1 : 0, zAxis ? 1 : 0));
    }

    //SetAttackWhenDestory : 이 오브젝트가 사라질 때 공격판정을 가지도록 한다.
    //hitSet 매개변수 사용시 생성되는 AttackObject.HitSet을 해당 변수로 설정한다.
    //공격 판정이 생기는 위치는 오브젝트의 transform과 동일함, 판정 범위는 Capsule 형태이며 scale에 비례한다.
    //이 Method를 수행하더라도, 외부에서 Destroy() 등을 통해 오브젝트를 파괴하면 공격 판정이 발동하지 않는다.
    //다른 Public Method로 오브젝트 내부에서 Destory 작업을 수행하도록 유도해야만 공격 판정이 발동한다.(ex : SetTimer Method 사용(TimerAction.Destory로 지정))
    public void SetAttackWhenDestory(float scale, float damage, float pushPower, string targetTag, GameUnit caster, EffectManager.EffectMethod effect = null)
    {
        SetAttackWhenDestory(scale, damage, pushPower, targetTag, caster, null, effect);
    }
    public void SetAttackWhenDestory(float scale,
                                     float damage,
                                     float pushPower,
                                     string targetTag,
                                     GameUnit caster,
                                     HashSet<GameObject> hitSet,
                                     EffectManager.EffectMethod effect = null)
    {
        if (targetTag == null || caster == null)
            throw new System.ArgumentNullException();
        destoryMethod += DeathAttack(scale, damage, pushPower, targetTag, caster, hitSet, effect);
    }

    //SetSignalWhenDestory : 이 오브젝트가 사라질 떄 SignalMethod method를 호출한다.
    //호출 시 매개변수는 이 오브젝트의 transform.
    //다른 Public Method로 오브젝트 내부에서 Destory 작업을 수행하도록 유도해야만 method 호출이 수행된다.
    public void SetSignalWhenDestory(SignalMethod method)
    {
        if (method == null)
            throw new System.ArgumentNullException();
        destoryMethod += method;
    }

    //오브젝트에 설정된 모든 동작들을 중지 또는 삭제 한다. AttackObject의 경우 충돌 이벤트도 포함된다.
    //       주의 : 동작에 의해 이미 일어난 변화를 초기화 시키지 않음
    public virtual void StopActions()
    {
        updateMethod = null;
        fixedUpdateMethod = null;
        destoryMethod = null;
        StopAllCoroutines();
        if (rigid != null)
            rigid.velocity = Vector3.zero;
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
    private SignalMethod DeathAttack(float scale, float damage, float pushPower, string targetTag, GameUnit caster, HashSet<GameObject> hitSet, EffectManager.EffectMethod effect)
    {
        return (objTransform) =>
        {
            var instant = Instantiate(GameData.PrefabCapsuleAttackArea, transform.position, transform.rotation);
            var script = instant.GetComponent<AttackObject>();
            instant.transform.localScale = Vector3.one * scale;
            script.HitSet = hitSet;
            script.Init(damage, targetTag, pushPower, instant.transform.position, caster, int.MaxValue, effect);
            script.SetTimer(0.25f, TimerAction.Destory);
        };
    }
    
    private Method ChaserMoving(float moveSpeed, float rotateSpeed, Transform target)
    {
        return () =>
        {
            startpos = transform.position;
            Vector2 bulletPos = new Vector2(transform.position.x, transform.position.z);
            Vector2 targetPos = new Vector2(target.position.x, target.position.z);
            Vector2 DirectionVec = (targetPos - bulletPos).normalized;
            Vector2 forward = new Vector2(transform.forward.x, transform.forward.z);
            float angle = Vector2.SignedAngle(forward, DirectionVec);
            int anglesign = angle < 0 ? -1 : 1;
            angle *= anglesign;
            float rotateangle = rotateSpeed * Time.fixedDeltaTime;
            if (angle < rotateangle)
                rotateangle = angle;
            transform.Rotate(Vector3.up * rotateangle * -anglesign);
            rigid.velocity = transform.forward * moveSpeed;
        };
    }
    private Method Rotating(float speed, Vector3 axis)
    {
        return () =>
        {
            transform.Rotate(axis * speed * Time.deltaTime);
        };
    }
}
