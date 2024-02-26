using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject[] doors;

    private Vector3 initialPos1;
    private Vector3 initialPos2;

    private bool isTrue = false;
    private bool returnTrue = false;
    private bool opendoor = false;

    public bool GetIsTrue
    {
        get { return isTrue; }

        set
        {
            isTrue = value;
            if (GetIsTrue == true)
            {
                opendoor = true;
                StartCoroutine(OpenDoor());
            }
            else if (GetIsTrue == false)
            {
                opendoor = false;
                StartCoroutine(CloseDoor());
            }
        }
    }

    public bool GetReturnTrue
    {
        get { return returnTrue; }
        set { returnTrue = value; }
    }

    private void Start()
    {
        initialPos1 = doors[0].transform.position;
        initialPos2 = doors[1].transform.position;

    }

    IEnumerator OpenDoor()
    {
        while (opendoor == true)
        {
            Vector3 target = new Vector3(3, 0, 0);

            doors[0].transform.Translate(target * 1f * Time.deltaTime);
            doors[1].transform.Translate(target * 1f * Time.deltaTime);

            yield return null;
        }

        yield return null;
    }

    IEnumerator CloseDoor()
    {
        while (opendoor == false)
        {
            Vector3 currentPos1 = doors[0].transform.position;
            Vector3 currentPos2 = doors[1].transform.position;


            doors[0].transform.position = Vector3.Lerp(currentPos1, initialPos1, Time.deltaTime);
            doors[1].transform.position = Vector3.Lerp(currentPos2, initialPos2, Time.deltaTime);

            yield return null;
        }

        yield return null;
    }
}
