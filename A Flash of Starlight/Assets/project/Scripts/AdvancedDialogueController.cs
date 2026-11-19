using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AdvancedDialogueController : MonoBehaviour
{
    [Header("数据")]
    [SerializeField] DialogueLine[] lines;

    [Header("UI")]
    [SerializeField] Image backgroundImage;
    [SerializeField] Image characterImage;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] Image nameBox;
    [SerializeField] GameObject nextHint;

    [Header("设置")]
    [SerializeField] float typeSpeed = 40f;           // 每秒显示几个字
    [SerializeField] float autoPlayDelay = 0f;        // 0 = 手动翻页
    [SerializeField] string nextScene = "Gameplay";

    int index = 0;
    bool isTyping;
    bool waitingClick;
    Coroutine typingRoutine;

    void Start()
    {
        if (lines == null || lines.Length == 0)
        {
            Debug.LogWarning("[ADV] 没有配置对话");
            return;
        }

        ShowLine(0);
    }

    void Update()
    {
        // 点击或空格翻页
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            OnClickNext();
        }
    }

    // 也给 ClickArea 的 OnClick 用
    public void OnClickNext()
    {
        if (isTyping)
        {
            // 打字中点击 → 立即显示完整
            FinishTyping();
            return;
        }

        // 打字完成 → 下一页
        index++;

        if (index >= lines.Length)
        {
            EndDialogue();
            return;
        }

        ShowLine(index);
    }

    void ShowLine(int i)
    {
        var line = lines[i];

        // 名字
        if (nameText != null)
        {
            bool hasName = !string.IsNullOrEmpty(line.speakerName);
            nameText.text = line.speakerName;
            if (nameBox != null) nameBox.gameObject.SetActive(hasName);
        }

        // 背景
        if (backgroundImage != null && line.background != null)
            backgroundImage.sprite = line.background;

        // 立绘
        if (characterImage != null)
        {
            if (line.character != null)
            {
                characterImage.sprite = line.character;
                characterImage.gameObject.SetActive(true);
            }
            else
            {
                characterImage.gameObject.SetActive(false);
            }
        }

        // 打字机
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        typingRoutine = StartCoroutine(TypeRoutine(line.text));
    }

    IEnumerator TypeRoutine(string fullText)
    {
        isTyping = true;
        if (nextHint != null) nextHint.SetActive(false);

        dialogueText.text = "";
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.text = fullText;
        dialogueText.ForceMeshUpdate();

        int total = dialogueText.textInfo.characterCount;

        float timer = 0f;
        int shown = 0;

        while (shown < total)
        {
            timer += Time.unscaledDeltaTime;
            shown = Mathf.FloorToInt(timer * typeSpeed);

            dialogueText.maxVisibleCharacters = shown;
            yield return null;
        }

        dialogueText.maxVisibleCharacters = total;
        isTyping = false;
        if (nextHint != null) nextHint.SetActive(true);

        // 自动播放
        if (autoPlayDelay > 0f)
        {
            yield return new WaitForSecondsRealtime(autoPlayDelay);
            OnClickNext();
        }
    }

    void FinishTyping()
    {
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        dialogueText.maxVisibleCharacters = 99999;
        isTyping = false;
        if (nextHint != null) nextHint.SetActive(true);
    }

    void EndDialogue()
    {
        if (ScreenFader.Instance != null)
            ScreenFader.Instance.LoadScene(nextScene);
        else
            SceneManager.LoadScene(nextScene);
    }
}