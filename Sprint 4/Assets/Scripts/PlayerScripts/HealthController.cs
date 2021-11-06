using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthController : MonoBehaviour
{
    public RectTransform rectTransform;
    public SpriteRenderer spriteRenderer;
    public Sprite[] healthSprites;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform.localScale = new Vector3(70f, 70f, 0);
        healthSprites = Resources.LoadAll<Sprite>("");

        // healthpng = this.GetComponent<Image>();
        //this.GetComponent<Image>().image = healthSprites[16].texture;

        spriteRenderer.sprite = healthSprites[16];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateHealth(int h)
    {
        spriteRenderer.sprite = healthSprites[h];
    }
}