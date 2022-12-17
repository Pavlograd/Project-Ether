using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintDebug : MonoBehaviour
{
    enum Student
    {
        MAURIN,
        VICTOR,
        ANTOINE,
        ANTONY,
        TRISTAN,
        CHRISTOPHE,
        ALL,
        NONE
    }

    static Student user = Student.NONE; // To replace with your name

    public static void Maurin(object message)
    {
        if (user == Student.MAURIN || user == Student.ALL)
            Debug.Log(message);
    }

    public static void Victor(object message)
    {
        if (user == Student.VICTOR || user == Student.ALL)
            Debug.Log(message);
    }

    public static void Antoine(object message)
    {
        if (user == Student.ANTOINE || user == Student.ALL)
            Debug.Log(message);
    }

    public static void Antony(object message)
    {
        if (user == Student.ANTONY || user == Student.ALL)
            Debug.Log(message);
    }

    public static void Tristan(object message)
    {
        if (user == Student.TRISTAN || user == Student.ALL)
            Debug.Log(message);
    }

    public static void Christophe(object message)
    {
        if (user == Student.CHRISTOPHE || user == Student.ALL)
            Debug.Log(message);
    }

    public static void All(object message) // For everyone
    {
        if (user != Student.NONE)
            Debug.Log(message);
    }
}
