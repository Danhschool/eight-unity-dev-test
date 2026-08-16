using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    private Player player;

    [SerializeField] private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Player>();
    }

    public void Speed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }
    public void Jump()
    {
               animator.SetBool("isJump", true);
    }
    public void Land()
    {
        animator.SetBool("isJump", false);
    }
    public void Attack()
    {
        animator.SetBool("isAttack", true);
    }
    public void EndAttack()
    {
        animator.SetBool("isAttack", false);
    }
    public void SetClimb(bool isClimbing, float direction = 0f)
    {
        animator.SetBool("isClimb", isClimbing);
        animator.SetFloat("ClimbSpeed", direction);
    }
    public void PauseAnim()
    {
        animator.speed = 0f;
    }
    public void RunAnim()
    {
        animator.speed = 1f;
    }
}
