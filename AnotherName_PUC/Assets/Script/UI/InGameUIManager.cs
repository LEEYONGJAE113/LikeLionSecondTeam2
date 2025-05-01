using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance { get; private set; }

    [Header("UI ���")]
    public Image portraitImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public Image passiveSkillIconImage;

    [Header("��ų ������")]
    public Image skillRIconImage;
    public Image skillEIconImage;
    public Image skillQIconImage;

    [Header("�޴� �˾� UI")]
    public GameObject menuUI;
    public GameObject optionUI;

    [Header("ĳ���� ���� �˾�")]
    public GameObject characterInfoUI;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindUIReferences();  // null�� ���� �翬��

        if (SelectedCharacterData.Instance != null)
        {
            var info = SelectedCharacterData.Instance.selectedCharacter;
            ApplyCharacterInfo(info);
        }
    }

    public void RebindUIReferences()
    {
        if (menuUI == null)
            menuUI = GameObject.Find("MenuUI");

        if (optionUI == null)
            optionUI = GameObject.Find("OptionUI");

        if (characterInfoUI == null)
            characterInfoUI = GameObject.Find("CharacterInfoUI");

        if (portraitImage == null)
            portraitImage = GameObject.Find("Portrait")?.GetComponent<Image>();

        if (nameText == null)
            nameText = GameObject.Find("NameText")?.GetComponent<TextMeshProUGUI>();

        if (levelText == null)
            levelText = GameObject.Find("LevelText")?.GetComponent<TextMeshProUGUI>();

        if (passiveSkillIconImage == null)
            passiveSkillIconImage = GameObject.Find("PassiveSkillIcon")?.GetComponent<Image>();

        if (skillRIconImage == null)
            skillRIconImage = GameObject.Find("SkillR")?.GetComponent<Image>();

        if (skillEIconImage == null)
            skillEIconImage = GameObject.Find("SkillE")?.GetComponent<Image>();

        if (skillQIconImage == null)
            skillQIconImage = GameObject.Find("SkillQ")?.GetComponent<Image>();
    }

    public void ApplyCharacterInfo(CharacterInfo info)
    {
        if (portraitImage != null)
            portraitImage.sprite = info.portraitSprite;

        if (levelText != null)
            levelText.text = "Lv.1";

        if (nameText != null)
            nameText.text = info.characterName;

        if (passiveSkillIconImage != null)
            passiveSkillIconImage.sprite = info.passiveSkillIcon;

        if (skillRIconImage != null)
            skillRIconImage.sprite = info.skillRIcon;

        if (skillEIconImage != null)
            skillEIconImage.sprite = info.skillEIcon;

        if (skillQIconImage != null)
            skillQIconImage.sprite = info.skillQIcon;
    }

    public void ShowMenuPopup()
    {
        if (menuUI != null)
            menuUI.SetActive(true);
        else
            Debug.LogWarning("[InGameUIManager] menuUI�� ������� �ʾҽ��ϴ�.");
    }

    public void ResumeGame()
    {
        if (menuUI != null)
            menuUI.SetActive(false);
    }

    public void OpenOptionPopup()
    {
        if (optionUI != null)
        {
            optionUI.SetActive(true);
            menuUI.SetActive(false);
        }
    }

    public void ShowCharacterInfoPopup()
    {
        if (characterInfoUI != null)
            characterInfoUI.SetActive(true);
        else
            Debug.LogWarning("[InGameUIManager] characterInfoUI�� ������� �ʾҽ��ϴ�.");
    }

    public void CloseCharacterInfoPopup()
    {
        if (characterInfoUI != null)
            characterInfoUI.SetActive(false);
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}