using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Unit Stat information
//BaseHp = Unit의 기본 최대체력
//BaseAtk = Weapon에 의해 결정되는 공격력에 BaseAtk만큼 곱하여 최종 공격력을 결정한다.
//BaseSpeed = 이동속도
//BaseArmor = Unit의 기본 방어력
//
//Weapon/Item 등급
//Common/Rare/Unique 로 구분되며, Enum ItemGrade를 사용하여 지정한다.
//지정 예시 : ItemGrade.Common, ItemGrade.Rare 등
//
//Weapon Data information
//weaponAtk = weapon에 의해 발생되는 충돌체 1개의 공격력
//weaponCooldown = weapon 재 공격까지 필요한 시간
//weaponRange = 탄환의 사거리, 미사용 무기는 -1로 반환한다.
//weaponAmmo = 공격 가능 횟수(탄약 수), 미사용 무기는 -1로 반환한다.
//
//Armor Data information
//Armor 계열은 단순히 스탯만 증가시킨다.
//증가시키는 스탯 종류 : 최대체력(bonusHp), 방어력(bonusArmor), 이동속도(bonusSpeed)

//Get Method를 사용하면 해당되는 데이터를 Tuple형태로 반환받음. 데이터들을 하나의 구조체로 묶어 반환한다고 생각하면 되며 Item1, Item2 등의 이름으로 자동으로 지정됨.
//
//아래의 모든 정보는 스탯 조정이 필요하다고 생각할 경우 수정하여도 된다.
public static class GameData
{
    //Player Stat
    private static readonly int playerBaseHp = 100;
    private static readonly float playerBaseAtk = 1.0f;
    private static readonly float playerBaseSpeed = 10;
    private static readonly int playerBaseArmor = 0;
    //Player의 기본적인 스탯 : 최대 체력/공격력 계수/이동속도/방어력 을 반환한다.
    public static (int,float,float,int) GetPlayerStat() { return (playerBaseHp, playerBaseAtk, playerBaseSpeed, playerBaseArmor); }

    //Monster Stat
    //Monster의 BaseAtk은 모든 종류가 1.0로 통일, 배열 구조를 사용하지 않음
    //층 수에 따른 Hp, Atk 차이는 multiple[] 배열의 값과 곱하여 구현한다.
    private static readonly int[] monsterBaseHp = { 150, 300, 100, 500 };
    private static readonly float monsterBaseAtk = 1.0f;
    private static readonly float[] monsterBaseSpeed = { 12, 8, 8, 6 };
    private static readonly int[] monsterBaseArmor = { 0, 2, -2, 10 };
    //매개변수 type으로 지정한 종류의 Monster 기본 스탯 : 최대 체력/공격력 계수/이동속도/방어력 을 반환한다.
    public static (int,float,float,int) GetMonsterStat(MonsterType type) 
    { return (monsterBaseHp[(int)type], monsterBaseAtk, monsterBaseSpeed[(int)type], monsterBaseArmor[(int)type]); }

    public static readonly float[] multiple = { 1.0f, 1.2f, 1.4f, 1.6f, 1.8f, 2.0f, 2.2f, 2.4f, 2.6f, 2.8f };


    //Weapon Data
    private static readonly int[,] weaponAtk = { { 30, 40, 50 }, { 40, 55, 70 }, { 25, 30, 35 }, { 15, 18, 22 } };
    private static readonly float[,] weaponCooldown = { { 1.4f, 1.3f, 1.2f }, { 1.6f, 1.55f, 1.5f }, { 0.8f, 0.7f, 0.6f }, { 1.25f, 1.15f, 1.05f } };
    private static readonly int[,] weaponRange = { { -1, -1, -1 }, { -1, -1, -1 }, { 14, 17, 20 }, { 10, 11, 12 } };
    private static readonly int[,] weaponAmmo = { { -1, -1, -1 }, { -1, -1, -1 }, { 60, 80, 100 }, { 20, 30, 40 } };
    //Get Method 사용 방법 : 첫번째 매개변수는 Weapon 종류 지정(Enum WeaponType), 두번째 매개변수는 Weapon 등급 지정(Enum ItemGrade)
    //매개변수 type과 grade로 지정한 무기의 기본 정보 : 공격 시 공격력/공격 쿨타임/사거리/공격 가능 횟수(탄약) 반환
    public static (int,float,int,int) GetWeaponStat(WeaponType type, ItemGrade grade)
    {
        return (weaponAtk[(int)type, (int)grade], weaponCooldown[(int)type, (int)grade], weaponRange[(int)type, (int)grade], weaponAmmo[(int)type, (int)grade]);
    }

    //Armor Data
    private static readonly int[,] bonusHp = { { 10,25,50},{ 30,60,100},{ 0,0,0} };
    private static readonly int[,] bonusArmor = { { 10, 15, 25 }, { 3, 6, 10 }, { 1, 2, 5 } };
    private static readonly float[,] bonusSpeed = { { 0, 0, 0 }, { 0, 0, 0 }, { 1, 2, 4 } };
    //Get Method 사용 방법 : 첫번째 매개변수는 Armor 종류 지정(Enum ArmorType), 두번째 매개변수는 Armor 등급 지정(Enum ItemGrade)
    //매개변수 type과 grade로 지정한 방어구의 기본 정보 : 최대체력 증가량/방어력 증가량/이동속도 증가량 을 반환한다.
    public static (int,int,float) GetArmorStat(ArmorType type, ItemGrade grade)
    {
        return (bonusHp[(int)type, (int)grade], bonusArmor[(int)type, (int)grade], bonusSpeed[(int)type, (int)grade]);
    }

    //Prefab 정보들
    //현재 아래 방법 사용시 에러가 발생함. GameManager를 통해 prefab을 얻을것
    private const string prefabPath = @"GamersUnited/Prefabs/";
    public static readonly GameObject prefabShotGunBullet = Resources.Load<GameObject>($"{prefabPath}ShotGun Bullet");
    public static readonly GameObject prefabGunBullet = Resources.Load<GameObject>($"{prefabPath}Gun Bullet");


}
public enum WeaponType { Sword, Longsword, Gun, Shotgun }
public enum ItemGrade { Common, Rare, Unique}
public enum ArmorType { Head, Body, Boots}
public enum MonsterType { A, B, C, D }