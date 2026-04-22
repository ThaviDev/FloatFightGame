using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonCambiarEscena : MonoBehaviour
{
    public string nombreEscena;

    public void CambiarEscena()
    {
        SceneManager.LoadScene(nombreEscena);
    }
}