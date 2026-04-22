using UnityEngine;
using UnityEngine.UI;

public class C_HealthUI : MonoBehaviour
{
    [SerializeField] Image[] m_Lives;
    [SerializeField] RectTransform m_HealthBar;
    [SerializeField] C_Jugador m_jugador;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_jugador != null)
        {
            for (int i = 0; i < m_Lives.Length; i++)
            {
                if (i < m_jugador.Lives)
                {
                    m_Lives[i].enabled = true;
                }
                else
                {
                    m_Lives[i].enabled = false;
                }
            }
            /*
            for (int i = 0; i < m_jugador.Lives; i++)
            {
                m_Lives[i].enabled = true;
            } */
            m_HealthBar.localScale = new Vector3(m_jugador.CurrentHealth / m_jugador.MaxHealth, 1, 1);
        }
    }
    public void SetPlayer(C_Jugador player)
    {
        m_jugador = player;
    }
}
