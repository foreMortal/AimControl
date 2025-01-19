using UnityEngine;

public class HM_LiftStrafe : MovementTypeParent
{
    public float maxTime;
    private float time, strafeTime, deltaSpeed = 5, speed = 6.5f, moveInSetrainDirTime;
    private bool firstCall = true, ending;
    private int dirIndex = -1, movingType = 1, dirType;
    private Vector3[] directions;
    Vector3 delta, target, current;

    public override bool Starfe(float deltaTime, out Vector3 strafe)
    {
        if (firstCall)
        {
            MakeNewStrafe();
            movement.animator.SetTrigger("Jump");
            time = 0;
            moveInSetrainDirTime = 0.05f;
            speed = 6.5f;
            deltaSpeed = 5f;
            movement.GravityOn = false;
            movement.Velocity = new Vector3(0f, 6f, 0f);
            firstCall = false;
        }

        strafeTime += Time.deltaTime;
        if (strafeTime >= maxTime)
        {
            movingType = 3;
            strafeTime -= maxTime;
        }

        if (movingType == 1)
        {
            time += deltaTime;
            if (time >= moveInSetrainDirTime)
            {
                movingType = 2;
                time -= moveInSetrainDirTime;
            }
            strafe = current * speed;
        }
        else if(movingType == 2)
        {
            if (Vector3.Dot(target, current) <= 0.9)
            {
                current += deltaSpeed * deltaTime * delta;
            }
            else
            {
                ChangeDirection();

                current += deltaSpeed * deltaTime * delta;
            }
            strafe = current * speed;
        }
        else
        {
            if (!ending)
            {
                movement.Velocity = new Vector3(0f, 8f, 0f);
                movement.GravityOn = true;
                ending = true;
                time = 0;

                dirType = Random.Range(0, 3);
                if (dirType == 1)
                    current = transform.right;
                else if(dirType == 0)
                    current = transform.forward;
                else
                    current = -transform.right;

                if(dirType != 2)
                    current += transform.forward * Random.Range(0f, 1f);
                
                ending = true;
            }

            time += deltaTime;
            if(time <= 1f)
                strafe = current * 9f;
            else if(time <= 2f)
            {
                if (dirType == 0)
                    dirType = Random.Range(1, 3);
                if(dirType == 1)
                    strafe = transform.right * 9f;
                else
                    strafe = -transform.right * 9f;
            }
            else
            {
                ending = false;
                strafeTime = 0;
                firstCall = true;
                strafe = current * speed;
                return true;
            }
        }

        return false;
    }

    private void ChangeDirection()
    {
        dirIndex++;
        if (dirIndex >= directions.Length)
            dirIndex = 0;

        current = target;
        target = directions[dirIndex];
        delta = target - current;
        movingType = 1;
    }

    private void MakeNewStrafe()
    {
        Vector3 firstDir;
        Vector3 secondDir;

        //choosing start direction
        if (Random.Range(1, 3) == 1)
        {
            if (Random.Range(1, 3) == 1)
                firstDir = transform.right;
            else
                firstDir = -transform.right;

            if (Random.Range(1, 3) == 1)
                secondDir = transform.forward;
            else
                secondDir = -transform.forward;
        }
        else
        {
            if (Random.Range(1, 3) == 1)
                firstDir = transform.forward;
            else
                firstDir = -transform.forward;

            if (Random.Range(1, 3) == 1)
                secondDir = transform.right;
            else
                secondDir = -transform.right;
        }

        directions = new[] { secondDir, -firstDir, -secondDir, firstDir };

        current = directions[^1];
        target = directions[0];
        delta = target - current;
        dirIndex = 0;
        movingType = 1;
    }
}
