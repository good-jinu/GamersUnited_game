using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterB : Monster
{
    private const float AssaultDamage = 12f;
    private const float AssaultForce = 20f;
    
    private IEnumerator Assault()
    {
        //TODO : 공격상태 확인 방법 확인하고 그거에 맞게 적용할 것
        Ani.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.2f);
        var area = Instantiate(GameData.PrefabMonsterMeleeAttackArea, transform.position + transform.forward * 2, transform.rotation, transform);
        var script = area.GetComponent<AttackObject>();
        script.Init(Atk * AssaultDamage, "Player", 0, transform.position, this, int.MaxValue, null, AttackObject.IgnoreType.IgnoreWallAndFloor);
        Rigid.AddForce(transform.forward * AssaultForce, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        Rigid.velocity = Vector3.zero;
        Destroy(script.gameObject);
        yield return new WaitForSeconds(0.8f);
        Ani.SetBool("isAttack", false);
        //공격 후딜레이
        yield return new WaitForSeconds(0.25f);
        //TODO : 공격 끝
    }
}
