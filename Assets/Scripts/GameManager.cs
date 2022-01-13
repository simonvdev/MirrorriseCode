using Character;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private static bool IsPaused {get; set; }
    private static bool _isInMainMenu = false;

    private static Transform _playerInstance = null;

    private CameraLook _cameraLook = null;
    private LaserWeapon _laserWeapon = null;

    [SerializeField] private GameObject pauseMenu = null;

    [SerializeField] private GameObject deathScreen = null;
    private Animator _anim;
    
    private static readonly int FadeOut = Animator.StringToHash("FadeOut");
    private static readonly int FadeIn = Animator.StringToHash("FadeIn");


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            IsPaused = false;
            _anim = GetComponent<Animator>();
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnLevelChange;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnLevelChange;
    }

    private void OnLevelChange(Scene current, Scene next)
    {
        _isInMainMenu = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (next.buildIndex == 0)
        {
            _isInMainMenu = true;
            Cursor.lockState = CursorLockMode.None;
        }

        PlayerMovement playerObject = FindObjectOfType<PlayerMovement>();
        if (playerObject != null)
        {
            _playerInstance = playerObject.transform;
            _cameraLook = _playerInstance.GetComponentInChildren<CameraLook>();
            _laserWeapon = _playerInstance.GetComponentInChildren<LaserWeapon>();
        }

       
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && _isInMainMenu == false)
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void LevelExit()
    {
        _anim.SetTrigger(FadeOut);
    }

    public void LoadNextLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (sceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            sceneIndex = 0;
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            SceneManager.LoadScene(sceneIndex);
        }
        
        _anim.SetTrigger(FadeIn);      
    }

    public void OnDeath()
    {    
        Pause();
        deathScreen.SetActive(true);
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        deathScreen.SetActive(false);
        Time.timeScale = 0f;
        IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        _cameraLook.enabled = false;
        _laserWeapon.enabled = false;
    }

    public void Resume()
    {
        deathScreen.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;

        _cameraLook.enabled = true;
        _laserWeapon.enabled = true;
    }

    public void LoadMenu()
    {
        Resume();
        SceneManager.LoadScene(0);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);      
    }
}
