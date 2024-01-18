using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace ToolActions
{
    [CreateAssetMenu(menuName = "Data/Tool Action/Switch Scene")]
    public class ReturnHomeAction : Base
    {
        public Scene scene;

        public override bool OnApply(Vector2 worldPoint)
        {
            SceneManager gameSceneManager = GameManager.Instance.sceneManager;

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != scene.name)
            {
                gameSceneManager.InitSwitchScene(scene.name, new Vector3(0, 0, 0));
                return true;
            }
            return false;
        }
    }
}

