using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CreatureClasses
{
    #region Helper Enums

    public enum Direction
    {
        North,  // up
        South, // down
        East, // ->
        West // <-
    }

    #endregion

    public class CreatureAnimationManager : MonoBehaviour
    {
        #region Fields

        [Header("Components")]

        [HideInInspector] public Animator side_Animator;
        [HideInInspector] public Animator front_Animator;
        [HideInInspector] public Animator back_Animator;

        public GameObject bunnyObj;
        public GameObject front;
        public GameObject back;
        public GameObject side;

        [Header("States")]

        public Direction direction;
        public Vector2 dirValue;

        #endregion

        #region Runtime

        // Start is called before the first frame update
        void Awake()
        {
            side_Animator = side.GetComponent<Animator>();
            front_Animator = front.GetComponent<Animator>();
            back_Animator = back.GetComponent<Animator>();
        }

        #endregion

        #region Public Methods

        public void UpdateAnimation()
        {
            CalculateDirection();
            HandleAnimatorDirection();
        }

        #endregion

        #region Helper Methods
        private void HandleAnimatorDirection()
        {
            switch (direction)
            {
                case Direction.North:
                    front.SetActive(false);
                    back.SetActive(true);
                    side.SetActive(false);
                    break;
                case Direction.South:
                    front.SetActive(true);
                    back.SetActive(false);
                    side.SetActive(false);
                    break;
                case Direction.East:
                    front.SetActive(false);
                    back.SetActive(false);
                    side.SetActive(true);
                    break;
                case Direction.West:
                    front.SetActive(false);
                    back.SetActive(false);
                    side.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        private void CalculateDirection()
        {

            if( Math.Abs(dirValue.x) > Math.Abs(dirValue.y) )
            {
                if (dirValue.x > 0)
                {
                    side.GetComponent<SpriteRenderer>().flipX = false;
                    direction = Direction.West;
                }
                else if (dirValue.x < 0)
                {
                    side.GetComponent<SpriteRenderer>().flipX = true;
                    direction = Direction.East;
                }
            }
            else
            {
                if (dirValue.y > 0)
                {
                    direction = Direction.South;
                }
                else if (dirValue.y < 0)
                {
                    direction = Direction.North;
                }
            }
        }
        #endregion

    }
}


