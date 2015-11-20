using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour
{
	public Texture2D[] availableTextures;
	
	void Awake ()
	{
		SpriteRenderer renderer = GetComponent<SpriteRenderer> ();
		renderer.sprite = RandomBlockSprite ();
	}

	private Sprite RandomBlockSprite ()
	{
		Sprite sprite = null;
		if (availableTextures.Length > 0) {
			int index = Random.Range (0, availableTextures.Length);
			Texture2D texture = availableTextures [index];

			int width = texture.width;
			int height = texture.height;

			sprite = Sprite.Create (texture, new Rect (0, 0, width, height), new Vector2 (0.5f, 0.5f), width);
		}
		return sprite;
	}

	void Update()
	{
	}
}
