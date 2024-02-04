using Cinemachine;
using UnityEngine;

public class GetCameraCollider : MonoBehaviour
{
    public CinemachineBrain brain;
    public GameObject sceneInfo;
    public PolygonCollider2D polygon;
    public CinemachineConfiner2D confiner;

    public void ResetCameraCollider()
    {
        brain = GetComponent<CinemachineBrain>();
        confiner = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineConfiner2D>();
        sceneInfo = GameObject.Find("Scene Information");

        if(sceneInfo != null )
        {
            polygon = sceneInfo.GetComponent<SceneInformation>().cameraCollider;
            confiner.m_BoundingShape2D = polygon;

            confiner.InvalidateCache();
        }
    }
}
