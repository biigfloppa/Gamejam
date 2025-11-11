using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float speed = 10f;
    Vector3 moveDir = Vector3.zero;

    public void SetBullet(Vector3 dir, float spd)
    {

        speed = spd;
        moveDir = dir;
    }

    void Update()
    {
        /*Vector3 point = moveDir;
        Vector3 lookDir = point - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);*/
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyScript>().SetStopTime(5);
            Destroy(gameObject);
        }
    }
}
