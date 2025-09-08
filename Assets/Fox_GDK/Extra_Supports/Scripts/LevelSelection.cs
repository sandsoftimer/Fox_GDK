using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelection : BaseGameBehaviour
{
    public GameObject numberPlatePrefab;
    public Transform holder;
    public Transform startingPoint;
    public float gap = 100;

    List<NumberPlate> numberPaltes = new List<NumberPlate>();

    public override void Start()
    {
        base.Start();

        int levelNumber = gameplayData.currentLevelNumber - 1;
        float currentGap = startingPoint.localPosition.y - (gap - 50);
        for (int i = 0; i < 5; i++)
        {
            GameObject go = Instantiate(numberPlatePrefab, holder);
            go.transform.localPosition += new Vector3(0, currentGap + gap, 0);
            currentGap = go.transform.localPosition.y;
            //go.transform.SetSiblingIndex(0);
            NumberPlate numberPlate = go.GetComponent<NumberPlate>();
            numberPlate.Initialize(levelNumber++);
            numberPaltes.Add(numberPlate);
        }

        float animationCallingTime = gameplayData.currentLevelNumber == 0 ? 0 : ConstantManager.ONE_FORTH_TIME + 0.5f;
        FoxTools.functionManager.ExecuteAfterWaiting(animationCallingTime, MakeTransition);
    }

    void MakeTransition()
    {
        float transitionTime = ConstantManager.ONE_FORTH_TIME + 0.5f;
        if (gameplayData.currentLevelNumber == 0)
        {
            transitionTime = 0;
        }
        for (int i = 0; i < numberPaltes.Count; i++)
        {
            numberPaltes[i].transform.DOLocalMove(numberPaltes[i].transform.localPosition.FOXE_ModifyThisVector(0, -gap, 0), transitionTime).SetEase(Ease.InBack);
        }

        FoxTools.functionManager.ExecuteAfterWaiting(0.5f, () =>
        {
            if (!gameState.Equals(GameState.GAME_PLAY_STARTED) && numberPaltes[1].isActiveAndEnabled)
                StartCoroutine(numberPaltes[1].Unlock_Effect());
        });
    }
}
