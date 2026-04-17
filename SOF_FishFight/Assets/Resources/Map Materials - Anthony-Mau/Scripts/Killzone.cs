using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    [Header("Player References (Optional)")]
    public List<GameObject> players = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        // First: check manual list (more precise)
        int playerIndex = players.IndexOf(other.gameObject);

        if (playerIndex != -1)
        {
            KillPlayer(other.gameObject, playerIndex + 1);
            return;
        }

        // Fallback: check by tag
        if (other.CompareTag("Player"))
        {
            int detectedIndex = players.IndexOf(other.gameObject);

            // If not in list, assign generic number
            int playerNumber = detectedIndex != -1 ? detectedIndex + 1 : 0;

            KillPlayer(other.gameObject, playerNumber);
        }
    }

    void KillPlayer(GameObject player, int playerNumber)
    {
        if (playerNumber > 0)
            Debug.Log($"Player {playerNumber} killed.");
        else
            Debug.Log($"A player was killed (not assigned in list).");

        // Disable ALL functionality
        player.SetActive(false);

        // Alternative (if you want softer kill instead):
        // player.GetComponent<YourHealthSystem>()?.Die();
    }
}