using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private bool GameIsPaused = false;
    //현재 플레이어 상태 관련
    public TextMeshProUGUI hp_bar;
    public TextMeshProUGUI ammo_bar;
    //일시정지 메뉴
    public GameObject PauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.UI = this;
    }

    private void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            //Cancel("esc")버튼이 눌러졋을 때 일시정지
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        ShowCurrentState();
    }

    //모든 UI 관련 작업을 수행할 클래스
    //구현해야할 UI 목록
    //필수 : 플레이어 체력 표기, 탄환 갯수 표기, 일시정지 버튼 또는 키 입력시 일시정지 구현, 일시정지 UI 창에서 재시작/메뉴 창으로 선택지 구현
    //선택 : 공격 쿨타임 표기, 몬스터 체력 표기, 특정 키 입력시 현재 장비한 모든 아이템/무기 정보를 볼 수 있는 UI창 생성
    public void Resume()
    {
        //일시정지 화면에서 빠져나가기 Resume 할때 사용
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        //게임을 일시정지 시킬 때 사용
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    void ShowCurrentState()
    {
        //현재 체력과 총알 상태 텍스트로 업데이트
        hp_bar.text = "Hi";
        ammo_bar.text = "ss";
        //위는 테스트 용
    }
    
    public void PrintDamage(float damage, Vector3 pos)
    {
        //선택사항 : 데미지 정보를 출력하도록 구현? => 힘들면 포기할 것
    }
}
