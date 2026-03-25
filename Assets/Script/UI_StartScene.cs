using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_StartScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Start_button(string x)
    {
        SceneManager.LoadScene(x);
    }

    public void ShowGameOverUI(string x)
    {

        SceneManager.LoadScene(x);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Stage1");
        UIManager.Instance.gameOverPanel.SetActive(false);
        GameManager.Instance.playerNowHP = GameManager.Instance.playerMaxHP;
        Time.timeScale = 1f;
    }

    public void OnClickNextStage(string x)
    {
        Time.timeScale = 1f;                // 멈춘 시간 다시 돌리기
        SceneManager.LoadScene(x); // 다음 스테이지로 이동
    }
}
