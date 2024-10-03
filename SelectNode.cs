//노드 클릭 시 타워 생성을 위한 코드
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectNode : MonoBehaviour
{
    
    private GameObject turret;
    int useGold = 30;
    private bool canBuild=true;//선택된 곳에 지을수 있는지 검사를 위한 bool
    public bool istower = true;//타워가 이미 있는지 검사
    private bool isStop = false;//게임이 멈춰있는지 검사
    // Start is called before the first frame update
    private void OnMouseDown()
    {
        if (turret != null)
        {
            return;
        }
        isStop = GameManager.instance.isStop;//게임매니저의 isStop값을 가져옴
        if (!isStop)
        {
            GameManager.instance.UseGold(useGold);//게임 매니저의 UseGold함수를 가져옴
            canBuild = GameManager.instance.build;//게임매니저의 build값을 가져옴
            
            if (canBuild && istower == true)
            {
                
                GameObject turretToBuild = GameManager.instance.GetTurretToBuild();//게임매니저의 함수를 가져옴
                Vector3 position = transform.position + Vector3.up + Vector3.up;//포지션을 지정
                turret = Instantiate(turretToBuild, position, transform.rotation);//게임 오브젝트를 생성하고 터렛값에 저장
                turret.transform.parent = transform;//자신의 위치를 터렛변수의 부모로 지정
                istower = false;
            }
            else
            {
                return;
            }
        }
    }

}
