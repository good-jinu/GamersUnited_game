﻿using System.Collections;
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
        if (maxHp < 1)
            throw new System.ArgumentOutOfRangeException(nameof(MaxHp), "Must be greater than or equal to 1.");
        if (currentHp <= 0f || maxHp < currentHp)
            throw new System.ArgumentOutOfRangeException(nameof(currentHp), $"Must be greater than 0, less than or equal to {nameof(maxHp)}.");
        if (atk < 0f)
            throw new System.ArgumentOutOfRangeException(nameof(atk), "Must be greater than or equal to 0.");
        if (speed < 0f)
            throw new System.ArgumentOutOfRangeException(nameof(speed), "Must be greater than or equal to 0.");
        this.maxHp = maxHp;
        health = currentHp;
        this.atk = atk;
        movespeed = speed;
        this.armor = armor;
    }

    //공격 투사체에 피격됬을 때 호출될 함수
    //반환값 : HifInfo class
    public virtual HitInfo HitbyAttack(AttackInfo attackInfo, Vector3 hitPosition)
    {
        _ = attackInfo ?? throw new System.ArgumentNullException(nameof(attackInfo));
        if (invincible || IsDead)
        {
            //테스트용 코드
            Debug.Log($"Damaged GameUnit Name : {gameObject.name}\nInvincible : {Invincible}, IsDead : {IsDead}, remainHp : {health}");

            //테스트용 코드 끝
            return new HitInfo(this, 0f, hitPosition);
        }
        //Damage 적용, 최소 1의 피해를 입도록 설정하였음.
        float validDamage = attackInfo.Damage - armor;
        if(validDamage < 1f)
        {
            validDamage = 1f;
        }
        health -= validDamage;
        //피격 방향 계산(타격 위치가 아닌 공격자의 위치를 기준으로 계산한다.)
        Vector3 dir = transform.position - attackInfo.AttackPosition;
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
            OnDamaged(dir, attackInfo.PushPower);
        }
        //테스트용 코드
        Debug.Log($"Damaged GameUnit Name : {gameObject.name}\noriginalDamage : {attackInfo.Damage}, validDamage : {validDamage}, remainHp : {health}");
        //테스트용 코드 끝
        return new HitInfo(this,validDamage,hitPosition);
    }
    protected virtual void OnDead(Vector3 dir)
    {
        GameManager.Instance.OnUnitDead(gameObject.name, transform.position, type);
        Rigid.AddForce(dir * 10 + Vector3.up * 5, ForceMode.Impulse);
        transform.LookAt(transform.position - dir);
        isDead = true;
        gameObject.layer = 12;
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
        Invoke("DamagedPhysicEnd", (pushPower / (pushPower + 20f)) * 2.5f);
    }
    protected virtual void DamagedPhysicEnd()
    {
        isDamaged = false;
        Rigid.velocity = Vector3.zero;
    }


    //매개변수로 지정한 시간 동안 무적상태로 만든다.
    public void SetInvincible(float seconds)
    {
        if (seconds < 0f)
            throw new System.ArgumentOutOfRangeException(nameof(seconds), "Must be greater than or equal to 0.");
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