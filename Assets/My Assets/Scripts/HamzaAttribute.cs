
using UnityEngine;
public class HamzaAttribute : PropertyAttribute
{
    public float Thickness;
    public Color Color;

    public HamzaAttribute(float thickness = 1, float r = 0.5f, float g = 0.5f, float b = 0.5f)
    {
        Thickness = thickness;
        Color = new Color(r, g, b);
    }
}
