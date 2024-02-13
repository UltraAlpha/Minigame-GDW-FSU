using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    // timer float (fetches a random float between the min and max values below, then ticks down frame by frame / sec by sec to 0)
    // Then, hit the phase_change trigger when it hits 0.
    public float backgroundTimer;
    
    // green to yellow Random.Range floats
    [SerializeField] float minGreen = 6;
    [SerializeField] float maxGreen = 12f;

    // yellow to red Random.Range floats
    [SerializeField] float minYellow = 3f;
    [SerializeField] float maxYellow = 6f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
