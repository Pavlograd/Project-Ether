using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carrousel : MonoBehaviour
{
    [Header("Number elements to show and to swipe")]
    [SerializeField] int nbrElementsToShow = 1;
    [SerializeField] int nbrElementsToSwipe = 1;
    [Header("The arrow to swipe in right direction, automatic rotation for left")]
    [SerializeField] Sprite buttonSprite;
    [Header("Strict values do not change")]
    [SerializeField] GameObject positionsGO;
    [SerializeField] GameObject content;
    [SerializeField] Transform leftElementsGO;
    [SerializeField] Transform centerElementsGO;
    [SerializeField] Transform rightElementsGO;
    [SerializeField] Button leftSwipe;
    [SerializeField] Button rightSwipe;
    List<Transform> elementsPositions = new List<Transform>();
    List<Transform> leftElements = new List<Transform>();
    List<Transform> centerElements = new List<Transform>();
    List<Transform> rightElements = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        leftSwipe.image.sprite = buttonSprite;
        rightSwipe.image.sprite = buttonSprite;

        // Get all positions of futur elements
        for (int i = 0; i < positionsGO.transform.childCount; i++)
            elementsPositions.Add(positionsGO.transform.GetChild(i));

        // Get all elements
        for (int i = 0; i < content.transform.childCount; i++)
        {
            leftElements.Add(content.transform.GetChild(i));
            // hide the object
            content.transform.GetChild(i).gameObject.SetActive(false);
        }

        // Simulate a first swipe to put all necessary object
        int tmpSwipe = nbrElementsToSwipe;
        nbrElementsToSwipe = nbrElementsToShow;

        Swipe(1);

        nbrElementsToSwipe = tmpSwipe;
    }

    public void Swipe(int swipeDirection)
    {
        List<Transform> trash = swipeDirection == 1 ? rightElements : leftElements;
        List<Transform> pickUp = swipeDirection == 1 ? leftElements : rightElements;

        // Put elements which will be hidden to right
        for (int i = 0; i < nbrElementsToSwipe; i++)
        {
            if (centerElements.Count > 0)
            {
                Transform element = centerElements[centerElements.Count - 1];

                element.gameObject.SetActive(false);
                trash.Add(element);
                centerElements.Remove(element);
            }
        }

        // Move elements to right
        for (int i = 0; i < centerElements.Count; i++)
        {
            //centerElements[i].transform.position = elementsPositions[elementsPositions.Count - 1 - i].position;
        }

        // Put elements which will be shown
        for (int i = 0; i < nbrElementsToSwipe; i++)
        {
            if (pickUp.Count > 0)
            {
                Transform element = pickUp[0];

                //element.transform.position = elementsPositions[i].position;
                element.gameObject.SetActive(true);
                centerElements.Add(element);
                pickUp.Remove(element);
            }
        }

        ToggleArrow(leftSwipe, rightElements.Count == 0 ? false : true);
        ToggleArrow(rightSwipe, leftElements.Count == 0 ? false : true);
    }

    void ToggleArrow(Button button, bool toggle)
    {
        button.interactable = toggle;
    }
}
