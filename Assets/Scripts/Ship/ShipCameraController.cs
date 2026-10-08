using UnityEngine;

public class ShipCameraController : MonoBehaviour
{
    [SerializeField] private Transform fpTarget;
    [SerializeField] private Transform tpTarget;

    private ShipInputHandler _input;

    private void Awake()
    {
        _input = GetComponent<ShipInputHandler>();
    }

    private void Start()
    {
        CameraManager.Instance.SetPlayer(fpTarget, tpTarget);
    }

    private void Update()
    {
        if (_input.TPCameraTogger)
        {
            CameraManager.Instance.SwitchCamera(true);
            if (_input.MouseLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                CameraManager.Instance.OnLockMouse(true);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                CameraManager.Instance.OnLockMouse(false);
            }
        }
        else
        {
            CameraManager.Instance.SwitchCamera(false);
        }
    }

}
