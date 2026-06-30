using System;
using UnityEngine;

public class SkullManKnockBack : MonoBehaviour
{
    [SerializeField] private float knockbackPower = 1.0f;
    [SerializeField] private Monster monster;
    [SerializeField] private DogAnimation dogAnimation;
    private Rigidbody2D rb;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dogAnimation = GetComponent<DogAnimation>();
    }
    public void Knockback()
    {


        dogAnimation.Hit();
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
