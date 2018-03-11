
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CircleInput : ScrollRect
{
    protected float mRadius = 0f;
    public Vector2 direction;
    public float length;
    protected override void Start()
    {
        base.Start();
        //计算摇杆块的半径
        mRadius = (transform as RectTransform).sizeDelta.x * 0.45f;
    }

    public override void OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnDrag(eventData);
        content.anchoredPosition *= 2;
        var contentPostion = this.content.anchoredPosition;
        
        if (contentPostion.magnitude > mRadius)
        {
            contentPostion = contentPostion.normalized * mRadius;
            SetContentAnchoredPosition(contentPostion);
        }
        
    }
    public void Update()
    {
        direction = this.content.anchoredPosition.normalized;
        length = this.content.anchoredPosition.magnitude/mRadius;
        if (this.content.anchoredPosition.magnitude < 0.1f) { direction = Vector2.zero;
            length = 0;
        }
   
 
    }
    
}