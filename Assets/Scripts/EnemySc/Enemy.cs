using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float heath;
    [SerializeField] protected Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float dmg)
    {
        heath -= dmg;
        HurtSequence();
        if(heath <= 0)
        {
            DeathSequence();
        }
    }

    public virtual void HurtSequence()
    {
        // do something
    }

    public virtual void DeathSequence()
    {
        // do something
    }
}
