using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    private GameObject player;
    private float smoothTime = 1f;

    private Vector3 velocity = Vector3.zero;

    public void ActivateCortoutine()
    {
        
        StartCoroutine("ChangeBackgroundScale");
    }
    private IEnumerator ChangeBackgroundScale()
    {
        yield return new WaitForSeconds(0.1f);
        player = GameObject.FindGameObjectWithTag("Player");
        Bounds bounds = Bounds();

        Vector3 newScale = new Vector3(1, 1, 1) * (bounds.size.magnitude / 8.5f);
        transform.localScale = newScale;
    }
    private Bounds Bounds()
    {
        Renderer[] renderers = player.GetComponentsInChildren<Renderer>();

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

    public void SmoothChangeScale(float targetSize)
    {
        StartCoroutine(SmoothScaleChangeCoroutine(targetSize));
    }
    IEnumerator SmoothScaleChangeCoroutine(float targetSize)
    {
        Vector3 targetTransform = new Vector3(targetSize / 5f, targetSize / 5f, 1f);
        while (Vector3.Distance(transform.localScale, targetTransform) > 0.01f)
        {
            transform.localScale = Vector3.SmoothDamp(transform.localScale, targetTransform, ref velocity, smoothTime);
            yield return null;
        }
        transform.localScale = targetTransform;
    }
}
