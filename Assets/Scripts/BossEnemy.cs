using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public int lives = 3;
    private bool weak = false;
    private Enemy enemy;
    private BossColor bossColor;
    private AggressiveEnemy aggresivenes;
    [SerializeField]
    private float hitbackForce = 200;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        bossColor = GetComponent<BossColor>();
        aggresivenes = GetComponent<AggressiveEnemy>();
        enemy.killOnFall = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (lives > 0 && transform.position.y <= -10)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = new Vector3(0, 5, 0);
        }
    }

    public void DealDamage()
    {
        lives--;
        if (lives <= 0)
        {
            weak = true;
            enemy.StopMovement();
            aggresivenes.TurnOffHit();
            bossColor.TurnOffColor();
            enemy.killOnFall = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && weak)
        {
            Rigidbody bossRb = gameObject.GetComponent<Rigidbody>();
            Vector3 direction = transform.position - collision.gameObject.transform.position;
            bossRb.AddForce(direction.normalized * hitbackForce, ForceMode.Impulse);
        }
    }
}
