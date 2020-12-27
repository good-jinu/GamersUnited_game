using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            this.com = com;
            this.rar = rar;
            this.uni = uni;
        }

        public void GenDW()
        {
            System.Random rand = new System.Random();
            GameObject dwObj;
        }
    }
}
