using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMover : MonoBehaviour
{
    [Header("Movement")]
    public float moveDistance = 5f;
    public float moveSpeed = 2f;

    [Header("Spawn Timing")]
    public float spawnIntervalMin = 2f;
    public float spawnIntervalMax = 5f;

    [Header("Fade")]
    public float fadeTime = 1f;
    public float activeDuration = 4f;

    [Header("Sprites")]
    public List<Sprite> fishSprites = new List<Sprite>();

    private SpriteRenderer sr;
    private Vector3 startPos;

    private float direction = -1f; // ALWAYS start moving LEFT

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = gameObject.AddComponent<SpriteRenderer>();

        startPos = transform.position;
        sr.enabled = false;
    }

    void Start()
    {
        StartCoroutine(VisibilityRoutine());
    }

    void Update()
    {
        MoveFish();
    }

    // 🔁 Continuous movement (rotation-aware)
    void MoveFish()
    {
        // Move along local right axis
        transform.position += transform.right * direction * moveSpeed * Time.deltaTime;

        // Flip sprite based on direction
        sr.flipX = direction < 0f;

        // Calculate distance along movement axis
        Vector3 offset = transform.position - startPos;
        float distanceAlongAxis = Vector3.Dot(offset, transform.right);

        // Bounce between limits
        if (distanceAlongAxis >= moveDistance)
        {
            direction = -1f;
        }
        else if (distanceAlongAxis <= -moveDistance)
        {
            direction = 1f;
        }
    }

    // 🎭 Handles spawn/despawn WITHOUT stopping movement
    IEnumerator VisibilityRoutine()
    {
        while (true)
        {
            // Wait before appearing
            yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));

            // Assign random sprite
            if (fishSprites.Count > 0)
            {
                sr.sprite = fishSprites[Random.Range(0, fishSprites.Count)];
            }

            yield return Fade(0f, 1f);

            // Stay visible
            yield return new WaitForSeconds(activeDuration);

            yield return Fade(1f, 0f);
        }
    }

    // 🎨 Smooth fade using LERP
    IEnumerator Fade(float from, float to)
    {
        sr.enabled = true;

        float t = 0f;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, t / fadeTime);

            sr.color = new Color(1f, 1f, 1f, alpha);

            yield return null;
        }

        if (to == 0f)
        {
            sr.enabled = false;
        }
    }

    // 🟢 Gizmos (rotation-aware)
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Vector3 rightDir = transform.right;

        Vector3 left = transform.position - rightDir * moveDistance;
        Vector3 right = transform.position + rightDir * moveDistance;

        Gizmos.DrawLine(left, right);
        Gizmos.DrawSphere(left, 0.2f);
        Gizmos.DrawSphere(right, 0.2f);
    }
}