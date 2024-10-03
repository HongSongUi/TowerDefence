//일반적인 발사체를 위한 스크립트

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    
    public Transform target;
    public GameObject hit;
    private int damage;
    public float speed = 15f;
    public void Seek(Transform _target) //적을 찾기 위한 함수 
    {
        target = _target;//찾은 적의 위치를 가져와서 저장
    }
    public void Awake()//이 스크립트가 발동 될 때 한 번만 불러오는 함수
    {
        Tower at = GameObject.FindGameObjectWithTag("Dragon").GetComponent<Tower>();//게임 내 Dragon이라는 태그를 가진 gameobject의 Tower스크립트를 가져오는 함수
        damage = at.damage; //타워의 데미지를 발사체의 데미지로 설정
    }
   
    void Update()//매 프레임마다 불러오는 함수
    {
        if (target == null)
        {
            Destroy(gameObject);//자기자신을 게임 상에서 지우는 함수
            return;
        }

        Vector3 dir = target.position - transform.position;//발사체와 목표물 까지 거리를 계산
        float distanceThisFram = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFram)//발사체가 적에게 닿았을때
        {
            HitTarget();
            return;
        }
        //-----------------발사체를  부드럽게 이동시키는 부가 함수----------------------------//
        transform.Translate(dir.normalized * distanceThisFram, Space.World);
        Vector3 directionVec = target.position - transform.position;
        Quaternion qua = Quaternion.LookRotation(directionVec);
        transform.rotation = Quaternion.Slerp(transform.rotation, qua, Time.deltaTime * 3f);
        //------------------------------------------------------------------------------------//
    }

    void HitTarget()
    {
        GameObject effectIns = Instantiate(hit, transform.position, transform.rotation);//hit 이라는 게임오브젝트 타입 변수를 생성하고 변수에 넣는것
        Destroy(effectIns, 2f);//2초 뒤 파괴
    
        Destroy(gameObject);
        GameObject hitTarget = target.gameObject;
        hitTarget.GetComponent<Enemy>().GetDamage(damage);//타겟이 된 객체를 불러와서 컴포넌트 중 GetDamage함수를 불러 데미지를 매개변수로 전달
    }
   
}