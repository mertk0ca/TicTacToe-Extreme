using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public GameObject xPrefab, oPrefab;
    public GameObject lineRendererPrefab; // Çizgi için prefab  
    private GameObject[,] grid = new GameObject[3, 3];
    private bool isXTurn = true;
    private Queue<GameObject> xMarkers = new Queue<GameObject>();
    private Queue<GameObject> oMarkers = new Queue<GameObject>();
    private int maxMarkers = 3;
    private float fullOpacity = 1f;

    private Vector3 xMaxScale = new Vector3(0.391f, 0.391f, 0.391f);
    private Vector3 oMaxScale = new Vector3(0.174f, 0.174f, 0.174f);

    private float pulseSpeed = 3f;
    private float minOpacity = 0.1f;
    private float maxOpacity = 0.8f;
    private GameObject currentPulsingMarker;

    public TMPro.TextMeshProUGUI redsTurn;  // Red Wins yazýsý
    public TMPro.TextMeshProUGUI bluesTurn; // Blue Wins yazýsý

    public GameObject EndGamePanel;
    public GameObject redWinsPanel;  // "Red Wins" paneli
    public GameObject blueWinsPanel; // "Blue Wins" paneli

    public AudioSource audioSource; // SES OYNA
    public AudioClip placeMarkerSound; // X veya O koyarken çalýnacak ses
    public AudioClip winLineSound; // Çizgi çekilirken çalacak ses efekti

    void Start()
    {
        Debug.Log("Oyun baþladý! X baþlýyor.");
        UpdateMarkerOpacity();

        redsTurn.gameObject.SetActive(true);
        bluesTurn.gameObject.SetActive(false);
    }

    void Update()
    {
        if (currentPulsingMarker != null)
        {
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
            float alpha = Mathf.Lerp(minOpacity, maxOpacity, pulse);
            SetMarkerOpacity(currentPulsingMarker, new Color(1, 1, 1, alpha));
        }
    }

    public void PlaceMarker(int row, int col, GameObject cellButton)
    {
        if (grid[row, col] != null) return;

        GameObject markerPrefab = isXTurn ? xPrefab : oPrefab;
        Vector3 position = cellButton.transform.position;
        GameObject newMarker = Instantiate(markerPrefab, position, Quaternion.identity);

        newMarker.transform.localScale = Vector3.zero;

        if (audioSource != null && placeMarkerSound != null)
        {
            audioSource.PlayOneShot(placeMarkerSound);
        }

        Vector3 targetScale = isXTurn ? xMaxScale : oMaxScale;

        LeanTween.scale(newMarker, targetScale, 0.5f).setEase(LeanTweenType.easeOutBack);

        if (isXTurn && xMarkers.Count >= maxMarkers)
        {
            GameObject oldMarker = xMarkers.Peek();
            int[] oldPos = FindMarkerPosition(oldMarker);
            if (oldPos != null)
            {
                grid[oldPos[0], oldPos[1]] = null;
            }
            xMarkers.Dequeue();
            StartCoroutine(ShrinkAndDestroy(oldMarker));
        }
        else if (!isXTurn && oMarkers.Count >= maxMarkers)
        {
            GameObject oldMarker = oMarkers.Peek();
            int[] oldPos = FindMarkerPosition(oldMarker);
            if (oldPos != null)
            {
                grid[oldPos[0], oldPos[1]] = null;
            }
            oMarkers.Dequeue();
            StartCoroutine(ShrinkAndDestroy(oldMarker));
        }

        grid[row, col] = newMarker;
        newMarker.tag = isXTurn ? "X" : "O";

        if (isXTurn)
        {
            xMarkers.Enqueue(newMarker);
            bluesTurn.gameObject.SetActive(true);
            redsTurn.gameObject.SetActive(false);
        }
        else
        {
            oMarkers.Enqueue(newMarker);
            redsTurn.gameObject.SetActive(true);
            bluesTurn.gameObject.SetActive(false);
        }

        if (CheckWin(row, col))
        {
            Debug.Log((isXTurn ? "X" : "O") + " kazandý!");
            currentPulsingMarker = null;
            StartCoroutine(DelayedSceneReload());
            return;
        }

        if (IsBoardFull())
        {
            Debug.Log("Oyun berabere!");
            currentPulsingMarker = null;
            StartCoroutine(DelayedSceneReload());
            return;
        }

        isXTurn = !isXTurn;
        UpdateMarkerOpacity();
    }

    // Oyun sonunda hemen sahne yenilemeyi engellemek için  
    IEnumerator DelayedSceneReload()
    {
        yield return new WaitForSeconds(2f); // 2 saniye bekle  
        EndGamePanel.gameObject.SetActive(true);
        //EndGamePanel.SetActive(false);
        //SceneManager.LoadScene("SampleScene");
    }

    private IEnumerator ShrinkAndDestroy(GameObject marker)
    {
        LeanTween.scale(marker, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBack);
        yield return new WaitForSeconds(0.5f);
        Destroy(marker);
    }

    private void UpdateMarkerOpacity()
    {
        foreach (var marker in xMarkers)
        {
            if (marker != null)
            {
                SetMarkerOpacity(marker, new Color(1, 1, 1, fullOpacity));
            }
        }
        foreach (var marker in oMarkers)
        {
            if (marker != null)
            {
                SetMarkerOpacity(marker, new Color(1, 1, 1, fullOpacity));
            }
        }

        currentPulsingMarker = null;
        if (isXTurn && xMarkers.Count == maxMarkers)
        {
            currentPulsingMarker = xMarkers.Peek();
        }
        else if (!isXTurn && oMarkers.Count == maxMarkers)
        {
            currentPulsingMarker = oMarkers.Peek();
        }
    }

    private void SetMarkerOpacity(GameObject marker, Color targetColor)
    {
        SpriteRenderer spriteRenderer = marker.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color currentColor = spriteRenderer.color;
            currentColor.a = targetColor.a;
            spriteRenderer.color = currentColor;
        }

        UnityEngine.UI.Image image = marker.GetComponent<UnityEngine.UI.Image>();
        if (image != null)
        {
            Color currentColor = image.color;
            currentColor.a = targetColor.a;
            image.color = currentColor;
        }
    }

    private int[] FindMarkerPosition(GameObject marker)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (grid[i, j] == marker)
                {
                    return new int[] { i, j };
                }
            }
        }
        return null;
    }

    bool CheckWin(int row, int col)
    {
        string currentTag = isXTurn ? "X" : "O";

        // Satýr kontrolü  
        if (grid[row, 0] != null && grid[row, 1] != null && grid[row, 2] != null)
        {
            if (grid[row, 0].tag == currentTag && grid[row, 1].tag == currentTag && grid[row, 2].tag == currentTag)
            {
                DrawWinLine(new Vector3[]
                {
                grid[row, 0].transform.position,
                grid[row, 2].transform.position
                });

                ShowWinnerPanel();
                return true;
            }
        }

        // Sütun kontrolü  
        if (grid[0, col] != null && grid[1, col] != null && grid[2, col] != null)
        {
            if (grid[0, col].tag == currentTag && grid[1, col].tag == currentTag && grid[2, col].tag == currentTag)
            {
                DrawWinLine(new Vector3[]
                {
                grid[0, col].transform.position,
                grid[2, col].transform.position
                });

                ShowWinnerPanel();
                return true;
            }
        }

        // Çapraz kontrolü (sol üstten sað alta)  
        if (grid[0, 0] != null && grid[1, 1] != null && grid[2, 2] != null)
        {
            if (grid[0, 0].tag == currentTag && grid[1, 1].tag == currentTag && grid[2, 2].tag == currentTag)
            {
                DrawWinLine(new Vector3[]
                {
                grid[0, 0].transform.position,
                grid[2, 2].transform.position
                });

                ShowWinnerPanel();
                return true;
            }
        }

        // Çapraz kontrolü (sað üstten sol alta)  
        if (grid[0, 2] != null && grid[1, 1] != null && grid[2, 0] != null)
        {
            if (grid[0, 2].tag == currentTag && grid[1, 1].tag == currentTag && grid[2, 0].tag == currentTag)
            {
                DrawWinLine(new Vector3[]
                {
                grid[0, 2].transform.position,
                grid[2, 0].transform.position
                });

                ShowWinnerPanel();
                return true;
            }
        }

        return false;
    }

    void DrawWinLine(Vector3[] points)
    {
        audioSource.PlayOneShot(winLineSound);

        // LineRenderer prefabýný instantiate et  
        GameObject lineObject = Instantiate(lineRendererPrefab);
        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();

        // LineRenderer ayarlarý  
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.positionCount = 2;

        // Çizginin rengini ayarla  
        //lineRenderer.startColor = Color.red;
        //lineRenderer.endColor = Color.red;

        // Çizgiyi biraz z ekseninde öne getir  
        lineObject.transform.position = new Vector3(
            lineObject.transform.position.x,
            lineObject.transform.position.y,
            -1f
        );

        // Animasyonlu çizgi çekme  
        StartCoroutine(AnimateWinLine(lineRenderer, points));
    }

    IEnumerator AnimateWinLine(LineRenderer lineRenderer, Vector3[] points)
    {
        float animationDuration = 0.5f; // Çizgi çizme süresi  
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            // Saðdan sola doðru çizgi çekme efekti  
            float t = elapsedTime / animationDuration;

            // Çizginin baþlangýç noktasý sabit, bitiþ noktasý yavaþ yavaþ uzasýn  
            lineRenderer.SetPosition(0, points[0]);
            float smoothT = 1f - Mathf.Cos((t * Mathf.PI) / 2f);
            lineRenderer.SetPosition(1, Vector3.Lerp(points[0], points[1], smoothT));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Son konumu kesin olarak ayarla  
        lineRenderer.SetPosition(0, points[0]);
        lineRenderer.SetPosition(1, points[1]);
    }

    bool IsBoardFull()
    {
        foreach (var cell in grid)
        {
            if (cell == null) return false;
        }
        return true;
    }

    void ShowWinnerPanel()
    {
        EndGamePanel.SetActive(true);

        if (isXTurn)
        {
            redWinsPanel.SetActive(true);
            blueWinsPanel.SetActive(false);
            bluesTurn.gameObject.SetActive(false);
            redsTurn.gameObject.SetActive(false);
        }
        else
        {
            blueWinsPanel.SetActive(true);
            redWinsPanel.SetActive(false);
            bluesTurn.gameObject.SetActive(false);
            redsTurn.gameObject.SetActive(false);
        }
    }
}