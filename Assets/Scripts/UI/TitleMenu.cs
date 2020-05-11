using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public GameObject mainMenuObject;
    public GameObject settingsMenuObject;

    [Header("Main Menu UI Elements")]
    public TextMeshProUGUI seedField;

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
        if (seedField.text == "New World")
        {
            int rnd = Random.Range(1, 99999);
            VoxelData.seed = rnd.GetHashCode() / VoxelData.WorldSizeInChunks;
        }
        else
        {
            VoxelData.seed = Mathf.Abs(seedField.text.GetHashCode()) / VoxelData.WorldSizeInChunks;
        }
        SceneManager.LoadScene("VoidWorld", LoadSceneMode.Single);
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
