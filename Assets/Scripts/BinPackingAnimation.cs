using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BinPackingAnimation : MonoBehaviour
{
    public List<float> testValues;

    public GameObject binPrefab;
    public GameObject binFillPrefab;
    public Transform binHolder;

    public GameObject inputItemPrefab;
    public Transform inputList;

    public Toggle sortDecreasingToggle;

    public Text displayText;

    public Color colorA;
    public Color colorB;

    Coroutine runningCo;

    private void Awake()
    {
        UpdateList();
    }

    private void ClearBins()
    {
        displayText.text = "";
        while (binHolder.childCount > 0)
        {
            DestroyImmediate(binHolder.GetChild(0).gameObject);
        }
    }

    private void ClearInputList()
    {
        while (inputList.childCount > 0)
        {
            DestroyImmediate(inputList.GetChild(0).gameObject);
        }
    }

    public void UpdateList()
    {
        ClearInputList();

        for (int i = 0; i < testValues.Count; i++)
        {
            GameObject g = Instantiate(inputItemPrefab, inputList);
            g.GetComponentInChildren<Text>().text = string.Format("{0}", testValues[i]);

            int remIndex = i;
            g.GetComponentInChildren<Button>().onClick.AddListener(
            () => {
                testValues.RemoveAt(remIndex);
                UpdateList();
            });
        }
    }

    public void NextFitAnimation()
    {
        if (runningCo != null)
        {
            StopCoroutine(runningCo);
        }

        ClearBins();

        runningCo = StartCoroutine(NextFitCo());
    }

    public IEnumerator NextFitCo()
    {
        List<float> values = new List<float>(testValues);
        if (sortDecreasingToggle.isOn)
        {
            values.Sort();
            values.Reverse();
        }

        List<Bin> bins = new List<Bin>();
        //List<Slider> binSliders = new List<Slider>();
        List<BinFillAnimation> binFills = new List<BinFillAnimation>();

        foreach (float item in values)
        {
            displayText.text = string.Format("Next item: {0}", item);
            yield return new WaitForSeconds(0.5f);
            displayText.text = "";

            bool packed = false;

            if (bins.Count > 0)
            {
                float oldCapacity = bins[bins.Count - 1].capacity;
                packed = bins[bins.Count - 1].Add(item);
                //binSliders[bins.Count - 1].value = 1f - bins[bins.Count - 1].capacity;
                if (packed)
                    binFills[bins.Count - 1].AddFill(Color.Lerp(colorA, colorB, oldCapacity-item), 100f*((1f - oldCapacity) + item / 2), 100f*item);
            }

            if (!packed)
            {
                Bin b = new Bin();
                //GameObject g = Instantiate(binPrefab, binHolder);
                //Slider s = g.GetComponent<Slider>();
                GameObject g = Instantiate(binFillPrefab, binHolder);
                BinFillAnimation s = g.GetComponent<BinFillAnimation>();

                if (b.CanFit(item))
                {
                    b.Add(item);
                    //s.value = 1f - b.capacity;
                    s.AddFill(Color.Lerp(colorA, colorB, b.capacity), 100f*(item / 2), 100f*item);
                }

                bins.Add(b);
                //binSliders.Add(s);
                binFills.Add(s);

                binFills[bins.Count - 1].Highlight();
                if (bins.Count > 1) { binFills[bins.Count - 2].UnHighlight(); }
            }

            yield return new WaitForSeconds(0.3f);
        }

        if (binFills.Count > 0) { binFills[bins.Count - 1].UnHighlight(); }
        yield return null;
    }

    public void FirstFitAnimation()
    {
        if (runningCo != null)
        {
            StopCoroutine(runningCo);
        }

        ClearBins();

        runningCo = StartCoroutine(FirstFitCo());
    }

    public IEnumerator FirstFitCo()
    {
        List<float> values = new List<float>(testValues);
        if (sortDecreasingToggle.isOn)
        {
            values.Sort();
            values.Reverse();
        }

        List<Bin> bins = new List<Bin>();
        //List<Slider> binSliders = new List<Slider>();
        List<BinFillAnimation> binFills = new List<BinFillAnimation>();

        foreach (float item in values)
        {
            displayText.text = string.Format("Inserting: {0}", item);
            yield return new WaitForSeconds(0.3f);

            bool packed = false;
            foreach (Bin bin in bins)
            {
                binFills[bins.IndexOf(bin)].Highlight();
                if (bin.CanFit(item))
                {
                    float oldCapacity = bin.capacity;
                    packed = bin.Add(item);
                    //binSliders[bins.IndexOf(bin)].value = 1f - bins[bins.IndexOf(bin)].capacity;
                    if (packed)
                        binFills[bins.IndexOf(bin)].AddFill(Color.Lerp(colorA, colorB, oldCapacity-item), 100f * ((1f - oldCapacity) + item / 2), 100f * item);

                    yield return new WaitForSeconds(0.3f);
                    binFills[bins.IndexOf(bin)].UnHighlight();
                    displayText.text = "";
                    break;
                }

                yield return new WaitForSeconds(0.3f);
                binFills[bins.IndexOf(bin)].UnHighlight();
            }
            if (!packed)
            {
                Bin b = new Bin();
                //GameObject g = Instantiate(binPrefab, binHolder);
                //Slider s = g.GetComponent<Slider>();
                GameObject g = Instantiate(binFillPrefab, binHolder);
                BinFillAnimation s = g.GetComponent<BinFillAnimation>();

                if (b.CanFit(item))
                {
                    b.Add(item);
                    //s.value = 1f - b.capacity;
                    s.AddFill(Color.Lerp(colorA, colorB, b.capacity), 100f * (item / 2), 100f * item);
                }

                bins.Add(b);
                //binSliders.Add(s);
                binFills.Add(s);
            }

            displayText.text = "";
            yield return new WaitForSeconds(0.3f);
        }

        yield return null;
    }

    public void BestFitAnimation()
    {
        if (runningCo != null)
        {
            StopCoroutine(runningCo);
        }

        ClearBins();

        runningCo = StartCoroutine(BestFitCo());
    }

    public IEnumerator BestFitCo()
    {
        List<float> values = new List<float>(testValues);
        if (sortDecreasingToggle.isOn)
        {
            values.Sort();
            values.Reverse();
        }

        List<Bin> bins = new List<Bin>();
        //List<Slider> binSliders = new List<Slider>();
        List<BinFillAnimation> binFills = new List<BinFillAnimation>();

        foreach (float item in values)
        {
            displayText.text = string.Format("Next item: {0}", item);
            yield return new WaitForSeconds(0.5f);

            bool packed = false;

            int binIndex = 0;
            float smallestCapacity = 1f;

            for (int i = 0; i < bins.Count; i++)
            {
                binFills[i].Highlight();

                if (bins[i].CanFit(item) && bins[i].capacity <= smallestCapacity)
                {
                    binIndex = i;
                    smallestCapacity = bins[i].capacity;
                }

                yield return new WaitForSeconds(0.3f);
                binFills[i].UnHighlight();
            }

            if (bins.Count > 0)
            {
                float oldCapacity = bins[binIndex].capacity;
                packed = bins[binIndex].Add(item);
                //binSliders[binIndex].value = 1f - bins[binIndex].capacity;
                if (packed)
                    binFills[binIndex].AddFill(Color.Lerp(colorA, colorB, oldCapacity-item), 100f * ((1f - oldCapacity) + item / 2), 100f * item);
            }
            else
            {
                packed = false;
            }

            if (!packed)
            {
                Bin b = new Bin();
                //GameObject g = Instantiate(binPrefab, binHolder);
                //Slider s = g.GetComponent<Slider>();
                GameObject g = Instantiate(binFillPrefab, binHolder);
                BinFillAnimation s = g.GetComponent<BinFillAnimation>();

                if (b.CanFit(item))
                {
                    b.Add(item);
                    //s.value = 1f - b.capacity;
                    s.AddFill(Color.Lerp(colorA, colorB, b.capacity), 100f * (item / 2), 100f * item);
                }

                bins.Add(b);
                //binSliders.Add(s);
                binFills.Add(s);
            }

            displayText.text = "";
            yield return new WaitForSeconds(0.3f);
        }

        yield return null;
    }

    public void WorstFitAnimation()
    {
        if (runningCo != null)
        {
            StopCoroutine(runningCo);
        }

        ClearBins();

        runningCo = StartCoroutine(WorstFitCo());
    }

    public IEnumerator WorstFitCo()
    {
        List<float> values = new List<float>(testValues);
        if (sortDecreasingToggle.isOn)
        {
            values.Sort();
            values.Reverse();
        }

        List<Bin> bins = new List<Bin>();
        //List<Slider> binSliders = new List<Slider>();
        List<BinFillAnimation> binFills = new List<BinFillAnimation>();

        foreach (float item in values)
        {
            displayText.text = string.Format("Next item: {0}", item);
            yield return new WaitForSeconds(0.5f);

            bool packed = false;

            int binIndex = 0;
            float largestCapacity = 0f;

            for (int i = 0; i < bins.Count; i++)
            {
                binFills[i].Highlight();

                if (bins[i].CanFit(item) && bins[i].capacity >= largestCapacity)
                {
                    binIndex = i;
                    largestCapacity = bins[i].capacity;
                }

                yield return new WaitForSeconds(0.3f);
                binFills[i].UnHighlight();
            }

            if (bins.Count > 0)
            {
                float oldCapacity = bins[binIndex].capacity;
                packed = bins[binIndex].Add(item);
                //binSliders[binIndex].value = 1f - bins[binIndex].capacity;
                if (packed)
                    binFills[binIndex].AddFill(Color.Lerp(colorA, colorB, oldCapacity-item), 100f * ((1f - oldCapacity) + item / 2), 100f * item);
            }
            else
            {
                packed = false;
            }

            if (!packed)
            {
                Bin b = new Bin();
                //GameObject g = Instantiate(binPrefab, binHolder);
                //Slider s = g.GetComponent<Slider>();
                GameObject g = Instantiate(binFillPrefab, binHolder);
                BinFillAnimation s = g.GetComponent<BinFillAnimation>();

                if (b.CanFit(item))
                {
                    b.Add(item);
                    //s.value = 1f - b.capacity;
                    s.AddFill(Color.Lerp(colorA, colorB, b.capacity), 100f * (item / 2), 100f * item);
                }

                bins.Add(b);
                //binSliders.Add(s);
                binFills.Add(s);
            }

            displayText.text = "";
            yield return new WaitForSeconds(0.3f);
        }

        yield return null;
    }

    public void TestAlgorithm(int test)
    {
        switch (test)
        {
            case 1:
                NextFitAnimation();
                break;
            case 2:
                FirstFitAnimation();
                break;
            case 3:
                BestFitAnimation();
                break;
            case 4:
                WorstFitAnimation();
                break;
            default:
                if (runningCo != null)
                    StopCoroutine(runningCo);
                ClearBins();
                break;
        }
    }

}