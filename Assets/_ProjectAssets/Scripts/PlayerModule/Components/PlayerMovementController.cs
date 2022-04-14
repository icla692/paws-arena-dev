using Anura.ConfigurationModule.Managers;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
	[SerializeField] private Collider2D ceilingCollider;

	private Rigidbody2D rigidbody2D;
	private Vector3 velocity = Vector3.zero;
	private bool facingRight;

	private void Awake()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private bool CheckIfIsGrounded()
	{
		return Physics2D.IsTouchingLayers(ceilingCollider);
	}


	public void Move(float move, bool jump)
	{
		if (move < 0 && !facingRight)
		{
			Flip();
		}
		else if (move > 0 && facingRight)
		{
			Flip();
		}

		if (CheckIfIsGrounded() || GetAirControl())
		{
			var targetVelocity = new Vector2(move * 10f, rigidbody2D.velocity.y);
			rigidbody2D.velocity = Vector3.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, GetMovementSmoothing());

		}

		if (CheckIfIsGrounded() && jump)
		{
			rigidbody2D.AddForce(Vector2.up * GetJumpForce(), ForceMode2D.Impulse);
		}
	}

	private bool GetAirControl()
	{
		return ConfigurationManager.Instance.Config.GetAirControl();
	}

	private float GetJumpForce()
	{
		return ConfigurationManager.Instance.Config.GetPlayerJumpForce();
	}

	private float GetMovementSmoothing()
	{
		return ConfigurationManager.Instance.Config.GetMovementSmoothing();
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		var theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}