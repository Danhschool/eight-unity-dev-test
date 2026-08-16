using System.Collections;
using UnityEngine;

public class CollectGem : MonoBehaviour
{
    public static CollectGem Instance;

    [Header("Tham chiếu Hệ thống")]
    public RectTransform canvasRect;
    public Camera mainCamera;

    [Header("Cài đặt Hiệu ứng")]
    public RectTransform targetIconUI;
    public GameObject rawImageGemPrefab;
    public float flyDuration = 0.6f;

    private Canvas parentCanvas;

    private void Awake()
    {
        Instance = this;
        parentCanvas = canvasRect.GetComponent<Canvas>();
    }

    public void CreateFlyingGem(Vector3 worldPos, int scoreValue)
    {
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(worldPos);

        if (screenPoint.z <= 0) return;

        Camera uiCam = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, screenPoint, uiCam, out Vector2 localPoint);

        GameObject flyingGem = ObjectPool.instance.GetObject(rawImageGemPrefab, Vector3.zero, Quaternion.identity, canvasRect);
        RectTransform gemRect = flyingGem.GetComponent<RectTransform>();

        gemRect.anchoredPosition = localPoint;

        StartCoroutine(FlyToIcon(gemRect, worldPos, scoreValue));
    }

    private IEnumerator FlyToIcon(RectTransform gemRect, Vector3 originalWorldPos, int scoreValue)
    {
        float time = 0;
        Vector2 startPos = gemRect.anchoredPosition;
        Vector2 targetPos = canvasRect.InverseTransformPoint(targetIconUI.position);

        while (time < flyDuration)
        {
            time += Time.deltaTime;
            float t = time / flyDuration;

            gemRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        ObjectPool.instance.ReturnObject(gemRect.gameObject);

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue, originalWorldPos);
        }
    }
}