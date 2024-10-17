using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class TilemapToSprite : MonoBehaviour
{
    public Tilemap tilemap; // Referenz zur Tilemap
    public Camera tilemapCamera; // Kamera, die die Tilemap rendert
    public string filePath = "Assets/TilemapSprite.png"; // Speicherpfad für das PNG

    public void Start()
    {
        ConvertTilemapToSprite();
    }


    public void ConvertTilemapToSprite()
    {
        BoundsInt bounds = tilemap.cellBounds;
        int width = bounds.size.x;
        int height = bounds.size.y;

        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        tilemapCamera.targetTexture = renderTexture;
        tilemapCamera.Render();
        tilemapCamera.targetTexture = null;

        // Schritt 2: Erstelle eine Textur aus dem RenderTexture
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        // Schritt 3: Konvertiere die Textur in ein Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Optional: Weise das Sprite einem SpriteRenderer zu
        GameObject spriteObject = new GameObject("TilemapSprite");
        SpriteRenderer renderer = spriteObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        Debug.Log("Tilemap erfolgreich in Sprite umgewandelt!");
    }
    }
