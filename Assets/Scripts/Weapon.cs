using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//이 클래스는 이름이 바뀌거나 삭제될 수 있음
public abstract class Weapon
{
    private ItemGrade grade;
    private EffectManager.EffectMethod hitEffect;
    private GameUnit unit;
    public ItemGrade Grade { get => grade; set => grade = value; }
    public EffectManager.EffectMethod HitEffect { get => hitEffect; set => hitEffect = value; }
    public GameUnit Unit { get => unit; set => unit = value; }

    //cooldown 정보를 어떻게 저장하고, 어떻게 쿨타임이 끝났는지 판정할 것인가?
    //공격 횟수 제한이 있는 무기의 경우 해당 정보를 어떻게 관리할 것인가?

    //Attack 입력시, Attack 작업을 수행할 method.
    //또한 공격 모션(애니메이션)도 이 함수에서 호출해야 한다.
    public abstract void Attack();

}
