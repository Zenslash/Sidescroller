using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private SNetworkManager networkManager;

    [Space(10)]
    [Header("UI")]
    [SerializeField] private CanvasGroup NicknameGroup;
    [SerializeField] private CanvasGroup MainMenuGroup;
    [SerializeField] private CanvasGroup PlayGroup;
    [SerializeField] private CanvasGroup JoinGameGroup;
    [SerializeField] private CanvasGroup HostGameGroup;

    [Header("UI")]
    [SerializeField] private TMP_InputField ipAddressField;
    [SerializeField] private Button joinButton;


    public void OnMainMenuPlayPressed()
    {
        ToggleMainMenu();
        ToggleNickname();
    }
    public void OnNicknamePlayPressed()
    {
        ToggleNickname();
        TogglePlay();
    }
    public void OnNicknameBackPressed()
    {
        ToggleNickname();
        ToggleMainMenu();
    }
    public void OnPlayBackPressed()
    {
        TogglePlay();
        ToggleNickname();
    }
    public void OnPlayHostPressed()
    {
        networkManager.StartHost();

        TogglePlay();
    }
    public void OnPlayJoinPressed()
    {
        TogglePlay();
        ToggleJoinGame();
    }

    private void ToggleNickname()
    {
        NicknameGroup.alpha = (NicknameGroup.alpha == 1f) ? 0f : 1f;
        NicknameGroup.blocksRaycasts = !NicknameGroup.blocksRaycasts;
    }
    private void TogglePlay()
    {
        PlayGroup.alpha = (PlayGroup.alpha == 1f) ? 0f : 1f;
        PlayGroup.blocksRaycasts = !PlayGroup.blocksRaycasts;
    }
    private void ToggleMainMenu()
    {
        MainMenuGroup.alpha = (MainMenuGroup.alpha == 1f) ? 0f : 1f;
        MainMenuGroup.blocksRaycasts = !MainMenuGroup.blocksRaycasts;
    }
    private void ToggleJoinGame()
    {
        JoinGameGroup.alpha = (JoinGameGroup.alpha == 1f) ? 0f : 1f;
        JoinGameGroup.blocksRaycasts = !JoinGameGroup.blocksRaycasts;
    }


    public void OnJoinJoinPressed()
    {
        string ipAddress = ipAddressField.text;

        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
    }
    public void OnJoinBackPressed()
    {
        ToggleJoinGame();
        TogglePlay();
    }

    public void HandleClientConnect()
    {
        joinButton.interactable = true;

        JoinGameGroup.alpha = 0f;
        JoinGameGroup.blocksRaycasts = false;
    }
    public void HandleClientDisconnect()
    {
        joinButton.interactable = true;
    }

}
