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

    public int MaxHp { get => maxHp; protected set => maxHp = value; }
    public float Health { get => health; protected set => health = value; }
    public int Armor { get => armor; protected set => armor = value; }
    public float Movespeed { get => movespeed; protected set => movespeed = value; }
    public float Atk { get => atk; protected set => atk = value; }
    public bool Invincible { get => invincible; }
    public Rigidbody Rigid { get => rigid; }
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
        maxHp = hp;
        health = (float)hp;
        this.atk = atk;
        movespeed = speed;
        this.armor = armor;

    }
    
    //공격 투사체에 피격됬을 때 호출될 함수, damage는 데미지, pos는 공격자의 위치, pushPower : 미는 힘
    //반환값 : 실제 적용된 데미지
    public virtual float HitbyAttack(float damage, Vector3 pos, float pushPower)
    {
        if (invincible)
        {
            //테스트용 코드
            Debug.Log($"Damaged GameUnit Name : {gameObject.name}\nInvincible : On, remainHp : {health}");

            //테스트용 코드 끝
            return 0f;
        }
        //Damage 적용
        float validDamage = damage - armor;
        health -= validDamage;
        //피격 방향 계산
        Vector3 dir = transform.position - pos;
        dir.y = 0;
        if (dir.x == 0 && dir.z == 0)
        {
            dir.x = Random.Range(0.1f, 1f);
            dir.z = Random.Range(0.1f, 1f);
        }
        dir = dir.normalized;
        //남은 체력에 따라 처리
        if (health > 0)
        {
            OnDamaged(dir, pushPower);
        }
        else
        {
            OnDead(dir);
        }
        //테스트용 코드
        Debug.Log($"Damaged GameUnit Name : {gameObject.name}\noriginalDamage : {damage}, validDamage : {validDamage}, remainHp : {health}");
        //테스트용 코드 끝
        return validDamage < 0 ? 0 : validDamage;
    }
    //피격 후 hp가 0이되면 호출할 함수
    //dir : 사망 애니메이션을 수행할 방향
    //TODO : 
    //사망 애니메이션(모션) 수행
    //더 이상 공격 투사체나 다른 Unit에 충돌되지 않도록 함
    //사망 애니메이션 종료 후 비활성화 또는 Destory 처리
    //GameManager의 OnUnitDead 호출
    protected abstract void OnDead(Vector3 dir);
    
    protected virtual void OnDamaged(Vector3 dir, float pushPower)
    {

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
