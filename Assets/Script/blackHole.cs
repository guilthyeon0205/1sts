using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float pullSpeed = 25f; // 끌어당기는 힘
    public float duration = 3f;  // 블랙홀 지속 시간

    void Start()
    {
        // 일정 시간 뒤에 블랙홀 스스로 파괴
        Destroy(gameObject, duration);
    }

    private void OnTriggerStay(Collider other)
    {
        // 미사일 태그를 가진 오브젝트만 흡수 (미사일에 "Missile" 태그가 있어야 함)
        if (other.CompareTag("Missile"))
        {
            // 1. 방향 계산 (미사일 -> 블랙홀 중심)
            Vector3 direction = transform.position - other.transform.position;

            // 2. 미사일을 중심으로 이동시킴
            other.transform.position = Vector3.MoveTowards(other.transform.position, transform.position, pullSpeed * Time.deltaTime);

            // 3. 중심에 거의 도달하면 미사일 파괴
            if (Vector3.Distance(transform.position, other.transform.position) < 5f)
            {
                Destroy(other.gameObject);
                Debug.Log("미사일 흡수됨!");
            }
        }
    }
}