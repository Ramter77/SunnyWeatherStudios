using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Element { NoElement, Fire, Ice, Blast };
public enum CombinationElement { NoCombination, Blast };
public class ElementManager : Singleton<ElementManager>
{
    [Header("Reaction Settings")]
    [Tooltip("List of all effects that can be created by combining elements")]
    public GameObject[] reactionPrefabs;

    [Tooltip("the GameObject that triggers the interaction between elements")]
    public GameObject requestingInteractor = null;

    [Tooltip("Product of the reaction")]
    public CombinationElement productElement;

    [Tooltip("Used to pick the corrosponding reaction element")]
    [SerializeField] private int reactionIndex = 0;

    public void combineElement(Element firstElement, Element secondElement)
    {
        if ((firstElement == Element.Fire && secondElement == Element.Ice) || (firstElement == Element.Ice && secondElement == Element.Fire))
        {
            productElement = CombinationElement.Blast;
            reactionIndex = 0;
            startReaction();
        }
    }

    void startReaction()
    {
        if(requestingInteractor != null)
        {
            GameObject effectPrefab;
            effectPrefab = reactionPrefabs[reactionIndex];
            requestingInteractor.GetComponent<ElementInteractor>().createReaction(effectPrefab);
            requestingInteractor = null;
        }
    }  
}
