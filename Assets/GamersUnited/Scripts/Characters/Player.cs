using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameUnit
{

    //인덱스 0 : 투구, 1 : 갑옷, 2 : 신발
    private Armor[] equip;
    private Weapon weapon;

    //테스트용 변수, 삭제하여도됨
    private ItemGrade testGrade = ItemGrade.Common;

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


        //아래는 무기 테스트용 코드, 삭제하여도 됨
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon = GetComponentInChildren<Gun>();
            weapon.Unit = this;
            weapon.Init(testGrade);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon = GetComponentInChildren<ShotGun>();
            weapon.Unit = this;
            weapon.Init(testGrade);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weapon = GetComponentInChildren<Sword>();
            weapon.Unit = this;
            weapon.Init(testGrade);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            weapon = GetComponentInChildren<LongSword>();
            weapon.Unit = this;
            weapon.Init(testGrade);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            testGrade = ItemGrade.Common;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            testGrade = ItemGrade.Rare;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            testGrade = ItemGrade.Unique;
        }
        if (Input.GetKey(KeyCode.A))
        {
            weapon.Attack();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameManager.Instance.Effect.WarningAreaEffect(new Vector3(0, 0, 3), 5, 0.5f);
        }
    }
    public void EquipArmor(Armor armor)
    {
        //방어구 습득시 호출, 방어구에 정해진 스탯만큼 Unit 스탯을 증가시킴
        if (equip[(int)armor.Type] != null)
        {
            UnequipArmor(armor.Type);
        }
        var armorStat = GameData.GetArmorStat(armor.Type, armor.Grade);
        MaxHp += armorStat.Item1;
        Health += armorStat.Item1;
        Armor += armorStat.Item2;
        Movespeed += armorStat.Item3;
    }
    public void UnequipArmor(ArmorType armorType)
    {
        //방어구 해제시 호출, ArmorType으로 지정한 현재 착용중인 방어구를 해제하고, 해당 방어구로 올랏던 스탯을 감소시킴.
        var unEquipArmor = equip[(int)armorType];
        if (unEquipArmor == null)
            throw new System.Exception("장착하지 않은 방어구를 Unequip 시도함");
        equip[(int)armorType] = null;

        var armorStat = GameData.GetArmorStat(unEquipArmor.Type, unEquipArmor.Grade);
        MaxHp -= armorStat.Item1;
        Armor -= armorStat.Item2;
        Movespeed -= armorStat.Item3;
        if (MaxHp < Health)
            Health = MaxHp;
    }
    public void EquipWeapon(Weapon equipWeapon)
    {
        //무기 습득시 호출, 무기 종류에 맞는 그래픽 불러오기
        equipWeapon.Unit = this;
        weapon = equipWeapon;
    }
    public void UnequipWeapon()
    {
        //무기 해제시 호출, 그래픽 해제/기본 무기로 변경시키기
    }
}
