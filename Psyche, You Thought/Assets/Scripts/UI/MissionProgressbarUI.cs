using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Mission progress bar displayed at the top of the Main scene.
/// Fills from left to right as storyProgress advances.
/// </summary>
public class MissionProgressBarUI : MonoBehaviour
{
    [Header("Bar Refs")]
    [SerializeField] private Image barFill;
    [SerializeField] private RectTransform shipIcon; // slides horizontally
    [SerializeField] private TextMeshProUGUI percentText;

    [Header("Bar Geometry")]
    [SerializeField] private float barLeftX = -530f; // X of left edge (Earth) in local space
    [SerializeField] private float barRightX =  530f; // X of right edge (Psyche)

    [Header("Animate")]
    [SerializeField] private float fillDuration = 0.6f;

    private float _displayedFill = 0f;


    private void OnEnable()
    {
        GameManager.OnMissionProgress += HandleProgress;
        if (GameManager.Instance != null)
            SetFillImmediate((float)GameManager.Instance.missionProgress / GameManager.Instance.missionGoal);
    }

    private void OnDisable()
    {
        GameManager.OnMissionProgress -= HandleProgress;
    }

    // ── Handlers ──────────────────────────────────────────────────────────────

    private void HandleProgress(int current, int goal)
    {
        float target = goal > 0 ? (float)current / goal : 0f;
        StopAllCoroutines();
        StartCoroutine(AnimateFill(target));
    }

    // ── Internal ──────────────────────────────────────────────────────────────

    private IEnumerator AnimateFill(float targetFill)
    {
        float startFill = _displayedFill;
        float elapsed = 0f;

        while (elapsed < fillDuration)
        {
            elapsed += Time.deltaTime;
            _displayedFill = Mathf.Lerp(startFill, targetFill, Mathf.SmoothStep(0f, 1f, elapsed / fillDuration));
            ApplyFill(_displayedFill);
            yield return null;
        }

        _displayedFill = targetFill;
        ApplyFill(_displayedFill);
    }

    private void ApplyFill(float t)
    {
        // Fill image
        if (barFill) {
            float fullWidth = barFill.rectTransform.parent
                .GetComponent<RectTransform>().rect.width;
            barFill.rectTransform.sizeDelta = new Vector2(
                Mathf.Clamp01(t) * fullWidth,
                barFill.rectTransform.sizeDelta.y
            );
        }

        // Ship icon position
        if (shipIcon)
        {
            float x = Mathf.Lerp(barLeftX, barRightX, t);
            shipIcon.anchoredPosition = new Vector2(x, shipIcon.anchoredPosition.y);
        }

        // Percentage text
        if (percentText) percentText.text = $"{Mathf.RoundToInt(t * 100f)}%";
    }

    private void SetFillImmediate(float t)
    {
        _displayedFill = t;
        ApplyFill(t);
    }
}