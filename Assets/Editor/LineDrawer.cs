using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HamzaAttribute))]
public class LineDrawer : DecoratorDrawer
{
    public override void OnGUI(Rect position)
    {
        HamzaAttribute line = (HamzaAttribute)attribute;
        position.height = line.Thickness;
        EditorGUI.DrawRect(position, line.Color);
    }

    public override float GetHeight()
    {
        HamzaAttribute line = (HamzaAttribute)attribute;
        return line.Thickness + 5f; // Add some spacing
    }
}
