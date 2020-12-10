using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject target = null;
    private Vector3 offset;
    void Start()
    {
        target = null;
    }

private void OnTriggerStay(Collider col)
    {
        if(col.tag == "Player")
        {
            target = col.gameObject;
            offset = target.transform.position - transform.position;
        }

      
    }
    void OnTriggerExit(Collider col)
    {

        if (col.tag == "Player")
        {
            target = null;
           
        }

    }
    void LateUpdate()
    {

      
        if (target !=null)
        {
            if (target.tag == "Player")
            {
                target.transform.position = transform.position + offset;
            }

         
        }

    }
        // Update is called once per frame
    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        transform.Translate(20f * (transform.forward * Mathf.Cos(Time.time) * Time.deltaTime));
    }
}
