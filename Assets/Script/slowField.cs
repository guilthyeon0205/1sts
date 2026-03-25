using System.Reflection;
using UnityEngine;

public class slowField : MonoBehaviour
{
    public float slowRate = 0.01f; // 원래 속도의 20%로 감소
    public float duration = 5f;        // 감속장 지속 시간

    void Start()
    {
        // 일정 시간 뒤 감속장 삭제
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile"))
        {
            // 미사일 컴포넌트를 직접 가져옵니다.
            Missile_script missile = other.GetComponent<Missile_script>();
            Sniper_Missile S_missile = other.GetComponent<Sniper_Missile>();
            goksa G_missile = other.GetComponent<goksa>();
            if (missile != null)
            {
                // 미사일 내부의 speed 변수를 직접 곱합니다.
                missile.speed *= slowRate;
                Debug.Log(other.name + " 감속됨!");
            }
            if (S_missile != null)
            {
                // 미사일 내부의 speed 변수를 직접 곱합니다.
                S_missile.speed *= slowRate;
                Debug.Log(other.name + " 감속됨!");
            }
            if (G_missile != null)
            {
                // 미사일 내부의 speed 변수를 직접 곱합니다.
                G_missile.speed *= slowRate;
                Debug.Log(other.name + " 감속됨!");
            }
        }
    }

    // 감속장을 나가는 순간 원래 속도로 복구
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Missile"))
        {
            Missile_script missile = other.GetComponent<Missile_script>();
            Sniper_Missile S_missile = other.GetComponent<Sniper_Missile>();
            goksa G_missile = other.GetComponent<goksa>();
            if (missile != null)
            {
                // 원래 속도로 되돌리기 위해 다시 나눕니다.
                missile.speed /= slowRate;
                Debug.Log(other.name + " 속도 복구!");
            }
            if (S_missile != null)
            {
                // 원래 속도로 되돌리기 위해 다시 나눕니다.
                S_missile.speed /= slowRate;
                Debug.Log(other.name + " 속도 복구!");
            }
            if (G_missile != null)
            {
                // 원래 속도로 되돌리기 위해 다시 나눕니다.
                G_missile.speed /= slowRate;
                Debug.Log(other.name + " 속도 복구!");
            }
        }
    }
    private void OnDestroy()
    {
        // 1. 현재 맵에 있는 모든 미사일을 다 찾습니다. (태그 기준)
        GameObject[] missiles = GameObject.FindGameObjectsWithTag("Missile");

        foreach (GameObject go in missiles)
        {
            // 2. 미사일과 나의 거리를 잽니다. 
            // 내 콜라이더 반지름(Radius) 안에 있는 미사일만 골라냅니다.
            float distance = Vector3.Distance(transform.position, go.transform.position);
            float radius = GetComponent<SphereCollider>().radius * transform.localScale.x;

            if (distance <= radius)
            {
                // 3. 범위 안에 있다면 속도를 복구시킵니다.
                Missile_script m = go.GetComponent<Missile_script>();
                Sniper_Missile sm = go.GetComponent<Sniper_Missile>();
                goksa g = go.GetComponent<goksa>();

                if (m != null) m.speed /= slowRate;
                if (sm != null) sm.speed /= slowRate;
                if (g != null) g.speed /= slowRate;

                Debug.Log(go.name + " (파괴 시 범위 내 존재) 속도 복구 완료!");
            }
        }
    }
}