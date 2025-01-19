using UnityEngine;

public class HM_Bhop : MovementTypeParent
{
    public float range;
    public float jumpForce;
    private bool firstCall = true, jumped;
    private float time, maxTime, speed = 6f, rangeCheck, dopDirt, jumpTime;
    private int dir;
    private Vector3 dopDir;

    public override bool Starfe(float deltaTime, out Vector3 strafe)
    {
        if (firstCall)
        {
            movement.animator.SetBool("Slide", true);
            maxTime = Random.Range(0.5f, 1.5f);
            if (Random.Range(1, 3) == 1)
                dir = 1;
            else
                dir = -1;
            firstCall = false;
        }

        rangeCheck += deltaTime;
        time += deltaTime;
        jumpTime += deltaTime;

        if (rangeCheck >= 0.2f)
        {
            Vector3 t = new Vector3(movement.Player.position.x, 0f, movement.Player.position.z) - new Vector3(transform.position.x, 0f, transform.position.z);
            if (t.sqrMagnitude > range)
                dopDirt = 1f;
            else
                dopDirt = -1f;
            rangeCheck -= 0.2f;
        }
        
        strafe = dir * speed * transform.right;

        dopDir = (movement.Player.position - transform.position) * dopDirt;
        dopDir.y = 0;
        strafe += dopDir;

        if (jumpTime >= 0.3f && !jumped)
        {
            movement.animator.SetTrigger("Jump");
            movement.Velocity = new Vector3(0f, jumpForce, 0f);
            jumped = true;
        }
        else if(jumpTime >= 0.4f && jumped)
        { 
            jumped = false;
            jumpTime -= 0.4f;
        }

        if (time >= maxTime)
        {
            movement.animator.SetBool("Slide", false);
            time = 0f;
            firstCall = true;
            return true;
        }
        else
        {
            return false;
        }
    }
}
