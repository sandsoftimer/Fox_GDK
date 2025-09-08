using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com.FunFox.Utility
{
    public class CanvasManager : FoxObject
    {
        public GameObject canvas;
        public GameObject messagePanel;
        public TextMeshProUGUI title, body;
        public Button okButton;

        [Header("Tutorial Hand Properties")]
        public GameObject tutorialHand;

        bool isTutorialShowing;
        float handSpeed;
        Vector3 handStartPoint, handEndPoint;
        Vector3[] path;
        int nextEndPointIndex;
        float repeatGap = 0;
        float remainingGap = 0;
        Action action;

        #region ALL UNITY FUNCTIONS

        public override void Awake()
        {
            base.Awake();

            messagePanel.SetActive(false);
            TutorialHide();
        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {
            if (isTutorialShowing)
            {
                if (remainingGap <= 0)
                {
                    tutorialHand.SetActive(true);
                    Vector3 direction = handEndPoint - tutorialHand.transform.position;
                    //Debug.LogError($"{direction} : {direction.normalized}");
                    direction = direction.normalized;
                    if ((handEndPoint - (tutorialHand.transform.position + handSpeed * Time.fixedDeltaTime * direction)).magnitude > 10f)
                        tutorialHand.transform.position += handSpeed * Time.fixedDeltaTime * direction;
                    else
                    {
                        if (++nextEndPointIndex == path.Length)
                        {
                            action?.Invoke();
                            tutorialHand.transform.position = path[0];
                            nextEndPointIndex = 1;
                            remainingGap = repeatGap;
                            tutorialHand.SetActive(false);
                        }
                        handEndPoint = path[nextEndPointIndex];
                    }
                }
                else
                {
                    remainingGap -= Time.fixedDeltaTime;
                    tutorialHand.SetActive(false);
                }
            }
        }

        #endregion ALL UNITY FUNCTIONS
        //=================================   
        #region ALL OVERRIDING FUNCTIONS


        #endregion ALL OVERRIDING FUNCTIONS
        //=================================
        #region ALL SELF DECLARE FUNCTIONS

        public void Show_TutorialHand(Vector3 screenStartPoint, Vector3 screenEndPoint, float handSpeed, float loopingGap = 0, bool overrideValues = false, Action action = null)
        {
            if (!isTutorialShowing || (isTutorialShowing && overrideValues))
            {
                Show_TutorialHand(new Vector3[] { screenStartPoint, screenEndPoint }, loopingGap, handSpeed, overrideValues, action);
            }
        }

        public void Show_TutorialHand(Vector2[] path, float handSpeed, float loopingGap = 0, bool overrideValues = false, Action action = null)
        {
            if (!isTutorialShowing || (isTutorialShowing && overrideValues))
            {
                Vector3[] _path = new Vector3[path.Length];
                for (int i = 0; i < path.Length; i++)
                    _path[i] = path[i];
                Show_TutorialHand(_path, handSpeed, loopingGap, overrideValues = false, action = null);
            }
        }

        public void Show_TutorialHand(Vector3[] path, float handSpeed, float loopingGap = 0, bool overrideValues = false, Action action = null)
        {
            this.action = action;
            repeatGap = loopingGap;
            if (!isTutorialShowing || (isTutorialShowing && overrideValues))
            {
                this.path = path;
                this.handSpeed = handSpeed;
                nextEndPointIndex = 1;

                this.handStartPoint = this.path[0];
                handEndPoint = path[nextEndPointIndex];
                remainingGap = 0;

                tutorialHand.transform.position = handStartPoint;
                isTutorialShowing = true;
                tutorialHand.SetActive(true);
            }
        }

        public void TutorialHide()
        {
            isTutorialShowing = false;
            tutorialHand.SetActive(false);
            remainingGap = repeatGap;
        }

        public void ShowMessage(string title, string body)
        {
            //if (gameState.Equals(GameState.GAME_PLAY_PAUSED))
            //    return;

            gameState = GameState.GAME_PLAY_PAUSED;
            foxManager.ChangeGameState(GameState.GAME_PLAY_PAUSED);

            this.title.text = title;
            this.body.text = body;

            messagePanel.SetActive(true);
        }

        public void OnMessageOkPress()
        {
            messagePanel.SetActive(false);

            foxManager.ChangeGameState(GameState.GAME_PLAY_UNPAUSED);
        }

        #endregion ALL SELF DECLARE FUNCTIONS

    }
}
