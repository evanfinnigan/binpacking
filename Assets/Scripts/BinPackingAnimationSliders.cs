using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BinPackingAnimationSliders : MonoBehaviour
{
    public List<float> testValues;

    public GameObject binPrefab;
    public Transform binHolder;

    Coroutine runningCo;

    private void ClearBins()
    {
        while (binHolder.childCount > 0)
        {
            DestroyImmediate(binHolder.GetChild(0).gameObject);
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

        List<Bin> bins = new List<Bin>();
        List<Slider> binSliders = new List<Slider>();

        foreach (float item in values)
        {

            bool packed = false;

            if (bins.Count > 0)
            {
                float oldCapacity = bins[bins.Count - 1].capacity;
                packed = bins[bins.Count - 1].Add(item);
                if (packed)
                    binSliders[bins.Count - 1].value = 1f - bins[bins.Count - 1].capacity;
            }

            if (!packed)
            {
                Bin b = new Bin();
                GameObject g = Instantiate(binPrefab, binHolder);
                Slider s = g.GetComponent<Slider>();

                if (b.CanFit(item))
                {
                    b.Add(item);
                    s.value = 1f - b.capacity;
                }

                bins.Add(b);
                binSliders.Add(s);
            }

            yield return new WaitForSeconds(0.3f);
        }

        yield return null;
    }

    /*
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
            displayText.text = string.Format("Next item: {0}", item);
            yield return new WaitForSeconds(0.5f);
            displayText.text = "";

            bool packed = false;
            foreach (Bin bin in bins)
            {
                if (bin.CanFit(item))
                {
                    float oldCapacity = bin.capacity;
                    packed = bin.Add(item);
                    //binSliders[bins.IndexOf(bin)].value = 1f - bins[bins.IndexOf(bin)].capacity;
                    if (packed)
                        binFills[bins.IndexOf(bin)].AddFill(Color.Lerp(colorA, colorB, oldCapacity-item), 100f * ((1f - oldCapacity) + item / 2), 100f * item);
                    break;
                }
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
            displayText.text = "";

            bool packed = false;

            int binIndex = 0;
            float smallestCapacity = 1f;

            for (int i = 0; i < bins.Count; i++)
            {
                if (bins[i].CanFit(item) && bins[i].capacity <= smallestCapacity)
                {
                    binIndex = i;
                    smallestCapacity = bins[i].capacity;
                }
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
            displayText.text = "";

            bool packed = false;

            int binIndex = 0;
            float largestCapacity = 0f;

            for (int i = 0; i < bins.Count; i++)
            {
                if (bins[i].CanFit(item) && bins[i].capacity >= largestCapacity)
                {
                    binIndex = i;
                    largestCapacity = bins[i].capacity;
                }
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

            yield return new WaitForSeconds(0.3f);
        }

        yield return null;
    }

    */

    public void TestAlgorithm(int test)
    {
        switch (test)
        {
            case 1:
                NextFitAnimation();
                break;
            default:
                if (runningCo != null)
                    StopCoroutine(runningCo);
                ClearBins();
                break;
        }
    }

}