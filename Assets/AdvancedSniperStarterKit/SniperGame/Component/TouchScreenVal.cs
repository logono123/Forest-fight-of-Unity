/// <summary>
/// Touch screen value. the touch screen controller System : Using one per Touch area
/// </summary>
using UnityEngine;
using System.Collections;

public enum TouchPosition
{
	Top,
	Middle,
	Bottom,
	TopLeft,
	TopRight,
	MiddleLeft,
	MiddleRight,
	BottomLeft,
	BottomRight
}

[System.Serializable]
public class TouchScreenVal
{
	
	public TouchPosition Position = TouchPosition.Middle;
	public Rect ButtonArea;
	[HideInInspector]
	public Rect AreaTouch;
	private Vector2 controllerPositionTemp;
	private Vector2 controllerPositionNext;
	public Texture2D ImgBG,ImgButton;
	
	public TouchScreenVal (Rect area, TouchPosition position)
	{
		Position = position;
		ButtonArea = area;
		Initial ();
	}
	
	public TouchScreenVal ()
	{
		Initial ();
	}

	public void Initial ()
	{
		Rect position = ButtonArea;
		switch (Position) {
		case TouchPosition.Top:
			AreaTouch = new Rect ((Screen.width / 2) - (position.width / 2) + position.x, position.y, position.width, position.height);
			break;
		case TouchPosition.TopLeft:
			AreaTouch = new Rect (position.x, position.y, position.width, position.height);
			break;
		case TouchPosition.TopRight:
			AreaTouch = new Rect ((Screen.width - position.width) + position.x, position.y, position.width, position.height);
			break;
		case TouchPosition.Middle:
			AreaTouch = new Rect ((Screen.width / 2) - (position.width / 2) + position.x, (Screen.height / 2) - (position.height / 2) + position.y, position.width, position.height);
			break;
		case TouchPosition.MiddleLeft:
			AreaTouch = new Rect (position.x, (Screen.height / 2) - (position.height / 2) + position.y, position.width, position.height);
			break;
		case TouchPosition.MiddleRight:
			AreaTouch = new Rect ((Screen.width - position.width) + position.x, (Screen.height / 2) - (position.height / 2) + position.y, position.width, position.height);
			break;
		case TouchPosition.Bottom:
			AreaTouch = new Rect ((Screen.width / 2) - (position.width / 2) + position.x, (Screen.height - position.height) + position.y, position.width, position.height);
			break;
		case TouchPosition.BottomLeft:
			AreaTouch = new Rect (position.x, (Screen.height - position.height) + position.y, position.width, position.height);
			
			break;
		case TouchPosition.BottomRight:
			AreaTouch = new Rect ((Screen.width - position.width) + position.x, (Screen.height - position.height) + position.y, position.width, position.height);
			
			break;
		}
		
		buttonPositionInitial = new Vector2 (AreaTouch.x + (AreaTouch.width / 2), AreaTouch.y + (AreaTouch.height / 2));
	
	}

	public void Update ()
	{
		for (var i = 0; i < Input.touchCount; ++i) {
			if(Input.GetTouch (i).phase == TouchPhase.Canceled || Input.GetTouch (i).phase == TouchPhase.Ended){
				IsPressed = false;
			}
		}
		Initial ();
	}

