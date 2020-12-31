using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameUnit : MonoBehaviour
{
    private int maxHp;
    private float health;
    private int armor;
    private float movespeed;
    private float atk;
    private bool invincible;
    private System.DateTime invincibleEndTime;
    private Rigidbody rigid;
    private bool isDamaged;
    private bool isDead;
    private GameUnitList type;

    public int MaxHp { get => maxHp; protected set => maxHp = value; }
    public float Health { get => health; protected set => health = value; }
    public int Armor { get => armor; protected set => armor = value; }
    public float Movespeed { get => movespeed; protected set => movespeed = value; }
    public float Atk { get => atk; protected set => atk = value; }
    public bool Invincible { get => invincible; }
    public Rigidbody Rigid { get => rigid; }
    protected bool IsDamaged { get => isDamaged; }
    public bool IsDead { get => isDead; }
    public GameUnitList Type { get => type; protected set => type = value; }

    protected virtual void Awake()
    {
        invincible = false;
        invincibleEndTime = System.DateTime.MinValue;
        rigid = GetComponent<Rigidbody>();
    }
    protected virtual void Start()
    {
        gameObject.name += gameObject.GetInstanceID().ToString();
        GameManager.Instance.Units.Add(gameObject.name,this);
    }
    //스탯 초기화용 함수, 매개변수로 넣은 스탯 값으로 스탯 정보를 초기화한다.
    public void InitStat(int hp, float atk, float speed, int armor)
    {
        InitStat(hp, hp, atk, speed, armor);
    }
    public void InitStat(int maxHp, float currentHp, float atk, float speed, int armor)
    {
        if (maxHp < currentHp)
            throw new System.ArgumentException("Set to (currentHp > maxHp) is unable");
        this.maxHp = maxHp;
        health = currentHp;
        this.atk = atk;
        movespeed = speed;
        this.armor = armor;
    }

    //공격 투사체에 피격됬을 때 호출될 함수, damage는 데미지, pos는 공격자의 위치, pushPower : 미는 힘
    //반환값 : 실제 적용된 데미지
    public virtual float HitbyAttack(float damage, Vector3 pos, float pushPower)
    {
        if (invincible || IsDead)
        {
            //테스트용 코드
            Debug.Log($"Damaged GameUnit Name : {gameObject.name}\nInvincible : {Invincible}, IsDead : {IsDead}, remainHp : {health}");

            //테스트용 코드 끝
            return 0f;
        }
        //Damage 적용
        float validDamage = damage - armor;
        health -= validDamage;
        //피격 방향 계산
        Vector3 dir = transform.position - pos;
        dir.y = 0;
        if (dir == Vector3.zero)
        {
            dir.x = Random.Range(0.1f, 1f);
            dir.z = Random.Range(0.1f, 1f);
        }
        dir = dir.normalized;
        //남은 체력에 따라 처리
        if(health <= 0f)
        {
            OnDead(dir);
        }
        else
        {
            OnDamaged(dir, pushPower);
        }
        //테스트용 코드
        Debug.Log($"Damaged GameUnit Name : {gameObject.name}\noriginalDamage : {damage}, validDamage : {validDamage}, remainHp : {health}");
        //테스트용 코드 끝
        return validDamage < 0 ? 0 : validDamage;
    }
    protected virtual void OnDead(Vector3 dir)
    {
        //TODO : 레이어 변경 추가
        GameManager.Instance.OnUnitDead(gameObject.name, transform.position, type);
        Rigid.AddForce(dir * 10 + Vector3.up * 5, ForceMode.Impulse);
        transform.LookAt(transform.position - dir);
        isDead = true;
    }

    protected virtual void OnDamaged(in Vector3 dir, in float pushPower)
    {
        if (pushPower > 0f)
        {
            DamagedPhysic(dir, pushPower);
        }
    }
    protected virtual void DamagedPhysic(in Vector3 dir, in float pushPower)
    {
        isDamaged = true;
        Rigid.AddForce(dir * pushPower, ForceMode.Impulse);
        transform.LookAt(transform.position - dir);
        Invoke("DamagedPhysicEnd", 0.5f + (pushPower / (pushPower + 20f)) * 2f);
    }
    protected virtual void DamagedPhysicEnd()
    {
        isDamaged = false;
        Rigid.velocity = Vector3.zero;
    }


    //매개변수로 지정한 시간 동안 무적상태로 만든다.
    public void SetInvincible(float seconds)
    {
        System.DateTime newEndTime = System.DateTime.Now.AddSeconds(seconds);
        if (invincibleEndTime < newEndTime)
        {
            invincibleEndTime = newEndTime;
            invincible = true;
            StopCoroutine(InvincibleTimer());
            StartCoroutine(InvincibleTimer());
        }
    
    }
    private IEnumerator InvincibleTimer()
    {
        while(System.DateTime.Now < invincibleEndTime) yield return null;
        invincible = false;
    }
}