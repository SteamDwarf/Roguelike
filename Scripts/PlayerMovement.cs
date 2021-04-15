using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //DungeonGenerator dungeon;

    //public Vector2Int playerPosition;
    public float speed = 3f;

    //private new CameraScript camera;
    private Rigidbody2D rB;
    private Animator anim;
    private Vector2 inputMovement;
    private Vector2 moveVelocity;

    public void Start()
    {
        rB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //camera = GetComponent<CameraScript>();
    }

    public void Update()
    {
        inputMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveVelocity = inputMovement * speed;
        if(inputMovement.x == 0 && inputMovement.y == 0)
        {
            anim.SetBool("isRunning", false);
        } else
        {
            anim.SetBool("isRunning", true);
            //camera.MoveCamera(moveVelocity);
        }
    }

    public void FixedUpdate()
    {
        rB.MovePosition(rB.position + moveVelocity * Time.deltaTime);
    }

}
