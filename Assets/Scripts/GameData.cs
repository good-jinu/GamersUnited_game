using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Unit Stat information
//BaseHp = Unit의 기본 최대체력
//BaseAtk = Weapon에 의해 결정되는 공격력에 BaseAtk만큼 곱하여 최종 공격력을 결정한다.
//BaseSpeed = 이동속도
//BaseArmor = Unit의 기본 방어력
//
//public static GetStat method는 Tuple 형태로 Unit의 4가지 스탯을 한번에 반환시킨다.
//
//Weapon/Item 등급
//Common/Rare/Unique 로 구분되며, Enum ItemGrade를 사용하여 지정한다.
//Static method를 통해서만 정보를 얻을 수 있도록 설정됨.. => 수정필요하면 말하기
//지정 예시 : ItemGrade.Common, ItemGrade.Rare 등
//
//Weapon Data information
//weaponAtk = weapon에 의해 발생되는 충돌체 1개의 공격력
//weaponCooldown = weapon 재 공격까지 필요한 시간
//weaponRange = 탄환의 사거리, 미사용 무기는 -1로 반환한다.
//weaponChance = 공격 가능 횟수, 미사용 무기는 -1로 반환한다.
//
//Armor Data information
//Armor 계열은 단순히 스탯만 증가시킨다.
//증가시키는 스탯 종류 : 최대체력(bonusHp), 방어력(bonusArmor), 이동속도(bonusSpeed)
//
//아래의 모든 Stat관련 정보는 스탯 조정이 필요하다고 생각할 경우 수정하여도 되며, 다만 수정 시 반드시 모두에게 수정사항을 알릴 것
public static class GameData
{
    //Player Stat
    public static readonly int playerBaseHp = 100;
    public static readonly float playerBaseAtk = 1.0f;
    public static readonly float playerBaseSpeed = 10;
    public static readonly int playerBaseArmor = 0;
    public static (int,float,float,int) GetPlayerStat() { return (playerBaseHp, playerBaseAtk, playerBaseSpeed, playerBaseArmor); }

    //Monster Stat
    //A,B,C,D 데이터 전부가 배열에 들어있으며, A는 인덱스 0 / B는 인덱스 1 / C는 인덱스 2 / D는 인덱스 3
    //Monster의 BaseAtk은 모든 종류가 1.0로 통일, 배열 구조를 사용하지 않음
    //층 수에 따른 Hp, Atk 차이는 multiple[] 배열의 값과 곱하여 구현한다.
    public static readonly int[] monsterBaseHp = { 150, 300, 100, 500 };
    public static readonly float monsterBaseAtk = 1.0f;
    public static readonly float[] monsterBaseSpeed = { 12, 8, 8, 6 };
    public static readonly int[] monsterBaseArmor = { 0, 2, -2, 10 };
    public static readonly float[] multiple = { 1.0f, 1.2f, 1.4f, 1.6f, 1.8f, 2.0f, 2.2f, 2.4f, 2.6f, 2.8f };
    public static (int,float,float,int) GetMonsterStat(int index) 
    { return (monsterBaseHp[index], monsterBaseAtk, monsterBaseSpeed[index], monsterBaseArmor[index]); }

    //Weapon Data
    //Get Method 사용 방법 : 첫번째 매개변수는 Weapon 종류 지정(Enum WeaponType), 두번째 매개변수는 Weapon 등급 지정(Enum ItemGrade)
    private static readonly int[,] weaponAtk = { { 30, 40, 50 }, { 40, 55, 70 }, { 25, 30, 35 }, { 15, 18, 22 } };
    private static readonly float[,] weaponCooldown = { { 1.4f, 1.3f, 1.2f }, { 1.6f, 1.55f, 1.5f }, { 0.8f, 0.7f, 0.6f }, { 1.25f, 1.15f, 1.05f } };
    private static readonly int[,] weaponRange = { { -1, -1, -1 }, { -1, -1, -1 }, { 7, 8, 9 }, { 4, 5, 6 } };
    private static readonly int[,] weaponChance = { { -1, -1, -1 }, { -1, -1, -1 }, { 30, 40, 50 }, { 10, 15, 20 } };
    public static (int,float,int,int) GetWeaponStat(WeaponType type, ItemGrade grade)
    {
        return (weaponAtk[(int)type, (int)grade], weaponCooldown[(int)type, (int)grade], weaponRange[(int)type, (int)grade], weaponChance[(int)type, (int)grade]);
    }

    //Armor Data
    //Get Method 사용 방법 : 첫번째 매개변수는 Armor 종류 지정(Enum ArmorType), 두번째 매개변수는 Armor 등급 지정(Enum ItemGrade)
    private static readonly int[,] bonusHp = { { 10,25,50},{ 30,60,100},{ 0,0,0} };
    private static readonly int[,] bonusArmor = { { 10, 15, 25 }, { 3, 6, 10 }, { 1, 2, 5 } };
    private static readonly float[,] bonusSpeed = { { 0, 0, 0 }, { 0, 0, 0 }, { 1, 2, 4 } };
    public static (int,int,float) GetArmorStat(ArmorType type, ItemGrade grade)
    {
        return (bonusHp[(int)type, (int)grade], bonusArmor[(int)type, (int)grade], bonusSpeed[(int)type, (int)grade]);
    }
}
public enum WeaponType { Sword, Longsword, Gun, Shotgun }
public enum ItemGrade { Common, Rare, Unique}
public enum ArmorType { Head, Body, Boots}
