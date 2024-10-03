//불타입 용의 발사체 실제 타격 역할

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttack : MonoBehaviour
{
    public Transform target;
    public GameObject hit;
    public int damage;
    public int lv;
    public float speed = 15f;
    public void Seek(Transform _target)
    {
        target = _target;
    }

    public void Start()
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
        GameObject hitTarget = target.gameObject;
        hitTarget.GetComponent<Enemy>().GetDamage(damage);
        GameObject effectIns = Instantiate(hit, transform.position, transform.rotation);
        Destroy(effectIns, 2f);

        Destroy(gameObject);
        
    }
    //위의 내용은 기존 Attack스크립트와 동일
    private void OnCollisionEnter(Collision collision)//게임내 물체끼리 닿은 순간 호출되는 함수
    {
        var obj = collision.gameObject;//닿은 물체의 정보를 가져와서 저장

        if (obj.tag == "Monster" &&obj!=target)//태그검사와 용의 타겟인지 검사
        {
            obj.GetComponent<Enemy>().GetDamage(damage*(0.5f*lv));//Enemy 스크립트의 GetDamage함수 호출
        }

    }
}
