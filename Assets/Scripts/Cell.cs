using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int row, col; // H�cre konumu
    private Button button;
    private GameManager gameManager;

    void Start()
    {
        button = GetComponent<Button>();
        gameManager = FindObjectOfType<GameManager>(); // GameManager'� bul
        button.onClick.AddListener(OnCellClicked); // T�klama olay�n� ekle
    }

    void OnCellClicked()
    {
        gameManager.PlaceMarker(row, col, gameObject); // GameManager'a butonun kendisini de g�nder
    }
}
