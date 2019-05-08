using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementInteractor : MonoBehaviour
{
    [Header("Element - Settings")]
    public Element elementType;

    [Tooltip("Tag used for the projectiles of players")]
    [SerializeField] private string projectileTag = "ElementProjectile";

    [Header("Interaction Settings")]
    [Tooltip("indicates if the prjectile is ready to interact with another one")]
    public bool allowInteraction = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == projectileTag)
        {
            if (other.GetComponent<ElementInteractor>())
            {
                if(allowInteraction)
                {
                    Element otherProjectileElement;
                    otherProjectileElement = other.GetComponent<ElementInteractor>().elementType;
                    Debug.Log(other.gameObject.name + other.GetComponent<ElementInteractor>().elementType);
                    if (otherProjectileElement != elementType)
                    {
                        ElementManager.Instance.requestingInteractor = this.gameObject;
                        ElementManager.Instance.combineElement(elementType, otherProjectileElement);
                        allowInteraction = false;
                    }
                }
            }
        }
    }

    public void createReaction(GameObject reactionPrefab)
    {
        Instantiate(reactionPrefab, transform.position, Quaternion.identity);
    }

}
