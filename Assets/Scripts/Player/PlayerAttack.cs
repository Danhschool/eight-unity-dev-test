using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Player player;

    [Header("Attack Setting")]
    public Transform attackPoint;
    public float attackRange = 1.5f;
    public LayerMask enemyLayers;

    [Header("Attack Rate")]
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    public bool isAttackPressed;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        if (CameraTour.isTouring) return;

        if (isAttackPressed && Time.time >= nextAttackTime)
        {
            player.animController.Attack();

            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    public void Attackk()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers, QueryTriggerInteraction.Collide);

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<GemController>().TakeDame();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
