using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor = Color.gray;
    private Color startColor;

    private GameObject tower; // Lưu trữ Tank đang đứng trên ô này
    private int towerLevel = 0; // 0 là Tank gốc, 1 là đã nâng cấp
    private int currentTowerIndex; // Lưu lại loại Tank nào đang đứng đây (Tank 1, 2 hay 3)

    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter() { sr.color = hoverColor; }
    private void OnMouseExit() { sr.color = startColor; }

    private void OnMouseDown()
    {
        // TRƯỜNG HỢP 1: Ô ĐẤT TRỐNG -> XÂY MỚI
        if (tower == null)
        {
            int cost = BuildManager.main.GetSelectedTowerCost();

            if (BuildManager.main.CanAfford(cost))
            {
                // Gọi hàm trừ tiền và truyền vị trí ô đất để hiện chữ bay
                BuildManager.main.SpendCurrency(cost, transform.position);

                // Lưu lại index loại tank để sau này nâng cấp cho đúng loại
                currentTowerIndex = BuildManager.main.GetSelectedTowerIndex();

                GameObject prefabToBuild = BuildManager.main.GetSelectedTower();
                tower = Instantiate(prefabToBuild, transform.position, Quaternion.identity);

                // Gắn tank vào làm con của Plot để Hierarchy gọn gàng hơn (tùy chọn)
                tower.transform.SetParent(transform);

                towerLevel = 0; // Đang ở level gốc
            }
            else
            {
                BuildManager.main.ShowWarning();
            }
        }
        // TRƯỜNG HỢP 2: ĐÃ CÓ TANK -> NÂNG CẤP
        else
        {
            if (towerLevel == 0) // Chỉ cho nâng cấp nếu chưa nâng cấp lần nào
            {
                int upgradeCost = BuildManager.main.GetUpgradeCost(currentTowerIndex);

                if (BuildManager.main.CanAfford(upgradeCost))
                {
                    // Gọi hàm trừ tiền nâng cấp và truyền vị trí để hiện chữ bay
                    BuildManager.main.SpendCurrency(upgradeCost, transform.position);

                    // Xóa Tank cũ
                    Destroy(tower);

                    // Tạo Tank nâng cấp mới dựa trên index của Tank cũ
                    GameObject upgradePrefab = BuildManager.main.GetUpgradePrefab(currentTowerIndex);
                    tower = Instantiate(upgradePrefab, transform.position, Quaternion.identity);

                    tower.transform.SetParent(transform);

                    towerLevel = 1; // Đánh dấu đã đạt cấp tối đa
                    Debug.Log("Đã nâng cấp lên Level Max!");
                }
                else
                {
                    BuildManager.main.ShowWarning();
                }
            }
            else
            {
                Debug.Log("Tank này đã đạt cấp độ tối đa!");
            }
        }
    }
}