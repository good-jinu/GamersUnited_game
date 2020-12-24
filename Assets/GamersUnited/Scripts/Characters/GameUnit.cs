using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnit : MonoBehaviour
{
    private int maxHp;
    private float health;
    private int armor;
    private float movespeed;
    private float atk;
    private bool invincible;

    public int MaxHp { get => maxHp; protected set => maxHp = value; }
    public float Health { get => health; protected set => health = value; }
    public int Armor { get => armor; protected set => armor = value; }
    public float Movespeed { get => movespeed; protected set => movespeed = value; }
    public float Atk { get => atk; protected set => atk = value; }
    public bool Invincible { get => invincible; protected set => invincible = value; }

    protected virtual void Start()
    {
        GameManager.Instance.Units.Add(gameObject.name,this);
        invincible = false;
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
    
    //공격 투사체에 피격됬을 때 호출될 함수, damage는 데미지, pos는 공격자의 위치
    //반환값 : 실제 적용된 데미지
    public virtual float hitbyAttack(float damage, Vector3 pos)
    {
        if (invincible)
            return 0f;
        float validDamage = damage - armor;
        health -= validDamage;
        Vector3 dir = transform.position - pos;
        if (health > 0)
        {
            OnDamaged(dir);
        }
        else
        {
            OnDead(dir);
        }
        //무적이 아닐 시 최소 데미지 1을 받음
        return validDamage >= 1 ? validDamage : 1;
    }
    //피격 후 hp가 0이되면 호출할 함수
    //dir : 사망 애니메이션을 수행할 방향
    public virtual void OnDead(Vector3 dir)
    {
        //사망 애니메이션(모션) 수행
        //더 이상 공격 투사체나 다른 Unit에 충돌되지 않도록 함
        //사망 애니메이션 종료 후 비활성화 또는 Destory 처리
        //GameManager의 OnUnitDead 호출
    }
    //피격 후 hp가 0 초과일때 호출할 함수
    public virtual void OnDamaged(Vector3 dir)
    {
        //경직 애니메이션(모션) 수행
        //경직 도중에는 다른 행동을 수행하지 않도록 처리할 것
    }
}
