using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCameraCollider : MonoBehaviour
{
    public CinemachineBrain brain;
    public GameObject sceneInfo;
    public PolygonCollider2D polygon;

    // Start is called before the first frame update
    void Start()
    {
        brain = GetComponent<CinemachineBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        if(polygon == null)
        {
            ResetCameraCollider();
        }
    }

    public void ResetCameraCollider()
    {
        sceneInfo = GameObject.Find("Scene Information");

        if(sceneInfo != null )
        {
            polygon = GetComponent<PolygonCollider2D>();

            if(polygon != null && brain != null)
            {
                CinemachineConfiner2D confiner = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineConfiner2D>();
                confiner.m_BoundingShape2D = polygon;
            }
        }
    }
}
