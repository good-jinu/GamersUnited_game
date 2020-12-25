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

    //특정 Object/Method에만 사용할 변수들..
    private Rigidbody rigid;
    private float bulletFireRange;
    private float bulletFireSpeed;
    private float scaleUpSize;
    private float scaleUpSpeed;
    private AttackObject deathAttack;

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

    public void BulletFire(float speed, float range, bool destroyAfterFire = true)
    {
        if (startpos == null) startpos = transform.position;
        bulletFireSpeed = speed;
        bulletFireRange = range;
        if(rigid == null)
            rigid = GetComponent<Rigidbody>();
        fixedUpdateMethod = destroyAfterFire ? (Method)BulletMovingAndDestory : (Method)BulletMoving;
    }

    public void SetTimer(float seconds, TimerMethod method)
    {
        Method timerMethod = null;
        switch (method)
        {
            case TimerMethod.Destory:
                timerMethod = DestoryThis;
                break;
        }
        StartCoroutine(Timer(seconds, timerMethod));
    }

    public void IncreaseScale(float speed, float startScale, float targetScale, IncreaseScaleMode mode = IncreaseScaleMode.WithoutYAxis)
    {
        scaleUpSpeed = speed;
        scaleUpSize = targetScale;
        Vector3 scale = Vector3.one * startScale;
        if (mode == IncreaseScaleMode.WithoutYAxis)
            scale.y = transform.localScale.y;
        transform.localScale = scale;
        updateMethod = (mode == IncreaseScaleMode.WithoutYAxis) ? (Method)ScaleUpWithoutYAxis : (Method)ScaleUp;
    }
    public void IncreaseScale(float speed, float startScale, float targetScale, float destroyTime, IncreaseScaleMode mode = IncreaseScaleMode.WithoutYAxis)
    {
        IncreaseScale(speed, startScale, targetScale, mode);
        SetTimer(destroyTime, TimerMethod.Destory);
    }
    public void SetAttackWhenDestory()
    {
        if (deathAttack != null)
            throw new System.NotSupportedException("SetAttackWhenDestory() Method는 2번 이상 호출할 수 없습니다.");
        destoryMethod = DeathAttack;
    }

    //Delegate 또는 Coroutine용 함수들
    private void BulletMovingAndDestory()
    {
        Vector3 diff = startpos - transform.position;
        float distance = Mathf.Sqrt(Mathf.Pow(diff.x, 2) + Mathf.Pow(diff.z, 2));
        if (distance < bulletFireRange)
        {
            rigid.velocity = transform.forward * bulletFireSpeed;
        }
        else
        {
            DestoryThis();
        }
    }
    private void BulletMoving()
    {
        Vector3 diff = startpos - transform.position;
        float distance = Mathf.Sqrt(Mathf.Pow(diff.x, 2) + Mathf.Pow(diff.z, 2));
        if (distance < bulletFireRange)
        {
            rigid.velocity = transform.forward * bulletFireSpeed;
        }
        else
        {
            rigid.velocity = Vector3.zero;
            fixedUpdateMethod = null;
        }
    }
    private void ScaleUp()
    {
        Vector3 scale = Vector3.one * scaleUpSpeed * Time.deltaTime;
        transform.localScale += scale;
        if (transform.localScale.x >= scaleUpSize)
        {
            scale = Vector3.one * scaleUpSize;
            transform.localScale = scale;
            updateMethod = null;
        }
    }
    private void ScaleUpWithoutYAxis()
    {
        Vector3 scale = Vector3.one * scaleUpSpeed * Time.deltaTime;
        scale.y = 0;
        transform.localScale += scale;
        if (transform.localScale.x >= scaleUpSize)
        {
            scale = Vector3.one * scaleUpSize;
            scale.y = transform.localScale.y;
            transform.localScale = scale;
            updateMethod = null;
        }
    }
    private IEnumerator Timer(float seconds, Method method)
    {
        yield return new WaitForSeconds(seconds);
        method();
    }
    private void DeathAttack()
    {
        deathAttack.enabled = true;
        deathAttack.transform.position = transform.position;
        deathAttack.SetTimer(0.25f,TimerMethod.Destory);
    }
}

public enum IncreaseScaleMode { WithoutYAxis, AllAxis}
public enum TimerMethod { Destory }