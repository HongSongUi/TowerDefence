//UI와 돈 타워생성 조합등 각종 처리 담당을 위한 스크립트 
//<<SingleTon>>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header ("GameObjects")]
    public GameObject turret;
    public GameObject[] standardTurretPrefabs;
    public GameObject[] UpgradeTower1;
    public GameObject[] UpgradeTower2;
    public GameObject[] lifeImage;
    private GameObject turretToBuild;
    public GameObject GameSetPannel;
    public GameObject InfoPannel;
    public GameObject StopPanel;
    private GameObject Parent;
    private GameObject selectParent;
    private GameObject draTemp;
    
    [Header("UI")]
    public Text goldText;
    public Text monHp;
    public Text monSpeed;
    public Text towerDamage;
    public Text towerRange;
    public Text towerLv;
    public Text towerAttackSpeed;
    public Text towerType;
    public Camera mainCam;
    public Camera subCam;

    [Header("Info")]
    private int life = 5;
    private RaycastHit hit;
    int rand;
    private float MHp;
    private float MSpeed;
    private int tDam;
    private float tRan;
    private int tlv;
    private float tAtsp;
    private string type;
    private int gold = 150;

    [Header("etc")]
    public Transform selectTr;
    public static GameManager instance;
    public bool build = true;
    public bool isGameActive = true;
    public bool combi = true;
    public float offsetX;
    public float offsetY;
    public float offsetZ;
    public int count = 0;
    public bool isTowerCheck = false;
    public bool isStop = false;

    // Start is called before the first frame update
    private void Awake()
    {  
        if (instance != null)//자기 자신이 있는지 검사
        {
            return;
        }
        instance = this;//없으면 생성
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
        ShowStopPanel();
        ischeck();
        if (isGameActive == false)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            offPannel();
        }
        
        if (selectTr.gameObject.tag == "Monster")//몬스터클릭 시 몬스터의 위치를 나타내주는 서브 카메라를 이동
        {
            Vector3 FixedPos =
                  new Vector3(
                  selectTr.transform.position.x+7,
                  selectTr.transform.position.y + 4,
                  selectTr.transform.position.z );
            subCam.transform.localRotation = Quaternion.Euler(5, -90, 0);
            subCam.transform.position = Vector3.Lerp(subCam.transform.position, FixedPos, Time.deltaTime * 5);
        }
        if (gold < 100)
        {
            combi = false;
        }

    }
    public void UpdateUI()//gold 텍스트 업데이트
    {
        goldText.text = "<color=#FFE400>" + gold.ToString() + "</color>";

    }
    public void AddGold(int newGold)//골드 추가
    {
        gold += newGold;
        build = true;
        if (gold >= 100)
        {
            combi = true;
        }
        
        UpdateUI();
    }
    public void UseGold(int useGold)//골드 소모 및 골드 검사
    {
        if(gold< useGold)
        {
            
            build = false;

       
            return;
        }
        else
        {
            gold -= useGold;
        }
        UpdateUI();
    }
    public GameObject GetTurretToBuild()//용 생성 코드
    {
         rand = Random.Range(0, 4);
         turretToBuild = standardTurretPrefabs[rand];//미리 배열에 들어있는 용들을 랜덤으로 넘겨줌
         return turretToBuild;
    }
    public GameObject ComTower()//타워 조합
    {
        int index = 0; 
        switch (type)
        {
            case "불":
                index = 0;
                break;
            case "일반":
                index = 1;
                break;
            case "물":
                index = 2;
                break;
            case "별":
                index = 3;
                break;
        }
        if (tlv == 1)
        {
            turretToBuild = UpgradeTower1[index];
            return turretToBuild;
        }
        else 
        {
            turretToBuild = UpgradeTower2[index];
            return turretToBuild;
        }
       
    }
    public void DimLife()//라이프 감소
    {
        life--;
        if (life <= 0)
        {
            GameSetPannel.SetActive(true);//게임 종료 출력
            isGameActive = false;//게임 비활성화
            
            Time.timeScale = 0;
        }
        else
        {
            GameSetPannel.SetActive(false);
            Time.timeScale = 1;
        }
        UpdateUI();
        lifeImage[life].SetActive(false);

    }
    public void Restart()
    {
        SceneManager.LoadScene("Main");//게임 화면을 load
        Time.timeScale = 1;
    }
    public void onPannel()// 정보창 출력
    {
        InfoPannel.SetActive(true);
    }
    public void offPannel()
    {
        InfoPannel.SetActive(false);
    }
    public void ischeck()//클릭한 개체가 용인지 몬스터인 검사
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);//raycast를 사용해 마우스가 클릭한 곳 검사

            if (Physics.Raycast(ray, out hit))//hit에 ray변수가 가져온 값 대입
            {
                string tag = hit.collider.gameObject.tag;//마우스가 클릭 한 개체의 태그를 가져와서 저장
                selectTr = hit.collider.gameObject.transform;//마우스가 클릭한 개체의 위치 저장(조합 시 사용)
                if (tag == "Dragon")
                {
                    isTowerCheck = true;
                    onPannel(); 
                    setTowerInfo();
                    DragonCamFollow();
                    draTemp = hit.collider.gameObject;
                }
                else if(tag == "Monster")
                {
                    isTowerCheck = false ;
                    onPannel();
                    SetMonsetInfo();
                    
                }
                else
                {
                    offPannel();
                    isTowerCheck = false;
                }
            }
        }
    }
    public void DragonCamFollow()//정보창에 용 화면을 띄어주기 위한 코드, 서브카메라를 용의 위치로 이동
    {
        Vector3 FixedPos =
                new Vector3(
               selectTr.position.x + offsetX,
                selectTr.position.y + offsetY,
                selectTr.position.z + offsetZ);
            subCam.transform.localRotation = Quaternion.Euler(30, -180, 0);
            subCam.transform.position = FixedPos;
    }

   public void getTowerInfo(int dam, float rang, int lv, float attackSpeed,string tp)//타워의 정보를 가져오는 함수
    {
        tDam = dam;
        tRan = rang;
        tlv = lv;
        tAtsp = attackSpeed;
        type = tp;
    }
    public void setTowerInfo()//가져온 코드를 정보창의 텍스트에 저장
    {
        monHp.text = "";
        monSpeed.text = ""; 
        towerDamage.text = "데미지 : " + tDam;
        towerRange.text = "사거리 : " + tRan;
        towerAttackSpeed.text = "공격 속도 : " + tAtsp;
        towerLv.text = "레벨 : " + tlv;
        towerType.text = "타입 : " + type;
    }
    public void GetMonsterInfo(float hp, float speed)//몬스터 정보를 가져오는 함수
    {
        MHp = hp;
        MSpeed = speed;
    }
    public void SetMonsetInfo()//가져온 몬스터 정보를 정보 창에 출력
    {
        
        towerDamage.text = "" ;
        towerRange.text = "";
        towerAttackSpeed.text = "";
        towerLv.text = "";
        towerType.text = "";
        monHp.text = "체력 : " + MHp;
        monSpeed.text = "속도 : "+ MSpeed;
    }
    public void Combination()//조합
    {
       
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Dragon");//게임상 Drangon이라는 태그를 가진 모든 개체를 가져와 배열에 저장
        List<GameObject> probs = new List<GameObject>();//게임오브젝트 타입 리스트 생성
        for (int i = 0; i < temp.Length; i++)
        {
            if (draTemp.GetComponent<Tower>().type == temp[i].GetComponent<Tower>().type && draTemp.GetComponent<Tower>().towerlv == temp[i].GetComponent<Tower>().towerlv)
            {//타입과 레벨 검사
                if(draTemp.transform != temp[i].transform)//조건에 충족하고 자기 자신이 아니라면 리스트에 저장
                {
                    probs.Add(temp[i]);
                    count++;        
                }
            }
        }
        if (count >=1)
        {          
            for (int p=0; p < 1; p++)
            {
                if (draTemp.transform.position != probs[p].transform.position)//리스트 검사
                {
                    Parent = probs[p].transform.parent.gameObject;//리스트에 들어있는 게임오브젝트의 부모를 가져옴
                    selectParent = draTemp.transform.parent.gameObject;//자기 자신의 부모를 가져옴
                    Destroy(probs[p]);
                    Destroy(draTemp);
                    GameObject turretToBuild = ComTower();//조합 함수 호출
                    turret = Instantiate(turretToBuild, draTemp.transform.position, transform.rotation);//타워 생성
                    turret.transform.parent = selectParent.transform;//부모를 재 지정
                    Parent.GetComponent<SelectNode>().istower = true;//부모의 컴포넌트중 SelectNode를 가져와 istower변수에 접근
                }
               
            }
        }
        count = 0;
        
    }
    public void MoneyCheck()//돈이 충분히 있는지 체크
    {
        if (combi == true)
        {
            Combination();
            UseGold(100);
        }
        else
        {
            return;
        }
    }
    public void Stop()//일시 정지함수
    { 

        isStop = true;
        Time.timeScale = 0;//게임을 정지
        
    }
    public void Resume()
    {
        Time.timeScale = 1;//게임 시작
        isStop = false;
    }
    public void GotoMenu()
    {
        Time.timeScale = 1;
        isStop = false;
        SceneManager.LoadScene(0);//씬 이동
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void ShowStopPanel()//일시 정지 화면을 출력
    {
        if (isStop == false)
        {
            StopPanel.SetActive(false);
        }
        else
        {
            StopPanel.SetActive(true);
        }
    }
}
