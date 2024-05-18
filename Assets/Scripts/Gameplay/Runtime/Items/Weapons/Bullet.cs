using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float force = 200;
    public Rigidbody2D rb;
    public Weapon weapon { get; set; }


    void Start()
    {
        Transform child = Shell.instance.player.transform.GetChild(0);
        Vector2 direction = child.up;
        rb.AddForce(direction * force, ForceMode2D.Force);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.TakeDamage(weapon.damage);
            Destroy(gameObject);
        }
    }
}
