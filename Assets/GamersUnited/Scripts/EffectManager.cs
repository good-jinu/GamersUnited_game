using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.Effect = this;
    }
    public void HitEffect(Vector3 pos)
    {
        var effect = GameManager.Instance.Pooling.GetParticleEffect(PoolManager.ParticleList.HitEffectA);
        effect.transform.position = pos + new Vector3(0, 10, -3);
        effect.SetTimer(0.2f, InstantObject.TimerAction.Destory);
    }
    //WarningAreaEffect() : pos 위치에 경고 장판 생성.
    //areaScale : 경고 장판의 크기
    //areaLifeTime : 경고 장판의 유지시간
    //return value : WarningArea Prefab 중 부모의 InstantObject Class
    public InstantObject WarningAreaEffect(Vector3 pos, float areaScale, float areaLifeTime)
    {
        if(areaScale <= 0f)
            throw new System.ArgumentOutOfRangeException(nameof(areaScale), "Must be greater than 0.");
        if (areaLifeTime <= 0f)
            throw new System.ArgumentOutOfRangeException(nameof(areaLifeTime), "Must be greater than 0.");

        var area = GameManager.Instance.Pooling.GetWarningAreaEffect();
        area.first.transform.localScale = new Vector3(areaScale, 1, areaScale);
        area.first.transform.position = pos;
        area.first.SetTimer(areaLifeTime, InstantObject.TimerAction.Destory);
        area.second.transform.localScale = new Vector3(1, 0.01f, 1);
        area.second.IncreaseScale(1f / areaLifeTime, 0, 1, InstantObject.IncreaseScaleMode.WithoutYAxis);
        return area.first;
    }

    //ShockWaveEffect() : pos 위치에 충격파 이펙트 생성.
    public void ShockWaveEffect(Vector3 pos)
    {
        var instant = Instantiate(GameData.PrefabShockWave, pos, new Quaternion(), transform);
        Destroy(instant, 1f);
    }

    //ExplosionEffect() : pos 위치에 폭발 이펙트 생성.
    public void ExplosionEffect(Vector3 pos)
    {
        var explosion = GameManager.Instance.Pooling.GetParticleEffect(PoolManager.ParticleList.Explosion);
        explosion.transform.position = pos;
        explosion.SetTimer(1f, InstantObject.TimerAction.Destory);
    }

    //선택 : 추가적인 각종 이펙트 구현, 필요한 에셋 직접 다운로드하여 사용하여도 됨.
    
    //이 클래스에서의 이펙트는 타격 이펙트를 의미하며, Youtube 강좌 내에서는 없는 내용이므로 후순위로 구현하고
    //구현이 힘들다 싶으면 포기하여도 됨
}
