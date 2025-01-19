using UnityEngine;

public class StrafeTrain : MonoBehaviour
{
    private LookAtPlayer shoot;
    private Controlls controlls;
    private float mouvingTime, crouchTime, betweenCrouchTime, betweenFakesTime, damageUpTimer, damageDownTimer;
    private bool mouvingRight, mouvingLeft, crouch, faked, damageUpper = false, damageLower = false;

    private void OnEnable()
    {
        controlls.Enable();
    }
    private void OnDisable()
    {
        controlls.Disable();
    }

    private void Awake()
    {
        //ApexApplySettings.GiveData.AddListener(TakeData);
        controlls = new Controlls();
    }

    private void Update()
    {

        if(controlls.GamepadControl.MoveHor.ReadValue<float>() > 0f)
        {
            if (mouvingLeft)
            {
                Fake(mouvingTime);
                GoodStrafe(mouvingTime);
                mouvingTime = 0f;
            }
            mouvingLeft = false;
            mouvingTime += Time.deltaTime;
            mouvingRight = true;
        }
        if (controlls.GamepadControl.MoveHor.ReadValue<float>() == 0f)
        {
            damageUpper = true;
            damageLower = false;
        }
        if (controlls.GamepadControl.MoveHor.ReadValue<float>() < 0f)
        {
            if (mouvingRight)
            {
                Fake(mouvingTime);
                GoodStrafe(mouvingTime);
                mouvingTime = 0f;
            }
            mouvingRight = false;
            mouvingTime += Time.deltaTime;
            mouvingLeft = true;
        }
        if (crouch)
        {
            if (betweenCrouchTime < 0.2f && betweenCrouchTime != 0f)
            {
                //print("betweenCrouchTime < 0.2f");
                damageUpper = true;
                damageLower = false;
            }
            if (betweenCrouchTime >= 0.25f && betweenCrouchTime < 1f)
            {
                damageLower = true;
                damageUpper = false;
            }
            betweenCrouchTime = 0f;
            crouchTime += Time.deltaTime;
        }
        if (!crouch)
        {
            if(crouchTime < 0.2f && crouchTime != 0f)
            {
                BadCrouch();
            }
            else if (crouchTime >= 0.7f)
            {
                BadCrouch();
            }
            if (crouchTime > 0.2f && crouchTime < 1f)
            {
                GoodCrouch();
            }
            crouchTime = 0f;
            betweenCrouchTime += Time.deltaTime;
            //print(betweenCrouchTime);
        }
        Strafe();
        DamageCount();
        //print(shoot.hittingProcent);
    }

    private void GoodCrouch()
    {
        damageUpper = false;
        damageLower = true;
    }
    private void BadCrouch()
    {
        //print("badCrouch");
        damageUpper = true;
        damageLower = false;
    }
    private void GoodStrafe(float time)
    {
        if(time >= 0.5f && time <= 1.1f)
        {
            //print("GoodStrafe");
            damageUpper = false;
            damageLower = true;
        }
        if(time < 0.5f)
        {
            //print("BadStrafe");
            damageUpper = true;
            damageLower = false;
        }
    }
    private void Strafe()
    {
        if(betweenCrouchTime > 3f)
        {
            //print("betweenCrouchTime");
            damageUpper = true;
            damageLower = false;
        }
        if(mouvingTime > 1.2f)
        {
            //print("mouvingTime");
            damageUpper = true;
            damageLower = false;
        }
        if(betweenFakesTime > 2f)
        {
            //print("betweenFakesTime");
            damageUpper = true;
            damageLower = false;
        }
    }

    private void Fake(float time)
    {
        if(time <= 0.4f && betweenFakesTime >= 0.5f)
        { 
            betweenFakesTime = 0f;
        }
        if (time <= 0.4f && betweenFakesTime <= 0.5f)
        {
            damageLower = false;
            damageUpper = true;
        }
    }

    private void CrouchOn()
    {
        crouch = true;
    }
    private void CrouchOff()
    {
        crouch = false;
    }

    private void DamageCount()
    {
        if (damageUpper)
        {
            damageLower = false;
            damageUpTimer += Time.deltaTime;
            if(damageUpTimer >= 0.25f && shoot.hittingProcent < 1.25f)
            {
                shoot.hittingProcent += 0.25f;
                damageUpTimer = 0f;
            }
            if(shoot.hittingProcent > 1.25f)
            {
                shoot.hittingProcent = 1.25f;
            }
        }
        if (damageLower)
        {
            damageUpper = false;
            damageDownTimer += Time.deltaTime;
            if (damageDownTimer >= 0.75f && shoot.hittingProcent > 0.25f)
            {
                shoot.hittingProcent -= 0.25f;
                damageDownTimer = 0f;
            }
            if (shoot.hittingProcent < 0.25f)
            {
                shoot.hittingProcent = 0.25f;
            }
        }
    }

    private void Jump()
    {
        damageUpper = true;
    }

    private void TakeData(UserRebinds rebinds, Controlls controlls)
    {
        this.controlls = controlls;

        this.controlls.GamepadControl.Jump.performed += ctx => Jump();
    }
}
