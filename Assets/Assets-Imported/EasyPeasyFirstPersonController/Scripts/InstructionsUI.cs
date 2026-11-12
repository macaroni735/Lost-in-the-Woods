
using UnityEngine;
using System.Collections;

public class InstructionsUI : MonoBehaviour
{
    public CanvasGroup instructionsPanel;
    public float fadeDuration = 2f;

    private bool hasMoved = false;
    private bool manuallyOpened = false;
    private Coroutine fadeCoroutine;

    void Start()
    {
        instructionsPanel.alpha = 1f;
        instructionsPanel.interactable = true;
        instructionsPanel.blocksRaycasts = true;
    }

    void Update()
    {
        if (!hasMoved && !manuallyOpened)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                hasMoved = true;
                HideInstructions();
            }
        }
    }

    public void ShowInstructions()
    {
        manuallyOpened = true;
        StopFade();

        instructionsPanel.alpha = 1f;
        instructionsPanel.interactable = true;
        instructionsPanel.blocksRaycasts = true;
    }

    public void HideInstructions()
    {
        StopFade();
        fadeCoroutine = StartCoroutine(FadeOut());
    }

    public void CloseInstructions()
    {
        manuallyOpened = false;
        HideInstructions();
    }

    private IEnumerator FadeOut()
    {
        instructionsPanel.interactable = false;
        instructionsPanel.blocksRaycasts = false;

        float elapsed = 0f;
        float startingAlpha = instructionsPanel.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            instructionsPanel.alpha = Mathf.Lerp(
                startingAlpha,
                0f,
                elapsed / fadeDuration
            );

            yield return null;
        }

        instructionsPanel.alpha = 0f;
        fadeCoroutine = null;
    }

    private void StopFade()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
    }
}
