using UnityEngine;
using System.Collections;

public class AreYouSure : MonoBehaviour {
	
	public Material icon_hover_texture;
	public Material icon_base_texture;
	public Material icon_click_texture;
	
	public Material text_hover_texture;
	public Material text_base_texture;
	public Material text_click_texture;
	
	public Transform icon;
	public Transform text;
	
	public int ButtonCode = -1;
	public Transform level1;

	// Use this for initialization
	void Start () {
		icon.renderer.material = (icon_base_texture);
		text.renderer.material = (text_base_texture);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown() {
		icon.renderer.material = (icon_click_texture);
		text.renderer.material = (text_click_texture);
    }
	void OnMouseUp() {
		icon.renderer.material = (icon_hover_texture);
		text.renderer.material = (text_hover_texture);
		if(OnAction()) {
			icon.renderer.material = (icon_base_texture);
			text.renderer.material = (text_base_texture);	
		}
    }
	void OnMouseEnter() {
		icon.renderer.material = (icon_hover_texture);
		text.renderer.material = (text_hover_texture);
    }
	void OnMouseExit() {
        icon.renderer.material = (icon_base_texture);
		text.renderer.material = (text_base_texture);
	}
	bool OnAction() {
		switch(ButtonCode) {
		case 0:
			level1.gameObject.SetActive(true);
			GameObject.Find("Level2 Exit").SetActive(false);
			return true;
		case 1:
			Application.Quit();
			return true;
		default:
			return false;
		}
	}
}
