using UnityEngine;

[CreateAssetMenu(fileName = "DialogColorAndFont", menuName = "Dialog color and font")]
public class DialogColorAndFont : ScriptableObject
{
    [SerializeField] private Color dialogColor;
    [SerializeField] private Font dialogFont;
    [SerializeField] private int dialogFontSize;
    [SerializeField] private FontStyle dialogFontStyle;
    [SerializeField] private TextAnchor dialogTextAnchor;
    [SerializeField] private bool letterByLetter;

    public Color GetColor() {
        return dialogColor;
    }

    public Font GetFont() {
        return dialogFont;
    }

    public int GetFontSize() {
        return dialogFontSize;
    }

    public FontStyle GetFontStyle() {
        return dialogFontStyle;
    }
    
    public TextAnchor GetTextAnchor() {
        return dialogTextAnchor;
    }

    public bool GetLetterByLetter() {
        return letterByLetter;
    }
}
