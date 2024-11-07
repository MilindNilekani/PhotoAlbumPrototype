using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        newEntryToShow.Initialize(newPhoto.id, newPhoto.title, newPhoto.url, OnClickedAlbumEntry);
        _currentAlbumEntries.Add(newEntryToShow);
    }

    void DeleteCurrentlySelectedAlbumEntry()
    {
        if(_selectedAlbumEntry != null)
        {
            _currentAlbumEntries.Remove(_selectedAlbumEntry);
            GameObject.Destroy(_selectedAlbumEntry.gameObject);
            DeselectAlbumEntry();
        }
    }

    void OutputAllCurrentAlbumEntries()
    {
        for(int i=0;i<_currentAlbumEntries.Count;i++)
        {
            _currentAlbumEntries[i].LogAlbumData();
        }
    }

    void OnClickedAlbumEntry(PhotoAlbumEntry photoAlbumEntry)
    {
        DeselectAlbumEntry();
        SelectAlbumEntry(photoAlbumEntry);
    }

    void DeselectAlbumEntry()
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
