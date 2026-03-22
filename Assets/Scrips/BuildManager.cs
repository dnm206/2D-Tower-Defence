using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Bắt buộc phải có để điều khiển font chữ bạn vừa cài

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private GameObject[] towerPrefabs; // Kéo 3 Tank Prefab vào đây
    [SerializeField] private int[] towerCosts = { 50, 100, 200 }; // Giá của Tank 1, 2, 3

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI goldText;    // Kéo đối tượng GoldText vào đây
    [SerializeField] private GameObject warningUI;       // Kéo đối tượng WarningText vào đây

    private int selectedTowerIndex = 0;
    public int currency = 500; // Tiền khởi đầu

    private void Awake()
    {
        if (main == null) main = this;
    }

    private void Start()
    {
        UpdateGoldUI(); // Hiển thị số tiền ngay khi vào game
        if (warningUI != null) warningUI.SetActive(false); // Ẩn thông báo lúc đầu
    }

    // --- LOGIC MUA BÁN ---

    public GameObject GetSelectedTower()
    {
        return towerPrefabs[selectedTowerIndex];
    }

    public int GetSelectedTowerCost()
    {
        return towerCosts[selectedTowerIndex];
    }

    public void SetSelectedTower(int index)
    {
        selectedTowerIndex = index;
    }

    public bool CanAfford(int amount)
    {
        return currency >= amount;
    }

    public void SpendCurrency(int amount)
    {
        currency -= amount;
        UpdateGoldUI();
        Debug.Log("Đã trừ tiền. Còn lại: " + currency);
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        UpdateGoldUI();
        Debug.Log("Đã nhận tiền! Tổng cộng: " + currency);
    }

    // --- CẬP NHẬT GIAO DIỆN (UI) ---

    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = "GOLD: " + currency;
        }
    }

    // Hàm gọi thông báo lỗi
    public void ShowWarning()
    {
        if (warningUI != null)
        {
            StopAllCoroutines(); // Dừng các hiệu ứng cũ đang chạy dở
            StartCoroutine(WarningRoutine());
        }
    }

    private IEnumerator WarningRoutine()
    {
        warningUI.SetActive(true);
        TextMeshProUGUI text = warningUI.GetComponent<TextMeshProUGUI>();
        Vector3 originalPos = warningUI.transform.localPosition;

        // 1. Hiệu ứng Rung (Shake)
        float elapsed = 0f;
        float duration = 0.4f; // Rung trong 0.4 giây
        while (elapsed < duration)
        {
            float x = Random.Range(-10f, 10f);
            float y = Random.Range(-10f, 10f);
            warningUI.transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        warningUI.transform.localPosition = originalPos; // Trả về vị trí cũ

        // 2. Chờ một chút
        yield return new WaitForSeconds(0.8f);

        // 3. Hiệu ứng Mờ dần (Fade Out)
        if (text != null)
        {
            Color originalColor = text.color;
            for (float t = 1; t > 0; t -= Time.deltaTime * 1.5f) // Tốc độ mờ
            {
                text.color = new Color(originalColor.r, originalColor.g, originalColor.b, t);
                yield return null;
            }
            text.color = originalColor; // Reset độ mờ về 1 cho lần sau hiện lại
        }

        warningUI.SetActive(false);
    }
}