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
    private List<PhotoAlbumEntry> _currentAlbumEntries = new List<PhotoAlbumEntry>();
    private PhotoAlbumEntry _selectedAlbumEntry;
    private PhotosAPIService _photosAPIService;

    //Initialize PhotosAPI Service on start of play mode
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

    }

    void DeleteCurrentlySelectedAlbumEntry()
    {

    }

    void OutputAllCurrentAlbumEntries()
    {

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
    }

    void SelectAlbumEntry(PhotoAlbumEntry albumEntry)
    {
        _selectedAlbumEntry = albumEntry;
        Debug.Log($"Selected AlbumEntry: {albumEntry.title} (ID: {albumEntry.id})");

        // Display the image URL in the console
        Debug.Log($"Image URL: {albumEntry.url}");
    }
}
