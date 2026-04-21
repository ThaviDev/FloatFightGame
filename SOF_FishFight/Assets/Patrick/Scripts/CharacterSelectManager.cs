using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager Instance;

    public string nextSceneName = "FightScene";

    private bool[] readyStates = new bool[4];
    private int[] selectedCharacters = new int[4];

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerReady(int playerIndex, int characterIndex)
    {
        readyStates[playerIndex] = true;
        selectedCharacters[playerIndex] = characterIndex;

        CheckAllReady();
    }

    void CheckAllReady()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!readyStates[i])
                return;
        }

        // Aquí puedes guardar selección si quieres persistencia
        Debug.Log("Todos listos, cargando escena...");

        SceneManager.LoadScene(nextSceneName);
    }
}