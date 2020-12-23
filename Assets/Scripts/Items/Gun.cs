using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    public Gun(ItemGrade grade) : base(GameManager.Instance.Effect.HitEffect, grade)
    {

    }
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }
}
