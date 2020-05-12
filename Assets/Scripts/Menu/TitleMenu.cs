using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleMenu : MonoBehaviour
{
    [Header("Main Menu Panels")]
    public GameObject mainMenuObject;
    public GameObject playMenuObject;
    public GameObject settingsMenuObject;
    public GameObject newWorldMenuObject;
    public GameObject loadingTextObject;

    [Header("Savegames")]
    public GameObject saveGameContentPanel;
    public GameObject saveGameButtonPrefab;
    public GameObject playSaveGameButton;
    string selectedWorld;

    [Header("Input Textbox")]
    public TextMeshProUGUI worldName;

    [Header("Settings Menu UI Elements")]
    public Slider viewDistSlider;
    public TextMeshProUGUI viewDistText;
    public Slider mouseSlider;
    public TextMeshProUGUI mouseTxtSlider;
    public Toggle threadingToggle;
    public Toggle chunkAnimToggle;

    Settings settings;



    private void Awake()
    {
        if (!File.Exists(Application.dataPath + "/settings.cfg"))
        {
            Debug.Log("No settings file found, reating new one.");
            settings = new Settings();
            string jsonExport = JsonUtility.ToJson(settings);
            File.WriteAllText(Application.dataPath + "/settings.cfg", jsonExport);
        }
        else
        {
            Debug.Log("Settings file found, loading settings.");
            string jsonImport = File.ReadAllText(Application.dataPath + "/settings.cfg");
            settings = JsonUtility.FromJson<Settings>(jsonImport);
        }
    }

    public void StartGame()
    {
        mainMenuObject.SetActive(false);
        playMenuObject.SetActive(true);
    }

    public void EnterSettings()
    {
        viewDistSlider.value = settings.viewDistance;
        UpdateViewDistanceSlider();
        mouseSlider.value = settings.mouseSensitivity;
        UpdateMouseSlider();
        threadingToggle.isOn = settings.enableThreading;
        chunkAnimToggle.isOn = settings.enableAnimatedChunks;

        mainMenuObject.SetActive(false);
        settingsMenuObject.SetActive(true);
    }

    public void NewWorldMenu()
    {
        playMenuObject.SetActive(false);
        newWorldMenuObject.SetActive(true);
    }

    public void StartNewWorld()
    {
        newWorldMenuObject.SetActive(false);
        loadingTextObject.SetActive(true);
        SceneManager.LoadScene("VoidWorld");

        settings.worldName = worldName.text;
        SaveSystem.tempWorldName = worldName.text;

        string jsonExport = JsonUtility.ToJson(settings);
        File.WriteAllText(Application.dataPath + "/settings.cfg", jsonExport);

    }

    public void LoadSavedWorld()
    {
        if (SaveSystem.tempWorldName != null)
        {
            playMenuObject.SetActive(false);
            loadingTextObject.SetActive(true);
            settings.worldName = worldName.text;
            SceneManager.LoadScene("VoidWorld");
        }
    }

    public void LeaveSettings()
    {
        settings.viewDistance = (int)viewDistSlider.value;
        settings.mouseSensitivity = mouseSlider.value;
        settings.enableThreading = threadingToggle.isOn;
        settings.enableAnimatedChunks = chunkAnimToggle.isOn;

        string jsonExport = JsonUtility.ToJson(settings);
        File.WriteAllText(Application.dataPath + "/settings.cfg", jsonExport);

        mainMenuObject.SetActive(true);
        settingsMenuObject.SetActive(false);
    }

    public void LookForSaveGames()
    {
        try
        {
            DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/saves/");

            // Get a reference to each directory in that directory.
            DirectoryInfo[] diArr = di.GetDirectories();

            // Display the names of the directories.
            foreach (DirectoryInfo dri in diArr)
            {
                Debug.Log("SaveGame found: " + dri.Name);
                GameObject menuButton = Instantiate(saveGameButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity);

                menuButton.transform.parent = saveGameContentPanel.transform;
                menuButton.GetComponentInChildren<TextMeshProUGUI>().text = dri.Name;

                Button btn = menuButton.GetComponent<Button>();
                btn.onClick.AddListener(EnablePlaySaveGameButton);
            }
        }
        catch
        {
            Debug.Log("No savegames found.");
        }
            
    }

    public void EnablePlaySaveGameButton()
    {
        playSaveGameButton.SetActive(true);
        
        GameObject go = EventSystem.current.currentSelectedGameObject;
        if (go != null)
        {
            SaveSystem.tempWorldName = go.GetComponentInChildren<TextMeshProUGUI>().text;
            Debug.Log("savesystem tempworld name assigned to: " + SaveSystem.tempWorldName);
        }

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateViewDistanceSlider()
    {
        viewDistText.text = "View Distance: " + viewDistSlider.value;
    }
    public void UpdateMouseSlider()
    {
        mouseTxtSlider.text = "Mouse Sensitivity: " + mouseSlider.value.ToString("F1");
    }

}
