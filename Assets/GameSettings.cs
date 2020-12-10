using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public bool DisplayMouseCursor  = false;
    public bool LockMouseCursor     = true;
    // Start is called before the first frame update
    void Start()
    {
        displayMouseCursor(DisplayMouseCursor);
        lockMouseCursor(LockMouseCursor);
    }

    // Update is called once per frame
    void Update()
    {
        displayMouseCursor(DisplayMouseCursor);
        lockMouseCursor(LockMouseCursor);

    }
    void displayMouseCursor(bool value)
    {
        if (!value)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }

    }

    void lockMouseCursor(bool value)
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
        }
        
    }

}
