using MyBox;
using TMPro;
using UnityEngine;

public class HintsController : MonoBehaviour
{
    public Snake snake;

    public TextMeshProUGUI timerText;

    public UnityEngine.UI.Image headImage, lastImage, activeImage;

    public UnityEngine.UI.Image passivePrefab;

    public RectTransform passivesList;

    public GameObject firstPhaseHint;

    private void LateUpdate()
    {
        if (!snake) return;

        timerText.text = (10 - (Time.time) % 10).RoundToInt().ToString();
    }

    public void RebuildLists()
    {
        if (!snake) return;

        headImage.sprite = snake.head.description.headUiImage;
        activeImage.sprite = snake.head.description.activeUiImage;

        lastImage.sprite = snake.tail.description.lastUiImage;
        
        foreach (Transform child in passivesList) {
            Destroy(child.gameObject);
        }

        var n = snake.head;

        while (n)
        {
            if (n.description.passiveImage)
            {
                Instantiate(passivePrefab, passivesList).sprite = n.description.passiveImage;
            }

            n = n.child;
        }
    }
}