using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PhotoAlbumEntry : MonoBehaviour
{
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private Button _clickableArea;

    private int _id;
    private string _title;
    private string _url;
    private Texture2D _texture;
    public Texture2D Texture=> _texture;
    private Action<PhotoAlbumEntry> _onClick;

    public void Initialize(int id, string title, string url, Action<PhotoAlbumEntry> onClick)
    {
        _id = id;
        _title = title;
        _url = url;
        _onClick = onClick;
        _clickableArea.onClick.AddListener(OnClickEntry);
        StartCoroutine(LoadImage(url));
    }

    void OnClickEntry()
    {
        _onClick?.Invoke(this);
    }

    IEnumerator LoadImage(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                LoadImage(texture);
            }
            else
            {
                Debug.LogError("Failed to load image: " + request.error);
            }
        }
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
        Debug.Log($"AlbumEntry title: {_title} (ID: {_id}) Image URL: {_url}");
    }
}
