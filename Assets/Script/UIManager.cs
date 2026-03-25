using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 기본 UI 사용 시

public class UIManager : MonoBehaviour
{
    // 어디서든 UIManager.Instance로 접근 가능하게 만듦 (싱글톤)
    public static UIManager Instance;
    public RectTransform hpBarRect;
    [Header("UI Panels")]
    public GameObject clearUIPanel;
    public GameObject shopUIPanel;
    public GameObject gameOverPanel;
    public GameObject stageClearPanel;
    [Header("UI 요소들")]
    public Text coinText;    // 코인 표시 텍스트
    [Header("Shop Settings")]
    public Button[] partButtons; // 상점의 5개 버튼을 인스펙터에서 넣어주세요.
    public Color boughtColor = Color.white;
    public Color lockedColor = new Color(0.3f, 0.3f, 0.3f);
    // public Slider playerHpBar; // 나중에 플레이어 HP바도 여기서 관리하면 편해요.


    void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        UpdateShopUI();
    }

    // 코인 텍스트를 업데이트하는 전용 함수
    public void UpdateCoinUI(float x)
    {
        if (coinText != null)
        {
            coinText.text = x.ToString();

        }
    }
    public void UpdateHPUI(float currentHP, float maxHP)
    {
        if (hpBarRect != null)
        {
            float hpRatio = currentHP / maxHP;
            hpBarRect.localScale = new Vector3(hpRatio, 1f, 1f);
        }
    }

    public void GameOver(string x)
    {

        SceneManager.LoadScene(x);
    }
    public void ShowGameOverUI()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // 여기서 Active를 사용해 창을 켭니다!
        }
    }
    public void ShowClearUI()
    {
        if (stageClearPanel != null)
        {
            stageClearPanel.SetActive(true); // Clear 창 활성화
            Time.timeScale = 0f;            // 게임 일시정지
        }
    }
    public void OpenShop()
    {
        if (clearUIPanel != null) clearUIPanel.SetActive(false);
        if (shopUIPanel != null) shopUIPanel.SetActive(true);

        // 3. (중요) 상점이 켜질 때 상점 스크립트의 데이터 갱신 함수 호출
        // shopUIPanel.GetComponent<ShopUI>().RefreshUI(); 
    }
    public void CloseShop()
    {
        if (clearUIPanel != null) clearUIPanel.SetActive(true);
        if (shopUIPanel != null) shopUIPanel.SetActive(false);

        // 3. (중요) 상점이 켜질 때 상점 스크립트의 데이터 갱신 함수 호출
        // shopUIPanel.GetComponent<ShopUI>().RefreshUI(); 
    }
    public void PurchasePart(int index)
    {
        // 1. 이미 샀는지 확인
        if (GameManager.Instance.hasParts[index])
        {
            Debug.Log("이미 구매한 파츠입니다. (인벤토리로 이동하거나 장착 로직 실행)");
            // 여기서 장착 로직을 실행하거나 안내 메시지를 띄울 수 있습니다.
            return;
        }

        // 2. 돈이 충분한지 확인
        if (GameManager.Instance.coin >= GameManager.Instance.partPrice)
        {
            // 3. 돈 차감
            GameManager.Instance.coin -= GameManager.Instance.partPrice;

            // 4. 구매 확정
            GameManager.Instance.hasParts[index] = true;

            // 5. UI 갱신
            UpdateCoinUI(GameManager.Instance.coin);
            UpdateShopUI(); // 버튼 밝기 조절 함수

            Debug.Log(index + "번 파츠 구매 완료!");
        }
        else
        {
            Debug.Log("돈이 부족합니다!");
        }
    }
    public void UpdateShopUI()
    {
        for (int i = 0; i < partButtons.Length; i++)
        {
            Image btnImg = partButtons[i].GetComponent<Image>();

            if (GameManager.Instance.hasParts[i])
            {
                btnImg.color = lockedColor;
            }
            else
            {
                btnImg.color = boughtColor;
            }
        }
    }
}
