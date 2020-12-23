using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//이 클래스는 이름이 바뀌거나 삭제될 수 있음
public abstract class Weapon
{
    private ItemGrade grade;
    private EffectManager.EffectMethod hitEffect;
    private GameUnit unit;
    private System.DateTime cooldownEndTime;
    public EffectManager.EffectMethod HitEffect { get => hitEffect;}
    public GameUnit Unit { get => unit; set => unit = value; }
    protected DateTime CooldownEndTime { get => cooldownEndTime; set => cooldownEndTime = value; }
    public ItemGrade Grade { get => grade; }

    public Weapon(EffectManager.EffectMethod hitEffect ,ItemGrade grade)
    {
        this.grade = grade;
        this.hitEffect = hitEffect;
    }

    //Attack 입력시, Attack 작업을 수행할 method.
    //또한 공격 모션(애니메이션)도 이 함수에서 호출해야 한다.
    public abstract void Attack();

}
