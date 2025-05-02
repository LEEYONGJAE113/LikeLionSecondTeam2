using UnityEngine;
using TMPro;

public class NPCController : MonoBehaviour
{
    [Header("�̵� �ӵ�")]
    public float moveSpeed = 2f;

    [Header("�÷��̾�� ���ߴ� �Ÿ�")]
    [SerializeField] private float stopDistance = 1.5f;

    [Header("����")]
    public AudioClip signSound;       // ���� ǥ�� ȿ����
    public AudioClip dialogueSound;   // ��� �ѱ� �� ȿ����
    public AudioClip walkSound;       // �ȱ� ȿ���� (����)

    private AudioSource audioSource;  // ����� �����

    private GameObject signObject;
    private Transform player;
    private Animator animator;

    private bool hasMovedToStartPosition = false;
    private bool signVisible = true;
    private bool dialogueEnded = false;

    public GameObject talkPanel;
    public TextMeshProUGUI text;

    private int clickCount = 0;

    private float walkSoundTimer = 0f; // �ȱ� �Ҹ� ���� Ÿ�̸�

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsWalk", true);
        }

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Sign"))
            {
                signObject = child.gameObject;
                signObject.SetActive(true);
                break;
            }
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // �ʱ� ���¿��� ��ȭâ ��Ȱ��ȭ
        talkPanel.SetActive(false);
    }

    void Update()
    {
        if (!hasMovedToStartPosition)
        {
            float randX = Random.Range(0f, 8.4f);
            float randY = Random.Range(-4.3f, 4.3f);
            transform.position = new Vector3(randX, randY, 0f);
            hasMovedToStartPosition = true;
        }

        if (player != null && !dialogueEnded)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer > stopDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                if (animator != null) animator.SetBool("IsWalk", true);

                // �ȱ� ���� ��� (�ӵ� ����)
                if (walkSound != null && audioSource != null && !audioSource.isPlaying)
                {
                    audioSource.clip = walkSound;
                    audioSource.loop = false; // ���� ��� ��Ȱ��ȭ
                    audioSource.pitch = 1f; // ���� �����̴� �״��
                }

                // 1�ʸ��� �ȱ� �Ҹ� ���
                walkSoundTimer += Time.deltaTime;
                if (walkSoundTimer >= 0.5f) // 1�ʰ� �����ٸ�
                {
                    walkSoundTimer = 0f; // Ÿ�̸� �ʱ�ȭ
                    audioSource.PlayOneShot(walkSound);  // �Ҹ� ���
                }

                if (signObject != null && signVisible && signObject.activeSelf)
                {
                    signObject.SetActive(false);
                }
            }
            else
            {
                if (animator != null) animator.SetBool("IsWalk", false);

                // �ȱ� ���� ����
                if (audioSource != null && audioSource.clip == walkSound && audioSource.isPlaying)
                {
                    audioSource.Stop();
                    audioSource.loop = false;
                    audioSource.clip = null;
                }

                if (signObject != null && signVisible && !signObject.activeSelf)
                {
                    signObject.SetActive(true);

                    if (signSound != null && audioSource != null)
                    {
                        audioSource.PlayOneShot(signSound);
                    }
                }
            }

            if (transform.position.x < player.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }

        // �����̽��ٸ� ������ ��ȭ ����
        if (Input.GetKeyDown(KeyCode.Space) && !dialogueEnded && talkPanel.activeSelf)
        {
            AdvanceDialogue();
        }
    }

    // NPC Ŭ�� �� ��ȭâ Ȱ��ȭ
    void OnMouseDown()
    {
        if (dialogueEnded) return;

        // �̹� ��ȭ ���̸� Ŭ�� ����
        if (talkPanel.activeSelf) return;

        if (signObject != null)
        {
            signObject.SetActive(false);
            signVisible = false;
        }

        if (animator != null)
        {
            animator.SetBool("IsWalk", false);
        }

        // �ȱ� ���� ���� (��ȭ ���̹Ƿ�)
        if (audioSource != null && audioSource.clip == walkSound && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = null;
        }

        // ��ȭâ Ȱ��ȭ �� ù ��� ���
        talkPanel.SetActive(true);
        AdvanceDialogue();
    }

    private void AdvanceDialogue()
    {
        if (dialogueEnded) return;

        if (dialogueSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(dialogueSound);
        }

        switch (clickCount)
        {
            case 0:
                text.text = "�� �� ���ô��� ��� �����̽��ϴ�.\n���� �� ������ ������ ī�̷��Դϴ�.";
                break;
            case 1:
                text.text = "�̰��� ���� �ູ �Ʒ� ��ư��� ������ ��������.\n������ ���� ������ �̻��� ���� �Ͼ�� �־��.";
                break;
            case 2:
                text.text = "�ֹε��� �ϳ��Ѿ� ������� �ֽ��ϴ�.\n���������� ��ݵ� ���� ���� ���� �ܰ��̾����.";
                break;
            case 3:
                text.text = "�����ٸ� ���縦 ��Ź����� �ɱ��?";
                text.rectTransform.anchoredPosition = new Vector2(text.rectTransform.anchoredPosition.x, 0f);
                break;
            case 4:
                talkPanel.SetActive(false);
                dialogueEnded = true;
                gameObject.SetActive(false);
                return;
        }

        clickCount++;
    }
}
