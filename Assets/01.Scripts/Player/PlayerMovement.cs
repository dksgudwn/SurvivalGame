using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerComponent
{
	private readonly float _gravity = -9.8f;

	[SerializeField] private float walkSpeed, runSpeed, rotationSpeed, CameraMinX = -90f, CameraMaxX = 90f;
	[SerializeField] private Camera PlayerSight;

	private CharacterController controller;

	public bool IsGround => controller.isGrounded;
	public bool IsRunning {  get; private set; }

	private Player _player;

	private Vector3 movementDirection;
	private float verticalVelocity;

	private float playerRotationX;
	private float playerRotationY;

	public void Initialize(Player player)
	{
		_player = player;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		if (TryGetComponent(out controller) == false) Debug.LogError("Player ChacterController is Null");
	}

	private void Update()
	{
		InputKeys();
	}

	private void FixedUpdate()
	{
		ApplyGravity();
		MovementCharacter();
		RotationCharacter();
	}

	private void InputKeys()
	{
		// 추후 Input Reader로 수정할 수도 있음

		movementDirection.x = Input.GetAxisRaw("Horizontal");
		movementDirection.z = Input.GetAxisRaw("Vertical");
		movementDirection = PlayerSight.transform.TransformDirection(movementDirection);

		float mouseHorizontal = Input.GetAxisRaw("Mouse X") * rotationSpeed * Time.deltaTime;
		float mouseVertical = Input.GetAxisRaw("Mouse Y") * rotationSpeed * Time.deltaTime;

		playerRotationX -= mouseVertical;
		playerRotationY += mouseHorizontal;

		if (Input.GetKeyDown(KeyCode.LeftShift)) IsRunning = true;
		else if(Input.GetKeyUp(KeyCode.LeftShift)) IsRunning = false;
	}

	private void MovementCharacter()
	{
		float speed = IsRunning ? runSpeed : walkSpeed;
		movementDirection *= speed * Time.fixedDeltaTime;

		controller.Move(movementDirection);
	}

	private void RotationCharacter()
	{
		playerRotationX = Mathf.Clamp(playerRotationX, -CameraMinX, CameraMaxX);

		PlayerSight.transform.rotation = Quaternion.Euler(playerRotationX, playerRotationY, 0);
		transform.rotation = Quaternion.Euler(0, playerRotationY, 0);
	}

	private void ApplyGravity()
	{
		if (IsGround && verticalVelocity < 0)
			verticalVelocity = -2f;
		else
			verticalVelocity += _gravity * Time.fixedDeltaTime;

		movementDirection.y = verticalVelocity;
	}


}
