using UnityEngine;
using System.Collections;
using KopernikusWrapper;


public class SpriteToggle : MonoBehaviour {

	public Sprite sprite1, sprite2;
	private SpriteRenderer spriteRenderer;
	private bool isForward;

	// IT'S A HACKATHON, GO CRAZY
	public static SpriteToggle INSTANCE;

	void Start() {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		INSTANCE = this;
		Debug.Log("gear selector setup");
	}

	public void Appear() {
		spriteRenderer.enabled = true;
		isForward = false;
		setForward();
		Debug.Log("gear selector appears!");
	}

	public void setBackward() {
		if (!isForward)
			return;
		spriteRenderer.sprite = sprite2;
		Vehicle.INSTANCE.SetGear(GearDirection.GEAR_DIRECTION_BACKWARD);
		isForward = false;
		Debug.Log("gear set to BACKWARD");
	}

	public void setForward() {
		if (isForward)
			return;
		spriteRenderer.sprite = sprite1;
		Vehicle.INSTANCE.SetGear(GearDirection.GEAR_DIRECTION_FORWARD);
		isForward = true;
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

