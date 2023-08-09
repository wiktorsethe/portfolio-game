using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraSize : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Transform parentObject;
    [SerializeField] private Transform shipCenter;
    private Camera mainCamera;
 
    private void Start()
    {
        mainCamera = Camera.main;
        Invoke("ChangeCamSize", 1f);
    }
    public void ChangeCamSize()
    {
        if (parentObject != null)
        {
            Bounds parentBounds = CalculateParentBounds();

            float objectHeight = parentBounds.size.y * 2f;

            float objectWidth = parentBounds.size.x * 1.5f;
            float distance = (objectHeight / 2f) / Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);

            //Vector3 cameraPosition = shipCenter.position - mainCamera.transform.forward * distance;
            //mainCamera.transform.position = cameraPosition;

            float frustumHeight = 2f * distance * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float frustumWidth = frustumHeight * mainCamera.aspect;

            if (objectWidth > objectHeight)
            {
                float targetSize = objectWidth * 0.5f;
                //mainCamera.orthographicSize = objectWidth * 0.5f;
                DOTween.To(() => mainCamera.orthographicSize, x => mainCamera.orthographicSize = x, targetSize, 1f).SetUpdate(UpdateType.Normal, true);
                if (targetSize <= 9f)
                {
                    DOTween.To(() => mainCamera.orthographicSize, x => mainCamera.orthographicSize = x, 5f, 1f).SetUpdate(UpdateType.Normal, true);
                }
            }
            else
            {
                float targetSize = objectHeight * 0.5f;
                DOTween.To(() => mainCamera.orthographicSize, x => mainCamera.orthographicSize = x, targetSize, 1f).SetUpdate(UpdateType.Normal, true);
                if (targetSize <= 9f)
                {
                    DOTween.To(() => mainCamera.orthographicSize, x => mainCamera.orthographicSize = x, 9f, 1f).SetUpdate(UpdateType.Normal, true);
                }
            }
        }
    }

    public Bounds CalculateParentBounds()
    {
        Renderer[] renderers = parentObject.GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            Bounds bounds = renderers[0].bounds;

            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }

        return new Bounds(Vector3.zero, Vector3.zero);
    }
    public void CamSize(float targetSize, float duration)
    {   
        
        DOTween.To(() => mainCamera.orthographicSize, x => mainCamera.orthographicSize = x, targetSize, duration).SetUpdate(UpdateType.Normal, true);
    }
}
