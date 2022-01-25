using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightHandler : MonoBehaviour
{
    [Header("Spheres")]
    [SerializeField] Renderer redLightSphere;
    [SerializeField] Renderer amberLightSphere;
    [SerializeField] Renderer greenLightSphere;

    [Header("Controls")]
    [SerializeField] NodeHandler node;
    [SerializeField] int lightTime = 5;
    [SerializeField] GameObject[] linkedTrafficLights;

    Color red;
    Color amber;
    Color green;
    Color fadeRed;
    Color fadeAmber;
    Color fadeGreen;

    int counter = 0;
    int amtOfTL;
    public int orderNum;

    bool lightActive = true;
    bool countUp;


    // Start is called before the first frame update
    void Start()
    {
        red = fadeRed = Color.red;
        amber = fadeAmber = Color.yellow;
        green = fadeGreen = Color.green;

        fadeRed.a = fadeAmber.a = fadeGreen.a = 0.1f;

        redLightSphere.material.color = red;
        amberLightSphere.material.color = fadeAmber;
        greenLightSphere.material.color = fadeGreen;

        amtOfTL = linkedTrafficLights.Length;
        //RandomizeOrder();
        StartCoroutine(LightSwitch());
    }

    /*
    void RandomizeOrder()
    {
        foreach(GameObject i in linkedTrafficLights)
        {
            int breakfree = 0;
            while(i.GetComponent<TrafficLightHandler>().orderNum == this.orderNum)
            {
                orderNum = Random.Range(0, linkedTrafficLights.Length + 1);
                breakfree++;
            }
            if (breakfree >9)
                print("BREAKFREE");
        }
        print("ORDER NUMBER = " + orderNum);
    }
    */

    IEnumerator LightSwitch()
    {
        float waitTime = lightTime * amtOfTL;
        if(orderNum != 0)
            yield return new WaitForSeconds(lightTime * orderNum);
        while (lightActive)
        {
            switch (counter)
            {
                case 0:
                    redLightSphere.material.color = red;
                    amberLightSphere.material.color = fadeAmber;
                    greenLightSphere.material.color = fadeGreen;
                    node.canGo = false;
                    yield return new WaitForSeconds(waitTime);
                    break;
                case 1:
                    redLightSphere.material.color = fadeRed;
                    amberLightSphere.material.color = amber;
                    greenLightSphere.material.color = fadeGreen;
                    node.canGo = false;
                    yield return new WaitForSeconds(1);
                    break;
                case 2:
                    redLightSphere.material.color = fadeRed;
                    amberLightSphere.material.color = fadeAmber;
                    greenLightSphere.material.color = green;
                    node.canGo = true;
                    yield return new WaitForSeconds(lightTime + 1);
                    break;
            }

            if (counter == 2)
                countUp = true;
            else if (counter == 0)
                countUp = false;

            if (!countUp)
                counter++;
            else
                counter--;

        }
        
    }
}
