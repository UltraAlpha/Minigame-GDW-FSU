using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAniManager : MonoBehaviour
{
    // THIS SCRIPT uses public variables from PlayerController, so this short reminder is important for cross-script communication:

    // public variables / objects can be accessed in other scripts, and show in the Inspector.
    // Objects / variables marked with [SerializeField] will show in the inspector, even when private.
    // Objects / variables marked with [HideInInspector] will not show in the inspector, even when public.
    // private objects / variables can't be accessed in other scripts, and don't show in the inspector.

    //The script in question. To call a script, type:
    //Public (cs script name), (Since the script is now a variable in this script, you can give it whatever nickname you like. I used "pc" for PlayerController.)
    public PlayerController pc;

    // This script is meant to be used alongside PlayerController.cs, and should change animations for "The Thief".
    // This script turns playerAnimator's bools on and off depending on player behavior and game state to trigger animation transitions from playerAnimator.

    // Animator will be called several times in this script.
    public Animator pam;

    // This script changes player's animations depending on game state and keyboard input. Whenever GameObject is called, it refers to The Thief.

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the animator. If this script is attached to the "The Thief" GameObject, then this should take that animator.
        pam = GetComponent<Animator>();
        
        // Player will start in idle pose.
        // All bools should be set to false already in Animator. Idle is the default state, and other states are layered on top of idle as needed.
    }

    // Update is called once per frame
    void Update()
    {
        // Idle is implemented by default. Line 40 only ensures the animator is on the "idle" node before continuing.
        if (pam != null)
        {
            // When setting bools, format as follows:
                // (animator variable name).SetBool("(bool name as string)", (true / false))
            // There are 5 bools: player_run, player_jump, player_jump_rise, player_sus, and player_hide.
            // Animator hierarchy of bools should be structured as follows. As you implement the bools to this script, read the following top the bottom, left to right, in order of indentation.
                // (Idle state is not a bool. This animation plays if all bools are false, and is the default animation. Associated with "idle".)
                    // player_hide (is true if the current game state is either red_light, red_light_safe, or red_light_caught. Overrides all other bools if true. Associated with "hide". If the player is airborne, player_hide is also associated with "hide_air".)
                        // player_jump (is true if player is airborne. If pc.isOnGround is true, this is false. Associated with "jump_fall".)
                    // player_jump_rise (is true if player pressed jump. When jump velocity <= 0, this becomes false. Associated with "jump_rise".)
                    // player_run (is true if player is inputting left or right. If pc.isOnGround is true, animation will play. Associated with "run".) 
                    // player_sus (is true if the player is holding down or S. Overrides all other bools except for player_hide if true. Associated with "sus".)

            // Implement the player_hide bool here, but ONLY IF if background animator is created and implemented first.

            // Implement the player_sus bool here.

            // Implement both the player_jump_rise and player_jump bools here.

            // Implement the player_run bool here.

     
        }
    }
}
