using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject player;
    public RectTransform hpBarRect;
    public float playerMaxHP = 225f;
    public float playerNowHP;
    public float coin = 0f;
    private bool isGameOver = false;
    private bool isTimeStopActive = false;
    [Header("Shop Data")]
    public bool[] hasParts = new bool[5];    // 5개 파츠 소유 여부
    public int[] equippedParts = { -1, -1 }; // 장착 슬롯 2개 (-1은 빈 슬롯)
    public int partPrice = 100;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. 멈췄던 시간 초기화
        Time.timeScale = 1f;
        isGameOver = false;

        // 2. 새 씬에서 "Player" 태그를 가진 오브젝트를 다시 찾음
        player = GameObject.FindWithTag("Player");

        // 3. 체력 복구 (원한다면)
        playerNowHP = playerMaxHP;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerHp();
    }

    // Update is called once per frame
    void Update()
    {
        Cheat_Money();
        UpdateHPUI();
        GameOver();
        Cheat_Damage();
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCoinUI(coin);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateSkill();
        }
        if (Input.GetKeyDown(KeyCode.R) && !isTimeStopActive) // 실행 중이 아닐 때만 발동
        {
            StartCoroutine(MissileTimeStopSkill(3.0f));
        }
    }

    public void PlayerHp()
    {
        playerNowHP = playerMaxHP;
    }
    public void Damage(float x)
    {
        playerNowHP = playerNowHP - x;
    }

    public void GameOver()
    {
        if (playerNowHP <= 0 && !isGameOver)
        {
            isGameOver = true;
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowGameOverUI();
            }
            player.SetActive(false);
            Time.timeScale = 0f;
        }
    }
    public void UpdateHPUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateHPUI(playerNowHP, playerMaxHP);
        }
    }

    public void Cheat_hp()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            playerNowHP = playerMaxHP;
        }
    }
    public void Cheat_Damage()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemyObj in enemies)
            {
                Enemy_Spaceship_script enemyScript = enemyObj.GetComponent<Enemy_Spaceship_script>();

                if (enemyScript != null)
                {
                    enemyScript.Enemy_Damage(999999999999);
                }
            }
        }
    }
    public void Cheat_Money()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            coin = coin + 10000f;
        }
    }
    public void Money(float x)
    {
        coin += x;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCoinUI(coin);
        }
    }

    public void ActivateSkill()
    {
        GameObject Enemy_target = GameObject.FindWithTag("Enemy");

        if (Enemy_target == null) return;

        GameObject[] allMissiles = GameObject.FindGameObjectsWithTag("Missile");

        foreach (GameObject missileObj in allMissiles)
        {
            Missile_script mscript = missileObj.GetComponent<Missile_script>();
            santan oscript = missileObj.GetComponent<santan>();
            if (mscript != null)
            {
                mscript.SetForceTarget(Enemy_target.transform);
            }
            if (oscript != null)
            {
                oscript.SetForceTarget(Enemy_target.transform);
            }
        }
    }
    IEnumerator MissileTimeStopSkill(float duration)
    {
        Debug.Log("모든 미사일 시간 정지 시작!");

        GameObject[] allMissiles = GameObject.FindGameObjectsWithTag("Missile");

        foreach (GameObject missileObj in allMissiles)
        {
            Missile_script mScript = missileObj.GetComponent<Missile_script>();
            if (mScript != null) mScript.SetTimeStop(true);

            Sniper_Missile sScript = missileObj.GetComponent<Sniper_Missile>();
            if (sScript != null) sScript.SetTimeStop(true);

            goksa fScript = missileObj.GetComponent<goksa>();
            if (fScript != null) fScript.SetTimeStop(true);
        }

        yield return new WaitForSeconds(duration);

        GameObject[] allMissilesAfter = GameObject.FindGameObjectsWithTag("Missile");
        foreach (GameObject missileObj in allMissilesAfter)
        {
            Missile_script mScript = missileObj.GetComponent<Missile_script>();
            if (mScript != null) mScript.SetTimeStop(false);

            Sniper_Missile sScript = missileObj.GetComponent<Sniper_Missile>();
            if (sScript != null) sScript.SetTimeStop(false);

            goksa fScript = missileObj.GetComponent<goksa>();
            if (fScript != null) fScript.SetTimeStop(false);
        }

        Debug.Log("모든 미사일 다시 이동!");
    }

}
