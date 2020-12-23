using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameUnit
{

    //인덱스 0 : 투구, 1 : 갑옷, 2 : 신발
    private Armor[] equip;
    private Weapon weapon;

    public Weapon Weapon { get => weapon; }
    public Armor[] Equip { get => equip; }
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        GameManager.Instance.Player = this;
        equip = new Armor[3];
    }

    // Update is called once per frame
    void Update()
    {
        //무기 없을 시 기본무기(단검,Common) 장착시키기
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
}
