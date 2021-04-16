using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //DungeonGenerator dungeon;

    //public Vector2Int playerPosition;
    
    public Image healthBar;
    public float speed = 3f;
    public int health = 20;
    public int maxHealth = 20;

    //private new CameraScript camera;
    private Transform canvasTransform;
    private Rigidbody2D rB;
    private Animator anim;
    private Vector2 inputMovement;
    private Vector2 moveVelocity;

    public void Start()
    {
        rB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canvasTransform = GameObject.Find("UI").transform;
        healthBar.fillAmount = 1f;

        Image toInstance = Instantiate(healthBar, new Vector3(130, 69, 0), Quaternion.identity) as Image; 
        toInstance.transform.SetParent(canvasTransform);
        //camera = GetComponent<CameraScript>();
    }

    public void Update()
    {
        Actions();
        HealthBarChange();
    }

    private void Actions()
    {
        inputMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveVelocity = inputMovement * speed;
        if (Input.GetMouseButtonDown(0))
            anim.SetTrigger("isAttackA1");
        if (inputMovement.x != 0 || inputMovement.y != 0)
        {
            if (inputMovement.y > 0)
                anim.SetBool("runFrontA1", true);
                //anim.SetTrigger("runFrontTrig");
            if (inputMovement.y < 0)
                anim.SetBool("runBackA1", true);
            if (inputMovement.x < 0)
                anim.SetBool("runLeftA1", true);
            if (inputMovement.x > 0)
                anim.SetBool("runRightA1", true);

            Debug.Log("x: " + inputMovement.x);
            Debug.Log("y: " + inputMovement.y);

        }
        else
        {
            anim.SetBool("runFrontA1", false);
            anim.SetBool("runBackA1", false);
            anim.SetBool("runLeftA1", false);
            anim.SetBool("runRightA1", false);

        }
    }

    private void HealthBarChange()
    {
        healthBar.fillAmount = health / maxHealth;
    }

    public void FixedUpdate()
    {
        rB.MovePosition(rB.position + moveVelocity * Time.deltaTime);
    }

}
