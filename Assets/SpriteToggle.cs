using UnityEngine;
using System.Collections;
using KopernikusWrapper;


public class SpriteToggle : MonoBehaviour {

	public Sprite sprite1, sprite2;
	private SpriteRenderer spriteRenderer;

	// IT'S A HACKATHON, GO CRAZY
	public static SpriteToggle INSTANCE;

	void Start() {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		INSTANCE = this;
		Debug.Log("gear selector setup");
	}

	public void Appear() {
		spriteRenderer.enabled = true;
		setForward();
		Debug.Log("gear selector appears!");
	}

	private void setBackward() {
		spriteRenderer.sprite = sprite2;
		Vehicle.INSTANCE.SetGear(GearDirection.GEAR_DIRECTION_BACKWARD);
		Debug.Log("gear set to BACKWARD");
	}

	private void setForward() {
		spriteRenderer.sprite = sprite1;
		Vehicle.INSTANCE.SetGear(GearDirection.GEAR_DIRECTION_FORWARD);
		Debug.Log("gear set to FORWARD");
	}

	void OnMouseDown() {
		if (Vehicle.INSTANCE != null) {
			if (spriteRenderer.sprite.Equals(sprite1)) setBackward();
			else setForward();
		} else {
			Debug.Log("no vehicle to shift gears!");
		}
	}
}

