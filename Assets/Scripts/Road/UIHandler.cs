using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [SerializeField] TMP_Text currentSpeed;
    [SerializeField] TMP_Text currentStatus;
    bool ready = true;
    int speed;
    int targetSpeed;
    bool brakeText;

    private void Update()
    {
        NewSpeed();
        currentSpeed.text = speed.ToString() + " Mph";

        if (GetComponentInParent<Driving>().brakeAmt > 0)
            currentStatus.text = "Braking...";
        else if (GetComponentInParent<Driving>().brakeAmt == 0)
            StartCoroutine(ShowHideBrake());
    }

    void NewSpeed()
    {
        targetSpeed = (int)GetComponentInParent<Driving>().currentSpeed;
        if (targetSpeed < 0)
            targetSpeed = 0;
        if(targetSpeed != speed && ready)
            StartCoroutine(HitTargetSpeed());
    }

    IEnumerator HitTargetSpeed()
    {
        ready = false;
        while(speed < targetSpeed)
        {
            speed++;
            yield return new WaitForSeconds(0.2f);
        }

        while(speed > targetSpeed)
        {
            speed--;
            yield return new WaitForSeconds(0.2f);
        }
        ready = true;
    }

    IEnumerator ShowHideBrake()
    {
        yield return new WaitForSeconds(0.5f);
        currentStatus.text = "";
    }
}
