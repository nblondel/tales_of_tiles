using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogXMLObject", menuName = "Dialog XML object")]
[XmlRoot("dialogs")]
public class DialogXMLObject : ScriptableObject {
    // Could add name, box color, position...
    [SerializeField] private TextAsset dialogXmlAsset;

    [XmlRoot("Dialog")]
    public class Dialog {
        [XmlElement("Line")] public List<Line> LinesList = new List<Line>();
    }

    public class Line {
        [XmlAttribute("Id")] public string id;
        [XmlElement("Text")] public List<string> text = new List<string>();
    }

    public Dialog Load() {
        return LoadXML(dialogXmlAsset);
    }

    private Dialog LoadXML(TextAsset textAsset) {
        var serializer = new XmlSerializer(typeof(Dialog));
        using (var reader = new System.IO.StringReader(textAsset.text)) {
            return serializer.Deserialize(reader) as Dialog;
        }
    }
}