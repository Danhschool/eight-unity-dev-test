using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerAttack attack { get; private set; }
    public PlayerMove move { get; private set; }
    public PlayerAnimController animController { get; private set; }

    private void Awake()
    {
        move = GetComponent<PlayerMove>();
        attack = GetComponentInChildren<PlayerAttack>();
        animController = GetComponentInChildren<PlayerAnimController>();
    }
}
