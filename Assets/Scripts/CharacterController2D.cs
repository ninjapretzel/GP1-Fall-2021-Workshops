using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharacterController2D : MonoBehaviour {

	/// <summary> Draw some debugging information </summary>
	public bool DEBUG_DRAW = false;

	public Collider2D col;
	public float snapDistance = 1f/16f;

	/// <summary> Preallocated array for collisions </summary>
	private Collider2D[] collisions = new Collider2D[16];
	/// <summary> Preallocated array for collisions </summary>
	private RaycastHit2D[] raycastHits = new RaycastHit2D[16];

	/// <summary> Attempt to move the object, and get how far they actually moved. </summary>
	/// <param name="movement"> Requested movement </param>
	/// <returns> Applied movement </returns>
	public Vector3 Move(Vector3 movement) {
		col = GetComponent<Collider2D>();
		//Physics.BoxCastNonAlloc()
		Vector3 moved = Vector3.zero;
		moved += DoMove(new Vector3(0, movement.y, 0));
		while (moved.y == 0 && Math.Abs(movement.y) > snapDistance) {
			movement.y *= .5f;
			moved += DoMove(new Vector3(0, movement.y, 0));
		}
		moved += DoMove(new Vector3(movement.x, 0, 0));
		while (moved.x == 0 && Math.Abs(movement.x) > snapDistance) {
			movement.x *= .5f;
			moved += DoMove(new Vector3(movement.x, 0, 0));
		}
		return moved;
	}

	/// <summary> Attempt to move the object, and get how far they actually moved. `</summary>
	/// <param name="movement"> Requested movement. </param>
	/// <returns> Applied movement </returns>
	public Vector3 DoMove(Vector3 movement) {
		bool move = true;

		if (col != null) {
			if (col is BoxCollider2D) {
				BoxCollider2D box = col as BoxCollider2D;
				Vector3 point = box.transform.position + (Vector3)box.offset + movement;
				DrawBox(point, box.size / 2f, Color.blue);
				int numCollisions = Physics2D.OverlapBoxNonAlloc(point, box.size, 0, collisions);
				if (numCollisions != 0) {
					for (int i = 0; i < numCollisions; i++) {
						if (collisions[i] == col) { continue; }
						if (!collisions[i].isTrigger) {
							move = false;
						}
					}
				}
			}

		}

		if (move) {
			transform.position = transform.position + movement;
			return movement;
		}

		return Vector3.zero;
	}

	/// <summary> Debug helper to draw boxes </summary>
	/// <param name="p"></param>
	/// <param name="s"></param>
	/// <param name="color"></param>
	protected void DrawBox(Vector3 p, Vector3 s, Color? color = null) {
		Color c = color ?? Color.white;
		Vector3 c1 = s;
		Vector3 c2 = s; c2.x *= -1;
		Vector3 c3 = s; c3.x *= -1; c3.y *= -1;
		Vector3 c4 = s; c4.y *= -1;

		Debug.DrawLine(p + c1, p + c2, c);
		Debug.DrawLine(p + c3, p + c2, c);
		Debug.DrawLine(p + c3, p + c4, c);
		Debug.DrawLine(p + c1, p + c4, c);
	}

	/// <summary> Check if the object would touch something when their collider is swept along a direction </summary>
	/// <param name="sweep"> Sweep direction/distance vector </param>
	/// <returns> True if they would hit something, false otherwise. </returns>
	public bool IsTouching(Vector3 sweep) {
		if (col != null) {
			if (col is BoxCollider2D) {
				BoxCollider2D box = col as BoxCollider2D;
				Vector3 point = box.transform.position + (Vector3)box.offset;
				//float adjWidth = 1f;


				if (DEBUG_DRAW) { // Draw the touching check.

					DrawBox(point, box.size * .5f);
					DrawBox(point + sweep, box.size * .5f, Color.cyan);
				}

				Vector2 adjSize = box.size;
				//adjSize.x *= adjWidth;

				int numCollisions = Physics2D.BoxCastNonAlloc(point, adjSize, 0, sweep, raycastHits, sweep.magnitude + snapDistance);
				if (numCollisions > 0) {
					int lowest = -1;
					float lowestDistance = sweep.magnitude + snapDistance;

					for (int i = 0; i < numCollisions; i++) {
						if (raycastHits[i].collider == col) { continue; } // Skip own collider.
						if (!raycastHits[i].collider.isTrigger) {

							if (raycastHits[i].distance < lowestDistance) {
								lowest = i;
								lowestDistance = raycastHits[i].distance;
							}

						}
					}

					if (lowest >= 0) {
						return true;
					}

				}
			}
		}

		return false;
	}
}
