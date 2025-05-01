using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Ÿ��Ʋ �̵���

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
    public GameObject menuUI;          // ���� �߰�: Menu UI ����
    public GameObject optionUI;        // ���� Option UI ����

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

    // �޴� ��ư Ŭ�� ��
    public void ShowMenuPopup()
    {
        if (menuUI != null)
            menuUI.SetActive(true);
        else
            Debug.LogWarning("[InGameUIManager] menuUI�� ������� �ʾҽ��ϴ�.");
    }

    // �޴� �˾� ���� ��ɵ�

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
    // �ʻ�ȭ Ŭ�� ��
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
        SceneManager.LoadScene("TitleScene"); // ���ư��� Ŭ�� �� Ÿ��Ʋ ������ �̵�
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}