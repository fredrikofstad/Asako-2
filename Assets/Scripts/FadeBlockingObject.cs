using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBlockingObject : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform player;
    [SerializeField] private Camera camera;
    [SerializeField] private float fadedAlpha = .33f;

    [SerializeField] private float checksPerSecond = 10;
    [SerializeField] private int fadeFPS = 30;
    [SerializeField] private int fadeSpeed = 1;
    [SerializeField] private FadeMode fadeMode;


    [Header("Read Only Data")] // not for assignment but nice to view in inspector
    [SerializeField] 
    private List<FadingObject> objectsBlockingView = new List<FadingObject>();
    private List<int> indexesToClear = new List<int>();
    private Dictionary<FadingObject, Coroutine> runningCoroutines = new Dictionary<FadingObject, Coroutine>();

    private RaycastHit[] hits = new RaycastHit[10]; // 10 nice middleground for performance

    private void Start()
    {
        player = GameManager.instance.player.transform;
        StartCoroutine(CheckForObjects());
    }

    private IEnumerator CheckForObjects()
    {
        WaitForSeconds wait = new WaitForSeconds(1f / checksPerSecond);

        while (true)
        {
            int castHits = Physics.RaycastNonAlloc(camera.transform.position, (player.transform.position - camera.transform.position).normalized,
                hits, Vector3.Distance(camera.transform.position, player.transform.position), layerMask);
            if (castHits > 0)
            {
                for (int i = 0; i < castHits; i++)
                {
                    FadingObject fadingObject = GetFadingObjectFromHit(hits[i]);
                    if( fadingObject != null && !objectsBlockingView.Contains(fadingObject))
                    {
                        if (runningCoroutines.ContainsKey(fadingObject))
                        {
                            if(runningCoroutines[fadingObject] != null)
                            {
                                StopCoroutine(runningCoroutines[fadingObject]);
                            }
                            runningCoroutines.Remove(fadingObject);
                        }
                        runningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectOut(fadingObject)));
                        objectsBlockingView.Add(fadingObject);
                    }
                }
            }
            FadeObjectsNoLongerHit();

            ClearHits();

            yield return wait;
        }

    }

    private void FadeObjectsNoLongerHit()
    {
        for (int i = 0; i < objectsBlockingView.Count; i++)
        {
            bool objectIsBeingHit = false;
            for (int j = 0; j < hits.Length; j++)
            {
                FadingObject fadingObject = GetFadingObjectFromHit(hits[j]);
                if(fadingObject != null && fadingObject == objectsBlockingView[i])
                {
                    objectIsBeingHit = true;
                    break;
                }
            }
            if (!objectIsBeingHit)
            {
                if (runningCoroutines.ContainsKey(objectsBlockingView[i]))
                {
                    if(runningCoroutines[objectsBlockingView[i]] != null)
                    {
                        StopCoroutine(runningCoroutines[objectsBlockingView[i]]);
                    }
                    runningCoroutines.Remove(objectsBlockingView[i]);
                }
                runningCoroutines.Add(objectsBlockingView[i], StartCoroutine(FadeObjectIn(objectsBlockingView[i])));
                objectsBlockingView.RemoveAt(i);
            }
        }
    }

    private IEnumerator FadeObjectOut(FadingObject fadingObject)
    {
        float waitTime = 1f / fadeFPS;
        WaitForSeconds wait = new WaitForSeconds(waitTime);
        int ticks = 1;

        // ****  Uses unity's default shaders ****

        // changes from opaque to transparant rendering

        for (int i = 0; i < fadingObject.materials.Count; i++)
        {
            fadingObject.materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            fadingObject.materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            fadingObject.materials[i].SetInt("_ZWrite", 0); // disables Z writing
            if(fadeMode == FadeMode.FADE)
            {
                fadingObject.materials[i].EnableKeyword("_ALPHABLEND_ON");
            }
            else
            {
                fadingObject.materials[i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
            }

            fadingObject.materials[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }

        // reduces transparancy over multiple frames
        if (fadingObject.materials[0].HasProperty("_Color"))
        {
            while(fadingObject.materials[0].color.a > fadedAlpha)
            {
                for (int i = 0; i < fadingObject.materials.Count; i++)
                {
                    if (fadingObject.materials[i].HasProperty("_Color"))
                    {
                        fadingObject.materials[i].color = new Color(
                            fadingObject.materials[i].color.r,
                            fadingObject.materials[i].color.g,
                            fadingObject.materials[i].color.b,
                            Mathf.Lerp(fadingObject.initialAlpha, fadedAlpha, waitTime * ticks * fadeSpeed)
                        );
                    }
                }
                ticks++;
                yield return wait;
            }
        }
        if (runningCoroutines.ContainsKey(fadingObject))
        {
            StopCoroutine(runningCoroutines[fadingObject]);
            runningCoroutines.Remove(fadingObject);

        }

    }

    // Inverse of fade out
    private IEnumerator FadeObjectIn(FadingObject fadingObject)
    {
        float waitTime = 1f / fadeFPS;
        WaitForSeconds wait = new WaitForSeconds(waitTime);
        int ticks = 1;

        if (fadingObject.materials[0].HasProperty("_Color"))
        {
            while (fadingObject.materials[0].color.a < fadingObject.initialAlpha)
            {
                for (int i = 0; i < fadingObject.materials.Count; i++)
                {
                    if (fadingObject.materials[i].HasProperty("_Color"))
                    {
                        fadingObject.materials[i].color = new Color(
                            fadingObject.materials[i].color.r,
                            fadingObject.materials[i].color.g,
                            fadingObject.materials[i].color.b,
                            Mathf.Lerp(fadedAlpha, fadingObject.initialAlpha, waitTime * ticks * fadeSpeed)
                        );
                    }
                }

                ticks++;
                yield return wait;
            }
        }

        for (int i = 0; i < fadingObject.materials.Count; i++)
        {
            if (fadeMode == FadeMode.FADE)
            {
                fadingObject.materials[i].DisableKeyword("_ALPHABLEND_ON");
            }
            else
            {
                fadingObject.materials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
            }
            fadingObject.materials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            fadingObject.materials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            fadingObject.materials[i].SetInt("_ZWrite", 1); // re-enable Z Writing
            fadingObject.materials[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
        }

        if (runningCoroutines.ContainsKey(fadingObject))
        {
            StopCoroutine(runningCoroutines[fadingObject]);
            runningCoroutines.Remove(fadingObject);
        }

    }

    private FadingObject GetFadingObjectFromHit(RaycastHit hit)
    {
        return hit.collider != null ? hit.collider.GetComponent<FadingObject>() : null;
    }

    private void ClearHits()
    {
        RaycastHit emptyHit = new RaycastHit();
        for (int i = 0; i < hits.Length; i++)
        {
            hits[i] = emptyHit;
        }

    }

    public enum FadeMode
    {
        TRANSPARENT,
        FADE
    }



}
