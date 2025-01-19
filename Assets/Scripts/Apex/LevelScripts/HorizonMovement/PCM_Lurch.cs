using UnityEngine;

public class PCM_Lurch : MovementTypeParent
{
    [Range(-1, 1)] public float slerp;
    [Range(0, 1)] public int type;
    public float changeDirPreferedTime, speed, time, jumpTime, force, shortTime, longTime, range;
    private float changeDirTimer, JumpTimer, globalTimer, correctTimer;
    private int dirIndex, startDir, posOrNegFr, posOrNegSc;
    private bool firstCall = true;
    private Vector3 current, target, t, correctVec;
    private Vector3[] directions;

    public override bool Starfe(float deltaTime, out Vector3 strafe)
    {
        if (firstCall)
        {
            //Make it work;
            time = Random.Range(1.5f, 4.5f);
            type = Random.Range(0, 2);
            startDir = Random.Range(0, 2);
            posOrNegFr = Random.Range(0, 2);
            posOrNegSc = Random.Range(0, 2);

            if (posOrNegFr != 1)
                posOrNegFr = -1;

            if (posOrNegSc != 1)
                posOrNegSc = -1;

            dirIndex = 0;

            if (type == 0)
                changeDirPreferedTime = 0.2f;

            SetVecs();
            /*current = movement.PrevDir;
            if (startDir == 0)
                target = transform.forward * posOrNegFr;
            else
                target = transform.right * posOrNegFr;*/
            if(startDir == 0)
                current = transform.forward * posOrNegFr;
            else
                current = transform.right * posOrNegFr;

            if(startDir == 0)
                target = transform.right * posOrNegSc;
            else
                target = transform.forward * posOrNegSc;

            firstCall = false;
        }

        globalTimer += deltaTime;
        changeDirTimer += deltaTime;
        JumpTimer += deltaTime;
        correctTimer += deltaTime;
        
        if(JumpTimer >= jumpTime)
        {
            movement.animator.SetTrigger("Jump");
            movement.Velocity += new Vector3(0, force, 0);
            JumpTimer = 0f;
        }

        float r = changeDirTimer / changeDirPreferedTime;
        if (r >= 1)
        {
            r = 1;
            ChangeDirection();
            changeDirTimer = 0;
        }
        
        t = Vector3.Slerp(current, target, r);


        if(correctTimer >= 0.3f)
        {
            Vector3 v = (movement.Player.position - transform.position);

            if (v.sqrMagnitude > range)
                correctVec = v;
            else
                correctVec = -v;
            correctTimer = 0;
        }

        strafe = t * speed;
        strafe += correctVec * deltaTime * 8f;

        if (globalTimer >= time)
        {
            firstCall = true;
            globalTimer = 0;
            return true;
        }
        else
            return false;
    }

    private void OnDrawGizmos()
    {
        Vector3 f = Vector3.Slerp(transform.forward, -transform.forward, slerp);
        Ray r = new Ray(transform.position, f);
        Gizmos.DrawRay(r);
    }

    private void ChangeDirection()
    {
        dirIndex++;
        if(type != 0)
        {
            if (dirIndex % 2 == 0)
            {
                changeDirPreferedTime = shortTime;
            }
            else
            {
                changeDirPreferedTime = longTime;
            }
        }

        if (dirIndex < directions.Length)
        {
            current = target;
            target = directions[dirIndex];

        }
        else
        {
            dirIndex = -1;
            SetVecs();
            ChangeDirection();
        }
    }

    private void SetVecs()
    {
        if(type == 0)
        {
            if(startDir == 0)
                directions = new Vector3[] {posOrNegSc * transform.right, posOrNegFr * -transform.forward, posOrNegSc * - transform.right, posOrNegFr * transform.forward, posOrNegSc * - transform.right, posOrNegFr * - transform.forward, posOrNegSc * transform.right, posOrNegFr * transform.forward};
            else
                directions = new Vector3[] { posOrNegSc * transform.forward, posOrNegFr * -transform.right, posOrNegSc * -transform.forward, posOrNegFr * transform.right, posOrNegSc * -transform.forward, posOrNegFr * -transform.right, posOrNegSc * transform.forward, posOrNegFr * transform.right };
        }
        else
        {
            if(startDir == 0)
                directions = new Vector3[] { posOrNegSc * transform.right, posOrNegFr * - transform.forward, posOrNegSc * -transform.right, posOrNegFr * transform.forward };
            else
                directions = new Vector3[] { posOrNegSc * transform.forward, posOrNegFr * -transform.right, posOrNegSc * -transform.forward, posOrNegFr * transform.right };
        }
    }
}
