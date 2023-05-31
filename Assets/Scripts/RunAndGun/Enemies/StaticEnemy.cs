using UnityEngine;
using UnityEngine.Serialization;

public class StaticEnemy : Enemy
{ 
    [SerializeField] private int lives = 5;
    public Transform characterPos;
    
    public override void ReceiveDamage(int damage)
    {
        lives -= damage;
        if (lives <= 0) Die();
    }
    
    protected override void OnTriggerEnter2D(Collider2D col) { }

    protected override void OnTriggerStay2D(Collider2D other) { }
}
