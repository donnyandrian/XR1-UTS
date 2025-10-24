using System.Collections;
using UnityEngine;

public class TraficLight : MonoBehaviour
{
    public Light[] lightsToActivate;
    public int[] lightsIndex;

    public float[] activationDurations;

    private static int currentIndex = 0;

    private Coroutine activeCoroutine;

    private void Start()
    {
        ActivateNextInQueue();
    }

    public void ActivateNextInQueue()
    {
        // Stop any previous coroutine
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);

            int previousIndex = (currentIndex == 0) ? activationDurations.Length - 1 : currentIndex - 1;

            if (previousIndex >= 0 && previousIndex < activationDurations.Length)
            {
                for (int i = 0; i < lightsIndex.Length; i++)
                {
                    if (lightsIndex[i] == previousIndex)
                    {
                        lightsToActivate[i].enabled = false;
                    }
                }
            }
        }

        activeCoroutine = StartCoroutine(ActivationRoutine(currentIndex));

        currentIndex++;

        if (currentIndex >= activationDurations.Length)
        {
            currentIndex = 0;
        }
    }

    private IEnumerator ActivationRoutine(int index)
    {
        for (int i = 0; i < lightsIndex.Length; i++)
        {
            if (lightsIndex[i] == index)
            {
                Light objectToActivate = lightsToActivate[i];

                if (objectToActivate != null)
                {
                    objectToActivate.enabled = true;
                }
            }
        }

        yield return new WaitForSeconds(activationDurations[index]);

        for (int i = 0; i < lightsIndex.Length; i++)
        {
            if (lightsIndex[i] == index)
            {
                Light objectToActivate = lightsToActivate[i];

                if (objectToActivate != null)
                {
                    objectToActivate.enabled = false;
                }

            }
        }

        activeCoroutine = null;
        ActivateNextInQueue();
    }
}
