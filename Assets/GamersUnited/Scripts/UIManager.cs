using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private bool GameIsPaused = false;

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
            //esc키가 눌러졋을 때 일시정지
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
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
    
    public void PrintDamage(float damage, Vector3 pos)
    {
        //선택사항 : 데미지 정보를 출력하도록 구현? => 힘들면 포기할 것
    }
}
