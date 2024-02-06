using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGoesLeft : MonoBehaviour
{
    public int moveSpeed = 3;
    public int leftBound = -17;
    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        // Box scrolls from right to left
        transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);
        
        // Destroy box when out of bounds
        if (transform.position.x < leftBound)
        {
            Destroy(gameObject);
        }
    }
}
