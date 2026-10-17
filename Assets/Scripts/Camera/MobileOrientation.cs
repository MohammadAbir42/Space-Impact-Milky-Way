
using UnityEngine;

public class MobileOrientation : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        Screen.orientation = ScreenOrientation.AutoRotation;
#endif
    }
}
