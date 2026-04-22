using UnityEngine;
using TMPro;

public class MostrarRanking : MonoBehaviour
{
    public TMP_Text[] textos; // 4 textos

    void Start()
    {
        var ranking = GameManager.Instance.ranking;

        for (int i = 0; i < ranking.Count; i++)
        {
            textos[i].text = ranking[ranking.Count - 1 - i];
        }
    }
}