using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

//This acts as the UI + interactive layer for the Photo album. Communicates with the service
public class PhotoAlbumCanvas : MonoBehaviour
{
    [SerializeField] private PhotoAlbumEntry _albumEntryPrefab;
    [SerializeField] private Button _createAlbumEntryBtn;
    [SerializeField] private Button _deleteCurrentAlbumEntryBtn;
    [SerializeField] private Button _outputAlbumEntriesBtn;
    [SerializeField] private RectTransform _contentPanel;
    [SerializeField] private RawImage _preview;


    private List<PhotoAlbumEntry> _currentAlbumEntries = new List<PhotoAlbumEntry>();
    private PhotoAlbumEntry _selectedAlbumEntry;
    private PhotosAPIService _photosAPIService;

    //Initialize PhotosAPI Service and buttons on start of play mode
    IEnumerator Start()
    {
        _photosAPIService = new PhotosAPIService(_albumEntryPrefab);
        yield return _photosAPIService.FetchAlbumEntries();
        _createAlbumEntryBtn.onClick.AddListener(CreateNewAlbumEntry);
        _deleteCurrentAlbumEntryBtn.onClick.AddListener(DeleteCurrentlySelectedAlbumEntry);
        _outputAlbumEntriesBtn.onClick.AddListener(OutputAllCurrentAlbumEntries);
    }

    void CreateNewAlbumEntry()
    {
        var newPhoto = _photosAPIService.FetchNextPhoto();
        var newEntryToShow = Instantiate(_albumEntryPrefab, _contentPanel);
        newEntryToShow.Initialize(newPhoto.id, newPhoto.title, newPhoto.url);
        StartCoroutine(LoadImage(newEntryToShow, newPhoto.url));
        _currentAlbumEntries.Add(newEntryToShow);
    }

    void DeleteCurrentlySelectedAlbumEntry()
    {
        _currentAlbumEntries.Remove(_selectedAlbumEntry);
        GameObject.Destroy(_selectedAlbumEntry);
        _selectedAlbumEntry = null;
    }

    void OutputAllCurrentAlbumEntries()
    {

    }

    void CreateAlbumEntry(PhotoInfo photoInfo, int id)
    {
        PhotoAlbumEntry albumEntry = GameObject.Instantiate(_albumEntryPrefab, Vector3.zero, Quaternion.identity);
        albumEntry.gameObject.SetActive(false);
        albumEntry.Initialize(id, photoInfo.title, photoInfo.url);

        albumEntry.StartCoroutine(LoadImage(albumEntry, photoInfo.url));
        _currentAlbumEntries.Add(albumEntry);

        Debug.Log($"Created AlbumEntry: ID = {id}, Title = {photoInfo.title}, URL = {photoInfo.url}");
    }

    IEnumerator LoadImage(PhotoAlbumEntry albumEntry, string url)
    {
        Debug.Log("Loading image: " + url);
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

    //Deals with clicking and unclicking album entries
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                PhotoAlbumEntry hitEntry = hit.collider.GetComponent<PhotoAlbumEntry>();
                if (hitEntry != null)
                {
                    if (_selectedAlbumEntry != null)
                    {
                        DeselectAlbumEntry(_selectedAlbumEntry);
                    }

                    SelectAlbumEntry(hitEntry);
                }
            }
        }
    }

    void DeselectAlbumEntry(PhotoAlbumEntry albumEntry)
    {
        _selectedAlbumEntry = null;
        _preview.gameObject.SetActive(false);
    }

    void SelectAlbumEntry(PhotoAlbumEntry albumEntry)
    {
        _selectedAlbumEntry = albumEntry;
        _preview.texture = albumEntry.Texture;
        _preview.gameObject.SetActive(true);
    }
}
