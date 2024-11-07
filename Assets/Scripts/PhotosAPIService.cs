using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;

public class PhotosAPIService
{
    private PhotoAlbumEntry _albumEntryPrefab;
    private List<PhotoAlbumEntry> _allAlbumEntries = new List<PhotoAlbumEntry>();
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
                List<PhotoInfo> photos = JsonConvert.DeserializeObject<List<PhotoInfo>>(jsonResponse);

                for (int i = 0; i < photos.Count; i++)
                {
                    CreateAlbumEntry(photos[i], i + 1);
                }
            }
            else
            {
                Debug.LogError("Failed to load data: " + request.error);
            }
        }
    }

    void CreateAlbumEntry(PhotoInfo photoInfo, int id)
    {
        PhotoAlbumEntry albumEntry = GameObject.Instantiate(_albumEntryPrefab, Vector3.zero, Quaternion.identity);
        albumEntry.gameObject.SetActive(false);
        albumEntry.Initialize(id, photoInfo.title, photoInfo.url);

        albumEntry.StartCoroutine(LoadImage(albumEntry, photoInfo.url));
        _allAlbumEntries.Add(albumEntry);

        Debug.Log($"Created AlbumEntry: ID = {id}, Title = {photoInfo.title}, URL = {photoInfo.url}");
    }

    IEnumerator LoadImage(PhotoAlbumEntry albumEntry, string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                albumEntry.LoadImage(texture);
            }
            else
            {
                Debug.LogError("Failed to load image: " + request.error);
            }
        }
    }

    public PhotoAlbumEntry FetchNextAlbumEntry()
    {
        var entry = _allAlbumEntries[_currentAlbumEntryIndexFetched];
        _currentAlbumEntryIndexFetched++;
        return entry;
    }
}
