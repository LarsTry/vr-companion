using UnityEngine;
using Valve.VR.InteractionSystem;

namespace MyStuff.MyScripts
{
    //shows or hides the controllers
    public class ShowController : MonoBehaviour
    {
        [SerializeField] bool showController = false;
        void Update()
        {
            //applies showController to each hand
            foreach (var hand in Player.instance.hands)
            {
                if (showController)
                {
                    hand.ShowController();
                    hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithController);
                }
                else
                {
                    hand.HideController();
                    hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithoutController);
                }
            }
        }
    }
}
