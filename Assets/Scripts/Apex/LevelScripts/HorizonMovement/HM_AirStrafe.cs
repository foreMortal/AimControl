using UnityEngine;

public class HM_AirStrafe : MovementTypeParent
{
    public float minRange, maxRange;
    private float time;
    private bool firstCall = true, slided;
    private int dirIndex = -1, movingType = 1, circles, slidingType;
    private Vector3[] directions;
    Vector3 delta, target, current;

    public override bool Starfe(float deltaTime, out Vector3 strafe)
    {
        if (firstCall)
        {
            //Make it work;
            slided = false;
            dirIndex = -1;
            movingType = 1;

            circles = Random.Range(1, 3);
            MakeNewJump();
            firstCall = false;
        }

        if(movingType == 1)
        {
            time += deltaTime;
            if (time >= 0.05f)
            {
                movingType = 2;
                time -= 0.05f;
            }
            strafe = current * 6.5f;
        }
        else if(movingType == 2)
        {
            if (Vector3.Dot(target, current) <= 0.9)
            {
                current += 5 * deltaTime * delta;
            }
            else
            {
                ChangeDirection();

                current += 5 * deltaTime * delta;
            }
            strafe = current * 6.5f;
        }
        else
        {
            time += deltaTime;
            if(time >= 0.7f)
            {
                time -= 0.7f;
                movement.animator.SetBool("Slide", false);
                MakeNewJump();
                circles--;
            }
            strafe = current * 7.5f;
        }

        if(circles > 0)
            return false;
        else
        {
            firstCall = true;
            return true;
        }
    }

    private void ChangeDirection()
    {
        dirIndex++;
        if(dirIndex != 0)
            current = target;
        if(dirIndex < directions.Length)
        {
            target = directions[dirIndex];
            delta = target - current;
            movingType = 1;
        }
        else if (!slided)
        {
            if ((movement.Player.position - transform.position).magnitude < minRange)
                current = -transform.forward;
            else if((movement.Player.position - transform.position).magnitude > maxRange)
                current = transform.forward;

            if (slidingType == 1)
            {
                movement.animator.SetBool("Slide", true);
                slided = true;
            }
            movingType = 3;
        }
    }

    private void MakeNewJump()
    {
        slided = false;
        slidingType = Random.Range(1, 3);
        dirIndex = -1;

        Vector3 firstDir;
        Vector3 secondDir;

        //choosing start direction
        if (Random.Range(1, 3) == 1)
        {
            if (Random.Range(1, 3) == 1)
                current = firstDir = transform.right;
            else
                current = firstDir = -transform.right;

            if (Random.Range(1, 3) == 1)
                secondDir = transform.forward;
            else
                secondDir = -transform.forward;
        }
        else
        {
            if (Random.Range(1, 3) == 1)
                current = firstDir = transform.forward;
            else
                current = firstDir = -transform.forward;

            if (Random.Range(1, 3) == 1)
                secondDir = transform.right;
            else
                secondDir = -transform.right;
        }

        directions = new[] { secondDir, -firstDir, -secondDir };
        movement.animator.SetTrigger("Jump");
        ChangeDirection();
        movement.Velocity += new Vector3(0f, 4f, 0f);
        movingType = 1;
    }
}
