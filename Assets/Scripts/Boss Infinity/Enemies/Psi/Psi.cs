using UnityEngine;
using UnityEngine.Serialization;

public class Psi : Enemy
{
    [SerializeField] protected float speed = 4.0f;
    [SerializeField] protected Vector3 targetPosition;
    
    private new Rigidbody2D rigidbody;
    public Vector3 direction;

    protected virtual void Start()
    {
        Damage = 1;
        rigidbody = GetComponent<Rigidbody2D>();
        var transform1 = transform;
        direction = transform1.up;
    }

    private protected virtual void FixedUpdate()
    {
        var position = transform.position;
        if ((position - targetPosition).magnitude < 0.1) direction.y *= -1;
        var newPosition = Vector2.MoveTowards(position, position + direction,
            speed * Time.deltaTime);
        rigidbody.MovePosition(newPosition);
    }

    public override void ReceiveDamage(int damage)
    {
        
    }
    
    public void SetTarget(Vector3 target) => targetPosition = target;
}
