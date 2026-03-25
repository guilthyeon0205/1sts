using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stones_script : MonoBehaviour
{
    public float rotateInput;
    private float rotationSpeed = 250f;
    float speed;
    private Vector3 randomDir;
    // Start is called before the first frame update
    void Start()
    {
        rotateInput = Random.Range(-1f, 1f);
        speed = Random.Range(2f, 8f);
        // 랜덤X 라는 변수에 -1부터 1까지의 무작위 수 대입
        float randomX = Random.Range(-1f, 0);
        // 랜덤Z 라는 변수에 -1부터 1까지의 무작위 수 대입
        float randomZ = Random.Range(-1f, 1f);
        // randomDir이라는 방향(변수에 랜덤X, y좌표는 0, 랜덤Z값, 화살표 길이는 1
        randomDir = new Vector3(randomX, 0, 0).normalized;
        // 박스 콜라이더 컴포넌트
        GetComponent<BoxCollider>();
        // 10초 뒤에 운석 자동으로 삭제하기
        Destroy(gameObject, 35f);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Rotate(Vector3.up * rotateInput * rotationSpeed * Time.deltaTime);
        // start에서 정한 randomDir에 속도와 time.deltaTime을 곱해 randomDir (게임의 절대적인)방향, 정해진 속도로 이동
        transform.Translate(randomDir * speed * Time.deltaTime, Space.World);
    }
    // 충돌 함수(제미나이가 충돌 함수는 업데이트 함수와 역할이 달라 업데이트 밖에 써야 한다 그랬어요
    private void OnTriggerEnter(Collider other)
    {
        Enemy_Spaceship_script Enemy = other.GetComponent<Enemy_Spaceship_script>();
        // 만약 부딛힌 객체가 태그가 플레이어인 객체라면
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Damage(25);
            // 게임 오브젝트 삭제
            Destroy(gameObject);
        }

        if (other.CompareTag("Enemy"))
        {
            Enemy.Enemy_Damage(25f);
            Destroy(gameObject);
        }

    }
}
