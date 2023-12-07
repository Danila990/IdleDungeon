using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizeSprite : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private Image image;

	public void ChangeSprite(Sprite sprite)
	{
		if (spriteRenderer)
			spriteRenderer.sprite = sprite;
		if (image)
			image.sprite = sprite;
	}
}
