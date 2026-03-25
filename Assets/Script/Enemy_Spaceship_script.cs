using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyType
{
    Null = 0,
    Scout,
    Triple,
    Big,
    Sniper,
    bomb,
    boss
}

public class Enemy_Spaceship_script : MonoBehaviour
{
    private int i = 0;
    public Image Hpbar;
    public float Coin;
    // public int enemy_type;
    [Header("적 설정")]
    public EnemyType Type_Selection;
    [Header("능력치 배열")]
    public float speed = 20f;
    public float rotationSpeed = 250f;
    public float attackSpeed = 2f;
    public float enemy_hp = 50f;
    [Header("미사일 설정")]
    public GameObject missile;
    public GameObject missile2;
    public GameObject missile3;
    public float stopDistance = 50.0f;
    private float cooldown;
    private float cooldown2;
    private float attack_cooldown3;
    private float attack_cooldown4;
    private float attack_cooldown5;
    private Transform playerTransform;
    private bool isDead = false;
    public float HP;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
        HP = enemy_hp;
        player = GameObject.FindWithTag("Player").transform;
        if (player != null)
        {
            playerTransform = player.transform;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null) return;

        if (Type_Selection != EnemyType.boss)
        {
            MoveTowardsPlayer();
            transform.LookAt(player); // 일반 몹은 플레이어를 바라봄
        }

        cooldown += Time.deltaTime;
        cooldown2 += Time.deltaTime;
        SpawnMissile();
        //UpdateHPUI();
        EnemyHpBar();

        attack_cooldown3 += Time.deltaTime;
        attack_cooldown4 += Time.deltaTime;
        attack_cooldown5 += Time.deltaTime;

    }
    public void SpawnMissile()
    {
        Vector3 spawnOffset = transform.forward * 15f;
        Vector3 spawnPoint = transform.position + spawnOffset;
        switch (Type_Selection)
        {
            case EnemyType.Scout:
                if (cooldown >= attackSpeed)
                {
                    Instantiate(missile, transform.position, transform.rotation);
                    cooldown = 0f;
                }
                break;
            case EnemyType.Triple:
                if (attack_cooldown3 >= attackSpeed)
                {
                    Instantiate(missile, transform.position, transform.rotation);
                    attack_cooldown3 = 0f;
                }
                if (attack_cooldown4 >= attackSpeed * 1.05)
                {
                    Instantiate(missile, transform.position, transform.rotation);
                    attack_cooldown4 = 0f;
                }
                if (attack_cooldown5 >= attackSpeed * 1.1)
                {
                    Instantiate(missile, transform.position, transform.rotation);
                    attack_cooldown5 = 0f;
                }
                break;
            case EnemyType.Big:
                if (cooldown >= attackSpeed)
                {
                    Instantiate(missile, transform.position, transform.rotation);
                    cooldown = 0f;
                }
                break;
            case EnemyType.Sniper:
                if (cooldown >= attackSpeed)
                {
                    Instantiate(missile, transform.position, transform.rotation);
                    cooldown = 0f;
                }
                break;
            case EnemyType.bomb:    
                if (cooldown >= attackSpeed)
                {
                    Instantiate(missile, transform.position, transform.rotation);
                    cooldown = 0f;
                }
                break;
            case EnemyType.boss:
                if (cooldown >= attackSpeed)
                {
                    float X = Random.Range(-40f, 40f);
                    float Z = 50f;
                    Vector3 wichi = new Vector3(X, 0, Z);

                   
                    Instantiate(missile, wichi, transform.rotation);
                    
                    i++;
                    cooldown = 0f;
                    if (i >= 3)
                    {
                        Debug.Log("산탄 발사 성공");
                        Instantiate(missile2, spawnPoint, transform.rotation);
                    }
                    if (i >= 6)
                    {
                        Debug.Log("3탄 발사 성공");
                        Instantiate(missile3, spawnPoint, transform.rotation);
                    }



                }
                break;

        }

    }
    public void MoveTowardsPlayer()
    {
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
        }
        
        if (distance > stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        }
    }
    public void Enemy_Damage(float x)
    {
        if (isDead) return; // 이미 죽고 있다면 무시

        HP -= x;
        if (HP <= 0)
        {
            isDead = true; // 죽음 확정

            if (GameManager.Instance != null)
            {
                GameManager.Instance.Money(Coin);
            }

            if (Enemy_Spawner.Instance != null)
            {
                // 현재 내 타입이 boss인지 정확히 체크해서 보냄
                Enemy_Spawner.Instance.EnemyDied(Type_Selection == EnemyType.boss);
            }

            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;
        // 만약 부딛힌 객체가 태그가 플레이어인 객체라면
        if (other.CompareTag("Player"))
        {
            if (Type_Selection == EnemyType.boss) {
                if (cooldown2 >= 1.5f)
                {
                    Enemy_Damage(50f);
                    cooldown2 = 0f;
                }
            }
            

        }
    }

    public void EnemyHpBar()
    {
        Hpbar.fillAmount = HP / enemy_hp;
    }
}
