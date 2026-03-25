using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_Spawner : MonoBehaviour
{
    public static Enemy_Spawner Instance;
    // Start is called before the first frame update
    public GameObject[] Enemy_ships;
    public int stage_int;
    public float cooldown;
    public float spawnInterval = 2f;
    public GameObject bossPrefab;
    public string nextSceneName;
    int Enemy_amount;
    private int spawnCount = 0;
    private int deadCount = 0;
    private bool isBossSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        Enemy_amount = stage_int + 2;
    }

    private void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnCount < Enemy_amount)
        {
            cooldown += Time.deltaTime;
            if (cooldown >= spawnInterval)
            {
                SpawnEnemy();
            }
        }
    }

    public void SpawnEnemy()
    {
        if (Enemy_ships.Length == 0) return; // 배열이 비어있으면 실행 안 함

        float X = Random.Range(-40f, 40f);
        float Z = 50f;
        Vector3 wichi = new Vector3(X, 0, Z);

        // spawnCount가 배열 크기를 넘지 않도록 나머지 연산(%)을 쓰거나 랜덤 추천!
        int index = spawnCount % Enemy_ships.Length;
        Instantiate(Enemy_ships[index], wichi, Quaternion.identity);

        spawnCount++;
        cooldown = 0f;
    }
    public void EnemyDied(bool isBoss)
    {
        // 1. 일반 적이 죽었을 때
        if (!isBoss)
        {
            deadCount++;
            Debug.Log("일반 적 죽음. 현재 카운트: " + deadCount);

            // 정확히 일반 적 3마리가 죽었고, 아직 보스가 안 나왔을 때만 소환
            if (deadCount >= Enemy_amount && !isBossSpawned)
            {
                SpawnBoss();
            }
        }
        // 2. 보스가 죽었을 때
        else
        {
            // 중요: '보스가 소환된 적이 있고' + '보스 타입이 죽었을 때'만 클리어
            if (isBossSpawned)
            {
                Debug.Log("보스 처치 완료! 클리어 UI 표시");
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ShowClearUI();
                }
            }
        }
    }
    void SpawnBoss()
    {
        isBossSpawned = true;
        Vector3 bossPos = new Vector3(0, 0, 20f); // 보스 위치

        Instantiate(bossPrefab, bossPos, Quaternion.Euler(0, 180f, 0));
    }
}
