using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControl : MonoBehaviour
{
    public static CameraControl Instance;

    #region 旋转变量
    //是否点击到UI
    private bool isUI = false;
    //记录x轴
    private float x = 0;
    //记录y轴
    private float y = 0;
    //y轴限制角度
    private float maxY = 40f;
    //是否旋转
    private bool isRota = false;
    //主相机1
    public Transform ctrlX;
    //主相机2
    public Transform ctrlY;
    //实际控制X
    public Transform entityX;
    //实际控制Y
    public Transform entityY;
    
    //缓动速度
    private float lerpSpeed;
    #endregion

    #region 缩放变量
    private bool isSca = false;
    //相机
    public Transform mainCamera;
    //初始相机Z
    private float initScaleZ;
    //当前变量Z
    private float currentZ;
    //Scale速度
    private float scaleSpeed = 3f;
    private float minZ = 1f;
    private float maxZ = 4f;
    #endregion

    #region 初始化与Update
    //初始化
    private void Awake()
    {
        Instance = this;
        initScaleZ = mainCamera.localPosition.z;
        currentZ = initScaleZ;
#if UNITY_ANDROID && !UNITY_EDITOR
        lerpSpeed = 6f;
#else
        lerpSpeed = 5f;
#endif
    }

    void Update()
    {
        //控制旋转
        ControlRotation();
        //控制缩放
        ControlScale();
    }

    private void LateUpdate()
    {
        //缓动旋转开启
        if (isRota)
        {
            if (Quaternion.Angle(entityY.rotation, ctrlY.rotation) <= 0.02f && Quaternion.Angle(entityX.rotation, ctrlX.rotation) <= 0.02f)
            {
                entityY.rotation = ctrlY.rotation;
                entityX.rotation = ctrlX.rotation;
                isRota = false;
            }
            else
            {
                entityX.rotation = Quaternion.Lerp(entityX.rotation, ctrlX.rotation, Time.deltaTime * lerpSpeed);
                entityY.rotation = Quaternion.Lerp(entityY.rotation, ctrlY.rotation, Time.deltaTime * lerpSpeed);
            }
        }
        if (isSca)
        {
            //打开协程
            StartCoroutine("LerpScale");
        }
    }

    #endregion

    #region 控制旋转
    //控制旋转
    private void ControlRotation()
    {
        //左键或者手指按下时 判断是否点击到UI
        if (IsDown0())
        {
            isUI = IsPointUI();
            if (!isUI)
            {
                //停止协程
                StopCoroutine("RotaLerpReset");
                //重置xy
                x = Confine360(ctrlX.localRotation.eulerAngles.y);
                y = Confine360(ctrlY.localRotation.eulerAngles.x);
            }
        }
        //旋转
        if (IsMove0() && !isUI)
        {
            x -= GetX() * Time.deltaTime * 30f;
            x = Confine360(x);
            //Debug.Log(x);
            y += GetY() * Time.deltaTime * 30f;
            y = Mathf.Clamp(y, -maxY, maxY);
            ctrlX.localRotation = Quaternion.Euler(0, x, 0);
            ctrlY.localRotation = Quaternion.Euler(y, 0, 0);
            isRota = true;
        }
    }


    #endregion

    #region 控制缩放
    private void ControlScale()
    {
        //如果双指点击或者滚轮滚动
        isSca = IsScroll();
        if (isSca)
        {
            currentZ -= GetScroll();
            currentZ = Mathf.Clamp(currentZ, minZ, maxZ);
            //停止协程
            StopCoroutine("LerpScale");
        }
    }
    private IEnumerator LerpScale()
    {
        Vector3 temp = mainCamera.localPosition;
        temp.z = currentZ;
        while (true)
        {
            if (Vector3.Distance(mainCamera.localPosition, temp) <= 0.01f)
            {
                mainCamera.localPosition = temp;
                break;
            }
            else
            {
                mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, temp, Time.deltaTime * scaleSpeed);
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region 工具方法
    /// <summary>
    /// 限制360度到-180 - 180
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    private float Confine360(float r)
    {
        if (r > 180)
        {
            r -= 360;

        }
        else if (r < -180)
        {
            r += 360;
        }
        return r;
    } 
    #endregion

    #region 点击复位

    //复位
    public void OnReset()
    {
        isRota = true;
        x = 0;
        y = 0;
        //停止协程
        StopCoroutine("RotaLerpReset");
        //开启协程
        StartCoroutine("RotaLerpReset");

        currentZ = initScaleZ;
        //停止协程
        StopCoroutine("LerpScale");
        //打开协程
        StartCoroutine("LerpScale");
    }

    //旋转缓动复位
    IEnumerator RotaLerpReset()
    {
        while (true)
        {
            if (Quaternion.Angle(ctrlX.localRotation, Quaternion.identity) <= 0.01f && Quaternion.Angle(ctrlY.localRotation, Quaternion.identity) <= 0.01f)
            {
                ctrlX.localRotation = Quaternion.identity;
                ctrlY.localRotation = Quaternion.identity;
                break;
            }
            else
            {
                ctrlX.localRotation = Quaternion.Lerp(ctrlX.localRotation, Quaternion.identity, Time.deltaTime * lerpSpeed);
                ctrlY.localRotation = Quaternion.Lerp(ctrlY.localRotation, Quaternion.identity, Time.deltaTime * lerpSpeed);
            }
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region 安卓PC判断封装

    #if UNITY_ANDROID && !UNITY_EDITOR
    //存储上一帧的Touch
    private Touch oldTouch1;  //第一根手指
    private Touch oldTouch2;  //第二根手指
    #endif

    /// <summary>
    /// 获取滚轮值
    /// </summary>
    /// <returns></returns>
    private float GetScroll()
    {
    #if UNITY_ANDROID && !UNITY_EDITOR
        //记录当前帧的两个手指
        Touch newTouch1 = Input.GetTouch(0);
        Touch newTouch2 = Input.GetTouch(1);
        //第2个点第一次按下时 赋予老点值为当前值
        if (newTouch2.phase == TouchPhase.Began)
        {
            oldTouch2 = newTouch2;
            oldTouch1 = newTouch1;
        }
        //计算老的两点距离和 新的两点间距离
        float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
        float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
        //缩放值 为负代表缩小
        float scaleFactor = (newDistance - oldDistance) / 160;
        //记录下当前帧的点，在下一帧使用
        oldTouch1 = newTouch1;
        oldTouch2 = newTouch2;
        return scaleFactor;
    #else
        return Input.GetAxis("Mouse ScrollWheel") * 3f;
    #endif
    }

    /// <summary>
    /// 滚轮或双指
    /// </summary>
    /// <returns></returns>
    private bool IsScroll()
    {
    #if UNITY_ANDROID && !UNITY_EDITOR
        return Input.touchCount == 2;
    #else
        return Input.GetAxis("Mouse ScrollWheel") != 0;
    #endif
    }

    /// <summary>
    /// 获取鼠标/手指X轴与上一帧的差值
    /// </summary>
    /// <returns></returns>
    private float GetX()
    {
    #if UNITY_ANDROID && !UNITY_EDITOR
        return Input.GetTouch(0).deltaPosition.x * 0.3f;
    #else
        return Input.GetAxis("Mouse X") * 40;
    #endif
    }
    /// <summary>
    /// 获取鼠标/手指Y轴与上一帧的差值
    /// </summary>
    /// <returns></returns>
    private float GetY()
    {
    #if UNITY_ANDROID && !UNITY_EDITOR
        return Input.GetTouch(0).deltaPosition.y * 0.3f;
    #else
        return Input.GetAxis("Mouse Y") * 20;
    #endif
    }
    /// <summary>
    /// 是否点击在UI上
    /// </summary>
    /// <returns></returns>
    private bool IsPointUI()
    {
    #if UNITY_ANDROID && !UNITY_EDITOR
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    #else
        return EventSystem.current.IsPointerOverGameObject();
    #endif
    }
    /// <summary>
    /// 左键或者第0根手指是否按下
    /// </summary>
    /// <returns></returns>
    private bool IsDown0()
    {
    #if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount != 1)
        {
            return false;
        }
        return Input.GetTouch(0).phase == TouchPhase.Began;
    #else
        return Input.GetMouseButtonDown(0);
    #endif
    }

    /// <summary>
    /// 左键或者第0根手指是否按住移动
    /// </summary>
    /// <returns></returns>
    private bool IsMove0()
    {
    #if UNITY_ANDROID && !UNITY_EDITOR
        return Input.touchCount == 1;
    #else
        return Input.GetMouseButton(0);
    #endif
    }

    #endregion
}