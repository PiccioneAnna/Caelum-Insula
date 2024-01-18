using System.Collections;
using Cinemachine;
using Visuals;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;
    public UnityEngine.GameObject player;
    [SerializeField] ScreenTint screenTint;

    private void Awake()
    {
        instance = this;
    }

    string currentScene;
    AsyncOperation unload;
    AsyncOperation load;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    public void InitSwitchScene(string to, Vector3 targetPosition)
    {
        StartCoroutine(Transition(to, targetPosition));
    }

    IEnumerator Transition(string to, Vector3 targetPosition)
    {
        screenTint.Tint();

        yield return new WaitForSeconds(1f / screenTint.speed + 0.1f); // 1 second divided by speed of tinting and small addiiton

        SwitchScene(to, targetPosition);

        while (load != null && unload != null)
        {
            if (load.isDone) { load = null; }
            if (unload.isDone) { unload = null; }
            yield return new WaitForSeconds(0.1f);
        }

        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(currentScene));

        screenTint.UnTint();
    }

    public void SwitchScene(string to, Vector3 targetPosition)
    {
        load = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        unload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene);
        currentScene = to;

        Transform playerTransform = player.transform;

        CinemachineBrain currentCamera = Camera.main.GetComponent<CinemachineBrain>();

        currentCamera.ActiveVirtualCamera.OnTargetObjectWarped(
            playerTransform,
            targetPosition - playerTransform.position
            );

        playerTransform.position = new Vector3(
            targetPosition.x,
            targetPosition.y,
            playerTransform.position.z);
    }
}
