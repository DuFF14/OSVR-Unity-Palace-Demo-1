using UnityEngine;
using System.Collections;

public class ScaleControls : MonoBehaviour {

    private Vector3 originalScale;

	// Use this for initialization
	void Start () {
        originalScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Equals))
        {
            transform.localScale += new Vector3(1f, 0, 1f) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Minus))
        {
            transform.localScale -= new Vector3(1f, 0, 1f) * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            transform.localScale = originalScale;
        }
    }
}
