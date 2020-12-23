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

    protected int MaxHp { get => maxHp; set => maxHp = value; }
    protected float Health { get => health; set => health = value; }
    protected int Armor { get => armor; set => armor = value; }
    protected float Movespeed { get => movespeed; set => movespeed = value; }
    protected float Atk { get => atk; set => atk = value; }
    public bool Invincible { get => invincible; set => invincible = value; }

    protected virtual void Start()
    {
        GameManager.Instance.Units.Add(gameObject.name,this);
        invincible = false;
    }
    //스탯 초기화용 함수, 매개변수로 넣은 스탯 값으로 스탯 정보를 초기화한다.
    public void initStat(int hp, float atk, float speed, int armor)
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
        //데미지 처리
        //피격 모션 또는 사망 모션 호출
        return 0;
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
