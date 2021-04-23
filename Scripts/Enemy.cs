using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    DungeonGenerator DG;
    GameObject gameManager;

    public Image healthBar;
    public float speed = 6f;
    public int health = 20;
    public int maxHealth = 20;
    public int moveRange;
    public float startWaitTime;
    public Vector2 playerPosition;
    public bool sawPlayer;

    private Rigidbody2D rB;
    private Animator anim;
    private Vector2 moveVelocity;
   
    private int xCord;
    private int yCord;
    private float mapk;
   
    private string enemyName;
    
    private float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        rB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        DG = gameManager.GetComponent<DungeonGenerator>();
        mapk = DG.mapk;
        enemyName = gameObject.name.Split('(')[0];
    }

    // Update is called once per frame
    void Update()
    {
       if (sawPlayer)
        {
            Move(playerPosition, speed * 5);
            anim.Play("Run" + enemyName);
        }
    }

    /*private IEnumerator EnemyTurn()
    {
        Patrol();
        go = true;
        yield return new WaitForSeconds(10f);
        go = false;            
    }*/

    private void FixedUpdate()
    {
        
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hitObj = collision.gameObject;
        
        if(hitObj.tag == "Wall" || hitObj.tag == "Enemy")
        {
            goBackX = !goBackX;
            Flip();
        }
    }*/

    void Flip()
    {
        Vector2 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void Move(Vector2 target, float moveSpeed)
    {
        xCord = Mathf.FloorToInt(transform.position.x / mapk + 0.5f);
        yCord = Mathf.FloorToInt(transform.position.y / mapk + 0.5f);
        MapManager.map[xCord, yCord].hasEnemy = false;
        MapManager.map[xCord, yCord].enemy = null;
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        xCord = Mathf.FloorToInt(transform.position.x / mapk + 0.5f);
        yCord = Mathf.FloorToInt(transform.position.y / mapk + 0.5f);
        MapManager.map[xCord, yCord].hasEnemy = true;
        MapManager.map[xCord, yCord].enemy = this.gameObject;

    }

    

    /*private void NoiseChecking()
    {
        for (int i = -4; i <= 4; i++)
        {
            for (int j = -4; j <= 4; j++)
            {
                int x = Mathf.FloorToInt(transform.position.x / mapk) + i;
                int y = Mathf.FloorToInt(transform.position.y / mapk) + j;
                if(MapManager.map[x , y].hasPlayer)
                {
                    Debug.Log("Игрок тут");
                }
            }
        }
    }*/
}
