namespace DW  //DW 네임스페이스 사용

public class DroppedWeapon : MonoBehaviour
{
	public Weapon GetWeapon();
	//떨어져있는 Weapon 객체의 스크립트를 받아옴

	public void DestroyObject()
	//DroppedWeapon 오브젝트 삭제
}

public class DroppedWeaponGenerator //DroppedWeapon 객체를 생성하는 클래스
{
	public DroppedWeaponGenerator();
	//그냥 생성자
	
	public void SetPos(Vector3 pos);
	//DroppedWeapon이 생성될 위치 설정, 설정안할 경우 (0, 0, 0)이 기본값

	public void SetGradeChance(int com, int rar, int uni);
	//무기등급의 확률을 설정 (1, 1, 1)이 기본값
	// 예) (5, 4, 1) common : rare : unique 등장확률 = 5 : 4 : 1
	// 예) (0, 0, 1) unique등장확률 100%

	public void SetWeaponType()
	//무기타입을 무작위로 설정

	public void SetWeaponType(WeaponType type);
	//무기타입을 설정, 설정안할경우 무작위가 기본값

	public void GenDW();
	//DroppedWeapon 객체 생성
}

--DroppedWeaponGenerator의 사용예--
using DW;

DroppedWeaponGenerator generator = new DroppedWeaponGenerator();

generator.GenDW(); 
//무작위의 무기 (0, 0, 0)위치에 생성

generator.SetPos(new Vector3(5, 5, 5));
generator.SetGradeChance(10, 5, 1);
generator.GenDW();
//(5, 5, 5)위치에 10:5:1의 등급확률로 무기생성

--DroppedWeapon의 사용예--
--DroppedWeapon 오브젝트와 충돌했을 경우--
using DW;

void OnTriggerStay(Collider other)
{
	DroppedWeapon dw;
	Weapon weapon;
	if(dw = other.gameObject.GetComponent<DroppedWeapon>())
	{
		weapon = dw.GetWeapon();
		//무기입수
		dw.DestroyObject()
		//DroppedWeapon 오브젝트 삭제
	}
}
//위의 코드는 떨어진 무기 근처로 가면 바로 입수하는 코드의 예시