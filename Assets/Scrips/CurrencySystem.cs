using UnityEngine;
using TMPro; // Cần có TextMeshPro để hiện chữ

public class CurrencySystem : MonoBehaviour
{
    public static CurrencySystem instance;

    [SerializeField] private int startingGold = 100;
    [SerializeField] private TextMeshProUGUI goldText; // Kéo UI Text vào đây

    private int currentGold;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentGold = startingGold;
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateUI();
    }

    public bool SpendGold(int amount)
    {
        if (amount <= currentGold)
        {
            currentGold -= amount;
            UpdateUI();
            return true;
        }
        Debug.Log("Không đủ vàng!");
        return false;
    }

    private void UpdateUI()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + currentGold;
        }
    }
}