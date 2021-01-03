using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterZone : MonoBehaviour
{
    public Vector3[] pos;
    public GameUnitList[] unit;
    public int[] hpMul;
    public int[] atkMul;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>())
        {
            GenMon();
            Destroy(gameObject);
        }
    }

    private void GenMon()
    {
        GameUnitList uTmp;
        Vector3 posTmp;
        int hpTmp;
        int atkTmp;
        for(int i=0;i<pos.Length;i++)
        {
            posTmp = pos[i];
            if (i < unit.Length)
                uTmp = unit[i];
            else
                uTmp = GameUnitList.MonsterA;
            if (i < hpMul.Length)
                hpTmp = hpMul[i];
            else
                hpTmp = 0;
            if (i < atkMul.Length)
                atkTmp = atkMul[i];
            else
                atkTmp = 0;

            GameManager.Instance.InstantiateUnit(uTmp, posTmp, hpTmp, atkTmp);
        }
    }
}
