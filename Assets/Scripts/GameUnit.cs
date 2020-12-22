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
    //인덱스 0 : 투구, 1 : 갑옷, 2 : 신발
    private Armor[] equipArmor;
    private Weapon weapon;

    public Weapon Weapon { get => weapon; set => weapon = value; }
    public bool Invincible { get => invincible; set => invincible = value; }
    public Armor[] EquipArmor1 { get => equipArmor; set => equipArmor = value; }

    private void Start()
    {
        GameManager.Instance.Units.Add(gameObject.name,this);
        invincible = false;
        equipArmor = new Armor[3];
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
    public void EquipArmor(Armor armor)
    {
        //방어구 습득시 호출, 방어구에 정해진 스탯만큼 Unit 스탯을 증가시킴
    }
    public void UnequipArmor(ArmorType armorType)
    {
        //방어구 해제시 호출, ArmorType으로 지정한 현재 착용중인 방어구를 해제하고, 해당 방어구로 올랏던 스탯을 감소시킴.
    }
    public void EquipWeapon(Weapon weapon)
    {
        //무기 습득시 호출, 무기 종류에 맞는 그래픽 불러오기/Weapon.HitEffect 설정
        weapon.Unit = this;
        weapon.HitEffect = GameManager.Instance.Effect.hitEffect;
    }
    public void UnequipWeapon()
    {
        //무기 해제시 호출, 그래픽 해제/기본 무기로 변경시키기
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
