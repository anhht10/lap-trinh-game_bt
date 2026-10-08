using UnityEngine;

public class PlayerCameraLook : MonoBehaviour
{

	[Header("References")]
	[SerializeField] private Behaviour inputAxisController;

	[Header("Look")]
	[SerializeField] private bool lockCursorOnStart = true;

	private PlayerInputHandler inputHandler;

	private bool lookEnabled;
	public bool LookEnabled
	{
		get => lookEnabled;
		set => SetLookEnabled(value);
	}

	private void Awake()
	{
		inputHandler = GetComponent<PlayerInputHandler>();
		lookEnabled = lockCursorOnStart;
	}

	private void OnEnable()
	{
		ApplyCursorState();
	}

	private void OnDisable()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	// Update is called once per frame
	void Update()
	{
		bool shouldEnableLook = !inputHandler.CursorUnLockHeld;
		if (lookEnabled != shouldEnableLook)
			SetLookEnabled(shouldEnableLook);

		if (!lookEnabled)
			return;

	}

	public void SetLookEnabled(bool enabled)
	{
		lookEnabled = enabled;
		ApplyCursorState();
	}

	public void ToogleLook()
	{
		SetLookEnabled(!lookEnabled);
	}

	private void ApplyCursorState(string context = "")
	{
		Cursor.lockState = lookEnabled ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = !lookEnabled;
		if (inputAxisController != null)
			inputAxisController.enabled = lookEnabled;
	}
}