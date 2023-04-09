using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public enum CollectableItemType
{
    Coin = 0,
    Mine = 1,
}

public class CollectableItem : MonoBehaviour
{
    public CollectableItemType itemType;
    public string otherColliderName = "player";
    public  int damageValue = 0;

    public bool disableAfterCollision = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Contact with Object");
        if (other.gameObject.name.Equals(otherColliderName, StringComparison.Ordinal))
        {
            Debug.Log("Test");
            GameManager.getInstance.ColliderWithSomeThing(itemType, damageValue);
            // should I add a pool here?
            PlayCollisionEffect();
            if (disableAfterCollision)
            {
                Destroy(this.gameObject);
            }
        }
        
    }

    /// <summary>
    /// 这个不确定要不要用继承写， 因为可能不同的东西播放的效果不一样？
    /// 或者统一写，只是有些空播放的，性能不好但是很general
    /// </summary>
    public void PlayCollisionEffect()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
    }
}
