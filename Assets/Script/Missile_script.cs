using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Missile_script : MonoBehaviour
{
    public float speed = 5f; // 이동 속도
    private float originalSpeed;
    private bool isStopped = false;
    public float rotationSpeed = 200f; // 회전 속도
    public float homingDuration = 3f; // 추적 시간(초)
    public float damage_missile = 75f;
    
    private Transform target; // 위치정보
    private float elapsedTime = 0f; // 시간이 얼마나 흘렀는지 체크할 변수
    // Start is called before the first frame update
    void Start()
    {
        originalSpeed = speed;
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null) // 플레이어 오브젝트 ^^^^^ 가 비어있지 않다면 = 플레이어를 찾는데 성공했다면
        {
            target = playerObject.transform; // 플레이어 오브젝트의 위치정보를 target에 담겠다
        }
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // 1. 일시정지 체크 (가장 먼저!)
        if (isStopped) return;

        // 2. 시간이 흐름
        elapsedTime += Time.deltaTime;

        // 3. 타겟이 있을 때만 유도(회전) 실행
        // 타겟이 죽어서 null이 되면 이 if문만 안 타면 됩니다. 
        // 미사일 자체를 지울 필요는 없어요!
        if (target != null && elapsedTime < homingDuration)
        {
            LookAtTarget();
        }

        // 4. 이동은 타겟 유무와 상관없이 '내 앞방향'으로 계속 합니다.
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // 5. 20초 뒤에 자동으로 사라지는 로직은 그대로 둡니다.
        Destroy(gameObject, 20f);
    }

    void LookAtTarget()
    {
        // 방향 = 플레이어의 위치 - 미사일(나)의 위치 + 정규화(.normalized)
        Vector3 direction = (target.position - transform.position);
        direction.y = 0;
        direction.Normalize();
        // 회전값      변수명       디렉션의 화살표 방향을 바라보는 회전값
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        // RotateTowards <- 각도로 확 회전하지 않고 유도미사일처럼 부드럽게 회전하게 해줌
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        Quaternion nextRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, nextRotation.eulerAngles.y, 0);

    }
    private void OnTriggerEnter(Collider other)
    {
        Enemy_Spaceship_script Enemy = other.GetComponent<Enemy_Spaceship_script>();
        var player = other.GetComponent<PlayerShip>();
        if (other == null) return;
        // 만약 부딛힌 객체가 태그가 플레이어인 객체라면
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
            GameManager.Instance.Damage(damage_missile);
            // 게임 오브젝트 삭제
            Destroy(gameObject);
            
        }
        
        if (other.CompareTag("Enemy"))
        {
            if (elapsedTime < 0.1f)
            {
                return;
            }
            if (Enemy.Type_Selection == EnemyType.boss)
            {
                return;
            }
            if (Enemy != null)
            {
                Enemy.Enemy_Damage(damage_missile);
                Destroy(gameObject);
            }
      
        }

        
    }
    public void SetForceTarget(Transform newTarget)
    {
        target = newTarget;
        elapsedTime = 0f; // 유도 시간을 0으로 되돌려 다시 3초간 쫓아가게 만듭니다.
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
        transform.forward = -transform.forward;
    }
}
