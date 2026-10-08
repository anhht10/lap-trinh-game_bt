using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private CinemachineCamera fpCamera;
    [SerializeField] private CinemachineCamera tpCamera;
    [SerializeField] private CinemachineInputAxisController inputAxisControl;

    public CinemachineCamera FPCamera => fpCamera;
    public CinemachineCamera TPCamera => tpCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetPlayer(Transform fpTarget, Transform tpTarget)
    {
        fpCamera.LookAt = fpTarget;
        fpCamera.Follow = fpTarget;
        tpCamera.LookAt = tpTarget;
        tpCamera.Follow = tpTarget;
    }

    public void SwitchCamera(bool switchToTP)
    {
        fpCamera.Priority = switchToTP ? 0 : 10;
        tpCamera.Priority = switchToTP ? 10 : 0;
    }

    public void OnLockMouse(bool lockMouse)
    {
        inputAxisControl.enabled = lockMouse;
    }
}
