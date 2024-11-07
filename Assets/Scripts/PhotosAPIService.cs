using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;

public class PhotosAPIService
{
    private PhotoAlbumEntry _albumEntryPrefab;
    private List<PhotoInfo> _photos = new List<PhotoInfo>();
    private int _currentAlbumEntryIndexFetched = 0;
    private const string _apiUrl = "https://jsonplaceholder.typicode.com/photos";

    public PhotosAPIService(PhotoAlbumEntry prefab)
    {
        _albumEntryPrefab = prefab;
    }

    public IEnumerator FetchAlbumEntries()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(_apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = request.downloadHandler.text;
                _photos = JsonConvert.DeserializeObject<List<PhotoInfo>>(jsonResponse);
            }
            else
            {
                Debug.LogError("Failed to load data: " + request.error);
                _photos = null;
            }
        }
    }

    public PhotoInfo FetchNextPhoto()
    {
        var entry = _photos[_currentAlbumEntryIndexFetched];
        _currentAlbumEntryIndexFetched++;
        Debug.Log($"Fetching PhotoData title: {entry.title} (ID: {entry.id}) Image URL: {entry.url}");
        return entry;
    }
}
