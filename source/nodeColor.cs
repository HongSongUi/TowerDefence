//노드의 색을 바꾸기 위한 스크립트
//노드는 게임의 초록색 판 부분
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeColor : MonoBehaviour
{
    public Color startColor;//컬러값을 받아와서 저장
    public Color selectColor;
    public Renderer rend;//색을 바꿔주기 위해 Renderer 변수 생성

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();//컴포넌트를 가져와서 저장
        startColor = rend.material.color;
    }

   

    private void OnMouseEnter()//마우스 커서가 들어올 때 호출
    {
        rend.material.color = selectColor;
    }
    private void OnMouseExit()//마우스 커서가 나갈 때 호출
    {
        rend.material.color = startColor;
    }

}
