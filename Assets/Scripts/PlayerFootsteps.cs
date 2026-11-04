
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioSource footstepAudio;
    public CharacterController controller;

    void Update()
    {
        if (controller == null || footstepAudio == null)
            return;

        Vector3 movement = controller.velocity;
        movement.y = 0f;

        bool isWalking = controller.isGrounded &&
                         movement.magnitude > 0.1f;

        if (isWalking && !footstepAudio.isPlaying)
        {
            footstepAudio.Play();
        }
        else if (!isWalking && footstepAudio.isPlaying)
        {
            footstepAudio.Stop();
        }
    }
}
