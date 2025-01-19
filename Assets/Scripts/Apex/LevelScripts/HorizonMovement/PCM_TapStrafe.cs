using UnityEngine;

public class PCM_TapStrafe : MovementTypeParent
{
    public float changeDirPreferedTime, speed, longStrafe, shortStrafe, force;
    private float changeDirTimer, time, strafeTime;
    private int dirIndex, startDir, posOrNegFr, posOrNegSc, type, circles, slide;
    private bool firstCall = true;
    private Vector3 current, target, t;
    private Vector3[] directions;

    public override bool Starfe(float deltaTime, out Vector3 strafe)
    {
        if (firstCall)
        {
            startDir = Random.Range(0, 2);
            posOrNegFr = Random.Range(0, 2);
            posOrNegSc = Random.Range(0, 2);
            slide = Random.Range(0, 2);

            if (posOrNegFr != 1)
                posOrNegFr = -1;

            if (posOrNegSc != 1)
                posOrNegSc = -1;

            strafeTime = shortStrafe;
            changeDirTimer = 0;
            time = 0;
            circles = 0;
            dirIndex = 0;
            type = 1;

            current = movement.PrevDir;

            if (startDir == 0)
                current = transform.forward * posOrNegFr;
            else
                current = transform.right * posOrNegFr;

            if (startDir == 0)
                target = transform.right * posOrNegSc;
            else
                target = transform.forward * posOrNegSc;

            t = current;

            SetVecs();
            movement.Velocity += new Vector3(0, force, 0);
            movement.animator.SetTrigger("Jump");
            firstCall = false;
        }

        if(type == 1)
        {
            time += deltaTime;
            if (time >= strafeTime)
            {
                if (++circles >= 2)
                {
                    movement.animator.SetBool("Slide", false);
                    firstCall = true;
                    strafe = t * speed;
                    return true;
                }
                type = 2;
                time = 0;
            }
        }
        else
        {
            changeDirTimer += deltaTime;
            float r = changeDirTimer / changeDirPreferedTime;
            if (r >= 1)
            {
                r = 1;
                ChangeDirection();
                changeDirTimer = 0;
            }

            t = Vector3.Slerp(current, target, r);
        }
        strafe = t * speed;
        return false;
    }

    private void OnDrawGizmos()
    {
        Ray r = new Ray(transform.position, t);
        Gizmos.DrawRay(r);
    }

    private void ChangeDirection()
    {
        dirIndex++;

        if (dirIndex < directions.Length)
        {
            current = target;
            target = directions[dirIndex];
        }
        else
        {
            type = 1;
            if (slide == 0)
                movement.animator.SetBool("Slide", true);
            strafeTime = longStrafe;
        }
    }

    private void SetVecs()
    {
        if (startDir == 0)
            directions = new Vector3[] { posOrNegSc * transform.right, posOrNegFr * -transform.forward };
        else
            directions = new Vector3[] { posOrNegSc * transform.forward, posOrNegFr * -transform.right };
    }
}
