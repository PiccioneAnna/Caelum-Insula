using System.Collections.Generic;
using UnityEngine;

namespace Enviro
{
    public class GrassObject : MonoBehaviour
    {
        #region Fields

        [Header("Components")]
        [HideInInspector] public GrassObject grass;
        [HideInInspector] public Animator anim;
        public GameObject topGrass;
        public GameObject middleGrass;
        public GameObject bottomGrass;

        public List<Sprite> spritePotentials;

        private SpriteRenderer topSprite;
        private SpriteRenderer middleSprite;
        private SpriteRenderer bottomSprite;

        [Header("Grass Data")]
        public Vector2 position;

        #endregion

        #region Runtime

        void Awake()
        {
            grass = this;
            anim = GetComponent<Animator>();

            topSprite = topGrass.GetComponent<SpriteRenderer>();
            middleSprite = middleGrass.GetComponent<SpriteRenderer>();
            bottomSprite = bottomGrass.GetComponent<SpriteRenderer>();

            InitialSpawn();
        }

        #endregion

        /// <summary>
        /// Method determines grass object's inital sprites & positions
        /// </summary>
        public void InitialSpawn()
        {
            topSprite.sprite = spritePotentials[Random.Range(0, spritePotentials.Count - 1)];
            middleSprite.sprite = spritePotentials[Random.Range(0, spritePotentials.Count - 1)];
            bottomSprite.sprite = spritePotentials[Random.Range(0, spritePotentials.Count - 1)];

            topSprite.flipX = Random.value > .5f;
            middleSprite.flipX = Random.value > .5f;
            bottomSprite.flipX = Random.value > .5f;

            topGrass.transform.position = topGrass.transform.position + new Vector3(Random.Range(-.5f, .5f), 0f);
            middleGrass.transform.position = middleGrass.transform.position + new Vector3(Random.Range(-.5f, .5f), 0f);
            bottomGrass.transform.position = bottomGrass.transform.position + new Vector3(Random.Range(-.5f, .5f), 0f);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            anim.SetTrigger("Shake");
        }

    }
}


