using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WhackAMoleController : MinigameController
{
    [Header("Config")]
    [SerializeField] private float gameDuration = 30f;
    [SerializeField] private float spawnInterval = 1.2f; // seconds between spawns
    [SerializeField] private float targetActiveTime = 1.5f; // how long a target stays up
    [SerializeField] private int pointsPerHit = 5;
    [SerializeField] private int penaltyPerMiss = 1;

    [Header("UI References")]
    [SerializeField] private Button[] targetButtons; // assign hole-buttons in Inspector
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private float _elapsed;
    private bool _running;
    private int _missCount;
    private bool[] _activeSlots; // tracks which holes currently have a target

    public override void StartMinigame()
    {
        Finish(); // TEMP
        minigameType = MinigameType.WhackAMole;
        minigameScore = 0;
        progress = 0;
        _elapsed = 0f;
        _running = true;
        _missCount = 0;

        _activeSlots = new bool[targetButtons != null ? targetButtons.Length : 0];

        // Wire button listeners
        if (targetButtons != null)
            for (int i = 0; i < targetButtons.Length; i++)
            {
                int captured = i;
                targetButtons[i].onClick.AddListener(() => OnTargetHit(captured));
                targetButtons[i].gameObject.SetActive(false); // start hidden
            }

        StartCoroutine(SpawnLoop());
        Debug.Log("[WhackAMoleController] Minigame started.");
    }

    private void Update()
    {
        if (!_running) return;

        _elapsed += Time.deltaTime;
        float remaining = Mathf.Max(0f, gameDuration - _elapsed);

        if (timerText) timerText.text = $"Time: {remaining:F1}s";
        if (scoreText) scoreText.text = $"Score: {minigameScore}";

        if (_elapsed >= gameDuration)
            Finish();
    }

    // ── Spawn Loop ────────────────────────────────────────────────────────────

    private IEnumerator SpawnLoop()
    {
        while (_running)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (!_running) yield break;
            SpawnTarget();
        }
    }

    private void SpawnTarget()
    {
        if (targetButtons == null || targetButtons.Length == 0) return;

        // Pick a random inactive slot
        int attempts = 0;
        int slot;
        do { slot = Random.Range(0, targetButtons.Length); attempts++; }
        while (_activeSlots[slot] && attempts < 20);

        if (_activeSlots[slot]) return; // all slots full

        _activeSlots[slot] = true;
        targetButtons[slot].gameObject.SetActive(true);
        // TODO: play pop-up animation

        StartCoroutine(RetractAfterDelay(slot, targetActiveTime));
    }

    private IEnumerator RetractAfterDelay(int slot, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_activeSlots[slot]) // still active = player missed
        {
            _missCount++;
            DeactivateSlot(slot);
        }
    }

    // ── Target Hit ────────────────────────────────────────────────────────────

    private void OnTargetHit(int slot)
    {
        if (!_running || !_activeSlots[slot]) return;

        AddPoints(pointsPerHit);
        DeactivateSlot(slot);
        // TODO: play hit animation / particle effect
    }

    private void DeactivateSlot(int slot)
    {
        _activeSlots[slot] = false;
        if (targetButtons != null && slot < targetButtons.Length)
            targetButtons[slot].gameObject.SetActive(false);
        // TODO: play retract animation
    }

    // ── Finish ────────────────────────────────────────────────────────────────

    private void Finish()
    {
        _running = false;
        int penalty = _missCount * penaltyPerMiss;
        Debug.Log($"[WhackAMoleController] Done. Score:{minigameScore} Misses:{_missCount} Penalty:{penalty}");
        ReturnToGame(bonus: minigameScore, penalty: penalty);
    }
}
