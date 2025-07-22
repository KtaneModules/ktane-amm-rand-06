using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;

public class script : MonoBehaviour {

    public KMBombInfo bombInfo;
    public TextMesh[] lines = new TextMesh[20];
    public SpriteRenderer face;
    public Sprite[] faces = new Sprite[24];
    public GameObject[] states = new GameObject[7];
    public KMBombModule module;
    public TextMesh[] graph = new TextMesh[52];

    public TextMesh avgText;
    public TextMesh avgJudg;
    public TextMesh peakShow;
    public TextMesh peakList;
    public TextMesh invArrow;

    private bool STOPCYCLING = false;

    private int state = 1;
    private bool selected = false;
    private bool appendBusy = false;

    private Color offwhite = new Color(230 / 255f, 223 / 255f, 215 / 255f); //#E6DFD7
    private Color offyellow = new Color(230 / 255f, 199 / 255f, 0f);
    private Color offred = new Color(230 / 255f, 45 / 255f, 22 / 255f);
    private Color offgreen = new Color(70 / 255f, 199 / 255f, 22 / 255f);

    private int[] limits = new int[11] { 1000, 1600, 2300, 2600, 4100, 4900, 5600, 7200, 8200, 9100, 10000 };
    private int[] freqs = new int[10];
    private int[] table = new int[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private int[] graphInts = new int[52];
    private int[] peakFreqs;
    private int[] peakAmps;
    private int[] peakIds;
    private int page = 0;
    private int peakAmount = 0;
    private int avgAmp = 0;

    private int inventoryIndex = 0;
    private int[] ABCD = new int[40] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

    private int AEAN;      // human i remember you're
    private bool picked = false;
    private int faceID = 20;

    string divideBy1000(int num) {
        if ((num % 1000) < 10) return (num / 1000).ToString() + ".00" + (num % 1000).ToString();
        else if((num % 1000) < 100) return (num / 1000).ToString() + ".0" + (num % 1000).ToString();
        else return (num / 1000).ToString() + "." + (num % 1000).ToString();
    }

    IEnumerator onDef()
    {
        yield return new WaitForSeconds(3f);
        if (!selected)
        {
            states[state].SetActive(false);
            states[0].SetActive(true);
        }
        yield return null;
    }
    IEnumerator onFoc()
    {
        states[0].SetActive(false);
        states[state].SetActive(true);
        yield return null;
    }

    void redrawPeakDisplay()
    {
        if (peakAmount > 0)
        {
            peakShow.text = "Local peak " + page + ":\n\n"+ divideBy1000(peakAmps[page]) +" dB @ "+ divideBy1000(peakFreqs[page]) + " kHz";
            peakList.text = (page + 1).ToString() + "/" + peakAmount;
            for (int i = 0; i< peakAmount; i++)
            {
                graph[peakIds[i]].color = i == page ? offyellow : offwhite;
            }
        }
        else
        {
            peakShow.text = "";
            peakList.text = "";
        }
    }
    void redrawIndex()
    {
        invArrow.text = "";
        for (int i = 0; i < inventoryIndex; i++) invArrow.text += "\n";
        invArrow.text += ">";
    }


    void generate()
    {
        int amount = Random.Range(7, 13);
        int nonCrit = Random.Range(2, 6);
        for (int i=0; i<amount; i++)
        {
            int n = Random.Range(0, 20);
            while(table[n]!=0) n = Random.Range(0, 20);
            table[n] = 1;
            if (n < 10) peakAmount++;
        }
        for (int i = 0; i < nonCrit; i++)
        {
            int n = Random.Range(0, 20);
            while (table[n] != 0) n = Random.Range(0, 20);
            table[n] = 2;
            if (n < 10) peakAmount++;
        }

        for (int i=0; i<10; i++)
        {
            freqs[i] = Random.Range(limits[i] + 1, limits[i + 1]);
        }

        peakFreqs = new int[peakAmount];
        peakAmps  = new int[peakAmount];
        peakIds   = new int[peakAmount];
        int k = 0;
        int j = 0;
        int sum = 0;
        for (int i=0; i<52; i++)
        {
            if (j<10 && (1000 + i * 9000 / 52) <= freqs[j] && (1000 + (i + 1) * 9000 / 52) > freqs[j])
            {
                graphInts[i] = table[j] == 1 ? Random.Range(5000, 7500) : table[j] == 2 ? Random.Range(3500, 5000) : Random.Range(2000, 2800);
                if (table[j] > 0)
                {
                    peakAmps[k] = graphInts[i];
                    peakFreqs[k] = freqs[j];
                    peakIds[k] = i;
                    k++;
                }
                j++;
            }
            else graphInts[i] = Random.Range(2000, 2800);
            sum += graphInts[i];
            graph[i].transform.localScale = new Vector3(1f, graphInts[i] / 7500f, 1f);
        }
        avgAmp = sum / 52;
        avgText.text = "Avg: "+divideBy1000(avgAmp);
        if (avgAmp < 2800)
        {
            avgJudg.text = "PASS";
            avgJudg.color = Color.green;
        }
        else if (avgAmp < 3200)
        {
            avgJudg.text = "WARN";
            avgJudg.color = offyellow;
        }
        else
        {
            avgJudg.text = "FAIL";
            avgJudg.color = Color.red;
        }
        
    }

    void lineFeed()
    {
        for (int i=0; i<19; i++)
        {
            lines[i].text = lines[i + 1].text;
            lines[i].color = lines[i + 1].color;
            lines[i].fontStyle = lines[i + 1].fontStyle;
        }
        lines[19].text = "";
        lines[19].color = offwhite;
        lines[19].fontStyle = FontStyle.Normal;
    }
    void clearScreen()
    {
        for (int i = 0; i < 20; i++)
        {
            lines[i].text = "";
            lines[i].color = offwhite;
            lines[i].fontStyle = 0;
        }
    }
    IEnumerator appendText(string text, Color color, bool rightAlign=false, FontStyle style = 0, bool anim =true)
    {
        if (!appendBusy)
        {
            appendBusy = true;
            for (int i = 0; i < text.Length; i++)
            {
                if ((i % 30) == 0)
                {
                    lineFeed();
                    lines[19].text = rightAlign ? "                              " : "";
                    lines[19].color = color;
                    lines[19].fontStyle = style;
                }
                if (anim) yield return new WaitForSeconds(.05f);
                lines[19].text = (rightAlign ? lines[19].text.Substring(1) : lines[19].text) + text[i].ToString();
            }
            appendBusy = false;
        }
        yield return null;
    }

    IEnumerator cycle()
    {
        while (!STOPCYCLING)
        {
            yield return new WaitForSeconds(1f);
            faceID ^= 1;
            face.sprite = faces[faceID];
        }
        yield return null;
    }

    void AbcdCalc()
    {
        int j = 0;
        for (int i=0; i<10; i++)
        {
            if (table[i] != 0)
            {
                ABCD[i] = peakFreqs[j] % 100 /10 ;
                ABCD[10 + i] = peakFreqs[j] % 10;
                ABCD[20 + i] = (peakAmps[j] - avgAmp) % 100 / 10;
                ABCD[30 + i] = (peakAmps[j] - avgAmp) % 10;
                j++;
            }
            else
            {
                ABCD[i] = -1;
                ABCD[10+i] = -1;
                ABCD[20+i] = -1;
                ABCD[30+i] = -1;
            }
        }
    }

    void pass(int index)
    {

    }

    void fail(int index)
    {

    }

    void Start () {
        AEAN = Random.Range(600, 800);
        states[2].SetActive(false);
        face.color = new Color(1, 1, 1, 1);
        GetComponent<KMSelectable>().OnFocus += delegate () { selected = true; StartCoroutine(onFoc()); };
        GetComponent<KMSelectable>().OnDefocus += delegate () { selected = false; StartCoroutine(onDef()); };
        face.sprite = faces[faceID];
        generate();
        AbcdCalc();
        redrawPeakDisplay();
        StartCoroutine(cycle());
        //clearScreen();


    }

    void Update()
    {
        if (selected)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                states[state].SetActive(false);
                state = state == 3 ? 1 : 3;
                states[state].SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                states[state].SetActive(false);
                state = state == 2 ? 1 : 2;
                states[state].SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                states[state].SetActive(false);
                state = state == 5 ? 1 : 5;
                states[state].SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (state == 2)
                {
                    page = page == 0 ? peakAmount - 1 : page - 1;
                    redrawPeakDisplay();
                }
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (state == 2)
                {
                    page = page == peakAmount - 1 ? 0 : page + 1;
                    redrawPeakDisplay();
                }
            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (state == 5)
                {
                    inventoryIndex = inventoryIndex == 0 ? 16 : inventoryIndex - 1;
                    redrawIndex();
                }
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (state == 5)
                {
                    inventoryIndex = inventoryIndex == 16 ? 0 : inventoryIndex + 1;
                    redrawIndex();
                }
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                StartCoroutine(appendText("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", offyellow, true, FontStyle.Bold, true));
            }

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                if (state == 5)
                {
                    int last = (int)bombInfo.GetTime() % 10;
                    switch (inventoryIndex)
                    {
                        case 0: //Screwdriver
                            if (last == (int)(10f * ((ABCD[4] + ABCD[14]) / (ABCD[24] + ABCD[34] + 1)) % 10) && table[4] != 0)
                                pass(4);
                            else fail(4);
                            break;
                        case 1: //Hammer
                            if (last == ((ABCD[3] + ABCD[33]) * (ABCD[23] + ABCD[13]) % 10) && table[3] != 0)
                                pass(3);
                            else fail(3);
                            break;
                        case 2: //Compressed Air
                            if (last!= ABCD[7] && last != ABCD[17] && last != ABCD[27] && last != ABCD[37] && table[7] != 0)
                                pass(7);
                            else fail(7);
                            break;
                        case 3: //Oilcan
                            if (last == 0 && table[0] != 0)
                                pass(0);
                            else fail(0);
                            break;
                        default: return;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (state == 5)
                {
                    int last = (int)bombInfo.GetTime() % 10;
                    switch (inventoryIndex)
                    {
                        case 1: //Hammer
                            if (last == ((ABCD[6] + ABCD[36]) * (ABCD[26] + ABCD[16]) % 10) && table[6] != 0)
                                pass(6);
                            else fail(6);
                            break;
                        case 2: //Compressed Air
                            if (last != ABCD[8] && last != ABCD[18] && last != ABCD[28] && last != ABCD[38] && table[8] != 0)
                                pass(8);
                            else fail(8);
                            break;
                        case 3: //Oilcan
                            if (last == 0 && table[1] != 0)
                                pass(1);
                            else fail(1);
                            break;
                        default: return;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (state == 5)
                {
                    int last = (int)bombInfo.GetTime() % 10;
                    switch (inventoryIndex)
                    {
                        case 2: //Compressed Air
                            if (last != ABCD[9] && last != ABCD[19] && last != ABCD[29] && last != ABCD[39] && table[9] != 0)
                                pass(9);
                            else fail(9);
                            break;
                        case 3: //Oilcan
                            if (last == 0 && table[2] != 0)
                                pass(2);
                            else fail(2);
                            break;
                        default: return;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (state == 5)
                    if (inventoryIndex == 3)
                        if ((int)bombInfo.GetTime() % 10 == 0 && table[5] != 0) pass(5);
                        else fail(5);
                    else return;
            }

        }
    }
}
