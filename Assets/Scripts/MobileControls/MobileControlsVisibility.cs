using UnityEngine;

public class MobileControlsVisibility : MonoBehaviour
{
    [SerializeField] private GameObject controlsRoot;

    [SerializeField] private bool showInEditor = false;

    private void Awake()
    {
        bool isMobilePlatform =
            Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer;

        bool isEditorTesting =
            Application.isEditor && showInEditor;

        controlsRoot.SetActive(isMobilePlatform || isEditorTesting);
    }
}