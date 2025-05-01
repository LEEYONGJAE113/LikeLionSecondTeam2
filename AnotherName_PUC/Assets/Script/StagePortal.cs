using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StagePortal : MonoBehaviour
{
    [Header("�� ��Ż�� ���� �̸� (PlayerSpawnManager�� �����)")]
    public string portalName = "ToStageA";

    [Header("�̵��� �� �̸� (Build Settings�� ��ϵ� �̸�)")]
    public string destinationSceneName = "StageScene";

    [Header("���� ���� �÷��̾ ��ȯ�� ��ġ")]
    public Transform spawnPoint;

    private bool hasTriggered = false;
    private Collider2D portalCollider;

    private float spawnIgnoreTime = 3; // �� ��ȯ ���� ������ �ð�
    private float spawnTime;

    private void OnEnable()
    {
        spawnTime = Time.time;
    }

    private void Awake()
    {
        portalCollider = GetComponent<Collider2D>();
        if (portalCollider != null)
            portalCollider.enabled = false; // �ʱ� ��Ȱ��ȭ
    }

    public void EnablePortalAfterSpawn()
    {
        StartCoroutine(EnableWithDelay());
    }

    private IEnumerator EnableWithDelay()
    {
        yield return new WaitForSeconds(0.1f); // �÷��̾� ���� �� ��� ���
        if (portalCollider != null)
        {
            portalCollider.enabled = true;
            Debug.Log($"[��Ż] {portalName} �ݶ��̴� Ȱ��ȭ��");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[��Ż �����] �浹 �߻� �� other.name: {other.name}, �±�: {other.tag}");

        if (Time.time - spawnTime < spawnIgnoreTime) return;
        if (hasTriggered || !other.CompareTag("Player")) return;

        hasTriggered = true;

        if (SceneLoadData.Instance != null)
        {
            SceneLoadData.Instance.LastPortalName = portalName;
            Debug.Log($"[��Ż] LastPortalName ���� �Ϸ�: {portalName}");
        }

        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return null;
        SceneManager.LoadScene(destinationSceneName);
    }
}