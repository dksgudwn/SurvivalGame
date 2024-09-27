using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerComponent
{
	private readonly float _gravity = -9.8f;

	[SerializeField] private float walkSpeed, runSpeed, rotationSpeed, CameraMinAngle, CameraMaxAngle, MouseSensitivity = 100f;
	[SerializeField] private Camera PlayerSight;

	private CharacterController controller;

	public bool IsGround => controller.isGrounded;
	public bool IsRunning {  get; private set; }

	private Player _player;

	private Vector3 movementDirection;
	private float verticalVelocity;

	private float playerRotationY;
	private float playerRotationX;

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
		RotateCamera();
		RotateCharacter();
	}

	private void InputKeys()
	{
		// ���� Input Reader�� ������ ���� ����

		movementDirection.x = Input.GetAxis("Horizontal");
		movementDirection.z = Input.GetAxis("Vertical");
		//Ű �Է����� �̵� ���� �ޱ�

		movementDirection = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up) * movementDirection;
		//Ʋ���� ������ŭ ������ ���

		float mouseHorizontal = Input.GetAxis("Mouse X");
		float mouseVertical = Input.GetAxis("Mouse Y");

		playerRotationY += mouseVertical * MouseSensitivity * Time.deltaTime;
		playerRotationX += mouseHorizontal * MouseSensitivity * Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.LeftShift)) IsRunning = true;
		else if(Input.GetKeyUp(KeyCode.LeftShift)) IsRunning = false;
	}

	private void MovementCharacter()
	{
		float speed = IsRunning ? runSpeed : walkSpeed;
		movementDirection *= speed;

		controller.Move(movementDirection * Time.fixedDeltaTime);
	}

	private void RotateCharacter()
	{
		transform.rotation = Quaternion.Euler(0, playerRotationX, 0);
		//�÷��̾� �� ���� ȸ��
	}

	private void RotateCamera()
	{
		playerRotationY = Mathf.Clamp(playerRotationY, -CameraMinAngle, CameraMaxAngle);

		PlayerSight.transform.rotation = Quaternion.Euler(-playerRotationY, playerRotationX, 0);
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