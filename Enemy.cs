//몬스터 스크립트 각종 정보와 waypoint를 따라 이동하기 위한 코드
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public float Speed = 10.0f;
    private Transform target;
    private int wavepointIndex = 0;
    public float startHp;
    private float currentHp;
    public GameObject HealthBar;
    private Animator animator;
    public int gold = 50;
    private float currentSpeed;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        target = WayPoint.points[0];//이동하기위해 waypoint를 저장
        animator = GetComponent<Animator>();//걷는 애니메이션을 위해 애니메이터 컴포넌트를 가져와서 저장
        currentHp = startHp;//hp바를 위해 변수 저장
        animator.SetInteger("MonsterAnim", 2);//몬스터 걷기 애니메이션 재생
        GameManager.instance.GetMonsterInfo(startHp, Speed);//게임매니저에게 몬스터의 정보를 넘겨줌(몬스터의 정보 출력을 위해)
        currentSpeed = Speed;//스피드 대입
    }
    // Update is called once per frame
    void Update()
    {
        Move();
        if (timer > 0)//속도가 줄었을 때의 제한 시간
        {
            timer -= Time.deltaTime;
        }
        else if (timer <= 0)
        {
            currentSpeed = Speed;
        }
    }
    private void GetNextWaypoint()
    {
        if (wavepointIndex >= WayPoint.points.Length - 1)//waypoint의 끝 즉 골인 지점에 도달
        {
            Destroy(gameObject);
            GameManager.instance.DimLife();//게임매니저의 함수 호출(Life감소 역할)
            return;
        }
        wavepointIndex++;
        target = WayPoint.points[wavepointIndex];

    }
    public void Move()
    {
        Vector3 dir = target.position - transform.position;//거리 계산
        transform.Translate(dir.normalized * currentSpeed * Time.deltaTime, Space.World);//거리*속도만큼 계속 이동시켜주는 코드
        if (Vector3.Distance(transform.position, target.position) <= 0.4f)//다음 목표 설정
        {
            GetNextWaypoint();
        }
        Quaternion rotation = Quaternion.LookRotation(dir.normalized);//몬스터 이동 시 이동방향을 바라보게하는 함수
        transform.rotation = rotation;
    
    }
    public void GetDamage(float damage)//데미지를 입었을때 호출되는 함수
    {
        currentHp -= damage;
        HealthBar.GetComponent<Image>().fillAmount = currentHp / startHp;//몬스터의 체력바 설정
        if (currentHp <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.AddGold(gold);//피가0이되면 자기 자신을 파괴하고 게임매니저의 AddGold함수를 호출하여 골드를 더해줍니다.
        }
    }
    public void Debuff(float time)//물 타입 용에게 공격받았을때 호출
    {
        currentSpeed = 10;
        timer = time;
    }

}
