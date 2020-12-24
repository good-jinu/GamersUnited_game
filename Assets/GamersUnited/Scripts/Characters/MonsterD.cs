using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterD : Monster
{
    //override Part
    public override void OnDead(Vector3 dir)
    {
        Separation();
    }

    //Pattern Method
    private IEnumerator Separation()
    {
        //사망시 분리 패턴
        //구현 방법은 GameManager와 상의해야함...
        //instance 생성 파트를 이 클래스에서 할지, GameManager의 method로할지..
        yield break;
    }

}
