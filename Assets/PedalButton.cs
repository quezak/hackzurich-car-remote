using UnityEngine;
using System.Collections;
using KopernikusWrapper;

public class PedalButton : MonoBehaviour {

    public Sprite pedal;
    private SpriteRenderer spriteRenderer;

    public static PedalButton instance;

    void Start() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		instance = this;
		Debug.Log("pedal button setup");
    }

    public float GetY() {
        return transform.position.y;
    }

    public void Appear() {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = pedal;
    }

    public void Go(float touchY) {
        //transform.position = Vector3.Lerp(transform.position, new Vector3(7, touchY, 0), Time.time);
        Vector3 oldPos = transform.position;
        float newY = (float)touchY / Screen.height * 10.0f - 5.0f;
        oldPos.y = newY;
        transform.position = oldPos;
        Debug.Log(touchY);
    }

    /*public void GoUp() {
        transform.Translate(Vector3.up * Time.deltaTime * 5);
    }

    public void GoDown() {
        transform.Translate(Vector3.down * Time.deltaTime * 5);
    }*/
}