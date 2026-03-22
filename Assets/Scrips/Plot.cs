using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor = Color.gray;
    private Color startColor;
    private GameObject tower;

    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter() { sr.color = hoverColor; }
    private void OnMouseExit() { sr.color = startColor; }

    private void OnMouseDown()
    {
        if (tower != null) return;

        // Lấy giá của loại Tank đang chọn
        int cost = BuildManager.main.GetSelectedTowerCost();

        // Kiểm tra tiền trong BuildManager
        if (BuildManager.main.CanAfford(cost))
        {
            BuildManager.main.SpendCurrency(cost); // Trừ tiền

            GameObject prefabToBuild = BuildManager.main.GetSelectedTower();
            tower = Instantiate(prefabToBuild, transform.position, Quaternion.identity);
            tower.transform.SetParent(transform);
        }
        else
        {
            Debug.Log("Không đủ tiền xây dựng!");
        }
    }
}