using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


[Serializable] public class SpriteAnim {
	public Sprite[] poses;
	public float animSpeed = 12;
	public Sprite Frame(float time) {
		if (poses == null || poses.Length == 0) { return null; }
		int frame = ((int)time) % poses.Length;
		return poses[frame];
	}
}

public class SidescrollController : CharacterController2D {


	[Header("Settings")]
	public float speed = 7;
	public float gravity = 33;
	public float jumpPower = 15;
	public float terminalVelocity = 40;
	public float skinWidth = .01f/16f;
	public Vector3 velocity;

	bool isGrounded;
	float facing;
	Vector3 input, movement, moved;

	[Header("Sprites")]
	public bool spritesFaceRight = false;
	public SpriteAnim currentAnim;
	SpriteAnim lastAnim;
	public SpriteAnim idle;
	public SpriteAnim walking;
	public SpriteAnim jump;
	public SpriteAnim fall;
	public float walkAnimSpeed = 4;
	float animTime = 0;

	/// <summary> See if we are currently grounded.</summary>
	public bool CheckGrounded() { return velocity.y <= 0 && IsTouching(Vector2.down * skinWidth); }
	/// <summary> Check if we will touch the ground during the next frame </summary>
	public bool CheckWillTouchGround() { return velocity.y <= 0 && IsTouching(new Vector2(0, velocity.y * Time.deltaTime)); }

	void Update() {
		input = Vector3.zero;

		if (Input.GetKey("left")) { input.x -= 1; }
		if (Input.GetKey("right")) { input.x += 1; }
		input = input.normalized;
		if (input.x != 0) {
			facing = Mathf.Sign(input.x);
		}

		if (isGrounded) {
			velocity.y = 0;
			if (Input.GetButtonDown("Jump")) {
				velocity.y = jumpPower;
			}
		} else {
			velocity.y -= gravity * Time.deltaTime;
		}

		if (velocity.y > 0 && Input.GetButtonUp("Jump")) {
			velocity.y = 0;
		}
		if (velocity.y < -terminalVelocity) { velocity.y = -terminalVelocity; }
		movement = input * speed;
		movement += velocity;
		
		moved = Move(movement * Time.deltaTime);
		if (moved.x == 0 && velocity.x != 0) { velocity.x *= .5f; }
		if (velocity.y < 0 && CheckWillTouchGround()) { velocity.y *= .5f; }
		isGrounded = CheckGrounded();


		UpdateAnimation();
	}

	SpriteRenderer rend;
	public void UpdateAnimation() {
		if (rend == null) { rend = GetComponent<SpriteRenderer>(); }
		if (rend == null) { return; }

		if (input.x < 0) { rend.flipX = spritesFaceRight; } 
		if (input.x > 0) { rend.flipX = !spritesFaceRight; }

		if (Mathf.Abs(moved.x) > 0) {
			currentAnim = walking;
		} else {
			currentAnim = idle;
		}
		if (!isGrounded) {
			currentAnim = (velocity.y > 0) ? jump : fall;
		}

		animTime += Time.deltaTime * currentAnim.animSpeed;
		if (currentAnim != lastAnim) { animTime = 0; }
		rend.sprite = currentAnim.Frame(animTime);
		lastAnim = currentAnim;
	}


}
