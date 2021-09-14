using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private Button playBtn;
    [SerializeField] private Button backBtn;

    [Header("Events")]
    [SerializeField] private GameEvent onPlayEvent;
    [SerializeField] private GameEvent onBackEvent;

    public static string DisplayName { get; private set; }

    private const string PlayerPrefsNameKey = "PlayerName";

    private void Start()
    {
        playBtn.onClick.AddListener(OnPlayPressed);
        backBtn.onClick.AddListener(OnBackPressed);

        SetupInputField();
        SetPlayerName(usernameField.text);
    }

    private void SetupInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey))
        {
            return;
        }

        usernameField.text = PlayerPrefs.GetString(PlayerPrefsNameKey);
    }

    public void SetPlayerName(string name)
    {
        playBtn.interactable = (!string.IsNullOrEmpty(name));
    }

    private void SavePlayerName()
    {
        DisplayName = usernameField.text;

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
    }

    private void OnPlayPressed()
    {
        SavePlayerName();

        onPlayEvent.Raise();
    }
    private void OnBackPressed()
    {
        onBackEvent.Raise();
    }
}
