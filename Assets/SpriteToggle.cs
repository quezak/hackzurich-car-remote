using UnityEngine;
using System.Collections;
using KopernikusWrapper;


public class SpriteToggle : MonoBehaviour {

	public Sprite sprite1, sprite2;
	private SpriteRenderer spriteRenderer;
	private int spriteNo;

	// IT'S A HACKATHON, GO CRAZY
	public static SpriteToggle INSTANCE;

	void Start() {
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		INSTANCE = this;
		Debug.Log("gear selector setup");
	}

	public void Appear() {
		spriteRenderer.enabled = true;
		spriteNo = 0;
		spriteRenderer.sprite = sprite1;
		Debug.Log("gear selector appears!");
	}

	void OnMouseDown() {
		if (Vehicle.INSTANCE != null) {
			if (spriteNo == 0) {
				spriteRenderer.sprite = sprite2;
				spriteNo = 1;
				Vehicle.INSTANCE.SetGear(GearDirection.GEAR_DIRECTION_BACKWARD);
				Debug.Log("gear set to BACKWARD");
			} else {
				spriteRenderer.sprite = sprite1;
				spriteNo = 0;
				Vehicle.INSTANCE.SetGear(GearDirection.GEAR_DIRECTION_FORWARD);
				Debug.Log("gear set to FORWARD");
			}
		} else {
			Debug.Log("no vehicle to shift gears!");
		}
	}
}

