using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHorizonMovement : MonoBehaviour
{
    [SerializeField] private MovementTypeParent[] movementTypes;
    [SerializeField] private Transform player;

    private MovementTypeParent currentStrafe;
    private CharacterController controller;
    public Transform groundCheck;
    public Animator animator;
    
    public LayerMask layerMask;
    private Vector3 velocity, moveDirection, prevPos;

    private float groundDistance = 0.3f, gravity = -10f, deltaTime, rangeCheckTimer, range;
    private bool isGrounded, gravityOn = true;

    public Vector3 PrevDir { get { return (transform.position - prevPos).normalized; } }
    public Transform Player { get { return player; } }
    public Vector3 Velocity { get {return velocity; } set { velocity = value; } }
    public bool GravityOn { get { return gravityOn; } set { gravityOn = value; velocity = Vector3.zero; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        foreach(var m in movementTypes)
        {
            m.Setup(this);
        }
        currentStrafe = movementTypes[Random.Range(0, movementTypes.Length)];
    }

    void Update()
    {
        deltaTime = Time.deltaTime;
        rangeCheckTimer += deltaTime;
        
        if(Time.frameCount % 5 == 0)
            prevPos = transform.position;

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        bool ended = currentStrafe.Starfe(deltaTime, out moveDirection);

        if (ended)
            currentStrafe = movementTypes[Random.Range(0, movementTypes.Length)];

        Gravitation();

        controller.Move((moveDirection + velocity) * deltaTime);
    }

    private void Gravitation()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, layerMask);
        animator.SetBool("Grounded", isGrounded);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
        else if(gravityOn)
            velocity.y += gravity * deltaTime;
    }
}
