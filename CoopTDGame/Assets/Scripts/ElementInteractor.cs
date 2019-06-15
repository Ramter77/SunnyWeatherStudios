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

    private Vector3 otherPos = Vector3.zero;

    private void OnTriggerEnter(Collider other)
    {
        if (allowInteraction) {
            if (other.gameObject.tag == projectileTag)
            {
                if (other.GetComponent<ElementInteractor>())
                {
                    Element otherProjectileElement;
                    otherProjectileElement = other.GetComponent<ElementInteractor>().elementType;
                    Debug.Log(other.gameObject.name + other.GetComponent<ElementInteractor>().elementType);
                    if (otherProjectileElement != elementType)
                    {
                        otherPos = other.transform.position;
                        ElementManager.Instance.requestingInteractor = this.gameObject;
                        ElementManager.Instance.combineElement(elementType, otherProjectileElement);
                        allowInteraction = false;
                        if (other.transform.parent)
                        {
                            Destroy(other.transform.parent.gameObject);
                        }
                        else
                        {
                            Destroy(other.gameObject);
                        }
                        /* if (transform.parent)
                        {
                            Destroy(transform.parent.gameObject);
                        }
                        else
                        {
                            Destroy(gameObject);
                        } */
                    }
                }
            }
        }
    }

    public void createReaction(GameObject reactionPrefab)
    {
        Vector3 pos = Vector3.Lerp(transform.position, otherPos, .5f);
        Instantiate(reactionPrefab, pos, Quaternion.identity);
    }

}
