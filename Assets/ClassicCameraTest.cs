using UnityEngine;

public class ClassicCameraTest : MonoBehaviour
{
    public Transform player;
    private Controlls controls;
    public float recoilCurve, verSense, horSense, ver, hor, xRot, yRot, deadzone, time, averageHor;
    public bool vertical, horizontal;
    public float dopPorovorotX, dopPorovorotY, dopRampTime, dopDelay, outerTreshhold;
    private bool timeStop;
    private float absMove, deadzoneSquared, tyt;
    private int frames;
    private float allHor;
    private float dopSpeedX, dopSpeedY, dopTimer, blendDopX, blendDopY;

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    private void Awake()
    {
        if(dopRampTime > 0)
        {
            dopSpeedX = dopPorovorotX / dopRampTime;
            dopSpeedY = dopPorovorotY / dopRampTime;
        }
        deadzone /= 100f;
        outerTreshhold = (100f - outerTreshhold) / 100f;
        deadzoneSquared = deadzone * deadzone;
        controls = new Controlls();
        controls.MenuControl.OpenSett.performed += ctx =>
        {
            if (!timeStop)
            {
                timeStop = true;
                Time.timeScale = 0f;
            }
            else
            {
                timeStop = false;
                time = 0f;
                Time.timeScale = 1f;
            }
        };
    }

    private void Update()
    {
        Vector2 move = controls.GamepadControl.Newaction1.ReadValue<Vector2>();
        float mag = move.magnitude;
        if (mag > deadzone)
        {
            float hSense = horSense;
            float vSense = verSense;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                print("Mag before: " + mag);
                print("Move before: " + move);
            }

            if (mag > 1)
                mag = 1;
            float t = 1 - outerTreshhold;
            t += deadzone;
            mag = (mag - t) / (outerTreshhold - deadzone);
            //mag = (mag - (1 - outerTreshhold)) / outerTreshhold;
            //mag = (mag - deadzone) / (1 - deadzone);

            move = move.normalized * mag;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                print("Mag after: " + mag);
                print("Move after: " + move);
            }

            float horMove = move.x;
            float verMove = move.y;

            hor = mag;

            allHor += horMove;
            averageHor = allHor / ++frames;

            /*if(mag >= 1)
            {
                blendDopX = DopPovorot(blendDopX, dopPorovorotX, dopSpeedX);
                blendDopY = DopPovorot(blendDopY, dopPorovorotY, dopSpeedY);

                hSense += blendDopX;
                vSense += blendDopY;
            }
            else if (dopTimer > 0 || blendDopX > 0 || blendDopY > 0)
            {
                blendDopX = 0f;
                blendDopY = 0f;
                dopTimer = 0f;
            }*/

            //float m = Mathf.Pow(mag, recoilCurve);

            if (horizontal)
                xRot += horMove * hSense * Time.deltaTime;
            if (vertical)
                yRot += verMove * vSense * Time.deltaTime;
        }
        
        player.rotation = Quaternion.Euler(-yRot, xRot, 0f);
    }

    private float DopPovorot(float currentState, float dopPovorot, float speed)
    {
        if(dopDelay > 0)
        {
            if (dopTimer >= dopDelay)
            {
                currentState = RampTime(currentState, dopPovorot, speed);
            }
            else
                dopTimer += Time.deltaTime;
        }
        else
        {
            currentState = RampTime(currentState, dopPovorot, speed);
        }
        return currentState;
    }

    private float RampTime(float currentState, float dopPovorot, float speed)
    {
        if(dopRampTime > 0f)
        {
            if(currentState < dopPovorot)
            {
                currentState += speed * Time.deltaTime;
                if (currentState >= dopPovorot)
                    currentState = dopPovorot;
            }
            else
            {
                currentState = dopPovorot;
            }
        }
        else
        {
            currentState = dopPovorot;
        }
        return currentState;
    }
    /*Vector2 move = controls.GamepadControl.Newaction1.ReadValue<Vector2>();
        float mag = move.magnitude;
        if (mag > deadzone)
        {
            float hSense = horSense;
            float vSense = verSense;

            move = move.normalized * (mag - deadzone) / (1 - deadzone);
            mag = (mag - deadzone) / (1 - deadzone);

            move /= 0.99f;
            mag /= 0.99f;

            float horMove = move.x;
            float verMove = move.y;

            hor = mag;
            if (mag >= 1f)
                mag = 1;
            if (verMove >= 1f)
                verMove = 1;
            if (horMove >= 1f)
                horMove = 1f;

            allHor += horMove;
            averageHor = allHor / ++frames;

            if(mag >= 1)
            {
                blendDopX = DopPovorot(blendDopX, dopPorovorotX, dopSpeedX);
                blendDopY = DopPovorot(blendDopY, dopPorovorotY, dopSpeedY);

                hSense += blendDopX;
                vSense += blendDopY;
            }
            else if (dopTimer > 0 || blendDopX > 0 || blendDopY > 0)
            {
                blendDopX = 0f;
                blendDopY = 0f;
                dopTimer = 0f;
            }

            //float m = Mathf.Pow(mag, recoilCurve);

            if (horizontal)
                xRot += horMove * hSense * Time.deltaTime;
            if (vertical)
                yRot += verMove * vSense * Time.deltaTime;*/
}
