using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject projectile;
    private GameObject _clone;

    private void Update()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _clone = Instantiate(projectile, transform.position, transform.rotation);
            _clone.GetComponent<Rigidbody>().AddForce(_clone.transform.forward * 100);
        } 
    }
    
}