	public bool OnTouchPress ()
	{
		buttonPosition = buttonPositionInitial;
		bool res = false;
		for (var i = 0; i < Input.touchCount; ++i) {
			Vector2 touchpos = Input.GetTouch (i).position;
			if (Input.GetTouch (i).phase == TouchPhase.Began || Input.GetTouch (i).phase == TouchPhase.Stationary) {
				if ((touchpos.x >= AreaTouch.xMin && touchpos.x <= AreaTouch.xMax && Screen.height - touchpos.y >= AreaTouch.yMin && Screen.height - touchpos.y <= AreaTouch.yMax)) {
					res = true;
					IsPressed = true;
				}
			} else {
				IsPressed = false;	
			}
		}
		return res;
	}
	public bool OnTouchRelease ()
	{
		buttonPosition = buttonPositionInitial;
		bool res = false;
		for (var i = 0; i < Input.touchCount; ++i) {
			Vector2 touchpos = Input.GetTouch (i).position;
			if (Input.GetTouch (i).phase == TouchPhase.Ended || Input.GetTouch (i).phase == TouchPhase.Canceled) {
				if ((touchpos.x >= AreaTouch.xMin && touchpos.x <= AreaTouch.xMax && Screen.height - touchpos.y >= AreaTouch.yMin && Screen.height - touchpos.y <= AreaTouch.yMax)) {
					res = true;
					IsPressed = true;
				}
			} else {
				IsPressed = false;	
			}
		}
		return res;
	}
	
	
	public Vector2 OnTouchDirection (bool fixdrag)
	{
		
		buttonPosition = buttonPositionInitial;
		Vector2 direction = Vector2.zero;
		for (var i = 0; i < Input.touchCount; ++i) {
			Vector2 touchpos = Input.GetTouch (i).position;
			if ((touchpos.x >= AreaTouch.xMin && touchpos.x <= AreaTouch.xMax && Screen.height - touchpos.y >= AreaTouch.yMin && Screen.height - touchpos.y <= AreaTouch.yMax)) {
				if (Input.GetTouch (i).phase == TouchPhase.Began) {
					controllerPositionNext = new Vector2 (Input.GetTouch (i).position.x, Screen.height - Input.GetTouch (i).position.y);
					controllerPositionTemp = controllerPositionNext;
					buttonPosition = controllerPositionNext;
					IsPressed = true;
				} else {
					controllerPositionNext = new Vector2 (Input.GetTouch (i).position.x, Screen.height - Input.GetTouch (i).position.y);
					Vector2 deltagrag = (controllerPositionNext - controllerPositionTemp);
					direction.x += deltagrag.x;
					direction.y -= deltagrag.y;
					if (fixdrag)
						controllerPositionTemp = Vector2.Lerp (controllerPositionTemp, controllerPositionNext, 0.5f);
					buttonPosition = controllerPositionNext;
					IsPressed = true;
				}	
			}
		}
		direction.Normalize ();
		return direction;
	}
	
	public Vector2 OnDragDirection (bool fixdrag)
	{

		buttonPosition = buttonPositionInitial;
		Vector2 direction = Vector2.zero;
		for (var i = 0; i < Input.touchCount; ++i) {
			Vector2 touchpos = Input.GetTouch (i).position;
			if ((touchpos.x >= AreaTouch.xMin && touchpos.x <= AreaTouch.xMax && Screen.height - touchpos.y >= AreaTouch.yMin && Screen.height - touchpos.y <= AreaTouch.yMax)) {
				if (Input.GetTouch (i).phase == TouchPhase.Began) {
					controllerPositionNext = new Vector2 (Input.GetTouch (i).position.x, Screen.height - Input.GetTouch (i).position.y);
					controllerPositionTemp = controllerPositionNext;
					buttonPosition = controllerPositionNext;
					IsPressed = true;
				} else {
					controllerPositionNext = new Vector2 (Input.GetTouch (i).position.x, Screen.height - Input.GetTouch (i).position.y);
					Vector2 deltagrag = (controllerPositionNext - controllerPositionTemp);
					direction.x = deltagrag.x;
					direction.y = deltagrag.y;
					if (fixdrag)
						controllerPositionTemp = Vector2.Lerp (controllerPositionTemp, controllerPositionNext, 0.5f);
					buttonPosition = controllerPositionNext;
					IsPressed = true;
				}	
			}
		}
		return direction;
	}

	public bool IsPressed = false;
	private Vector2 buttonPosition, buttonPositionInitial;

	public void Draw ()
	{ 
		Update ();
		float size = AreaTouch.width / 2.0f;
		buttonPositionInitial = Vector2.Lerp (buttonPositionInitial, new Vector2 (AreaTouch.x + (AreaTouch.width / 2), AreaTouch.y + (AreaTouch.height / 2)), Time.deltaTime * 3);
		if (ImgBG) {
			GUI.DrawTexture (AreaTouch, ImgBG);
		}
		if (ImgButton) {
			if (IsPressed) {
				float sizeup = size * 1.5f;
				GUI.DrawTexture (new Rect (buttonPosition.x - (sizeup / 2.0f), buttonPosition.y - (sizeup / 2.0f), sizeup, sizeup), ImgButton);
			} else {
				GUI.DrawTexture (new Rect (buttonPosition.x - (size / 2.0f), buttonPosition.y - (size / 2.0f), size, size), ImgButton);
			}
		}
	}

}
