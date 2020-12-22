using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Effect = this;
    }
    public delegate void EffectMethod(Vector3 pos, Vector3 dir);
    //각종 이펙트 처리를 이 클래스 내 메소드로 수행함
    public void hitEffect(Vector3 pos, Vector3 dir)
    {
        //기본 타격 이펙트
    }
    //선택 : 추가적인 각종 이펙트 구현, 필요한 에셋 직접 다운로드하여 사용하여도 됨.
}
