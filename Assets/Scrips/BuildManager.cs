using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private GameObject[] towerPrefabs;
    [SerializeField] private int[] towerCosts = { 50, 100, 200 };

    [Header("Upgrade Settings")]
    [SerializeField] private GameObject[] upgradePrefabs;
    [SerializeField] private int[] upgradeCosts = { 100, 150, 250 };

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private GameObject warningUI;
    [SerializeField] private GameObject floatingTextPrefab;

    private int selectedTowerIndex = 0;
    public int currency = 500;

    private bool isWarningRunning = false; // Biến chặn nháy Warning

    private void Awake()
    {
        if (main == null) main = this;
    }

    private void Start()
    {
        UpdateGoldUI();
        if (warningUI != null) warningUI.SetActive(false);
    }

    // --- LOGIC LẤY DỮ LIỆU ---
    public void SetSelectedTower(int index) { selectedTowerIndex = index; }
    public int GetSelectedTowerIndex() { return selectedTowerIndex; }
    public GameObject GetSelectedTower() { return towerPrefabs[selectedTowerIndex]; }
    public int GetSelectedTowerCost() { return towerCosts[selectedTowerIndex]; }
    public GameObject GetUpgradePrefab(int index) { return upgradePrefabs[index]; }
    public int GetUpgradeCost(int index) { return upgradeCosts[index]; }

    // --- LOGIC KINH TẾ ---
    public bool CanAfford(int amount) { return currency >= amount; }

    public void SpendCurrency(int amount, Vector3 pos)
    {
        currency -= amount;
        UpdateGoldUI();
        // Chỉ hiện chữ bay 1 lần duy nhất
        ShowFloatingText("-" + amount, pos, Color.red);
    }

    public void AddCurrency(int amount, Vector3 pos)
    {
        currency += amount;
        UpdateGoldUI();
        ShowFloatingText("+" + amount, pos, Color.yellow);
    }

    private void UpdateGoldUI()
    {
        if (goldText != null) goldText.text = " " + currency;
    }

    // --- HIỆU ỨNG UI ---
    private void ShowFloatingText(string message, Vector3 worldPos, Color textColor)
    {
        if (floatingTextPrefab != null)
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas == null) return;

            GameObject textObj = Instantiate(floatingTextPrefab, canvas.transform);
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPos);
            textObj.transform.position = screenPosition;

            var tmpro = textObj.GetComponent<TextMeshProUGUI>();
            if (tmpro != null)
            {
                tmpro.text = message;
                tmpro.color = textColor;
            }
        }
    }

    public void ShowWarning()
    {
        // Nếu đang hiện cảnh báo rồi thì không chạy đè thêm lần nữa
        if (isWarningRunning) return;

        if (warningUI != null)
        {
            StartCoroutine(WarningRoutine());
        }
    }

    private IEnumerator WarningRoutine()
    {
        isWarningRunning = true;
        warningUI.SetActive(true);

        TextMeshProUGUI text = warningUI.GetComponent<TextMeshProUGUI>();
        Vector3 originalPos = warningUI.transform.localPosition;
        Color originalColor = (text != null) ? text.color : Color.white;

        // 1. Rung
        float elapsed = 0f;
        float duration = 0.4f;
        while (elapsed < duration)
        {
            float x = Random.Range(-5f, 5f);
            float y = Random.Range(-5f, 5f);
            warningUI.transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        warningUI.transform.localPosition = originalPos;

        yield return new WaitForSeconds(0.6f);

        // 2. Mờ dần
        if (text != null)
        {
            for (float t = 1; t > 0; t -= Time.deltaTime * 2f)
            {
                text.color = new Color(originalColor.r, originalColor.g, originalColor.b, t);
                yield return null;
            }
            text.color = originalColor; // Reset Alpha về 1
        }

        warningUI.SetActive(false);
        isWarningRunning = false;
    }
}