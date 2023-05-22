using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IntegralV2 : Integral
{
    [SerializeField] private float speed = 5.0f;
    
    private new Rigidbody2D rigidbody;
    
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        direction = transform.right;
    }

    private void FixedUpdate()
    {
        if (direction.y == 0 && Random.Range(0,1000) % 200 == 0)
        {
            var y = transform.position.y > 0 ? -1 : 1;
            direction = new Vector3(0,y);
        }
        var position = transform.position;
        var newPosition = Vector2.MoveTowards(position, position + direction,
            speed * Time.deltaTime);
        rigidbody.MovePosition(newPosition);
        CheckCharacter();
    }
}
