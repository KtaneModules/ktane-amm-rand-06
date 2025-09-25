using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

public class dialogWrap : MonoBehaviour
{
    public TextMesh screenText;
    public TextAsset dialogText;
    private DialogEntry[] dialogs;
    //private const string dialogsJson = "{\"dialogs\":[{\"name\": \"initial\", \"dialog\": [{\"text\": [\"Dialogs are in WIP.\"], \"color\": 4, \"rightAlign\": false, \"style\": 0, \"anim\": false}]}]}";
    public bool dialogBusy;
    private bool appendBusy;
    
    void Awake()
    {
        if (screenText != null) screenText.text = "";
        dialogs = JsonConvert.DeserializeObject<DialogData>(dialogText.text).dialogs;
    }

    public IEnumerator playRandom(string[] names)
    {
        yield return PlayDialogSequence(names[Random.Range(0,names.Length)]);
    }
    
    public IEnumerator playSequence(string[][] tuples)
    {
        foreach (string[] tuple in tuples)
        {
            if (tuple.Length<2) yield return PlayDialogSequence(tuple[0]);
            else yield return PlayDialogSequence(tuple[0],tuple.Skip(1).ToArray());
        }
    }
    
    public IEnumerator PlayDialogSequence(string sequenceName)
    {
        if (!dialogBusy){
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
    }
    
    public IEnumerator PlayDialogSequence(string sequenceName, params string[] args)
    {
        if (!dialogBusy)
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
    }

    private IEnumerator appendText(string text, bool rightAlign = false, bool anim = true)
    {
        if (!appendBusy)
        {
            appendBusy = true;
            if (!string.IsNullOrEmpty(screenText.text)) { screenText.text += '\n'; if (screenText.text.Split('\n').Length > 20) screenText.text = screenText.text.Substring(screenText.text.IndexOf('\n')+1);}
            
            Stack<string> openTags = new Stack<string>();
            Stack<string> closingTags = new Stack<string>();
            int visibleLength = 0;
            string currentLine = "";
            int reverseInsertIndex = 0;
    
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '<')
                {
                    int tagEnd = text.IndexOf('>', i);
                    if (tagEnd != -1)
                    {
                        string tag = text.Substring(i, tagEnd - i + 1);
                        if (tag.StartsWith("</")) { if (closingTags.Count > 0) {
                            reverseInsertIndex += closingTags.Pop().Length;
                            openTags.Pop();
                            //Debug.Log(reverseInsertIndex);
                        } }
                        else
                        {
                            int endingTagStart = text.IndexOf("</"+tag[1], tagEnd);
                            int endingTagFinish = text.IndexOf('>', text.IndexOf("</"+tag[1], tagEnd) + 1);
                            string endingTag = text.Substring(endingTagStart, endingTagFinish - endingTagStart + 1);
                            //Debug.Log(tag+"test"+endingTag);
                            currentLine = currentLine.Insert(currentLine.Length + reverseInsertIndex, tag+endingTag);
                            closingTags.Push(endingTag);
                            openTags.Push(tag);
                            reverseInsertIndex -= endingTag.Length;
                            //Debug.Log(reverseInsertIndex);
                        }
                        i = tagEnd;
                        continue;
                    }
                }
                if (visibleLength % 39 == 0 && visibleLength > 0)
                {
                    
                    screenText.text += '\n';
                    if (screenText.text.Split('\n').Length > 20) screenText.text = screenText.text.Substring(screenText.text.IndexOf('\n')+1);
                    currentLine = "";
                    foreach (var element in openTags) currentLine += element;
                    foreach (var element in closingTags.Reverse()) currentLine += element;
                    

                }
                if (anim) yield return new WaitForSeconds( text[i]=='.' || text[i]=='?' || text[i]=='!' || text[i]=='-'?.15f:.03f);

                try
                {
                    currentLine = currentLine.Insert(currentLine.Length + reverseInsertIndex, text[i].ToString());
                    // баг при переносе строки с тегами
                }
                catch
                {
                    Debug.Log("");
                }
            
                visibleLength++;
                string displayText = screenText.text;
                if (rightAlign)
                {
                    int padding = 39 - (visibleLength % 39 == 0 ? 39 : visibleLength % 39);
                    displayText = displayText.Substring(0,displayText.LastIndexOf('\n')+1)+new string(' ', padding) + currentLine;
                }
                else displayText = displayText.Substring(0,displayText.LastIndexOf('\n') + 1) + currentLine;
                //displayText += text[i];
                screenText.text = displayText;
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