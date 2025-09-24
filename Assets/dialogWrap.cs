using System;
using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

public class dialogWrap : MonoBehaviour
{
    public TextMesh lines;
    
    private DialogEntry[] dialogs;
    private const string dialogsJson = "{\"dialogs\":[{\"name\": \"initial\", \"dialog\": [{\"text\": [\"Dialogs are in WIP.\"], \"color\": 4, \"rightAlign\": false, \"style\": 0, \"anim\": false}]}]}";
    public bool dialogBusy;
    private bool appendBusy;
    
    void Awake()
    {
        if (lines != null) lines.text += '\n';
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
                    part.rightAlign,
                    part.anim
                );
                yield return new WaitForSeconds(0.7f);
            }

        dialogBusy = false;
    }
    
    public IEnumerator PlayDialogSequence(string sequenceName, params string[] args)
    {
        dialogBusy = true;
        DialogEntry dialog0 = dialogs.FirstOrDefault(e => e.name == sequenceName);
        if (dialog0 != null)
            foreach (var part in dialog0.dialog)
            {
                yield return appendText(
                    part.text.Length > 0 ? string.Format(part.text[Random.Range(0, part.text.Length)], args) : "",
                    part.rightAlign,
                    part.anim
                );
                yield return new WaitForSeconds(0.7f);
            }

        dialogBusy = false;
    }

    private IEnumerator appendText(string text, bool rightAlign = false, bool anim = true)
    {
        if (!appendBusy)
        {
            appendBusy = true;
            for (int i = 0; i < text.Length; i++)
            {
                if (i % 39 == 0)
                {
                    lines.text += '\n';
                    lines.text += rightAlign ? new string(' ', 39) : "";
                }

                if (anim) yield return new WaitForSeconds(.03f);
                lines.text = (rightAlign ? lines.text.Substring(0,lines.text.Length-39) + lines.text.Substring(lines.text.Length-38,38) : lines.text) + text[i];
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
    public bool rightAlign = false;
    public bool anim = true;
}