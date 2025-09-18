using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

public class dialogWrap : MonoBehaviour
{
    public TextMesh[] lines = new TextMesh[20];
    
    private DialogEntry[] dialogs;
    private const string dialogsJson = "{\"dialogs\":[{\"name\": \"initial\", \"dialog\": [{\"text\": [\"Dialogs are in WIP.\"], \"color\": 4, \"rightAlign\": false, \"style\": 0, \"anim\": false}]}]}";
    public bool dialogBusy;
    private bool appendBusy;

    static Color rgb(int r, int g, int b) {return new Color(r / 255f, g / 255f, b / 255f);}

    private Color[] colorArray;
    
    void Awake()
    {
        colorArray = new[]
        {
            rgb(230, 223, 215),
            rgb(203, 60, 60),
            rgb(236, 219, 68),
            rgb(110, 197, 92),
            rgb(98, 139, 243),
            rgb(220, 140, 64),
            rgb(88, 88, 88),
            rgb(139, 76, 22),
            rgb(178, 93, 214),
            rgb(159, 156, 152),
            rgb(217, 142, 138),
            rgb(104, 168, 168),
            rgb(106, 178, 142),
            rgb(102, 158, 193),
            rgb(168, 86, 121)
        };
        
        for (int i = 0; i < 20; i++)
        {
            if (lines[i] != null)
            {
                lines[i].text = "";
                lines[i].color = colorArray[0];
                lines[i].fontStyle = FontStyle.Normal;
            }
        }
        dialogs = JsonConvert.DeserializeObject<DialogData>(dialogsJson).dialogs;
    }
    
    public IEnumerator PlayDialogSequence(string sequenceName)
    {
        dialogBusy = true;
        DialogEntry dialog0 = dialogs.FirstOrDefault(e => e.name == sequenceName);
        if (dialog0 != null)
            foreach (var part in dialog0.dialog)
            {
                yield return appendText(
                    part.text.Length > 0 ? part.text[Random.Range(0, part.text.Length)] : "",
                    colorArray[part.color],
                    part.rightAlign,
                    (FontStyle)part.style,
                    part.anim
                );
                yield return new WaitForSeconds(0.7f);
            }

        dialogBusy = false;
    }
    
    void lineFeed()
    {
        for (int i = 0; i < 19; i++)
        {
            lines[i].text = lines[i + 1].text;
            lines[i].color = lines[i + 1].color;
            lines[i].fontStyle = lines[i + 1].fontStyle;
        }

        lines[19].text = "";
        lines[19].color = colorArray[0];
        lines[19].fontStyle = FontStyle.Normal;
    }
    
    public IEnumerator appendText(string text, Color color, bool rightAlign = false, FontStyle style = FontStyle.Normal, bool anim = true)
    {
        if (!appendBusy)
        {
            appendBusy = true;
            for (int i = 0; i < text.Length; i++)
            {
                if (i % 30 == 0)
                {
                    lineFeed();
                    lines[19].text = rightAlign ? new string(' ', 30) : "";
                    lines[19].color = color;
                    lines[19].fontStyle = style;
                }

                if (anim) yield return new WaitForSeconds(.03f);
                lines[19].text = (rightAlign ? lines[19].text.Substring(1) : lines[19].text) + text[i];
            }

            appendBusy = false;
        }

        yield return null;
    }
}

[System.Serializable]
public class DialogData
{
    public DialogEntry[] dialogs;
}

public class DialogEntry
{
    public string name;
    public DialogPart[] dialog;
}

public class DialogPart
{
    public string[] text;
    public int color;
    public bool rightAlign;
    public int style;
    public bool anim;
}