using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;    // Tốc độ bay lên
    [SerializeField] private float destroyTime = 1f;  // Thời gian tồn tại (1 giây)
    private TextMeshProUGUI textMesh;
    private Color originalColor;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        originalColor = textMesh.color;
    }

    private void Start()
    {
        // Tự hủy sau destroyTime giây
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        // Bay lên theo trục Y
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        // Hiệu ứng mờ dần (Fade out)
        float alpha = Mathf.Lerp(originalColor.a, 0, (Time.time % destroyTime) / destroyTime);
        textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }
}