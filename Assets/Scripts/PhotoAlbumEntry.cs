using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoAlbumEntry : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;
    private int _id;
    private string _title;
    private string _url;
    private Texture2D _texture;

    public void Initialize(int id, string title, string url)
    {
        _id = id;
        _title = title;
        _url = url;
    }

    public void LoadImage(Texture2D texture)
    {
        _texture = texture;
        if (_rawImage != null && texture != null)
        {
            _rawImage.texture = texture;
        }
    }

    public void LogAlbumData()
    {
        Debug.Log($"Selected AlbumEntry title: {_title} (ID: {_id}) Image URL: {_url}");
    }
}
