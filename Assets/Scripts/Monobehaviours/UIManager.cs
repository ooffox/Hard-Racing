using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    private LevelData _currentSelectedLevel;
    [SerializeField] private TMP_Dropdown carViewDropdown;
    [SerializeField] private GameObject raceOverlay;
    [SerializeField] private GameObject pauseOverlay;
    [SerializeField] private GameObject postraceOverlay;
    [SerializeField] private bool isInMainMenu;
    private TMP_Text _timer;
    private Image _throttleFill;
    private Image _brakeFill;
    private PlayerController _playerController;
    public bool _isInPause = false;
    private bool _isCountdownOver = false;
    private float _vertical;
    private float _lastFrameTime;
    GameManager _gameManager;
    void Start()
    {
        if (isInMainMenu)
        {
            return;
        }

        _lastFrameTime = Time.time;
        _timer = GameObject.FindWithTag("Time Text").GetComponent<TMP_Text>();
        _throttleFill = GameObject.FindWithTag("Throttle").GetComponent<Image>();
        _brakeFill = GameObject.FindWithTag("Brake").GetComponent<Image>();
        _playerController = GameObject.FindObjectOfType<PlayerController>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        StartCoroutine(DoCountdown());

        
    }

    void Update()
    {
        if (isInMainMenu || _gameManager.HasFinishedRace)
        {
            return;
        }

        if (Input.GetButtonDown("Pause") && _isCountdownOver && !isInMainMenu)
        {
            Pause(!_isInPause); // if its in pause then unpause and viceversa
            return;
        }


        _brakeFill.enabled = _vertical < 0.0f;
        _throttleFill.enabled = _vertical > 0.0f;

        if (!_isCountdownOver) { return; }

        _vertical = Input.GetAxisRaw("Vertical");

        UpdateTimer();
    }

    private IEnumerator DoCountdown()
    {
        _playerController.CanMove = false;
        TMP_Text countdownText = GameObject.FindWithTag("Countdown Text").GetComponent<TMP_Text>();
        for (int i = 3; i >= 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(0.5f);
        }
        countdownText.text = "";
        _playerController.CanMove = true;
        _isCountdownOver = true;
    }

    void UpdateTimer()
    {
        string content = _timer.text;
        int colonIndex = content.IndexOf(":");
        float currentTime = Convert.ToSingle(content.Substring(colonIndex + 2));
        currentTime += Time.deltaTime;
        _timer.text = String.Format("Time: {0}", Math.Round(currentTime, 3));
    }


    public void Pause(bool shouldPause)
    {
        _isInPause = shouldPause;
        raceOverlay.SetActive(!shouldPause);
        pauseOverlay.SetActive(shouldPause);
        _playerController.IsInPause = shouldPause;
        Time.timeScale = shouldPause ? 0.0f : 1.0f;
    }

    // The Hide and Show functions are 2 separate functions because, on the Unity OnClick() event, if
    // the function has >=2 parameters, it won't show up.
    public void Hide(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void Show(GameObject obj)
    {
        obj.SetActive(true);
    }


    public void SetCurrentChosenLevel(LevelUIBehaviour level)
    {
        _currentSelectedLevel = level.Data;
    }

    public void UpdateMapDisplay(GameObject mapDisplay)
    {
        Image thumbnail = mapDisplay.transform.GetChild(1).GetComponent<Image>();
        TMP_Text title = mapDisplay.transform.GetChild(2).GetComponent<TMP_Text>();
        TMP_Text description = mapDisplay.transform.GetChild(3).GetComponent<TMP_Text>();
        TMP_Text pbText = mapDisplay.transform.GetChild(4).GetComponent<TMP_Text>();
        thumbnail.sprite = UIManager.TextureToSprite(_currentSelectedLevel.ThumbnailTexture);
        title.text = "Level " + _currentSelectedLevel.Number.ToString();
        description.text = "Description: " + _currentSelectedLevel.Description;

        Timer bestTime = new Timer(0, 0, PlayerPrefs.GetInt(_currentSelectedLevel.Number.ToString()));
        if (bestTime.Miliseconds == 0)
        {
            pbText.text = "Personal Best: None";
        }
        else
        {
            pbText.text = "Personal Best: " + bestTime.ToDisplay();
        }
    }

    public static Sprite TextureToSprite(Texture2D tex)
    {
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    public void LoadSelectedLevel()
    {
        SceneManager.LoadScene(_currentSelectedLevel.SceneName);
    }

    public void ChangeCarView(TMP_Dropdown dropdown)
    {
        if (dropdown.value == 0)
        {
            PlayerPrefs.SetInt("Car View", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Car View", 3);
        }
        PlayerPrefs.Save();
    }

    public void WritePostraceTime(Timer timer)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string currentLevelNumber = sceneName[sceneName.Length - 1].ToString();
        int pbTimeMiliseconds = PlayerPrefs.GetInt(currentLevelNumber, 1000 * 60 * 60);
        Timer pbTimer = new Timer(0, 0, pbTimeMiliseconds);

        TMP_Text titleText = postraceOverlay.transform.GetChild(1).GetComponent<TMP_Text>();
        TMP_Text timeText = postraceOverlay.transform.GetChild(2).GetComponent<TMP_Text>();
        TMP_Text pbText = postraceOverlay.transform.GetChild(3).GetComponent<TMP_Text>();

        titleText.text = "Level " + currentLevelNumber;
        int timeColon = timeText.text.IndexOf(":");
        int pbColon = pbText.text.IndexOf(":");

        timeText.text = timeText.text.Substring(0, timeColon + 2) + timer.ToDisplay();
        if (Timer.IsSmaller(timer, pbTimer))
        {
            pbText.text = pbText.text.Substring(0, pbColon + 2) + timer.ToDisplay() + " (NEW)";
            PlayerPrefs.SetInt(currentLevelNumber, timer.ToMiliseconds());
        }
        else
        {
            pbText.text = pbText.text.Substring(0, pbColon + 2) + pbTimer.ToDisplay();
        }
        PlayerPrefs.Save();
    }

    public void EnablePostraceOverlay()
    {
        Show(postraceOverlay);
    }
}
