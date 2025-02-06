using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    private AudioSource audioSource; // Butonun kendi AudioSource bileþeni

    void Start()
    {
        // Ayný GameObject içindeki AudioSource bileþenini al
        audioSource = GetComponent<AudioSource>();

        // Buton bileþenini bul ve týklanma olayýna PlaySound fonksiyonunu ekle
        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioSource.clip); // AudioSource içinde atanmýþ sesi bir kez çal
        }
        else
        {
            Debug.LogWarning("AudioSource bulunamadý! Lütfen butona AudioSource eklediðinizden emin olun.");
        }
    }
}
