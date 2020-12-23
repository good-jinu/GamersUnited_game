using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.UI = this;
    }

    //모든 UI 관련 작업을 수행할 클래스
    //구현해야할 UI 목록
    //필수 : 체력 표기, 탄환 갯수 표기, 일시정지 버튼 또는 키 입력시 일시정지 구현, 일시정지 UI 창에서 재시작/메뉴 창으로 선택지 구현
    //선택 : 공격 쿨타임 표기, 몬스터 체력 표기, 특정 키 입력시 현재 장비한 모든 아이템/무기 정보를 볼 수 있는 UI창 생성

    
    public void PrintDamage(float damage, Vector3 pos)
    {
        //선택사항 : 데미지 정보를 출력하도록 구현? => 힘들면 포기할 것
    }
}
