using UnityEngine;
using TMPro;

public class ColorChanger : MonoBehaviour
{
    public TextMeshProUGUI textElement;
    public float colorChangeSpeed = 1.5f; // Renk deðiþim hýzý
    public float spacingChangeSpeed = 2f; // Harf aralýðý deðiþim hýzý
    public float spacingAmount = 10f; // Maksimum artýþ miktarý

    private float baseSpacing; // Baþlangýç spacing deðeri

    void Start()
    {
        if (textElement != null)
        {
            baseSpacing = textElement.characterSpacing; // Orijinal spacing deðerini kaydet
        }
    }

    void Update()
    {
        if (textElement != null)
        {
            // Renk deðiþimi (sinüs fonksiyonu ile)
            float r = Mathf.Sin(Time.time * colorChangeSpeed) * 0.5f + 0.5f;
            float g = Mathf.Sin(Time.time * colorChangeSpeed + 2f) * 0.5f + 0.5f;
            float b = Mathf.Sin(Time.time * colorChangeSpeed + 4f) * 0.5f + 0.5f;
            textElement.color = new Color(r, g, b, 1f);

            // Character Spacing deðiþimi
            float spacingOffset = Mathf.Sin(Time.time * spacingChangeSpeed) * spacingAmount;
            textElement.characterSpacing = baseSpacing + spacingOffset;
        }
    }
}
