using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase; 

public class PlayerControls : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 offset;

    private float maxLeft;
    private float maxRight;
    private float maxDown;
    private float maxUp;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;

        StartCoroutine(SetBoundaries());
    }

    // Update is called once per frame
    void Update()
    {
        //if (Touch.fingers[0].isActive) - dòng này không có tác dụng với đt samsung vì lý do gì đó.
        if(Touch.activeTouches.Count > 0)
        {
            if (Touch.activeTouches[0].finger.index == 0) //obj chỉ đi theo 1 ngón tay, tránh dùng ngón thứ hai thay đổi pos obj đột ngột
            {
                Touch myTouch = Touch.activeTouches[0];
                Vector3 touchPos = myTouch.screenPosition; //lấy vị trí trên màn hình

#if UNITY_EDITOR
                if (touchPos.x == Mathf.Infinity)
                    return;
#endif

                touchPos = mainCam.ScreenToWorldPoint(touchPos); //Chuyển pos màn hình thành world pos

                //Touch support có 3 phase (Bắt đầu, di chuyển, giữ yên)
                if (Touch.activeTouches[0].phase == TouchPhase.Began)
                {
                    offset = touchPos - transform.position;
                    //khoảng cách bù trừ từ vị trí chạm ngón tay đến obj
                }
                if (Touch.activeTouches[0].phase == TouchPhase.Moved)
                {
                    transform.position = new Vector3(touchPos.x - offset.x, touchPos.y - offset.y, 0);
                }
                if (Touch.activeTouches[0].phase == TouchPhase.Stationary)
                {
                    transform.position = new Vector3(touchPos.x - offset.x, touchPos.y - offset.y, 0);
                }
            }

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, maxLeft, maxRight), Mathf.Clamp(transform.position.y, maxDown, maxUp), 0);
        }
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    /// <summary>
    /// Hàm chờ để camera có đủ thời gian để lấy dữ liệu màn hình để căn chỉnh đường ranh giới
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetBoundaries()
    {
        yield return new WaitForEndOfFrame();

        maxLeft = mainCam.ViewportToWorldPoint(new Vector2(0.15f, 0)).x;
        maxRight = mainCam.ViewportToWorldPoint(new Vector2(0.85f, 0)).x;

        maxDown = mainCam.ViewportToWorldPoint(new Vector2(0, 0.08f)).y;
        maxUp = mainCam.ViewportToWorldPoint(new Vector2(0, 0.6f)).y;
    }
}
