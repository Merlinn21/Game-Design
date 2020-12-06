using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerUnit : MonoBehaviour
{
    public Camera battleCamera;

    Vector3 cameraOriginalLocation;
    float cameraOriginalSize;

    public float shakeDuration = 1f;
    public float shakeStrength = 0.5f;
    public float zoomInDuration = 0.8f;
    public float zoomOutDuration = 0.5f;
    public float zoomSize = 3.5f;

    private void Awake()
    {
        cameraOriginalLocation = battleCamera.transform.localPosition;
        cameraOriginalSize = 5f;
    }

    public void MinusCost(GhostMoveBase moveBase)
    {
        costType type = moveBase.getCostType();
        var cost = moveBase.getCost();

        if(type == costType.Hp)
        {
            cost = Mathf.Round((moveBase.getCost() + 1 * PlayerStat.maxHealth / 100));
            PlayerStat.health -= (int)cost;
        }
        else
        {
            cost = Mathf.Round((moveBase.getCost() + 1 * PlayerStat.maxMana / 50));
            PlayerStat.mana -= (int)cost;
        }
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(battleCamera.DOShakePosition(shakeDuration, shakeStrength));
    }

    public void PlayZoomInAnimation(Vector3 position)
    {
        var sequence = DOTween.Sequence();
        var sequence2 = DOTween.Sequence();

        if (position.x == 0)
        {
            sequence.Append(battleCamera.transform.DOLocalMove(position, zoomInDuration));
        }
        else
        {
            if (position.x > 0)
                position.x = 4.5f;
            else
                position.x = -4.5f;

            sequence.Append(battleCamera.transform.DOLocalMove(position, zoomInDuration));            
        }

        sequence2.Append(battleCamera.DOOrthoSize(zoomSize, zoomInDuration));
    }

    public void PlayZoomOutAnimation()
    {
        var sequence = DOTween.Sequence();
        var sequence2 = DOTween.Sequence();

        sequence.Append(battleCamera.transform.DOLocalMove(cameraOriginalLocation, zoomOutDuration));
        sequence2.Append(battleCamera.DOOrthoSize(cameraOriginalSize, zoomOutDuration));

    }
}
