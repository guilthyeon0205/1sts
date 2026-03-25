using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class goksa: MonoBehaviour
{
    [Header("이동 설정")]
    public float speed = 15f;          // 전진 속도
    public float curveStrength = 50f; // 휘어지는 회전 속도 (클수록 원을 그리며 크게 휨)
    public float damage = 100f;
    private float originalSpeed;
    private bool isStopped = false;
    private float elapsedTime = 0f;
    private int curveDirection;       // 1이면 오른쪽, -1이면 왼쪽
    private Transform target;
    void Start()
    {
        originalSpeed = speed;
        // 생성될 때 랜덤하게 왼쪽(-1) 또는 오른쪽(1) 휘어짐 결정
        curveDirection = Random.Range(0, 2) == 0 ? 1 : -1;

        // 시작할 때 Y축 이외의 회전(기울기)은 초기화하여 수평 유지
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }

    void Update()
    {
        if (isStopped) return;
        elapsedTime += Time.deltaTime;
        // 1. 매 프레임 정해진 방향으로 조금씩 회전 (이게 핵심!)
        // 좌우(Y축)로만 계속 회전시키면 궤적이 원형/곡선형이 됩니다.
        transform.Rotate(0, curveDirection * curveStrength * Time.deltaTime, 0);

        // 2. 현재 바라보는 방향으로 전진
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // 10초 뒤 자동 파괴
        Destroy(gameObject, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 발사 직후 본인 충돌 방지
        var player = other.GetComponent<PlayerShip>();
        if (other == null) return;
        if (other.CompareTag("Player"))
        {
            if (player == null) player = other.GetComponentInParent<PlayerShip>();
            if (player == null) player = other.GetComponentInChildren<PlayerShip>();
            if (player != null && player.isReflecting)
            {
                // [공격 반사 성공!]
                Reflect(other.transform);
                return; // 아래의 데미지 입히고 파괴되는 코드 실행 안 함
            }
            GameManager.Instance.Damage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            // 발사 후 0.5초 이후에만 적에게 데미지 (팀킬 방지)
            // (필요 시 변수 추가하여 체크)
            Enemy_Spaceship_script enemy = other.GetComponent<Enemy_Spaceship_script>();
            if (enemy != null)
            {
                if(elapsedTime > 0.1f)
                {
                    enemy.Enemy_Damage(damage);
                    Destroy(gameObject);
                }
            }
        }
    }
    public void SetTimeStop(bool stop)
    {
        isStopped = stop;
        if (stop)
        {
            speed = 0; // 속도를 0으로
        }
        else
        {
            speed = originalSpeed; // 원래 속도로 복구
        }
    }
    void Reflect(Transform playerTransform)
    {
        Debug.Log("미사일 반사됨!");

        // 1. 타겟을 적(Enemy)으로 변경
        GameObject enemyTarget = GameObject.FindWithTag("Enemy");
        if (enemyTarget != null)
        {
            target = enemyTarget.transform;
        }
        // 4. 방향을 즉시 반대로 꺾음
        transform.forward = -transform.forward;
    }
}