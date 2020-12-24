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
    private IncreaseScaleMode increaseScaleMode;

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
        bulletFireSpeed = speed;
        bulletFireRange = range;
        if(rigid == null)
            rigid = GetComponent<Rigidbody>();
        fixedUpdateMethod = destroyAfterFire ? (Method)BulletMovingAndDestory : (Method)BulletMoving;
    }

    public IEnumerator SetTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DestoryThis();
    }

    public void IncreaseScale(float speed, float startScale, float targetScale, IncreaseScaleMode mode = IncreaseScaleMode.WithoutYAxis)
    {
        scaleUpSpeed = speed;
        scaleUpSize = targetScale;
        increaseScaleMode = mode;
        Vector3 scale = Vector3.one * startScale;
        if (increaseScaleMode == IncreaseScaleMode.WithoutYAxis)
            scale.y = transform.localScale.y;
        transform.localScale = scale;
        updateMethod = ScaleUp;
    }
    public void IncreaseScale(float speed, float startScale, float targetScale, float destroyTime, IncreaseScaleMode mode = IncreaseScaleMode.WithoutYAxis)
    {
        IncreaseScale(speed, startScale, targetScale, mode);
        StartCoroutine(SetTimer(destroyTime));
    }

    //Delegate 또는 Coroutine용 함수들
    protected void BulletMovingAndDestory()
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
    protected void BulletMoving()
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
    protected void ScaleUp()
    {
        Vector3 scale = Vector3.one * scaleUpSpeed * Time.deltaTime;
        if (increaseScaleMode == IncreaseScaleMode.WithoutYAxis)
            scale.y = 0;
        transform.localScale += scale;
        if(transform.localScale.x >= scaleUpSize)
        {
            scale = Vector3.one * scaleUpSize;
            if (increaseScaleMode == IncreaseScaleMode.WithoutYAxis)
                scale.y = transform.localScale.y;
            transform.localScale = scale;
            updateMethod = null;
        }
    }
}

public enum IncreaseScaleMode { WithoutYAxis, AllAxis};