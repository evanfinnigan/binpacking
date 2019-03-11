using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public List<float> testValues;

    public void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            TestAlgorithm(i);
        }
    }

    public static void printBins(string algorithm, List<Bin> bins)
    {
        Debug.LogFormat("Using Algorithm: {0}", algorithm);

        for (int i = 0; i < bins.Count; i++)
        {
            string s = "";
            for (int j = 0; j < bins[i].items.Count; j++)
            {
                s += bins[i].items[j];
                if (j < bins[i].items.Count - 1)
                {
                    s += ", ";
                }
            }

            Debug.LogFormat("Bin {0}: {1}", i, s);
        }
    }

    public void NextFit()
    {
        List<Bin> nextFitBins = BinPacking.NextFit(testValues);
        printBins("Next Fit", nextFitBins);
    }

    public void FirstFit()
    {
        List<Bin> firstFitBins = BinPacking.FirstFit(testValues);
        printBins("First Fit", firstFitBins);
    }

    public void BestFit()
    {
        List<Bin> bestFitBins = BinPacking.BestFit(testValues);
        printBins("Best Fit", bestFitBins);
    }
	
    public void WorstFit()
    {
        List<Bin> worstFitBins = BinPacking.WorstFit(testValues);
        printBins("Worst Fit", worstFitBins);
    }

    public void TestAlgorithm(int test)
    {
        switch (test)
        {
            case 1:
                NextFit();
                break;
            case 2:
                FirstFit();
                break;
            case 3:
                BestFit();
                break;
            case 4:
                WorstFit();
                break;
            default:
                break;
        }
    }

}
