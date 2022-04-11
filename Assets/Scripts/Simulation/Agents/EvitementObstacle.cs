/*


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui permet à un agent d'éviter un obstacle
/// 
/// 
/// Fait par HAMICHE Bilal le 09/04/2022.
/// </summary>
public class EvitementObstacle : AgentAction
{

	private CharacterController controller;

	private float verticalVelocity;
	private float gravity = 14.0f;
	private float jumpForce = 10.0f;


	private void Start()
	{
		controller = GetComponent<CharacterController>();
	}




	// ou private
	public override void update()
	{
		if (controller.isGrounded)
		{
			verticalVelocity = -gravity * Time.deltaTime;
			if (Input.GetKeyDown(KeyCode.Space))
			{
				verticalVelocity = jumpForce;

			}
			else
			{
				verticalVelocity -= gravity * Time.deltaTime;
			}

			Vector3 moveVector = new Vector3(0, verticalVelocity, 0);
			controller.Move(moveVector * Time.deltaTime);

		}

	}

}
*/
