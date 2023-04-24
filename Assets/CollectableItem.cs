using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
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
    public Animator animator;
    public AudioClip soundEffect;

    public bool isTriggerred = false;

    public bool disableAfterCollision = true;
    public float delayTime = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        isTriggerred = false;
    }

    private void OnEnable()
    {
        Debug.Log("Coin Enabled!");
        animator = GetComponent<Animator>();
        if (animator) 
        {
            animator.Play("Base Layer.OrangeIdle");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggerred) return;
        isTriggerred = true;
        Debug.Log("Contact with Object");
        Debug.Log(other.gameObject.tag);
        Debug.Log(otherColliderName);
        if (other.gameObject.tag.Equals(otherColliderName, StringComparison.Ordinal))
        {
            Debug.Log(itemType.ToString());
            if(animator)animator.SetTrigger("Disapear");
            GameManager.getInstance.ColliderWithSomeThing(itemType, damageValue);
            // should I add a pool here?
            PlayCollisionEffect();
            if (disableAfterCollision)
            {
                StartCoroutine(DelayDestroy(delayTime));
               
            }
        }
        
    }

    IEnumerator DelayDestroy(float time) 
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 这个不确定要不要用继承写， 因为可能不同的东西播放的效果不一样？
    /// 或者统一写，只是有些空播放的，性能不好但是很general
    /// </summary>
    public void PlayCollisionEffect()
    {
        var Go = GameObject.Find("OtherSound");
        if (!Go) return;
        AudioSource tempSource = Go.GetComponent<AudioSource>();
        tempSource.clip = soundEffect;
        tempSource.loop = false;
        tempSource.Play();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
    }
}
