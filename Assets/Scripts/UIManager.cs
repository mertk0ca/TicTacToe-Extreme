using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // UI Image için
using System.Collections;

public class UIManager : MonoBehaviour
{
    public GameObject panel; // Açýlacak/kapanacak panel
    public Image fadeImage;  // Fade efektini yapacak Image (Canvas içinde olmalý)

    private void Start()
    {
        fadeImage.gameObject.SetActive(true); // Fade Image'ý baþta görünür yap
        StartCoroutine(FadeOut()); // Baþlangýçta fade-out efekti
    }

    // Panelin görünürlüðünü deðiþtirir
    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }

    // Devam Et Butonu - Sadece paneli kapatýr
    public void ClosePanel()
    {
        StartCoroutine(ClosePanelWithDelay());
    }

    private IEnumerator ClosePanelWithDelay()
    {
        // 0.5 saniye bekle
        yield return new WaitForSeconds(0.3f);

        // Paneli kapat
        panel.SetActive(false);
    }

    // Ana Menü Butonu - Fade efekti ile sahneyi yükler
    public void LoadMainMenu()
    {
        StartCoroutine(LoadSceneWithFade("MainMenu"));
    }

    // SampleScene Butonu - Fade efekti ile sahneyi yükler
    public void LoadScene()
    {
        StartCoroutine(LoadSceneWithFade("SampleScene"));
    }

    // Fade efekti ile sahne yükler
    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        yield return StartCoroutine(FadeIn()); // Fade-in yap
        SceneManager.LoadScene(sceneName); // Sahneyi yükle
        yield return StartCoroutine(FadeOut()); // Fade-out yap
    }

    // Fade-in efekti (siyah ekranda görünürlük artar)
    private IEnumerator FadeIn()
    {
        float timeElapsed = 0f;
        float fadeDuration = 0.5f; // 1 saniye süresince fade in

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            fadeImage.color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration)); // Opaklýk arttýr
            yield return null;
        }
    }

    // Fade-out efekti (siyah ekranda görünürlük azalýr)
    private IEnumerator FadeOut()
    {
        float timeElapsed = 0f;
        float fadeDuration = 0.5f; // 1 saniye süresince fade out

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            fadeImage.color = new Color(0f, 0f, 0f, Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration)); // Opaklýk azalt
            yield return null;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
