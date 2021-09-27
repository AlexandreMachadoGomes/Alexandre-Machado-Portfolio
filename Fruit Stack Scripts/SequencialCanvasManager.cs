using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencialCanvasManager : MonoBehaviour
{

    public Transform[] CanvasOrder;

    private int counter = 0;

    public bool NextCanvas()
    {
        if(CanvasOrder.Length == 0 || counter >= CanvasOrder.Length-1) return false;

        int oldCounter = counter;
        counter = Mathf.Clamp(counter + 1, 0, CanvasOrder.Length);

        // Disable past canvas
        CanvasOrder[oldCounter].gameObject.SetActive(false);

        // Enable next canvas
        CanvasOrder[counter].gameObject.SetActive(true);

        return true;
    }

    public bool PreviousCanvas()
    {
        if(CanvasOrder.Length == 0 || counter <= 0) return false;

        int oldCounter = counter;
        counter = Mathf.Clamp(counter - 1, 0, CanvasOrder.Length);

        // Disable past canvas
        CanvasOrder[oldCounter].gameObject.SetActive(false);

        // Enable next canvas
        CanvasOrder[counter].gameObject.SetActive(true);

        return true;

    }
}
