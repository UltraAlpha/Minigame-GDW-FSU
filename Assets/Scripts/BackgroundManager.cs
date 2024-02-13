using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public enum BackgroundState {
        Green,
        Yellow,
        Stopped,
        FoundYou,
        WinRound
    }
    
    // the animator object for the background
    public Animator bgAnim;

    // the current background state
    public BackgroundState state = BackgroundState.Green;

    // green to yellow Random.Range floats
    [SerializeField] float minGreen = 6;
    [SerializeField] float maxGreen = 12f;

    // yellow to red Random.Range floats
    [SerializeField] float minYellow = 3f;
    [SerializeField] float maxYellow = 6f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StepThroughState());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StepThroughState()
    {
        while (true){
            float greenTime = Random.Range(minGreen, maxGreen);
            float yellowTime = Random.Range(minYellow, maxYellow);

            yield return new WaitForSeconds(greenTime);
            state = BackgroundState.Yellow;
            Debug.Log("state = "+state);
            bgAnim.SetTrigger("phase_change");

            yield return new WaitForSeconds(yellowTime);
            state = BackgroundState.Stopped;
            Debug.Log("state = "+state);
            bgAnim.SetTrigger("phase_change");

            yield return new WaitForSeconds(1.5f);

            // replace "false" with logic here to determine if player is behind block
            if (false){
                state = BackgroundState.FoundYou;
                break;
            } else {
                state = BackgroundState.WinRound;
                Debug.Log("state = "+state);
                bgAnim.SetTrigger("phase_change");
                yield return new WaitForSeconds(1.5f);

                state = BackgroundState.Green;
                Debug.Log("state = "+state);
                bgAnim.SetTrigger("phase_change");
            }
        }
    }
}
