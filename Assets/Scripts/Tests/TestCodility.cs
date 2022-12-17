using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestCodility : MonoBehaviour
{
    [SerializeField] string input;

    // Start is called before the first frame update
    void Start()
    {
        int result = solution4(new int[] { 6, 1, 4, 6, 3, 2, 7, 4 }, 3, 2);

        Debug.Log(result);

        result = solution4(new int[] { 16, 11, 4, 6, 3, 2, 7, 4 }, 3, 2);

        Debug.Log(result);
    }

    public int solution4(int[] A, int K, int L)
    {
        // Check if they can pick-up in two segments
        if (K + L > A.Length) return -1;
        // Only one possibility
        if (L + L == A.Length) return A.Sum();

        // Get all possibilities of harvest for K and L
        Dictionary<int, int> possibilitiesK = FindPossibilities(A, K);
        Dictionary<int, int> possibilitiesL = FindPossibilities(A, L);

        // Init futures values
        int bestIndexK = 0;
        int bestIndexL = 0;
        int maxNumberApples = 0;
        int numberApples = 0;

        // Now check all possibilities
        // focus on correctness not speed and from far...
        foreach (KeyValuePair<int, int> pairK in possibilitiesK)
        {
            foreach (KeyValuePair<int, int> pairL in possibilitiesL)
            {
                numberApples = pairK.Value + pairL.Value;

                // Check if it's a better choice and if they don't harvest on the same tree
                if (numberApples > maxNumberApples && AreIndexesCompatibles(pairK.Key, K, pairL.Key, L))
                {
                    // Assign new values
                    bestIndexK = pairK.Key;
                    bestIndexL = pairL.Key;
                    maxNumberApples = numberApples;
                }
            }
        }

        // return the final max number of apples we can collect
        return maxNumberApples;
    }

    // Find all possibilities for the person to collect
    private Dictionary<int, int> FindPossibilities(int[] A, int N)
    {
        // The dictionary contains the index and the number of apples you collect from it
        Dictionary<int, int> possibilities = new Dictionary<int, int>();

        for (int i = 0; i < A.Length - N + 1; i++) possibilities.Add(i, SumInArray(A, i, N));

        return possibilities;
    }

    // Sum in an array from the index start with the number of items we want
    private int SumInArray(int[] A, int start, int N)
    {
        int sum = 0;

        for (int i = start; i < start + N; i++) sum += A[i];

        return sum;
    }

    // Check if the index choice we found are compatibles between each other
    private bool AreIndexesCompatibles(int indexK, int K, int indexL, int L)
    {
        // Is the end of one the collection is outside another ?
        return (indexK > indexL + L || indexL > indexK + K);
    }

    public int solution3(string S)
    {
        // Convert to char[] to change it later
        char[] plots = S.ToCharArray();
        int numberOfTanks = 0;
        // If left or right have either a tank of nothing (is empty)
        bool canReceiveWater = false;
        // If left has a tank
        bool alreadyReceiveWater = false;
        // Plot == \0 if doesn't exist
        // The plot on the left
        char previousPlot = '\0';
        // The plot on the right
        char nextPlot = '\0';

        for (int i = 0; i < plots.Length; i++)
        {
            // Only check houses
            if (plots[i] != 'H') continue;

            canReceiveWater = false;
            alreadyReceiveWater = false;
            previousPlot = i > 0 ? plots[i - 1] : '\0';
            nextPlot = i < plots.Length - 1 ? plots[i + 1] : '\0';

            if (previousPlot != '\0' && previousPlot != 'H')
            {
                canReceiveWater = true;

                // There is a tank on the left
                if (previousPlot == 'T') alreadyReceiveWater = true;
            }
            if (nextPlot != '\0' && nextPlot != 'H')
            {
                canReceiveWater = true;

                // Doesn't already received water from the left but can receive water from the right
                // Place a tank there then
                if (!alreadyReceiveWater)
                {
                    plots[i + 1] = 'T';
                }
            }

            // Error that house cannot receive water
            if (!canReceiveWater) return -1;
            if (!alreadyReceiveWater) numberOfTanks++;
        }

        return numberOfTanks;
    }

    public int solution(int[] A)
    {
        if (!A.Any(n => n > 1)) return 1;

        A = A.Distinct().ToArray();
        Array.Sort(A);

        for (int i = 0; i < A.Length - 1; i++)
        {
            if (A[i + 1] != A[i] + 1)
            {
                return A[i] + 1;
            }
        }

        return A[A.Length - 1] + 1;
    }

    public string solution2(string message, int K)
    {
        // Check first if message is already correct
        if (message.Length <= K) return message;

        // Split to compare words and not the complete string
        string[] words = message.Split(" ");
        string croppedMessage = "";

        // Take in account the dots
        int totalSize = 3;

        // Check each word
        foreach (string word in words)
        {
            // Actual size + word size and space size after the word
            totalSize += word.Length + 1;

            // Check if size is correct
            if (totalSize > K) break;

            // Add the word and a space just after
            croppedMessage += word + " ";
        }

        // Return message with dots
        return croppedMessage + "...";
    }
}
