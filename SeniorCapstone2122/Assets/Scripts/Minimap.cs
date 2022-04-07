using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
	public Camera mapCamera;
	public int width = 300;
	public int height = 300;
	public int depth = 24;
	public RenderTexture renderTexture;
	public Texture2D texture;
	public Rect rect;
	public Image minimapImage;

    // Start is called before the first frame update
    void Start()
    {
		mapCamera.targetTexture = renderTexture;
		//renderTexture = new RenderTexture(width, height, depth);
		texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
		rect = new Rect(0, 0, width, height);
		minimapImage = GameObject.Find("Minimap").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void LateUpdate()
	{
		mapCamera.Render();
		RenderTexture currentRenderTexture = RenderTexture.active;
		RenderTexture.active = renderTexture;
		texture.ReadPixels(rect, 0, 0);
		texture.Apply();
		Sprite sprite = Sprite.Create( texture, rect, Vector2.zero );
		minimapImage.sprite = sprite;
	}
}
