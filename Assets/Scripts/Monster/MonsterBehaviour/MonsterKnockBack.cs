using System;
using UnityEngine;

public class MonsterKnockBack : MonoBehaviour
{
    [SerializeField] private float knockbackPower = 1.0f;
    [SerializeField] private Monster monster;
    private Rigidbody2D rb;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Knockback()
    {
        if(monster.Direction)
        {
            rb.linearVelocity = new Vector2(-knockbackPower, knockbackPower);
        }
        else 
        {
            rb.linearVelocity = new Vector2(knockbackPower, knockbackPower);
        }
    }
}
