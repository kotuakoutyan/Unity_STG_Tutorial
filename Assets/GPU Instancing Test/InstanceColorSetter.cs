using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[ExecuteInEditMode]
public class InstanceColorSetter : MonoBehaviour
{
    [SerializeField]
    Color color;

    Renderer renderer;
    MaterialPropertyBlock props;

    static readonly int id = Shader.PropertyToID("_Color");

    void Start()
    {
        color = Random.ColorHSV(0, 1, 0.5f, 1, 1, 1, 1, 1);
        renderer = GetComponent<Renderer>();
        props = new MaterialPropertyBlock();

        props.SetColor(id, color);
        renderer.SetPropertyBlock(props);

        renderer.material.EnableKeyword("_EMISSION");

        Destroy(this);
    }
}