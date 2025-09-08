/*
 * Developer Name: Md. Imran Hossain
 * E-mail: sandsoftimer@gmail.com
 * FB: https://www.facebook.com/md.imran.hossain.902
 * in: https://www.linkedin.com/in/md-imran-hossain-69768826/
 * 
 * Features:
 * Scene FadeIn-Out MakeTransition
 * Loading Next level
 * Reloading Current Level
 * Get level Index  
 */


using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Com.FunFox.Utility
{
    [RequireComponent(typeof(Animator))]
    public class SceneManager : FoxObject
    {
        public KV_Scene_Transition_Type kV_Scene_Transition_Type;

        public Image cutoffTex;
        public List<Texture> cutoffSprites;

        [HideInInspector]
        public FoxTools owner;
        bool isBusy;
        Animator sceneFadeanimator;
        Animator SceneFadeanimator
        {
            get
            {
                if (sceneFadeanimator == null)
                    sceneFadeanimator = GetComponent<Animator>();
                return sceneFadeanimator;
            }
        }
        string levelToLoadByName;
        int levelToLoadByIndex;

        float lastPauseValue = 1;
        const string FadeOut = "FadeOut";
        const string FadeIn = "FadeIn";
        const string SlideOut = "SlideOut";
        const string SlideIn = "SlideIn";

        LoadSceneType loadLevelType;
        internal static Action sceneLoaded;

        public override void Awake()
        {
            base.Awake();

            cutoffTex.gameObject.SetActive(false);
        }

        public override void OnEnable()
        {
            base.OnEnable();

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnLoadCallback;
        }

        public override void OnDisable()
        {
            base.OnDisable();

            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnLoadCallback;
        }

        void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
        {
            //Debug.LogError("Fade calling");
            string trigger = GetTransitionType(false);

            //SceneFadeanimator.SetTrigger(trigger);
        }

        string publisherBootSceneName = "MoonSDKScene";
        private string GetTransitionType(bool transitionIn)
        {
            string transitionType = "";
            if (GetLevelName().Equals(publisherBootSceneName) || GetLevelName().Equals("Fox_BootScene") && !transitionIn)
            {
                transitionType = transitionIn ? FadeIn : FadeOut;
            }
            else
            {
                switch (kV_Scene_Transition_Type)
                {
                    case KV_Scene_Transition_Type.FADE:
                        transitionType = transitionIn ? FadeIn : FadeOut;
                        SceneFadeanimator.SetTrigger(transitionType);
                        break;
                    case KV_Scene_Transition_Type.SHATTER_ANIMATION:
                        transitionType = transitionIn ? SlideIn : SlideOut;
                        SceneFadeanimator.SetTrigger(transitionType);
                        break;
                    case KV_Scene_Transition_Type.TEXTURE_ANIMATION:
                        if (transitionIn)
                        {
                            cutoffTex.material.SetFloat("_Cutoff", 1);
                            cutoffTex.material.SetTexture("_MainTex", cutoffSprites[Random.Range(0, cutoffSprites.Count)]);
                            cutoffTex.gameObject.SetActive(true);
                            cutoffTex.material.DOFloat(-0.1f, "_Cutoff", ConstantManager.ONE_HALF_TIME + ConstantManager.ONE_FORTH_TIME).OnComplete(() =>
                            {
                                OnFadeInComplete();
                            }).SetEase(Ease.InSine);
                        }
                        else
                        {
                            cutoffTex.material.SetFloat("_Cutoff", 0);
                            //cutoffTex.material.SetTexture("_MainTex", cutoffSprites[Random.Range(0, cutoffSprites.Count)]);
                            cutoffTex.material.DOFloat(1.1f, "_Cutoff", ConstantManager.ONE_HALF_TIME + ConstantManager.ONE_FORTH_TIME).OnComplete(() =>
                            {
                                OnFadeOutComplete();
                                cutoffTex.gameObject.SetActive(false);
                            }).SetEase(Ease.InSine);
                        }
                        break;
                }
            }

            return transitionType;
        }

        public void OnFadeOutComplete()
        {
            isBusy = false;
        }

        public void OnFadeInComplete()
        {
            switch (loadLevelType)
            {
                case LoadSceneType.LOAD_BY_NAME:

                    UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoadByName);
                    break;
                case LoadSceneType.LOAD_BY_INDEX:
                    UnityEngine.SceneManagement.SceneManager.LoadScene(levelToLoadByIndex);
                    break;
            }
        }

        public void LoadLevel(string levelName)
        {
            //owner.poolManager.ResetPoolManager();
            isBusy = true;
            levelToLoadByName = levelName;
            loadLevelType = LoadSceneType.LOAD_BY_NAME;
            string trigger = GetTransitionType(true);
            //SceneFadeanimator.SetTrigger(trigger);
        }

        public void LoadLevel(int levelIndex)
        {
            //owner.poolManager.ResetPoolManager();
            isBusy = true;
            levelToLoadByIndex = levelIndex;
            loadLevelType = LoadSceneType.LOAD_BY_INDEX;
            string trigger = GetTransitionType(true);
            //SceneFadeanimator.SetTrigger(trigger);
        }

        // This will re-load current level;
        public void ReLoadLevel()
        {
            if (isBusy)
                return;

            LoadLevel(GetLevelIndex());
        }

        // This will load next index scene
        // If not exist the it will open auto First scene of BuildIndex.
        public void LoadNextLevel()
        {
            if (isBusy)
                return;

            int loadedIndex = GetLevelIndex() + 1;

            if (loadedIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
            {
                LoadLevel(loadedIndex);
            }
            else
            {
                loadedIndex = 1;
#if PUBLISHER_SDK_INSTALLED
                                loadedIndex++;
#endif
                LoadLevel(loadedIndex);
            }
        }

        public int GetLevelIndex()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        }

        public string GetLevelName()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.R))
                ReLoadLevel();
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (Time.timeScale == 0)
                {
                    Time.timeScale = lastPauseValue;
                }
                else
                {
                    lastPauseValue = Time.timeScale;
                    Time.timeScale = 0;
                }
            }
#endif
        }
    }
}
