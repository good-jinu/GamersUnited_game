using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.Effect = this;
    }
    public delegate void EffectMethod(Vector3 pos, Vector3 dir);
    public void HitEffect(Vector3 pos, Vector3 dir)
    {
        //기본 타격 이펙트
    }
    //WarningAreaEffect() : pos 위치에 경고 장판 생성.
    //areaScale : 경고 장판의 크기
    //areaLifeTime : 경고 장판의 유지시간
    //return value : WarningArea Prefab 중 부모의 InstantObject Class
    public InstantObject WarningAreaEffect(Vector3 pos, float areaScale, float areaLifeTime)
    {
        var area = Instantiate(GameData.PrefabWarningArea, pos, new Quaternion());
        InstantObject returnValue = null;
        area.transform.localScale = new Vector3(areaScale, 1, areaScale);
        foreach (var script in area.GetComponentsInChildren<InstantObject>())
        {
            if(script.gameObject == area)
            {
                script.SetTimer(areaLifeTime,TimerMethod.Destory);
                returnValue = script;
            }
            else
            {
                script.IncreaseScale(1f / areaLifeTime, 0, 1, IncreaseScaleMode.WithoutYAxis);
            }
        }
        return returnValue;
    }

    //선택 : 추가적인 각종 이펙트 구현, 필요한 에셋 직접 다운로드하여 사용하여도 됨.
    
    //이 클래스에서의 이펙트는 타격 이펙트를 의미하며, Youtube 강좌 내에서는 없는 내용이므로 후순위로 구현하고
    //구현이 힘들다 싶으면 포기하여도 됨
}
