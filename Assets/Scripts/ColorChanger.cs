using UnityEngine;
using TMPro;

public class ColorChanger : MonoBehaviour
{
    public TextMeshProUGUI textElement;
    public float colorChangeSpeed = 1.5f; // Renk de�i�im h�z�
    public float spacingChangeSpeed = 2f; // Harf aral��� de�i�im h�z�
    public float spacingAmount = 10f; // Maksimum art�� miktar�

    private float baseSpacing; // Ba�lang�� spacing de�eri

    void Start()
    {
        if (textElement != null)
        {
            baseSpacing = textElement.characterSpacing; // Orijinal spacing de�erini kaydet
        }
    }

    void Update()
    {
        if (textElement != null)
        {
            // Renk de�i�imi (sin�s fonksiyonu ile)
            float r = Mathf.Sin(Time.time * colorChangeSpeed) * 0.5f + 0.5f;
            float g = Mathf.Sin(Time.time * colorChangeSpeed + 2f) * 0.5f + 0.5f;
            float b = Mathf.Sin(Time.time * colorChangeSpeed + 4f) * 0.5f + 0.5f;
            textElement.color = new Color(r, g, b, 1f);

            // Character Spacing de�i�imi
            float spacingOffset = Mathf.Sin(Time.time * spacingChangeSpeed) * spacingAmount;
            textElement.characterSpacing = baseSpacing + spacingOffset;
        }
    }
}
