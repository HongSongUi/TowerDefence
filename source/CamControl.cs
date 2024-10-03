//카메라를 움직이게 하기 위한 코드
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    private float zoomSpeed = 10.0f;
    private float moveSpeed = 20.0f;
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();//카메라를 가져와 변수에 저장
    }
    // Update is called once per frame
    void Update()
    {
        Zoom();
        CamMove();
        CamInit();
    }
    private void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;//마우스 휠 조작
        if (distance != 0)
        {
            mainCamera.fieldOfView += distance;
            if (mainCamera.fieldOfView > 60)
            {
                mainCamera.fieldOfView = 60;
            }

        }
    }
    private void CamMove()
    {
        float keyH = Input.GetAxis("Horizontal");//수평방향이동
        float keyV = Input.GetAxis("Vertical");//수직방향 이동
        keyH = keyH * moveSpeed * Time.deltaTime;
        keyV = keyV * moveSpeed * Time.deltaTime;
        transform.Translate(Vector3.right * keyH);//카메라 이동
        transform.Translate(Vector3.up * keyV);//카메라 이동
    }
    private void CamInit()
    {
        if (Input.GetKeyDown(KeyCode.Space))//space를 눌렀을때 발동
        {
            mainCamera.fieldOfView = 60;
            transform.position = new Vector3(30, 83, -117);//원래 포지션으로 이동
        }
    }
}
