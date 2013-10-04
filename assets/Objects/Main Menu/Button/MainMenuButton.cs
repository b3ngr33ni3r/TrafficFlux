using UnityEngine;
using System.Collections;

public class MainMenuButton : MonoBehaviour {
	
	public Material icon_hover_texture;
	public Material icon_base_texture;
	public Material icon_click_texture;
	
	public Material text_hover_texture;
	public Material text_base_texture;
	public Material text_click_texture;
	
	public Transform icon;
	public Transform text;
	
	public int ButtonCode = -1;

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
	
	public Transform level2_exit;
	
	bool OnAction() {
		switch (ButtonCode) {
		case 0: // start
			/*
			 * launches the game
			 */ 
			return false;
		case 1: // options
			/*
			 * obviously we need an options menu
			 */
			return false;
		case 2: // friends
			/*
			 * probably some screen that show friends and their rankings/locations
			 */
			return false;
		case 3: // exit
			/* 
			 * needs to have like an 'are you sure' dialoge
			 */
			level2_exit.gameObject.SetActive(true);
			GameObject.Find("Level1").SetActive(false);
			//Application.Quit();
			return true;
		default:
			return false;
		}
	}
}
