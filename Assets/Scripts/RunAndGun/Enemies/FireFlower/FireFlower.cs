using UnityEngine;

public class FireFlower : StaticEnemy
{
    private AudioManager audioManager;
    private GameObject fireEffect;
    private Fireball fireball;
    private Animator animator;
    
    public float attackRate = 0.5f;
    private float nextAttackTime;
    
    private void Awake()
    {
        fireEffect = Resources.Load<GameObject>("RunAndGun/Effects/FireEffect");
        fireball = Resources.Load<Fireball>("RunAndGun/Effects/Fireball");
        audioManager = gameObject.GetComponentInChildren<AudioManager>();
        animator = gameObject.GetComponent<Animator>();
        nextAttackTime = Time.time + 1f / attackRate;
        Damage = 1;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(Time.time - (nextAttackTime - 1)) < 1e-3) FlareUp();
        if (Time.time >= nextAttackTime) Shoot();
    }

    private void FlareUp()
    {
        animator.SetTrigger("IsAttack");
        Instantiate(fireEffect, transform.position, Quaternion.identity);
        audioManager.Play("Attack");
    }
    
    private void Shoot()
    {
        var transform1 = transform;
        var flowerPosition = transform1.position;
        var attackPoint = flowerPosition + transform1.up * 5;
        var direction = characterPos.position - attackPoint;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Instantiate(fireball, attackPoint, Quaternion.Euler(0, 0, angle));
        nextAttackTime = Time.time + 1f / attackRate;
    }
    
    protected override void Die()
    {
        Destroy(gameObject);
    }
}
