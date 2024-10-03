//별 타입 용의 투사체를 위한 스크립트
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public GameObject hit;
    public int damage = 100;
    public float speed = 15f;
    public int lv;
    public void Seek(Transform _target)
    {
        target = _target;
    }
    public void Awake()
    {
        Tower at = GameObject.FindGameObjectWithTag("Dragon").GetComponent<Tower>();
        damage = at.damage;
        lv = at.towerlv;
        
    }
    // Start is called before the first frame update
    void Update()
    {
        
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFram = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFram)
        {
           
            HitTarget();
            
            return;
        }

        transform.Translate(dir.normalized * distanceThisFram, Space.World);
        Vector3 directionVec = target.position - transform.position;
        Quaternion qua = Quaternion.LookRotation(directionVec);
        transform.rotation = Quaternion.Slerp(transform.rotation, qua, Time.deltaTime * 3f);

    }

    void HitTarget()
    {

        GameObject effectIns = Instantiate(hit, transform.position, transform.rotation);
        GameObject hitTarget = target.gameObject;
       
        hitTarget.GetComponent<Enemy>().GetDamage(damage);
        
        Destroy(effectIns, 2f);
       
        Destroy(gameObject);

    }
    //위의 내용은 기존 Attack스크립트와 동일
    private void OnCollisionEnter(Collision collision)
    {
        var obj = collision.gameObject;
       
        
        if (obj==target.gameObject)//닿은 물체가 자신의 target일때
        {
            
            Collider[] colliders = Physics.OverlapSphere(target.position, 10*lv);//10*용의 레벨만큼의 범위에 있는 모든 개체들을 가져옴
            for(int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Monster")//그 개체의 태그가 몬스터일때 
                {
                    colliders[i].GetComponent<Enemy>().GetDamage(damage*0.5f*lv);//데미지함수 호출
                }
            }
        }

    }
}
