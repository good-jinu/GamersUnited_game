using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DW;

namespace DW
{
    public class DroppedWeaponGenerator
    {
        private bool isTypeRandom = true;
        private WeaponType type;
        //생성할 무기 타입
        private Vector3 pos = Vector3.zero;
        //생성할 무기 위치
        private int com = 1;
        private int rar = 1;
        private int uni = 1;
        //생성할 무기 등급의 확률 비율

        public void SetPos(Vector3 pos)
        {
            this.pos = pos;
        }

        public void SetWeaponType()
        {
            isTypeRandom = true;
        }

        public void SetWeaponType(WeaponType type)
        {
            isTypeRandom = false;
            this.type = type;
        }

        public void SetGradeChance(int com, int rar, int uni)
        {
            if(com<0)
            {
                com = 0;
            }
            if (rar < 0)
            {
                rar = 0;
            }
            if (uni < 0)
            {
                uni = 0;
            }
            //음수를 받게 되면 0으로 저장
            this.com = com;
            this.rar = rar;
            this.uni = uni;
        }

        public void GenDW()
        {
            System.Random rand = new System.Random();
            int randNum = 0;//rand.Next()를 임시저장하기 위한 공간
            GameObject dwObj = Resources.Load<GameObject>("DroppedWeapon/Weapon");
            dwObj = Object.Instantiate<GameObject>(dwObj);
            //무기 오브젝트 인스턴스화

            dwObj.transform.position = pos;
            //무기 위치 조정

            if(isTypeRandom)
            {
                type = (WeaponType)rand.Next(0, 3);
            }//무기 타입 무작위 설정

            randNum = rand.Next(com + rar + uni);
            if(randNum<com)
            {
                dwObj.GetComponent<DroppedWeapon>().Init(ItemGrade.Common, type);
            }
            else if(randNum<(com+rar))
            {
                dwObj.GetComponent<DroppedWeapon>().Init(ItemGrade.Rare, type);
            }
            else
            {
                dwObj.GetComponent<DroppedWeapon>().Init(ItemGrade.Unique, type);
            }//무기 등급 무작위 설정
        }
    }
}
