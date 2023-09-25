using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; private set; }
    public Vector2 MousePosition { get; private set; }

    private Camera _mainCam;

    private void Start()
    {
        _mainCam = Camera.main;
        if ((Instance && Instance != this) || !_mainCam)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }


    private void Update()
    {
        if (!Input.GetMouseButtonUp(0))
            return;

        MousePosition = _mainCam.ScreenToWorldPoint(Input.mousePosition);
    }
}