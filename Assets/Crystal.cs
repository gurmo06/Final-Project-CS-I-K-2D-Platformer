using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    Collider2D crystalCollider;
    SpriteRenderer crystalRenderer;
    Transform crystalTransform;
    public LayerMask playerLayer;
    public float checkPlayerRadius;
    public int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        crystalCollider = GetComponent<Collider2D>();
        crystalRenderer = GetComponent<SpriteRenderer>();
        crystalTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayer();
    }

    void CheckPlayer()
    {
        Collider2D collider = Physics2D.OverlapCircle(crystalTransform.position, checkPlayerRadius, playerLayer);
        if (collider != null && count < 20)
        {
            crystalRenderer.enabled = false;
            count++;
        }
        else if (collider != null && count > 19)
        {
            crystalTransform.position = new Vector3(-500, 500, 0);
        }
    }
}
