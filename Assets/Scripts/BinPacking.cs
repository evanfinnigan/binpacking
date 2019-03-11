using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BinPacking  {

    public static List<Bin> NextFit(List<float> S)
    {
        List<Bin> bins = new List<Bin>();

        foreach (float item in S)
        {
            bool packed = false;

            if (bins.Count > 0)
            {
                packed = bins[bins.Count - 1].Add(item);
            }
            
            if (!packed)
            {
                Bin b = new Bin();
                if (b.CanFit(item))
                {
                    b.Add(item);
                }
                bins.Add(b);
            }
        }
        return bins;
    }

    public static List<Bin> FirstFit(List<float> S)
    {
        List<Bin> bins = new List<Bin>();

        foreach (float item in S)
        {
            bool packed = false;
            foreach (Bin bin in bins)
            {
                if (bin.CanFit(item))
                {
                    packed = bin.Add(item);
                    break;
                }
            }
            if (!packed)
            {
                Bin b = new Bin();
                if (b.CanFit(item))
                {
                    b.Add(item);
                }
                bins.Add(b);
            }
        }
        return bins;
    }

    public static List<Bin> BestFit(List<float> S)
    {
        List<Bin> bins = new List<Bin>();

        foreach (float item in S)
        {
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
                packed = bins[binIndex].Add(item);
            }

            if (!packed)
            {
                Bin b = new Bin();
                if (b.CanFit(item))
                {
                    b.Add(item);
                }
                bins.Add(b);
            }
        }
        return bins;
    }

    public static List<Bin> WorstFit(List<float> S)
    {
        List<Bin> bins = new List<Bin>();

        foreach (float item in S)
        {
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
                packed = bins[binIndex].Add(item);
            }

            if (!packed)
            {
                Bin b = new Bin();
                if (b.CanFit(item))
                {
                    b.Add(item);
                }
                bins.Add(b);
            }
        }
        return bins;
    }

}
