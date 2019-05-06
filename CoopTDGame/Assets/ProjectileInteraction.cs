using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileInteraction : MonoBehaviour
{
    public enum Element { Fire, Ice };
    [Header("Element - Settings")]
    public Element elementType;

    [Tooltip("Tag used for the projectiles of players")]
    [SerializeField] private string projectileTag = "ElementProjectile";

    [Header("Interaction Settings")]
    [Tooltip("indicates if the prjectile is ready to interact with another one")]
    public bool allowInteraction = false;

    private void Start()
    {
        allowInteraction = true;    
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == projectileTag)
        {
            if(other.GetComponent<ProjectileInteraction>())
            {
                Element otherProjectileElement;
                otherProjectileElement = other.GetComponent<ProjectileInteraction>().elementType;
                Debug.Log(other.gameObject.name + other.GetComponent<ProjectileInteraction>().elementType);
                if(otherProjectileElement != elementType && allowInteraction)
                {
                    InteractionManager(elementType , otherProjectileElement);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    Element InteractionManager(Element myElement, Element otherElement)
    {
        if((myElement == Element.Fire && otherElement == Element.Ice) || (myElement == Element.Ice && otherElement == Element.Fire))
        {
            /// make the interaction happen
            Debug.Log("combine elements into blast");
            // allow no further interaction 
            allowInteraction = false;
        } 
        return myElement;
    }
}
