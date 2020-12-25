using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterD : Monster
{
    //override Part
    protected override void OnDead(Vector3 dir)
    {
        StartCoroutine(Separation());
        base.OnDead(dir);
    }

    //Pattern Method
    private IEnumerator Separation()
    {
        Monster[] monster = new Monster[2];
        //Layer를 "Dead" 로 임시로 변경하여 벽/바닥에만 충돌이 되도록 할것
        for(int i = 0; i < 2; ++i)
        {
            monster[i] = GameManager.Instance.InstantiateUnit((GameUnitList)Random.Range(1, 4), transform.position) as Monster;
            monster[i].AIActive = false;
            monster[i].transform.Rotate(Vector3.up * Random.Range(0,360));
            monster[i].Rigid.AddForce((Vector3.up + monster[i].transform.forward) * 30, ForceMode.Impulse);
            monster[i].SetInvincible(1f);
        }
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < 2; ++i)
        {
            monster[i].AIActive = true;
        }
    }

}
