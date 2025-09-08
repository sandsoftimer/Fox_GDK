using UnityEngine;

namespace Com.FoxVerse.Utility
{
    public class TextureAnimation : FoxObject
    {
        public Vector2 animationSpeed;
        public int materialIndex = 0;

        Renderer _renderer;
        Material _material;

        #region ALL UNITY FUNCTIONS

        // Awake is called before Start
        public override void Awake()
        {
            base.Awake();

            if (_renderer == null)
                _renderer = GetComponent<Renderer>();
            _material = _renderer.materials[materialIndex];
        }

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        void Update()
        {
            if (_renderer == null)
                return;

            _material.SetTextureOffset(
                "_BaseMap",
                _material.GetTextureOffset("_BaseMap") + animationSpeed * Time.deltaTime);

        }

        #endregion ALL UNITY FUNCTIONS
        //=================================   
        #region ALL OVERRIDING FUNCTIONS


        #endregion ALL OVERRIDING FUNCTIONS
        //=================================
        #region ALL SELF DECLARE FUNCTIONS


        #endregion ALL SELF DECLARE FUNCTIONS
    }
}