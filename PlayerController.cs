using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace MyStuff.MyScripts
{
    //script makes the player able to move via controller input
    public class PlayerController : MonoBehaviour
    {
        //receives 2-dimensional input from SteamVR
        [SerializeField] SteamVR_Action_Vector2 input;
        [Tooltip("Sets the Player Movement Speed")]
        [SerializeField] float movementSpeed = 4f;
        
        //enables connection with Character Controller so the script can move the controller
        [SerializeField] CharacterController characterController;
        private void Start()
        {
            //setup connection between script and Character Controller
            characterController = GetComponent<CharacterController>();
        }
        private void FixedUpdate()
        {
            //receives x and z axis to enable 2D movement
            var direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
        
            //moves the player based on direction, gravity, time and movementSpeed
            characterController.Move(movementSpeed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up) -
                                     new Vector3(0, 9.83f, 0) * Time.deltaTime);
        }
    }
}