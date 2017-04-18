﻿using UnityEngine;

[RequireComponent(typeof(Character))]
public class KeyboadController : Controller
{
	[SerializeField]
	KeyCode left;

	[SerializeField]
	KeyCode right;

	[SerializeField]
	KeyCode up;

	[SerializeField]
	KeyCode down;

	[SerializeField]
	KeyCode specialAttack;

	[SerializeField]
	KeyCode fastAtack;

	[SerializeField]
	KeyCode heavyAttack;

	[SerializeField]
	KeyCode jump;


	[SerializeField]
	Character character;

	protected override void Init()
	{
		fastAtack = KeyCode.U;
		heavyAttack = KeyCode.K;
		specialAttack = (KeyCode)18;
		jump = (KeyCode)19;
	}

	void Update()
	{
		if (!Enabled)
			return;

		if (Input.GetKey(left)) {
			character.MoveH(-1);
		} else if (Input.GetKey(right)) {
			character.MoveH(1);	
		}
		if (Input.GetKey(up)) {
			character.MoveV(1);
		} else if (Input.GetKey(down)) {
			character.MoveV(-1);	
		}
		if (Input.GetKeyDown(jump)) {
			character.Jump();			
		}
		if (Input.GetKeyDown(fastAtack)) {
			character.FastAttack();			
		}
		if (Input.GetKeyDown(heavyAttack)) {
			character.HeavyAttack();			
		}

		if (Input.GetKeyUp(heavyAttack)) {
			character.ChargedAttackReleased();			
		}
			
		if (Input.GetKeyDown(specialAttack)) {
			character.AttackSpecial();			
		}


	}
}

