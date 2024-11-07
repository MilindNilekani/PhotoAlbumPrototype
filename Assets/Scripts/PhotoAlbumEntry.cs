using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoAlbumEntry : MonoBehaviour
{
    public int id;
    public string title;
    public string url;
    public Texture2D texture;

    public void Initialize(int id, string title, string url)
    {
        this.id = id;
        this.title = title;
        this.url = url;
    }

    public void LoadImage(Texture2D texture)
    {
        this.texture = texture;
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && texture != null)
        {
            renderer.material.mainTexture = texture;
        }
    }
}
