//몬스터를 생성하기 위한 코드
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class waveSpawner : MonoBehaviour
{
    public Text timeText;
    public GameObject[] enemyPrefab;
    public Transform spawnPoint;
    public Text waveText;

    public float time;
    public float timeBetweenWaves;
    private float countdown = 2f;
    private int waveIndex = 0;
    private void Awake()
    {
        time = 30f;//30초로 지정
    }
    private void Update()
    {
        waveText.text = "Stage : "+(waveIndex + 1);//텍스트 지정
        if (countdown <= 1f&&time>20)
        {
            //함수 추가-
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }
        if (waveIndex == 6)
        {
            return;
        }
        countdown -= Time.deltaTime;
        Timer();
    }
    IEnumerator SpawnWave()//코루틴 함수
    {
        SpawnEnemy();
        yield return new WaitForSeconds(0.5f);//0.5초후 함수 반복
        
    }
    private void SpawnEnemy()//몬스터 생성
    {

        Instantiate(enemyPrefab[waveIndex], spawnPoint.position, spawnPoint.rotation);
       
    }
    private void Timer()
    {
        if (time >= 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            time = 30f;
            waveIndex++;
        }
        timeText.text = "<color=lime>" + Mathf.Ceil(time).ToString() + "</color>";//텍스트 지정
        
    }
}
