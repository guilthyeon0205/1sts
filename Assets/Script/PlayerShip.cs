using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    public float speed = 35.0f;
    public bool isReflecting = false;
    public float rotationSpeed = 250f;
    public GameObject blackHolePrefab;
    public GameObject slowFieldPrefab;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float rotateInput = 0;
        if (Input.GetKey(KeyCode.A))
        {
            rotateInput = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rotateInput = 1f;
        }
        transform.Rotate(Vector3.up * rotateInput * rotationSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.E)) // E키를 누르면 반사 시도!
        {
            StartCoroutine(ReflectWindow());
        }

        GameManager.Instance.Cheat_hp();
        if (Input.GetKeyDown(KeyCode.T)) // Q키를 누르면 블랙홀 생성
        {
            SpawnBlackHole();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) // R키를 누르면 감속장 생성
        {
            // 플레이어 위치를 따라다니게 하려면 부모를 Player로 설정하세요.
            GameObject field = Instantiate(slowFieldPrefab, transform.position, Quaternion.identity);
            field.transform.SetParent(this.transform);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ClearAllMissiles();
        }

    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            // 좌표를 계산해서 넣지 말고, 그냥 "앞으로 이만큼의 속도로 달려!"라고 명령하세요.
            // 그러면 앞에 벽이 있을 때 물리 엔진이 알아서 멈춰 세웁니다.
            rb.velocity = transform.forward * speed;
        }
        else
        {
            // 키를 떼면 미끄러지지 않게 즉시 멈춤
            rb.velocity = Vector3.zero;
        }
    }
    IEnumerator ReflectWindow()
    {
        isReflecting = true;
        Debug.Log("반사 활성화!");
        // 반사 이펙트나 애니메이션이 있다면 여기서 재생
        yield return new WaitForSeconds(0.2f); // 0.2초라는 짧은 시간 동안만 판정
        isReflecting = false;
        Debug.Log("반사 비활성화");
    }

    void SpawnBlackHole()
    {
        // 플레이어의 약간 앞쪽에 블랙홀 생성
        Vector3 spawnPos = transform.position + transform.forward * 5f;
        Instantiate(blackHolePrefab, spawnPos, Quaternion.identity);
        Debug.Log("블랙홀 소환!");
    }
    void ClearAllMissiles()
    {
        // 1. 현재 맵(Scene)에 존재하는 모든 "Missile" 태그 오브젝트를 배열로 가져옴
        GameObject[] allMissiles = GameObject.FindGameObjectsWithTag("Missile");

        // 미사일이 하나라도 있다면 실행
        if (allMissiles.Length > 0)
        {
            Debug.Log($"폭탄 발동! {allMissiles.Length}개의 미사일을 제거합니다.");

            // 2. 배열을 돌면서 하나씩 즉시 파괴
            foreach (GameObject missile in allMissiles)
            {
                Destroy(missile);
            }
        }
        else
        {
            Debug.Log("제거할 미사일이 없습니다.");
        }
    }
}
