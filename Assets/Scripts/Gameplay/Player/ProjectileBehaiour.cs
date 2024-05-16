using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float Speed = 4.5f;

    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;

    // Update is called once per frame
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.right * Speed;
    }

    private void OnBecameInvisible()
    {
        //gameObject.SetActive(false); // Deactivate the bullet
        // Return the bullet to the object pool
        ObjectPool.SharedInstance.ReturnPooledObject(gameObject);
    }

    public void Fire(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        gameObject.SetActive(true);
        // Apply velocity
        if (rigidbody != null)
        {
            rigidbody.velocity = transform.right * Speed;
        }
    }

}
