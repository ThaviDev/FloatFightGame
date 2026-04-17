using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    [Header("Players (optional but recommended)")]
    public List<GameObject> players = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.transform.root.gameObject;

        if (!obj.CompareTag("Player")) return;

        int index = players.IndexOf(obj);

        if (index != -1)
        {
            Debug.Log($"Player {index + 1} killed.");
        }
        else
        {
            Debug.Log($"{obj.name} killed (not in list).");
        }

        Destroy(obj);
    }
}