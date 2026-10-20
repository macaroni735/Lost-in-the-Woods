using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text statusText;
    public TMP_Text messageText;

    [Header("Menu")]
    public GameObject menuPrefab;      // assign your menu prefab here
    GameObject currentMenu;            // tracks the open menu

    public int pointsPerDelivery = 100;

    public bool HasPackage { get; private set; }
    int score;
    bool gameOver;

    void Awake()
    {
        Time.timeScale = 1f;

        // Only one GameManager allowed
        if (Instance != null && Instance != this)
        {
            // The new scene's copy has the correct UI references,
            // so hand them to the persistent manager before destroying this one.
            Instance.AdoptSceneReferences(this);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (Instance != this) return; // skip for the duplicate being destroyed
        Refresh();
        ShowMessage("Find the relic hidden in the forest.", 5f);
    }

    // Called by a duplicate GameManager when a scene loads
    void AdoptSceneReferences(GameManager newer)
    {
        scoreText = newer.scoreText;
        statusText = newer.statusText;
        messageText = newer.messageText;

        // Fresh scene = fresh run (same as the old behavior)
        score = 0;
        HasPackage = false;
        gameOver = false;
        currentMenu = null;

        Refresh();
        ShowMessage("Find the relic hidden in the forest.", 5f);
    }

    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (!gameOver && Input.GetKeyDown(KeyCode.Escape))
            ToggleMenu();
    }

    public void ToggleMenu()
    {
        if (menuPrefab == null) return;

        if (currentMenu == null)
        {
            currentMenu = Instantiate(menuPrefab);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Destroy(currentMenu);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void PackagePickedUp()
    {
        HasPackage = true;
        Refresh();
        ShowMessage("Relic collected! Get to the glowing circle.", 4f);
    }

    public void DeliveryCompleted()
    {
        HasPackage = false;
        score += pointsPerDelivery;
        Refresh();
        ShowMessage("Delivered! +" + pointsPerDelivery, 4f);
    }

    public void PlayerCaught()
    {
        gameOver = true;
        ShowMessage("You were caught! Press R to restart.", 0f);
        Time.timeScale = 0f;
    }

    void Refresh()
    {
        if (scoreText) scoreText.text = "Score: " + score;
        if (statusText) statusText.text = HasPackage ? "Carrying: Relic" : "No relic";
    }

    public void ShowMessage(string msg, float seconds)
    {
        if (!messageText) return;
        messageText.text = msg;
        CancelInvoke(nameof(ClearMessage));
        if (seconds > 0f) Invoke(nameof(ClearMessage), seconds);
    }

    void ClearMessage()
    {
        if (messageText) messageText.text = "";
    }
}