using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingObject : MonoBehaviour
{
    public List<Renderer> renderers = new List<Renderer>();
    public Vector3 position;
    public List<Material> materials = new List<Material>();

    public float initialAlpha;

    private void Awake()
    {
        position = transform.position;
        if(renderers.Count == 0)
        {
            renderers.AddRange(GetComponentsInChildren<Renderer>());
        }
        foreach(Renderer renderer in renderers)
        {
            materials.AddRange(renderer.materials);
        }

        initialAlpha = materials[0].color.a;
    }

    public bool Equals(FadingObject other) => position.Equals(other.position);
    public override int GetHashCode() => position.GetHashCode();


}
