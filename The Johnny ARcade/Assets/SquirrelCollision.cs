using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelCollision : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collider)
    {
        gameObject.GetComponent<Animation>().clip.SampleAnimation(this.gameObject,0);
        AddScore();
    }

    public void AddScore()
    {
        
    }
}
