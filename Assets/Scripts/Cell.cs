using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int row, col; // Hücre konumu
    private Button button;
    private GameManager gameManager;

    void Start()
    {
        button = GetComponent<Button>();
        gameManager = FindObjectOfType<GameManager>(); // GameManager'ý bul
        button.onClick.AddListener(OnCellClicked); // Týklama olayýný ekle
    }

    void OnCellClicked()
    {
        gameManager.PlaceMarker(row, col, gameObject); // GameManager'a butonun kendisini de gönder
    }
}
