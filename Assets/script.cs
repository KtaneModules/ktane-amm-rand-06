using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class script : MonoBehaviour
{
    static int ModuleIdCounter = 1;
    int ModuleId;
    private bool ModuleSolved;

    public dialogWrap dialogWrapper;
    public VideoScreenPlayer videoScreenPlayer;
    

    public KMBossModule Boss;
    private string[] ignoredModules;
    private int nonIgnored;
    private int solvedModules;

    public KMBombInfo bombInfo;
    public SpriteRenderer face;
    public Sprite[] faces = new Sprite[24];
    public GameObject[] states = new GameObject[10];
    public TextMesh[] graph = new TextMesh[52];
    public TextMesh question;
    public TextMesh answer;

    private bool holdBool;
    private bool charging;
    private bool start;

    public TextMesh[] amounts;
    public TextMesh[] brackets;
    private readonly int[] itemAmount = new int[12];
    private readonly int[] itemBuffer = new int[12];
    private readonly int[] wireComposerConfig = new int[15];
    private int wireComposerIndex;
    private int batteryAmount;

    private int wireComposerCircuit = -1;

    public TextMesh avgText;
    public TextMesh avgJudg;
    public TextMesh peakShow;
    public TextMesh peakList;
    public TextMesh invArrow;
    public TextMesh varList;

    public TextMesh[] ammWires = new TextMesh[4];
    public TextMesh colorblind;
    public TextMesh[] wires = new TextMesh[6];
    public TextMesh[] wireConfigs = new TextMesh[6];
    public TextMesh batteryConfig;
    private bool firstPickup = true;
    public TextMesh lockAEAN;

    private readonly string[] lockSentences =
    {
        "Hope you're proud\nof yourself.", "You had one job.", "Restart the mission.", "Was it worth it?",
        "Task successfully failed.\nCongratulations.", "What did you expect?", "Happy now?"
    };

    private int state = 1;
    private bool selected;
    private bool done;

    private readonly float[] itemRes = { 1f, 4f, 10f, 25f, 40f, 200f, 100f, 250f, 440f, 720f, 1000f, 5000f };

    private readonly int[] limits = { 1000, 1600, 2300, 2600, 4100, 4900, 5600, 7200, 8200, 9100, 10000 };
    private readonly int[] freqs = new int[10];
    private readonly int[] table = new int[20];
    private readonly int[] graphInts = new int[52];
    private int[] peakFreqs;
    private int[] peakAmps;
    private int[] peakIds;
    private int page;
    private int peakAmount;
    private int avgAmp;

    private readonly string[] varNames =
        { "HVO1", "HVO2", "HVO3", "HVO4", "HVO5", "LVO1", "LVO2", "LVO3", "LVO4", "LVO5", "AEAN", "BTR%" };

    private int inventoryIndex;

    private readonly int[] ABCD = Enumerable.Repeat(-1, 40).ToArray();

    private readonly int[] voltages = new int[10];
    private readonly int[] nomVoltages = { 416000, 750000, 1200000, 3000000, 4000000, 500, 800, 1200, 2000, 4500 };
    private readonly int[] nomResistances = { 52000, 60000, 100000, 50000, 250000, 200, 360, 600, 200, 2250 };


    private int AEAN; // human i remember you're
    private int BTR;
    private float dAEAN = 1;
    private int initialStep;
    private bool picked;
    private int faceID = 20;
    private int distance;
    private int chargeDigit;
    private readonly int[] wireCode = new int[4];
    private readonly int[] batteryCode = new int[4];
    private bool wireCharge;
    public GameObject wireChargeMenu;
    public GameObject batteryChargeMenu;

    private readonly string[] itemNames =
    {
        "Screwdriver", "Hammer", "Compressed Air", "Oilcan", "Black Wire", "Red Wire", "Yellow Wire", "Blue Wire",
        "White Wire", "Green Wire", "NKN-Resistor", "RGN-Resistor", "YYN-Resistor", "PRN-Resistor", "NKR-Resistor",
        "GKR-Resistor", "Car Battery"
    };

    public TextMesh inventory;
    private string sourceChargeConfig = "";

    private static readonly Color[] colorArray = {
        new Color(230 / 255f, 223 / 255f, 215 / 255f),  //#e6dfd7ff
        new Color(203 / 255f, 60 / 255f, 60 / 255f),    //#cb3c3cff
        new Color(236 / 255f, 219 / 255f, 68 / 255f),   //#ecdb44ff
        new Color(110 / 255f, 197 / 255f, 92 / 255f),   //#6ec55cff
        new Color(098 / 255f, 139 / 255f, 243 / 255f),  //#628bf3ff
        new Color(220 / 255f, 140 / 255f, 64 / 255f),   //#dc8c40ff
        new Color(088 / 255f, 88 / 255f, 88 / 255f),    //#585858ff
        new Color(139 / 255f, 76 / 255f, 22 / 255f),    //#8b4c16ff
        new Color(178 / 255f, 93 / 255f, 214 / 255f),   //#b25dd6ff
        new Color(159 / 255f, 156 / 255f, 152 / 255f),  //#9f9c98ff
        new Color(217 / 255f, 142 / 255f, 138 / 255f),  //#d98e8aff
        new Color(104 / 255f, 168 / 255f, 168 / 255f),  //#68a8a8ff
        new Color(106 / 255f, 178 / 255f, 142 / 255f),  //#6ab28eff
        new Color(102 / 255f, 158 / 255f, 193 / 255f),  //#669ec1ff
        new Color(168 / 255f, 86 / 255f, 121 / 255f)    //#a85679ff
    };

    private readonly string[] wiresText = {
        "<color={0}>--</color><color=#585858ff>K</color><color={0}>--</color>",
        "<color={0}>--</color><color=#cb3c3cff>R</color><color={0}>--</color>",
        "<color={0}>--</color><color=#ecdb44ff>Y</color><color={0}>--</color>",
        "<color={0}>--</color><color=#628bf3ff>B</color><color={0}>--</color>",
        "<color={0}>--</color><color=#e6dfd7ff>W</color><color={0}>--</color>",
        "<color={0}>--</color><color=#6ec55cff>G</color><color={0}>--</color>",
        "<color={0}>#</color><color=#8b4c16ff>N</color><color=#585858ff>K</color><color=#8b4c16ff>N</color><color={0}>#</color>",
        "<color={0}>#</color><color=#cb3c3cff>R</color><color=#6ec55cff>G</color><color=#8b4c16ff>N</color><color={0}>#</color>",
        "<color={0}>#</color><color=#ecdb44ff>Y</color><color=#ecdb44ff>Y</color><color=#8b4c16ff>N</color><color={0}>#</color>",
        "<color={0}>#</color><color=#b25dd6ff>P</color><color=#cb3c3cff>R</color><color=#8b4c16ff>N</color><color={0}>#</color>",
        "<color={0}>#</color><color=#8b4c16ff>N</color><color=#585858ff>K</color><color=#cb3c3cff>R</color><color={0}>#</color>",
        "<color={0}>#</color><color=#6ec55cff>G</color><color=#585858ff>K</color><color=#cb3c3cff>R</color><color={0}>#</color>"
    };

    private bool VGopened;
    private int drops;
    private int batFails;
    private int wireFails;
    
    
    string divideBy1000(int num)
    {
        return num / 1000 + (num % 1000 < 10 ? ".00" : num % 1000 < 100 ? ".0" : ".") + num % 1000;
    }

    string divideBy100(int num)
    {
        return num / 100 + (num % 100 < 10 ? ".0" : ".") + num % 100;
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
        if (!start)
        {
            start = true;
            faceID = 18;
            face.sprite = faces[faceID];
            yield return new WaitForSeconds(.5f);
            StartCoroutine(dialogWrapper.PlayDialogSequence("initial"));
        }
        else yield return new WaitForSeconds(.5f);

        updateFace();
        states[0].SetActive(false);
        states[state].SetActive(true);
        yield return null;
    }

    void deleteItem(int index)
    {
        int[] peakFreqs1 = new int[peakFreqs.Length - 1];
        int[] peakAmps1 = new int[peakFreqs.Length - 1];
        int[] peakIds1 = new int[peakFreqs.Length - 1];
        for (int i = 0; i < peakFreqs.Length - 1; i++)
        {
            peakFreqs1[i] = peakFreqs[i + (i < index ? 0 : 1)];
            peakAmps1[i] = peakAmps[i + (i < index ? 0 : 1)];
            peakIds1[i] = peakIds[i + (i < index ? 0 : 1)];
        }
        peakFreqs = peakFreqs1;
        peakAmps = peakAmps1;
        peakIds = peakIds1;
        peakAmount--;
        page = 0;
    }

    void redrawPeakDisplay()
    {
        if (peakAmount > 0)
        {
            peakShow.text = "Local peak " + page + ":\n\n" + divideBy1000(peakAmps[page]) + " dB @ " +
                            divideBy1000(peakFreqs[page]) + " kHz";
            peakList.text = page + 1 + "/" + peakAmount;
            for (var i = 0; i < peakAmount; i++) graph[peakIds[i]].color = i == page ? colorArray[2] : colorArray[0];
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

    void redrawVariables()
    {
        if (AEAN < 0) AEAN = 0;
        string ans = "";
        for (int i = 0; i < 10; i++)
        {
            ans += varNames[i] + ":" + new string(' ', 24 - divideBy100(voltages[i]).Length) + divideBy100(voltages[i]) + "\n";
        }

        ans += "\n\n";
        ans += varNames[10] + ":" + new string(' ', 24 - divideBy100(AEAN).Length) + divideBy100(AEAN) + "\n";
        ans += varNames[11] + ":" + new string(' ', 24 - divideBy100(BTR).Length) + divideBy100(BTR) + "\n";
        varList.text = ans;
    }

    void setState(int newState)
    {
        states[state].SetActive(false);
        state = newState;
        states[state].SetActive(true);
    }

    void generate()
    {
        List<int> numbers = Enumerable.Range(0, 19).ToList().Shuffle();
        int amount = Random.Range(7, 13);
        int nonCrit = Random.Range(2, 6);
        for (int i = 0; i < amount; i++)
        {
            table[numbers[i]] = 1;
            if (numbers[i] < 10) peakAmount++;
        }

        for (int i = 0; i < nonCrit; i++)
        {
            table[numbers[i + amount]] = 2;
            if (numbers[i + amount] < 10) peakAmount++;
        }

        for (int i = 0; i < 10; i++)
        {
            freqs[i] = Random.Range(limits[i] + 1, limits[i + 1]);
        }

        initialStep = 20000 / (4 * amount - 5) + 1;
        peakFreqs = new int[peakAmount];
        peakAmps = new int[peakAmount];
        peakIds = new int[peakAmount];
        int sum = 0;
        int j = 0;
        for (int ii = 0; ii < 10; ii++)
        {
            if (table[ii] == 0) continue;
            int amp = table[ii] == 1 ? Random.Range(5000, 7500) : Random.Range(3500, 5000);
            peakIds[j] = (freqs[ii] - 1000) * 52 / 9000;
            if (j > 0 && peakIds[j] == peakIds[j - 1]) peakIds[j]++;
            graphInts[peakIds[j]] = amp;
            peakAmps[j] = amp;
            peakFreqs[j] = freqs[ii];
            j++;
        }

        for (int i = 0; i < 52; i++)
        {
            if (graphInts[i] == 0) graphInts[i] = Random.Range(2000, 2800);
            sum += graphInts[i];
        }

        avgAmp = sum / 52;
        redrawGraphScreen();

        for (int i = 0; i < 10; i++)
        {
            if (table[10 + i] == 0)
                voltages[i] = Random.Range(
                    (int)((i < 5 ? 0.97f : 0.9f) * nomVoltages[i] + 1),
                    (int)((i < 5 ? 1.03f : 1.1f) * nomVoltages[i])
                );
            else if (table[10 + i] == 1) voltages[i] = i < 5 ? Random.Range(0, 19) : 0;
            else
            {
                voltages[i] = Random.Range(nomVoltages[i] / 2, nomVoltages[i] * 2);
                while ((float)voltages[i] / nomVoltages[i] >= (i < 5 ? 0.97f : 0.9f) &&
                       (float)voltages[i] / nomVoltages[i] <= (i < 5 ? 1.03f : 1.1f))
                    voltages[i] = Random.Range(nomVoltages[i] / 2, nomVoltages[i] * 2);
            }

            Debug.LogFormat("[AMM-041-292 #{0}] Generated voltage of {1} is {2} V.", ModuleId, varNames[i],
                divideBy100(voltages[i]));
            // good luck with that one, you have 10^15 combinations in wire composer
        }

        redrawVariables();
    }

    void refreshQuestion()
    {
        question.text = wireComposerCircuit < 10
            ? "Which circuit needs\nto be adjusted?"
            : "How do you want\nto connect it?\n\n1 - Serial\n0 - Parallel";
        answer.text = wireComposerCircuit > -1 && wireComposerCircuit < 10 ? varNames[wireComposerCircuit] : "";
    }

    void refreshWireComposer()
    {
        for (int i = 0; i < 15; i++) wireComposerConfig[i] = -1;
        wireComposerIndex = 0;
        for (int i = 0; i < 12; i++) itemBuffer[i] = itemAmount[i];
        redrawWireComposerScreen();
    }

    void redrawWireComposerScreen()
    {
        for (int i = 0; i < 12; i++) amounts[i].text = "x" + itemBuffer[i];
        for (int i = 0; i < 15; i++) brackets[i].text = wireComposerConfig[i] == -1?
            i == wireComposerIndex ? "<color=#ecdb44ff>> ? <</color>" : "":
            string.Format(wiresText[wireComposerConfig[i]], i == wireComposerIndex ? "#ecdb44ff": "#e6dfd7ff");
    }

    void selectInWireComposer(int item)
    {
        if (item == -1)
        {
            if (wireComposerConfig[wireComposerIndex] != -1) itemBuffer[wireComposerConfig[wireComposerIndex]]++;
            wireComposerConfig[wireComposerIndex] = item;
        }
        else if (wireComposerConfig[wireComposerIndex] == item)
        {
            itemBuffer[item]++;
            wireComposerConfig[wireComposerIndex] = -1;
        }
        else if (itemBuffer[item] != 0)
        {
            itemBuffer[item]--;
            if (wireComposerConfig[wireComposerIndex] != -1) itemBuffer[wireComposerConfig[wireComposerIndex]]++;
            wireComposerConfig[wireComposerIndex] = item;
        }

        redrawWireComposerScreen();
    }

    void redrawGraphScreen()
    {
        for (int i = 0; i < 52; i++) graph[i].transform.localScale = new Vector3(1f, graphInts[i] / 7500f, 1f);
        avgText.text = "Avg: " + divideBy1000(avgAmp);
        if (avgAmp < 2800) avgJudg.text = "<color=#6ec55cff>PASS</color>";
        else if (avgAmp < 3200) avgJudg.text = "<color=#ecdb44ff>WARN</color>";
        else avgJudg.text = "<color=#cb3c3cff>FAIL</color>";
    }


    IEnumerator cycle()
    {
        while (!done)
        {
            yield return new WaitForSeconds(1f);
            faceID ^= 1;
            face.sprite = faces[faceID];
        }
    }

    void AbcdCalc()
    {
        int j = 0;
        for (int i = 0; i < 10; i++)
        {
            if (table[i] != 0)
            {
                ABCD[i] = peakFreqs[j] % 100 / 10;
                ABCD[10 + i] = peakFreqs[j] % 10;
                ABCD[20 + i] = (peakAmps[j] - avgAmp) % 100 / 10;
                ABCD[30 + i] = (peakAmps[j] - avgAmp) % 10;
                Debug.LogFormat(
                    "[AMM-041-292 #{0}] Component {1}: A={2}, B={3}, C={4}, D={5}. Peak frequency: {6}, Delta-amplitude: {7}.",
                    ModuleId, i, ABCD[i], ABCD[10 + i], ABCD[20 + i], ABCD[30 + i], peakFreqs[j], peakAmps[j] - avgAmp);
                j++;
            }
            else
            {
                ABCD[i] = -1;
                ABCD[10 + i] = -1;
                ABCD[20 + i] = -1;
                ABCD[30 + i] = -1;
                Debug.LogFormat("[AMM-041-292 #{0}] Component {1}: no need to fix.", ModuleId, i);
            }
        }
    }

    void LOCK()
    {
        Debug.LogFormat("[AMM-041-292 #{0}] AEAN is bigger than 120.0. Guess what.", ModuleId);
        done = true;
        GetComponent<KMSelectable>().OnFocus = null;
        GetComponent<KMSelectable>().OnDefocus = null;
        selected = false;
        lockAEAN.text = "AMM ran away.\n\n" + lockSentences[Random.Range(0, lockSentences.Length)] + "\n\nAEAN: " +
                        divideBy100(AEAN);
        setState(4);
    }

    

    IEnumerator SOLVE()
    {
        done = true;
        Debug.LogFormat("[AMM-041-292 #{0}] Yo, you did it! Good job!", ModuleId);
        GetComponent<KMSelectable>().OnFocus = null;
        GetComponent<KMSelectable>().OnDefocus = null;
        StopCoroutine(onFoc());
        StopCoroutine(onDef());
        setState(1);
        states[1].SetActive(false);
        states[0].SetActive(false);
        selected = false;
        yield return videoScreenPlayer.playRandomVideo();
        setState(6);
        GetComponent<KMBombModule>().HandlePass();
        ModuleSolved = true;
        yield return null;
    }

    bool check()
    {
        if (AEAN >= 12000)
        {
            LOCK();
            return false;
        }

        bool judge = true;
        for (int i = 0; i < 20; i++)
            if (table[i] == 1) return true;
            else if (table[i] == 1) judge = false;
        if ((AEAN < 2000 || judge) && (BTR > 499 || charging)) {StartCoroutine(SOLVE());
            return false;
        }
        return true;
    }

    void updateFace()
    {
        if (picked && AEAN < 9000) faceID = 22;
        else if (AEAN < 3000) faceID = 0;
        else if (AEAN < 4500) faceID = 2;
        else if (AEAN < 6000) faceID = 4;
        else if (AEAN < 8000) faceID = 6;
        else if (AEAN < 9000) faceID = 8;
        else if (AEAN < 10500) faceID = 10;
        else if (AEAN < 12000) faceID = 12;
    }

    void pass(int index, bool fromZero = false)
    {
        if (index < 10)
        {
            int ind = 0;
            for (int i = 0; i < index; i++) if (table[i] != 0) ind++;
            table[index] = 0;
            int old = graphInts[peakIds[ind]];
            graphInts[peakIds[ind]] = Random.Range(2000, 2800);
            avgAmp += (graphInts[peakIds[ind]] - old) / 52;
            deleteItem(ind);
            for (int i = 0; i < 52; i++) graph[i].color = colorArray[0];
            redrawPeakDisplay();
            redrawGraphScreen();
            AbcdCalc();
        }
        else
        {
            fromZero = table[index] == 1;
            table[index] = 0;
            for (int i = 0; i < 10; i++) itemAmount[i] = itemBuffer[i];
            redrawInventory();
            refreshWireComposer();
        }

        aeanChange(0);
        setState(1);
        updateFace();
        if (check())
        {
            switch (index)
            {
                case 0: case 1: case 2: case 5:
                    StartCoroutine(
                        dialogWrapper.playRandom(
                            new[]
                            {
                                "BScorrect1",
                                AEAN > 5500 ? "BScorrect2H" : "BScorrect2L"
                            }
                        )
                    ); break;
                case 3: case 6:
                    StartCoroutine(dialogWrapper.PlayDialogSequence("SHcorrect")); break;
                case 4: StartCoroutine(dialogWrapper.PlayDialogSequence(AEAN > 5500 ? "MEcorrectH" : "MEcorrectL"));
                    break;
                case 7: case 8: case 9: StartCoroutine(
                    dialogWrapper.playRandom(
                        new[]
                        {
                            AEAN > 5500 ? "CScorrect1H" : "CScorrect1L",
                            AEAN > 5500 ? "CScorrect2H" : "CScorrect2L"
                        }
                    )
                ); break;
                case 10: case 11:  StartCoroutine(dialogWrapper.PlayDialogSequence(fromZero?"HVOcorrect0":AEAN>5500?"HLVOcorrectH":"HLVOcorrectL", "arm")); break;
                case 12: case 13:  StartCoroutine(dialogWrapper.PlayDialogSequence(fromZero?"HVOcorrect0":AEAN>5500?"HLVOcorrectH":"HLVOcorrectL", "leg")); break;
                case 14:           StartCoroutine(dialogWrapper.PlayDialogSequence(fromZero?"HVOcorrect0":AEAN>5500?"HLVOcorrectH":"HLVOcorrectL", "tail")); break;
                case 15: case 16:  StartCoroutine(dialogWrapper.PlayDialogSequence(fromZero?"LVOcorrect0":AEAN>5500?"HLVOcorrectH":"HLVOcorrectL", "hand")); break;
                case 17: case 18:  StartCoroutine(dialogWrapper.PlayDialogSequence(fromZero?"LVOcorrect0":AEAN>5500?"HLVOcorrectH":"HLVOcorrectL", "foot")); break;
                case 19:           StartCoroutine(dialogWrapper.PlayDialogSequence(fromZero?"LVOcorrect0":AEAN>5500?"HLVOcorrectH":"HLVOcorrectL", "tail")); break;
            }
        }
    }

    void fail(int index, bool fromZero = false)
    {
        if (index > 9) refreshWireComposer();
        aeanChange(1);
        setState(1);
        updateFace();
        if (check())
        {
            if (AEAN < 10500)
            {
                switch (index)
                {
                    case 0: case 1: case 2: case 5: StartCoroutine(dialogWrapper.PlayDialogSequence("BSwrong")); break;
                    case 3: case 6:                 StartCoroutine(dialogWrapper.PlayDialogSequence("SHwrong")); break;
                    case 4:                         StartCoroutine(dialogWrapper.PlayDialogSequence("MEwrong")); break;
                    case 7: case 8: case 9:         StartCoroutine(dialogWrapper.PlayDialogSequence(AEAN > 5500 ? "CSwrongH" : "CSwrongL")); break;
                    case 10: case 11:               StartCoroutine(dialogWrapper.PlayDialogSequence(fromZero?"HLVOwrong0":"HLVOwrong", "arm")); break;
                    case 12: case 13:               StartCoroutine(dialogWrapper.PlayDialogSequence(fromZero?"HLVOwrong0":"HLVOwrong", "leg")); break;
                    case 14: case 19:               StartCoroutine(dialogWrapper.PlayDialogSequence(fromZero?"HLVOwrong0":"HLVOwrong", "tail")); break;
                    case 15: case 16:               StartCoroutine(dialogWrapper.PlayDialogSequence(fromZero?"HLVOwrong0":"HLVOwrong", "hand")); break;
                    case 17: case 18:               StartCoroutine(dialogWrapper.PlayDialogSequence(fromZero?"HLVOwrong0":"HLVOwrong", "foot")); break;
                }
            }
            else
            {
                StartCoroutine(dialogWrapper.PlayDialogSequence("themostexcitingpart"));
            }
        }
    }

    float resistance()
    {
        float ans = 0;
        for (int j = 0; j < 3; j++)
        {
            float midAns = 0;
            for (int i = 0; i < 5; i++)
                if (wireComposerConfig[j * 5 + i] != -1)
                    midAns += 1 / itemRes[wireComposerConfig[j * 5 + i]];
            if (midAns != 0) ans += 1 / midAns;
        }
        return ans;
    }

    void checkRes(bool parallel)
    {
        int index = wireComposerCircuit % 10;
        int res = table[index + 10] == 2
            ? (int)(nomResistances[index] * ((float)voltages[index] / nomVoltages[index]))
            : 0;
        int newres = parallel ? (int)(100 / (1 / resistance() + 1 / (res / 100f))) : res + (int)(resistance() * 100);
        int newVolt = (int)(nomVoltages[index] * ((float)newres / nomResistances[index]));
        Debug.LogFormat(
            "[AMM-041-292 #{0}] Resistance on Wire Composer: {1}.\nCombined resistance: {2}.\nNew voltage: {3}",
            ModuleId, resistance(), divideBy100(newres), divideBy100(newVolt));
        if ((float)newVolt / nomVoltages[index] >= (index < 5 ? 0.97f : 0.9f) &&
            (float)newVolt / nomVoltages[index] <= (index < 5 ? 1.03f : 1.1f))
        {
            Debug.LogFormat("[AMM-041-292 #{0}] Yep, that's it.", ModuleId);
            voltages[index] = newVolt;
            pass(10 + index);
        }
        else
        {
            Debug.LogFormat("[AMM-041-292 #{0}] No, that's wrong.", ModuleId);
            fail(10 + index);
        }
    }

    void generateChargeConfigs()
    {
        string colorString = "WRYGBOKNPAICJZS";

        int[] wireNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Shuffle().ToArray();
        int[] wireConfig = new List<int> { 0, 1, 2, 3, 4, 5 }.Shuffle().ToArray();
        int[] wireIndexes = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Shuffle().ToArray();
        int[] batteryNumbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }.Shuffle().ToArray();
        int[] batteryWireIndexes = new List<int> { 0, 1, 2, 3 }.Shuffle().ToArray();
        int[] wiresLocal = new List<int> { 0, 1, 2, 3 }.Shuffle().ToArray();
        bool[] batteryShuffler = new List<bool> { true, true, true, false, false, false, false }.Shuffle().ToArray();
        sourceChargeConfig = "";
        for (int i = 0; i < 6; i++)
        {
            wireConfigs[i].text = colorString[wireIndexes[i] + 5] + "          " + wireNumbers[i];
            wires[i].color = colorArray[wireIndexes[i] + 5];
        }

        int switcher = 0;
        string batteryConfigAns = "";
        for (int i = 0; i < 4; i++)
        {
            ammWires[i].color = colorArray[1 + wiresLocal[i]];
        }

        for (int i = 0; i < 7; i++)
        {
            if (batteryShuffler[i])
            {
                batteryConfigAns += colorString[batteryWireIndexes[switcher / 16] + 1].ToString();
                switcher += 16;
            }
            else
            {
                batteryConfigAns += batteryNumbers[switcher % 4].ToString();
                switcher += 1;
            }
        }

        colorblind.text = colorString[1 + wiresLocal[0]] + colorString[1 + wiresLocal[1]] +
                          "         " + colorString[1 + wiresLocal[2]] +
                          colorString[1 + wiresLocal[3]];
        batteryConfig.text = batteryConfigAns;

        for (int i = 0; i < 4; i++)
        {
            wireCode[i] = wireNumbers[wireConfig[wiresLocal[i]]];
            batteryCode[i] = batteryNumbers[Array.IndexOf(batteryWireIndexes, wiresLocal[i])];
            sourceChargeConfig += colorString[wireIndexes[wireConfig[i]] + 5].ToString();
        }
    }
    
    IEnumerator btrIncrement()
    {
        int batteryCapacity = wireCharge ? 10000 - BTR : Random.Range(7500, 8500);
        while (batteryCapacity > 0)
        {
            yield return new WaitForSeconds(1.44f);
            BTR++;
            batteryCapacity--;
            redrawVariables();
            if (BTR>9999) yield break;
        }
    }

    void redrawInventory()
    {
        string ans = "";
        for (int i = 0; i < itemNames.Length; i++) ans += itemNames[i] + ( 
            i > 3 ? 
                new string(' ', 32 - itemNames[i].Length - (i == itemNames.Length - 1 ? batteryAmount : itemAmount[i - 4]).ToString().Length - 1)
                + "x" + (i == itemNames.Length - 1 ? batteryAmount : itemAmount[i - 4])
                :"") + "\n";
        inventory.text = ans;
    }

    void chargePress(int digit)
    {
        if (digit == (wireCharge ? wireCode[chargeDigit] : batteryCode[chargeDigit]))
        {
            chargeDigit++;
            if (chargeDigit != 4) return;
            StartCoroutine(btrIncrement());
            if (wireCharge) StartCoroutine(
                dialogWrapper.playSequence(
                    new[]
                    {
                        new[] { "charge" },
                        new[] { "supplysucceded", divideBy100(BTR) },
                        new[] {AEAN>5500?"supply2H":BTR>200?"supply2LH":"supply2LL"}
                    }
                )
            );
            else StartCoroutine(dialogWrapper.playSequence(new[]{new[]{"charge"}, new[]{"batterysucceded"}}));
            setState(1);
            charging = true;
            chargeDigit = 0;
            check();
        }
        else
        {
            chargeDigit = 0;
            generateChargeConfigs();
            //StartCoroutine(dialogWrapper.PlayDialogSequence("failedconfig", sourceChargeConfig));

            if (wireCharge)
            {
                wireFails++;
                StartCoroutine(dialogWrapper.PlayDialogSequence(wireFails==1?"supplyfailed0":"supplyfailed", sourceChargeConfig));
            }
            else
            {
                batFails++;
                switch (batFails)
                {
                    case 1: StartCoroutine(dialogWrapper.PlayDialogSequence(AEAN>5500?"batteryfailed1H":"batteryfailed1L")); break;
                    case 2: StartCoroutine(dialogWrapper.PlayDialogSequence(AEAN>5500?"batteryfailed2H":"batteryfailed2L")); break;
                    case 3: StartCoroutine(dialogWrapper.PlayDialogSequence("batteryfailed3")); break;
                    default: StartCoroutine(dialogWrapper.PlayDialogSequence("batteryfailed")); break;
                }
            }
            
            setState(1);
            aeanChange(2);
            check();
            updateFace();
        }
    }

    IEnumerator search()
    {
        if (firstPickup)
        {
            firstPickup = false;
            StopCoroutine(cycle());
            faceID = 16;
            face.sprite = faces[faceID];
            setState(0);
            yield return new WaitForSeconds(.2f);
            setState(1);
            yield return new WaitForSeconds(3f);
            updateFace();
            StartCoroutine(cycle());
            StartCoroutine(dialogWrapper.PlayDialogSequence("picked"));
        }

        while (picked && distance != 0)
        {
            yield return new WaitForSeconds(0.05f);
            distance--;
        }

        if (distance == 0 && picked)
            StartCoroutine(dialogWrapper.PlayDialogSequence(
                "foundpower", 
                AEAN>5500?"Put me down, <b>please</b>.":"Could you put me down now?",
                sourceChargeConfig, 
                AEAN>5500?"Put me down now.":"Please put me down."
            ));
    }

    IEnumerator holding()
    {
        holdBool = true;
        yield return new WaitForSeconds(5f);
        if (holdBool)
        {
            StartCoroutine(dialogWrapper.PlayDialogSequence(picked ? "placed" : "pickedWarning"));
            picked = !picked;
            holdBool = false;
            StartCoroutine(search());
        }
        updateFace();
        yield return null;
    }

    IEnumerator solvablesCheck()
    {
        while (!done)
        {
            yield return new WaitForSeconds(5f);
            int solvedAmount = bombInfo.GetSolvedModuleIDs().Count;
            if (solvedAmount <= solvedModules) continue;
            for (int i = 0; i < 12; i++)
            {
                int temp = i != 5 && i != 6? 150 / (nonIgnored + 1) + Random.Range(1, 10): Random.Range(0, 4) * (solvedAmount - solvedModules);
                itemBuffer[i] += temp;
                itemAmount[i] += temp;
            }
            batteryAmount += Random.Range(0, 5) / 3 * (solvedAmount - solvedModules);
            solvedModules = solvedAmount;
            redrawInventory();
            redrawWireComposerScreen();
        }
    }
    void aeanChange(int id)
    {
        switch (id)
        {
            // 0: pass().
            case 0:
                if (dAEAN < 0) dAEAN = 1f;
                else dAEAN += dAEAN > 1.8f ? 0f : .25f;
                AEAN -= (int)(Random.Range(initialStep, (int)(1.3f * initialStep)) * dAEAN);
                break;
            // 1: fail().
            case 1:
                if (dAEAN > 0) dAEAN = -1f;
                else dAEAN -= dAEAN < -1.8f ? 0f : .25f;
                AEAN -= (int)(Random.Range(2 * initialStep, 4 * initialStep) * dAEAN);
                break;
            // 2: chargePress(). Penalty for wrong connection.
            case 2: AEAN += Random.Range(300, 600); break;
            // 3: Q release.
            case 3: AEAN += Random.Range(500, 1000); break;
        }
        redrawVariables();
    }

    void Awake() { ModuleId = ModuleIdCounter++; }

    void Start()
    {
        distance = Random.Range(150, 301) * 20;
        AEAN = Random.Range(6000, 8000);
        BTR = Random.Range(10, 1000);
        states[0].SetActive(true);
        for (int i = 1; i < 10; i++) states[i].SetActive(false);
        GetComponent<KMSelectable>().OnFocus += delegate
        {
            selected = true;
            StartCoroutine(onFoc());
        };
        GetComponent<KMSelectable>().OnDefocus += delegate
        {
            selected = false;
            StartCoroutine(onDef());
        };
        face.sprite = faces[faceID];
        generate();
        AbcdCalc();
        redrawPeakDisplay();
        StartCoroutine(cycle());
        refreshWireComposer();
        generateChargeConfigs();

        if (ignoredModules == null)
            ignoredModules = Boss.GetIgnoredModules("AMM-041-292", new[]
            {
                "14",
                "42",
                "501",
                "A>N<D",
                "AMM-041-292",
                "Bamboozling Time Keeper",
                "Black Arrows",
                "Brainf---",
                "The Board Walk",
                "Busy Beaver",
                "Don't Touch Anything",
                "Floor Lights",
                "Forget Any Color",
                "Forget Enigma",
                "Forget Ligma",
                "Forget Everything",
                "Forget Infinity",
                "Forget It Not",
                "Forget Maze Not",
                "Forget Me Later",
                "Forget Me Not",
                "Forget Perspective",
                "Forget The Colors",
                "Forget Them All",
                "Forget This",
                "Forget Us Not",
                "Iconic",
                "Keypad Directionality",
                "Kugelblitz",
                "Multitask",
                "OmegaDestroyer",
                "OmegaForest",
                "Organization",
                "Password Destroyer",
                "Purgatory",
                "Reporting Anomalies",
                "RPS Judging",
                "Security Council",
                "Shoddy Chess",
                "Simon Forgets",
                "Simon's Stages",
                "Souvenir",
                "Speech Jammer",
                "Tallordered Keys",
                "The Time Keeper",
                "Timing is Everything",
                "The Troll",
                "Turn The Key",
                "The Twin",
                "Übermodule",
                "Ultimate Custom Night",
                "The Very Annoying Button",
                "WAR",
                "Whiteout"
            });
        if (!ignoredModules.Contains("AMM-041-292"))
            ignoredModules = ignoredModules.ToList().Concat(new List<string> { "AMM-041-292" }).ToArray();

        GetComponent<KMBombModule>().OnActivate += delegate
        {
            nonIgnored = bombInfo.GetSolvableModuleNames().Where(a => !ignoredModules.Contains(a)).ToList().Count;
            for (int i = 0; i < 12; i++)
                itemAmount[i] += i != 5 && i != 6 ? 150 / (nonIgnored + 1) + Random.Range(1, 10) : Random.Range(0, 4);
            for (int i = 0; i < 12; i++) itemBuffer[i] = itemAmount[i];
            redrawInventory();
            redrawWireComposerScreen();
        };
        StartCoroutine(solvablesCheck());
    }

    int expectedLastDigit(int id)
    {
        switch (id)
        {
            case 3: return (ABCD[3] + ABCD[33]) * (ABCD[23] + ABCD[13]) % 10;
            case 6: return (ABCD[6] + ABCD[36]) * (ABCD[26] + ABCD[16]) % 10;
            case 4: return 10 * (ABCD[4] + ABCD[14]) / (ABCD[24] + ABCD[34] + 1) % 10;
            default: return -1;
        }
    }

    void ACTION(char action)
    {
        switch (action)
        {
            case 'V':
                if (!VGopened)
                {
                    VGopened = true;
                    StartCoroutine(dialogWrapper.playSequence(new[] {
                            new[] { "gv", "voltages" },
                            new[] { "gv3normal" }
                        }));
                }
                states[state].SetActive(false);
                state = state == 3 ? 1 : 3;
                states[state].SetActive(true);
                return;
            case 'G':
                if (!VGopened)
                {
                    VGopened = true;
                    StartCoroutine(dialogWrapper.playSequence(new[] {
                        new[] { "gv", "vibrodiagnostics graph" },
                        new[] { "gv3normal" }
                    }));
                }
                states[state].SetActive(false);
                state = state == 2 ? 1 : 2;
                states[state].SetActive(true);
                return;
            case 'I':
                states[state].SetActive(false);
                state = state == 5 ? 1 : 5;
                states[state].SetActive(true);
                return;
            case 'A':
                if (state == 2)
                {
                    page = page == 0 ? peakAmount - 1 : page - 1;
                    redrawPeakDisplay();
                }

                if (state == 7)
                {
                    wireComposerIndex = (wireComposerIndex + 10) % 15;
                    redrawWireComposerScreen();
                }

                return;
            case 'D':
                if (state == 2)
                {
                    page = page == peakAmount - 1 ? 0 : page + 1;
                    redrawPeakDisplay();
                }

                if (state == 7)
                {
                    wireComposerIndex = (wireComposerIndex + 5) % 15;
                    redrawWireComposerScreen();
                }

                return;
            case 'W':
                if (state == 5)
                {
                    inventoryIndex = inventoryIndex == 0 ? 16 : inventoryIndex - 1;
                    redrawIndex();
                }

                if (state == 7)
                {
                    wireComposerIndex = 5 * (wireComposerIndex / 5) + (wireComposerIndex + 4) % 5;
                    redrawWireComposerScreen();
                }

                return;
            case 'S':
                if (state == 5)
                {
                    inventoryIndex = inventoryIndex == 16 ? 0 : inventoryIndex + 1;
                    redrawIndex();
                }

                if (state == 7)
                {
                    wireComposerIndex = 5 * (wireComposerIndex / 5) + (wireComposerIndex + 1) % 5;
                    redrawWireComposerScreen();
                }

                return;
            case '0':
                if (state == 5)
                {
                    int last = (int)bombInfo.GetTime() % 10;
                    switch (inventoryIndex)
                    {
                        case 0:
                        {
                            //Screwdriver
                            Debug.LogFormat("[AMM-041-292 #{0}] Pressed on {1}, expected on {2}. {3}", ModuleId, last,
                                expectedLastDigit(4), last == expectedLastDigit(4) ? "Correct." : "Wrong.");
                            if (last == expectedLastDigit(4) && table[4] != 0)
                                pass(4);
                            else fail(4);
                            break;
                        }
                        case 1:
                        {
                            //Hammer
                            Debug.LogFormat("[AMM-041-292 #{0}] Pressed on {1}, expected on {2}. {3}", ModuleId, last,
                                expectedLastDigit(3), last == expectedLastDigit(3) ? "Correct." : "Wrong.");
                            if (last == expectedLastDigit(3) && table[3] != 0)
                                pass(3);
                            else fail(3);
                            break;
                        }
                        case 2:
                        {
                            //Compressed Air
                            Debug.LogFormat("[AMM-041-292 #{0}] Pressed on {1}, expected not on [{2},{3},{4},{5}]. {6}",
                                ModuleId,
                                last, ABCD[7], ABCD[17], ABCD[27], ABCD[37],
                                last != ABCD[7] && last != ABCD[17] && last != ABCD[27] && last != ABCD[37]
                                    ? "Correct."
                                    : "Wrong.");
                            if (last != ABCD[7] && last != ABCD[17] && last != ABCD[27] && last != ABCD[37] &&
                                table[7] != 0)
                                pass(7);
                            else fail(7);
                            break;
                        }
                        case 3:
                        {
                            //Oilcan
                            Debug.LogFormat("[AMM-041-292 #{0}] Pressed on {1}, expected on 0. {2}", ModuleId, last,
                                last == 0 ? "Correct." : "Wrong.");
                            if (last == 0 && table[0] != 0)
                                pass(0);
                            else fail(0);
                            break;
                        }
                        default: return;
                    }
                }

                else if (state == 8 && wireComposerCircuit < 10)
                {
                    wireComposerCircuit = 9;
                    refreshQuestion();
                }
                else if (state == 9)
                {
                    chargePress(0);
                }
                else if (state == 7) selectInWireComposer(-1);
                else if (state == 8 && wireComposerCircuit > 9)
                {
                    checkRes(true);
                }

                return;
            case '1':
                if (state == 5)
                {
                    int last = (int)bombInfo.GetTime() % 10;
                    switch (inventoryIndex)
                    {
                        case 1:
                        {
                            //Hammer
                            Debug.LogFormat("[AMM-041-292 #{0}] Pressed on {1}, expected on {2}. {3}", ModuleId, last,
                                expectedLastDigit(6), last == expectedLastDigit(6) ? "Correct." : "Wrong.");
                            if (last == expectedLastDigit(6) && table[6] != 0)
                                pass(6);
                            else fail(6);
                            break;
                        }
                        case 2:
                        {
                            //Compressed Air
                            Debug.LogFormat("[AMM-041-292 #{0}] Pressed on {1}, expected not on [{2},{3},{4},{5}]. {6}",
                                ModuleId,
                                last, ABCD[8], ABCD[18], ABCD[28], ABCD[38],
                                last != ABCD[8] && last != ABCD[18] && last != ABCD[28] && last != ABCD[38]
                                    ? "Correct."
                                    : "Wrong.");
                            if (last != ABCD[8] && last != ABCD[18] && last != ABCD[28] && last != ABCD[38] &&
                                table[8] != 0)
                                pass(8);
                            else fail(8);
                            break;
                        }
                        case 3:
                        {
                            //Oilcan
                            Debug.LogFormat("[AMM-041-292 #{0}] Pressed on {1}, expected on 0. {2}", ModuleId, last,
                                last == 0 ? "Correct." : "Wrong.");
                            if (last == 0 && table[1] != 0)
                                pass(1);
                            else fail(1);
                            break;
                        }
                        default: return;
                    }
                }
                else if (state == 8 && wireComposerCircuit < 10)
                {
                    wireComposerCircuit = 0;
                    refreshQuestion();
                }
                else if (state == 9)
                {
                    chargePress(1);
                }
                else if (state == 7) selectInWireComposer(0);
                else if (state == 8 && wireComposerCircuit > 9)
                {
                    checkRes(false);
                }

                return;
            case '2':
                if (state == 5)
                {
                    int last = (int)bombInfo.GetTime() % 10;
                    switch (inventoryIndex)
                    {
                        case 2:
                        {
                            //Compressed Air
                            Debug.LogFormat("[AMM-041-292 #{0}] Pressed on {1}, expected not on [{2},{3},{4},{5}]. {6}",
                                ModuleId,
                                last, ABCD[9], ABCD[19], ABCD[29], ABCD[39],
                                last != ABCD[9] && last != ABCD[19] && last != ABCD[29] && last != ABCD[39]
                                    ? "Correct."
                                    : "Wrong.");
                            if (last != ABCD[9] && last != ABCD[19] && last != ABCD[29] && last != ABCD[39] &&
                                table[9] != 0)
                                pass(9);
                            else fail(9);
                            break;
                        }
                        case 3:
                        {
                            //Oilcan
                            Debug.LogFormat("[AMM-041-292 #{0}] Pressed on {1}, expected on 0. {2}", ModuleId, last,
                                last == 0 ? "Correct." : "Wrong.");
                            if (last == 0 && table[2] != 0)
                                pass(2);
                            else fail(2);
                            break;
                        }
                        default: return;
                    }
                }
                else if (state == 8 && wireComposerCircuit < 10)
                {
                    wireComposerCircuit = 1;
                    refreshQuestion();
                }
                else if (state == 9)
                {
                    chargePress(2);
                }
                else if (state == 7) selectInWireComposer(1);

                return;

            case '3':
                if (state == 5)
                {
                    if (inventoryIndex == 3)
                    {
                        Debug.LogFormat("[AMM-041-292 #{0}] Pressed on {1}, expected on 0. {2}", ModuleId,
                            (int)bombInfo.GetTime() % 10,
                            (int)bombInfo.GetTime() % 10 == 0 ? "Correct." : "Wrong.");
                        if ((int)bombInfo.GetTime() % 10 == 0 && table[5] != 0) pass(5);
                        else fail(5);
                    }
                }
                else if (state == 8 && wireComposerCircuit < 10)
                {
                    wireComposerCircuit = 2;
                    refreshQuestion();
                }
                else if (state == 9)
                {
                    chargePress(3);
                }
                else if (state == 7) selectInWireComposer(2);
                return;
            case '4':
                if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                    if (state == 8 && wireComposerCircuit < 10)
                    {
                        wireComposerCircuit = 3;
                        refreshQuestion();
                    }
                    else if (state == 9)
                    {
                        chargePress(4);
                    }
                    else if (state == 7) selectInWireComposer(3);

                return;
            case '5':
                if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
                    if (state == 8 && wireComposerCircuit < 10)
                    {
                        wireComposerCircuit = 4;
                        refreshQuestion();
                    }
                    else if (state == 9)
                    {
                        chargePress(5);
                    }
                    else if (state == 7) selectInWireComposer(4);

                return;
            case '6':
                if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
                    if (state == 8 && wireComposerCircuit < 10)
                    {
                        wireComposerCircuit = 5;
                        refreshQuestion();
                    }
                    else if (state == 9)
                    {
                        chargePress(6);
                    }
                    else if (state == 7) selectInWireComposer(5);

                return;
            case '7':
                if (state == 8 && wireComposerCircuit < 10)
                {
                    wireComposerCircuit = 6;
                    refreshQuestion();
                }
                else if (state == 9)
                {
                    chargePress(7);
                }

                return;
            case '8':
                if (state == 8 && wireComposerCircuit < 10)
                {
                    wireComposerCircuit = 7;
                    refreshQuestion();
                }
                else if (state == 9)
                {
                    chargePress(8);
                }

                return;
            case '9':
                if (state == 8 && wireComposerCircuit < 10)
                {
                    wireComposerCircuit = 8;
                    refreshQuestion();
                }
                else if (state == 9)
                {
                    chargePress(9);
                }

                return;
            case 'E':
            case 'R':
                if (state == 5)
                {
                    if (inventoryIndex > 3 && inventoryIndex < 16) setState(7);
                    else if (inventoryIndex == 16 && !charging && batteryAmount > 0)
                    {
                        batteryAmount--;
                        redrawInventory();
                        wireCharge = false;
                        wireChargeMenu.SetActive(false);
                        batteryChargeMenu.SetActive(true);
                        setState(9);
                    }
                }
                else if (state == 7)
                {
                    if (resistance() != 0)
                    {
                        wireComposerCircuit = -1;
                        refreshQuestion();
                        setState(8);
                    }
                }
                else if (state == 8)
                {
                    if (wireComposerCircuit > -1 && wireComposerCircuit < 10)
                    {
                        if (table[10 + wireComposerCircuit] == 0) fail(10 + wireComposerCircuit);
                        else if (table[10 + wireComposerCircuit] == 1) checkRes(false);
                        else if (table[10 + wireComposerCircuit] == 2)
                        {
                            wireComposerCircuit += 10;
                            refreshQuestion();
                        }
                    }
                }

                return;
            case 'B':
                if (state == 7) setState(5);
                else if (state == 8) setState(7);
                return;
            case '!':
                selectInWireComposer(6);
                return;
            case '@':
                selectInWireComposer(7);
                return;
            case '#':
                selectInWireComposer(8);
                return;
            case '$':
                selectInWireComposer(9);
                return;
            case '%':
                selectInWireComposer(10);
                return;
            case '^':
                selectInWireComposer(11);
                return;

            case 'Q':
                if (!picked && distance == 0)
                {
                    wireCharge = true;
                    wireChargeMenu.SetActive(true);
                    batteryChargeMenu.SetActive(false);
                    setState(9);
                }
                else StartCoroutine(holding());

                return;
            case 'q':
                if (picked || distance != 0)
                {
                    holdBool = false;
                    picked = false;
                    aeanChange(3);
                    check();
                    updateFace();
                    StartCoroutine(dialogWrapper.PlayDialogSequence("drop"));
                    StopCoroutine(holding());
                    drops++;
                    switch (drops)
                    {
                        case 1: StartCoroutine(dialogWrapper.PlayDialogSequence("dropped1")); break;
                        case 2: StartCoroutine(dialogWrapper.PlayDialogSequence("dropped2")); break;
                        default: StartCoroutine(dialogWrapper.PlayDialogSequence("dropped3")); break;
                    }

                }

                return;
            case '§':
                if (!picked && distance == 0)
                {
                    wireCharge = true;
                    wireChargeMenu.SetActive(true);
                    batteryChargeMenu.SetActive(false);
                    setState(9);
                }
                else
                {
                    StartCoroutine(dialogWrapper.PlayDialogSequence(picked ? "placed" : "pickedWarning"));
                    picked = !picked;
                    StartCoroutine(search());
                    updateFace();
                }
                return;
        }
    }

    void Update()
    {
        if (!selected || dialogWrapper.dialogBusy || ModuleSolved) return;
        if (Input.GetKeyDown(KeyCode.V)) ACTION('V');
        if (Input.GetKeyDown(KeyCode.G)) ACTION('G');
        if (Input.GetKeyDown(KeyCode.I)) ACTION('I');
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) ACTION('A');
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) ACTION('D');
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) ACTION('W');
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) ACTION('S');
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) ACTION('0');
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) if (state == 7 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) ACTION('!'); else ACTION('1');
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) if (state == 7 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) ACTION('@'); else ACTION('2');
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) if (state == 7 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) ACTION('#'); else ACTION('3');
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) if (state == 7 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) ACTION('$'); else ACTION('4');
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) if (state == 7 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) ACTION('%'); else ACTION('5');
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) if (state == 7 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) ACTION('^'); else ACTION('6');
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) ACTION('7');
        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) ACTION('8');
        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)) ACTION('9');
        if (Input.GetKeyDown(KeyCode.Q) && !holdBool && !charging) ACTION('Q');
        if (Input.GetKeyUp(KeyCode.Q) && holdBool && !charging) ACTION('q');
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) ACTION('E');
        if (Input.GetKeyDown(KeyCode.Backspace)) ACTION('B');
    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage =
        @"Use !{0} notice to make AMM notice you. Use !{0} <key sequence> to input keys. To input Shift+<num> use !, @, #, $, %, ^. To input Enter/Return use E or R. To input Backspace use B. Do not put spaces between keys. Example: !{0} ISSSSEDD@E1";
#pragma warning restore 414
    public IEnumerator ProcessTwitchCommand(string Command)
    {
        yield return null;
        var commandArgs = Command.ToUpperInvariant().Trim();
        Debug.Log(commandArgs);
        if (commandArgs == "NOTICE" && !start) {
            selected = true;
            yield return onFoc();
        }
        else if (start)
        {
            if (!commandArgs.RegexMatch("([0-9VGIADWSERBQ!@#$%\\^])+")) yield return "sendtochaterror Сommand is not valid.";
            foreach (var t in commandArgs) ACTION(t == 'Q' ? '§' : t);
        }
    }

    public IEnumerator TwitchHandleForcedSolve()
    {
        yield return null;
        yield return SOLVE();
    }
}