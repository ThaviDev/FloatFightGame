using UnityEngine;
using UnityEngine.InputSystem;

public class C_Jugador : MonoBehaviour
{
    private PlayerInput m_PlayerInput;
    private InputAction m_MoveInput;

    private Rigidbody m_rb;
    //private Vector2 m_MoveDir2DRaw;
    private Vector2 m_MoveDir;
    [SerializeField] float m_MoveMaxSpeed;
    [SerializeField] float m_MoveAccel;
    [SerializeField] float m_MoveDecel;
    private void Awake()
    {
        m_PlayerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        m_MoveInput = m_PlayerInput.actions["Move"];
        //jumpInput.performed += OnJump() *EJEMPLO DE SUSCRIPCION DE EVENTO*
    }
    private void OnDisable()
    {
        //jumpInput.performed -= OnJump() *POR SI SE USA SUSCRIPCION*
    }
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        m_MoveDir = m_MoveInput.ReadValue<Vector2>();

    }
    private void FixedUpdate()
    {
        MovimientoFisico();
    }

    private void MovimientoFisico()
    {
        if (m_MoveDir != Vector2.zero)
        {
            m_rb.AddForce(m_MoveDir * m_MoveAccel, ForceMode.Force);
            // Limitar Velocidad Maxima
            if (m_rb.linearVelocity.magnitude > m_MoveMaxSpeed)
            {
                m_rb.linearVelocity = m_rb.linearVelocity.normalized * m_MoveMaxSpeed;
            }
        }
        else
        {
            m_rb.AddForce(m_rb.linearVelocity * -m_MoveDecel, ForceMode.Force);
        }
    }
}
