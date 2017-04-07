﻿using UnityEngine;
using System;
using System.Collections.Generic;


namespace Battle.Comp
{
	public class Jumping : CharacterComponent
	{
		[SerializeField]
		float jumpingSpeed;


		float jumpSpeedX;
		bool jumping;

		List<Type> requiredComponents = new List<Type> {
			typeof(Attacking),
			typeof(Moving),
			typeof(Animating)
		};

		public void Perform()
		{
			if (!jumping && !GetComp<Attacking>().IsAttacking()) {
				jumping = true;
				jumpSpeedX = Math.Sign(GetComp<Moving>().SpeedX()) * jumpingSpeed;
				GetComp<Animating>().SetTrigger(Defs.Animations.Jump);
			}
		}

		public void Stop()
		{
			jumping = false;
		}

		public bool IsJumping()
		{
			return jumping;
		}

		public void SetSpeedX (float speed)
		{
			jumpSpeedX = speed;
		}

		public override void UpdateMe()
		{
			if (jumping) {
				var pos = transform.position;
				pos.x += jumpSpeedX * Time.deltaTime;
				transform.position = pos;
			}
		}
	}
}

