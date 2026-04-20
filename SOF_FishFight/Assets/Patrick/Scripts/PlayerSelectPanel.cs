using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectPanel : MonoBehaviour
{
    public int playerIndex; // 0–3

    [Header("Botones de personaje (3)")]
    public Button[] characterButtons;

    [Header("Botón Ready")]
    public Button readyButton;

    private int selectedCharacterIndex = -1;
    private bool isReady = false;

    private void Start()
    {
        readyButton.gameObject.SetActive(false);

        // Asignar listeners a botones de personaje
        for (int i = 0; i < characterButtons.Length; i++)
        {
            int index = i; // importante para closure
            characterButtons[i].onClick.AddListener(() => SelectCharacter(index));
        }

        readyButton.onClick.AddListener(SetReady);
    }

    void SelectCharacter(int index)
    {
        if (isReady) return;

        selectedCharacterIndex = index;

        readyButton.gameObject.SetActive(true); // <-- esto en lugar de interactable

        Debug.Log($"P{playerIndex + 1} seleccionó personaje {index}");
    }

    void SetReady()
    {
        if (selectedCharacterIndex == -1) return;

        isReady = true;
        readyButton.interactable = false;

        // Notificar al manager
        CharacterSelectManager.Instance.PlayerReady(playerIndex, selectedCharacterIndex);
    }
}