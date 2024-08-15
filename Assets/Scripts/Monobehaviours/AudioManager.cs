using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    private bool _isInMainMenu = true;
    private int _currentSongIndex = -1;
    private static AudioManager S_instance;
    [SerializeField] private SongList songList;
    AudioSource _audioSource;

    void Start()
    {
        
        _audioSource = GetComponent<AudioSource>();
        if (!S_instance)
        {
            DontDestroyOnLoad(gameObject);
            S_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode lsm)
    {
        _isInMainMenu = SceneManager.GetActiveScene().name == "Main Menu";
    }

    void Update()
    {
        /*
        if (_isInMainMenu)
        {
            _audioSource.clip = null;
            _audioSource.Stop();
            return;
        }
        */
        if (!_audioSource.isPlaying)
        {
            int newSongIndex = Random.Range(0, songList.Songs.Length - 1);
            bool isRepeated = newSongIndex == _currentSongIndex;
            if (isRepeated)
            {
                bool isFirst = _currentSongIndex == 0;
                if (isFirst) { _currentSongIndex = 1; }
                else { _currentSongIndex = newSongIndex - 1; }
            }

            else
            {
                _currentSongIndex = newSongIndex;
            }

            _audioSource.clip = songList.Songs[_currentSongIndex];
            _audioSource.Play();
        }
    }
}
