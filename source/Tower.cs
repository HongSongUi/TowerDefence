//모든 타워의 기본 스크립트
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
 
    public enum TowerState//타워의 상태
    {
        idel, fire
    }
    private TowerState state = TowerState.idel;
    public float range = 10f;
    public string monsterTag = "Monster";
    private Animator animator;
    private GameObject FirePoint;
    public GameObject Bulletprefabs;
    public AudioSource shootingAudio;
    public AudioClip fireClip;
    public Transform enemy;
    public float shootSpeed;
    private float shootCooltime = 0f;
    private float currentShootCoolTime;
    public int towerlv=1;
    public int damage;
    public string type;
    // Start is called before the first frame update
    void Start()
    {
       
        animator = GetComponent<Animator>();//애니메이터 컴포넌트를 가져와 저장
        InvokeRepeating("UpdateTarget", 0f, 0.5f);//UpdateTarget을 0.5초마다 실행
        FirePoint = transform.Find("FirePoint").gameObject;//발사체가 발사되는 곳
        SetAttackSpeed(shootSpeed);//공격속도저장
       
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null)
        {
            return;
        }
        switch (state)
        {
            case TowerState.fire:
                ShootControl();
                break;
        }

    }
    private void SetAttackSpeed(float _shootSpeed)
    {
        shootSpeed = _shootSpeed;
        shootCooltime = 1f / shootSpeed;
        currentShootCoolTime = shootCooltime;
        animator.SetFloat("aniSpeed", shootSpeed);

    }
    void UpdateTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Monster");//게임상에 Monster라는 태그를 갖고있는 모든 개체를 가져와 배열에 저장
        float shortesDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;//가장 가까운 적
        foreach(GameObject target in targets)
        {
            float distance = Vector3.Distance(transform.position , target.transform.position);//거리를 계산
            if (type == "불")
            {
                if (distance <= range)//범위에 들어오면 대상을 한 번만 지정
                {
                    shortesDistance = distance;
                    nearestEnemy = target;
                }
            }
            else
            {
                if (distance < shortesDistance)//거리를 계산하여 지속적으로 가까운 적을 검사
                {
                    shortesDistance = distance;
                    nearestEnemy = target;
                }
            }
        }
        if(nearestEnemy!=null && shortesDistance <= range)
        {

            enemy = nearestEnemy.transform;//가까운 적의 위치를 저장
            
            Vector3 dir = enemy.position - transform.position;//거리 계산
            Quaternion rotation = Quaternion.LookRotation(dir.normalized);//방향 계산
            transform.rotation = rotation;//적이 있는 방향을 바라보도록 지정
            state = TowerState.fire;//공격 상태
        }
        else
        {
            enemy = null;
            state = TowerState.idel;
            animator.SetInteger("animation", 1);//대기 애니메이션 재생
        }
    }
    private void Shoot()
    {
        animator.SetInteger("animation", 2);//공격 애니매이션 재생
        shootingAudio.clip = fireClip;//공격 소리재생
        shootingAudio.Play();
        GameObject Bullet = (GameObject)Instantiate(Bulletprefabs, FirePoint.transform.position, FirePoint.transform.rotation);//발사체 생성
        if (type == "불")
        {
            FireAttack bullet = Bullet.GetComponent<FireAttack>();//FireAttack스크립트를 가져옴
            if (bullet != null)
            {
                bullet.Seek(enemy);
                
            }

        }
        else if (type == "별")
        {
            StarAttack bullet = Bullet.GetComponent<StarAttack>();//StarAttack스크립트를 가져옴
            if (bullet != null)
                bullet.Seek(enemy);
        }
        else
        {
            Attack bullet = Bullet.GetComponent<Attack>();
            if (bullet != null)
            {
                bullet.Seek(enemy);
            }
            if (type == "물")
            {
                enemy.GetComponent<Enemy>().Debuff(towerlv);//Enemy스크립트의 Debuff함수 호출
            }
        }
        
       
        
    }
    private void ShootControl()//공격속도 계산
    {
        if (currentShootCoolTime >= shootCooltime)
        {
            currentShootCoolTime = 0;
            Shoot();
        }
        currentShootCoolTime += Time.deltaTime;
    }
    private void OnMouseDown()//마우스 클릭시
    {
        GameManager.instance.getTowerInfo(damage, range, towerlv, shootSpeed, type);//게임매니저에게 정보를 전달
    }

}
