using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    public Ghost Ghost { get; set; } //properties
    public int level;

    Image sprite;
    Vector3 originalPos;
    Color originalColor;

    public float fadeInSpeed = 1.5f;
    public float fadeOutSpeed = 0.3f;

    private void Awake()
    {
        sprite = this.GetComponent<Image>();
        originalPos = sprite.transform.localPosition;
        originalColor = sprite.GetComponent<Image>().color;
    }

    public void Setup(GhostBase ghost, int lvl)
    {
        level = lvl;
        Ghost = new Ghost(ghost, level);

        if(Ghost != null)
        {
            GetComponent<Image>().sprite = Ghost.Base.getSprite();
            this.gameObject.SetActive(true);
        }

        if(Ghost.Base.getName() == "NoGhost")
        {
            this.gameObject.SetActive(false);
        }
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(sprite.DOFade(255, 0.8f));
        sequence.Append(sprite.DOFade(0, fadeInSpeed));
        sequence.Append(sprite.DOFade(255, fadeOutSpeed));
    }

    public Vector3 GetPosition() { return originalPos; }
}
