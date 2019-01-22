using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Overworld;

public class OverworldInterfaceManager : MonoBehaviour {

    #region Data
    [SerializeField] GameManager _gameManager;
    [SerializeField] OverworldManager _overworldManager;

    public Button enterNodeButton;
    public RectTransform infoBox;
    public Text infoBoxInfoText;
    public Text infoBoxNodeTitle;
    public Button backToWorldButton; //reference to button to go back to world
    private bool textCondition; // used to set the text once
    [SerializeField]
    private Overworld.UserInterface.LoadingScreen _loadingScreen;
    public Overworld.UserInterface.LoadingScreen loadingScreen { get { return _loadingScreen; } }

    public List<Button> buttonList;
    #endregion
    public void InitializeOverworldInterface(GameManager gameManager, OverworldManager overworldManager) {
        _gameManager = gameManager;
        _overworldManager = overworldManager;

        infoBox.gameObject.SetActive(false);
        _loadingScreen = FindObjectOfType<Overworld.UserInterface.LoadingScreen>();
        if (_loadingScreen)
            _loadingScreen.gameObject.SetActive(false);

        textCondition = false;
        buttonList = new List<Button>();
        buttonList.Add(enterNodeButton);
        buttonList.Add(backToWorldButton);
        enterNodeButton.onClick.AddListener(() => EnterNodeButtonWasPressed());
        enterNodeButton.gameObject.SetActive(false);
        backToWorldButton.onClick.AddListener(() => BackToWorldButtonWasPressed());
        backToWorldButton.gameObject.SetActive(false);

        foreach (Button button in buttonList) {
            button.GetComponent<ButtonInterface>().InitializeButton(this);
        }
    }
    //TODO: Move information box User interface into seperate class
    /// <summary>
    /// Display information box with node information.
    /// If the resources are not discovered, we use "????" until they are discovered
    /// </summary>
    /// <param name="node">Node information</param>
    void DisplayHelpNodeInformation(Node node) {
        if (!textCondition) {
            infoBoxNodeTitle.text = node.nodeName;
            infoBoxInfoText.text = "";
            foreach (ResourceInfo resource in node.resourceList) {
                if (resource.discovered)
                    infoBoxInfoText.text += resource.nodeType.ToString() + "\n";
                else
                    infoBoxInfoText.text += "????" + "\n";
            }
            textCondition = true;
        }
        infoBox.gameObject.SetActive(true);
    }
    public void EnableLoadingScreen(bool condition) {
        _loadingScreen.gameObject.SetActive(condition);
    }
    void EnterNodeButtonWasPressed() {
        _overworldManager.EnterNode();
        ResetButton();
    }
    void BackToWorldButtonWasPressed() {
        RefreshHelpNodeInfoBox();
        DisplayEnterNodeButton(false);
        DisplayBackToWorldButton(false);
        _overworldManager.BackToWorld();
        ResetButton();
    }
    public void NodeWasSelected(Node node) {
        DisplayHelpNodeInformation(node);
        DisplayEnterNodeButton(true);
    }
    public void DisplayEnterNodeButton(bool condition) {
        DisplayBackToWorldButton(true);
        enterNodeButton.gameObject.SetActive(condition);
    }
    public void DisplayBackToWorldButton(bool condition) {
        backToWorldButton.gameObject.SetActive(condition);
    }
    void ResetButton() {
        foreach (Button button in buttonList) {
            button.GetComponent<ButtonInterface>().ResetButtonInterface();
        }
    }
    /// <summary>
    /// Reset the information box
    /// </summary>
    public void RefreshHelpNodeInfoBox() {
        if (infoBox.gameObject.activeSelf) {
            textCondition = false;
            infoBox.gameObject.SetActive(false);
        }
    }
}
