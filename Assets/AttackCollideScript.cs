using UnityEngine;

public class AttackCollideScript : MonoBehaviour
{
    [SerializeField] GameObject player;

    int damage = 0;
    private void Start()
    {
        damage = player.GetComponent<PlayerScript>().GetDamage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;
        if (tag.Equals("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyScript>();
            enemy.GetDamage(damage);
        }
    }
}
