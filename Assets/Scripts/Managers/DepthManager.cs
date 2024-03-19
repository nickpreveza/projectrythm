using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class DepthManager : MonoBehaviour
{
    public delegate void NewTriggerPoints(List<Vector2> triggerPoints);
    public static event NewTriggerPoints OnTriggerPoints = null;

    [SerializeField] MultiSourceManager multiSourceManager;
    KinectSensor sensor; //the actual sensor
    CoordinateMapper mapper; //deapth data under color points

    ushort[] depthData;
    CameraSpacePoint[] cameraSpacePoints;
    ColorSpacePoint[] colorSpacePoints;
    public List<ValidPoint> validPoints;
    List<Vector2> triggerPoints;

    private readonly Vector2Int depthResolution = new Vector2Int(512, 424); //deapth sensor actual resolution
    Texture2D activeTexture;

    [Header("Depth Settings")]
    [Range(0, 1f)]
    [SerializeField] float depthSensiitivity;
    [Range(-10, 10f)]
    [SerializeField] float wallDepth;
    [Header("Sensor Bounds")]
    [Range(-1, 1f)]
    [SerializeField] float topCutoff = 1;
    [Range(-1, 1f)]
    [SerializeField] float bottomCutoff = -1;
    [Range(-1, 1f)]
    [SerializeField] float rightCutoff = 1;
    [Range(-1, 1f)]
    [SerializeField] float leftCutoff = -1;
    [Range(2, 25)]
    [SerializeField] int sampleIndexMultiplier = 8;
    [Range(1,20)]
    [SerializeField] int rectBoxSize = 10;
    Rect boxRect;

    private int totalPoints 
    {
        get
        {
            return depthResolution.x * depthResolution.y;
        }
    }
    private void Awake()
    {
        sensor = KinectSensor.GetDefault();
        mapper = sensor.CoordinateMapper;

        cameraSpacePoints = new CameraSpacePoint[totalPoints];
        colorSpacePoints = new ColorSpacePoint[totalPoints];
    }

    private void FixedUpdate()
    {
        validPoints = GetValidPoints();
        triggerPoints = FilterToTrigger(validPoints);

        if (OnTriggerPoints != null && triggerPoints.Count != 0)
        {
            OnTriggerPoints(triggerPoints);
        }

        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            boxRect = CreateRect(validPoints);
            UpdateDepthTexture();
        }*/
    }

    private void OnGUI()
    {
        GUI.Box(boxRect, "");

        if (triggerPoints == null)
        {
            return;
        }

        foreach(Vector2 point in triggerPoints)
        {
            Rect rect = new Rect(point, new Vector2(rectBoxSize, rectBoxSize));
            GUI.Box(rect, "");
        }
    }

    private Rect CreateRect(List<ValidPoint> points)
    {

        Vector2 topLeft = new Vector2(int.MaxValue, int.MaxValue);
        Vector2 bottomRight = new Vector2(0, 0);

        foreach (ValidPoint point in points)
        {
            if (point.colorSpace.X < topLeft.x)
            {
                topLeft.x = point.colorSpace.X;
            }

            if (point.colorSpace.Y < topLeft.y)
            {
                topLeft.y = point.colorSpace.Y;
            }

            if (point.colorSpace.X > bottomRight.x)
            {
                bottomRight.x = point.colorSpace.X;
            }

            if (point.colorSpace.Y > bottomRight.y)
            {
                bottomRight.y = point.colorSpace.Y;
            }
        }

        if (points.Count == 0)
        {
            return new Rect();
        }

        //Translate to viewport
        Vector2 screenTopLeft = ScreenToCamera(topLeft);
        Vector2 screenBottomRight = ScreenToCamera(bottomRight);

        int width = (int)(screenBottomRight.x - screenTopLeft.x);
        int height = (int)(screenBottomRight.y - screenTopLeft.y);

        Vector2 size = new Vector2(width, height);
        Rect rect = new Rect(screenTopLeft, size);

        return rect;
    }

    private Vector2 ScreenToCamera(Vector2 screenPos)
    {
        Vector2 normalizedScreen = new Vector2(Mathf.InverseLerp(0, 1920, screenPos.x), Mathf.InverseLerp(0, 1080, screenPos.y));
        Vector2 screenPoint = new Vector2(normalizedScreen.x * Camera.main.pixelWidth, normalizedScreen.y * Camera.main.pixelHeight);

        return screenPoint;
    }

    private List<Vector2> FilterToTrigger(List<ValidPoint> points)
    {
        List<Vector2> triggerPoints = new List<Vector2>();

        foreach(ValidPoint point in points)
        {
            if (!point.withinDepth)
            {
                if (point.z < wallDepth * depthSensiitivity)
                {
                    Vector2 screenPoint = ScreenToCamera(new Vector2(point.colorSpace.X, point.colorSpace.Y));

                    triggerPoints.Add(screenPoint);
                }
            }
        }

        return triggerPoints;
    }

    public Texture2D GetDepthTexture()
    {
        return activeTexture;
    }

    public List<ValidPoint> GetValidPoints()
    {
        List<ValidPoint> validPoints = new List<ValidPoint>();

        depthData = multiSourceManager.GetDepthData();
        mapper.MapDepthFrameToCameraSpace(depthData, cameraSpacePoints);
        mapper.MapDepthFrameToColorSpace(depthData, colorSpacePoints);

        for (int x = 0; x < depthResolution.x / sampleIndexMultiplier; x++)
        {
            for (int y = 0; y < depthResolution.y / sampleIndexMultiplier; y++)
            {
                int sampleIndex = (y * depthResolution.x) + x;
                sampleIndex *= sampleIndexMultiplier;


                if (cameraSpacePoints[sampleIndex].X < leftCutoff)
                {
                    continue;
                }
                if (cameraSpacePoints[sampleIndex].X > rightCutoff)
                {
                    continue;
                }
                if (cameraSpacePoints[sampleIndex].Y > topCutoff)
                {
                    continue;
                }
                if (cameraSpacePoints[sampleIndex].Y < bottomCutoff)
                {
                    continue;
                }

                ValidPoint newPoint = new ValidPoint(colorSpacePoints[sampleIndex], cameraSpacePoints[sampleIndex].Z);
                if (cameraSpacePoints[sampleIndex].Z >= wallDepth)
                {
                    newPoint.withinDepth = true;
                }

                validPoints.Add(newPoint);
            }
        }


        return validPoints;
    }

    public void UpdateDepthTexture()
    {
        activeTexture = new Texture2D(1920, 1080, TextureFormat.Alpha8, false);


        for (int x = 0; x < 1920; x++)
        {
            for (int y = 0; y < 1080; y++)
            {
                activeTexture.SetPixel(x, y, Color.clear);
            }
        }

        foreach (ValidPoint point in validPoints)
        {
            activeTexture.SetPixel((int)point.colorSpace.X, (int)point.colorSpace.Y, Color.black);
        }

        activeTexture.Apply();
    }


} 


public class ValidPoint
{
    public ColorSpacePoint colorSpace;
    public float z = 0.0f;
    public bool withinDepth = false;

    public ValidPoint(ColorSpacePoint newColorSpace, float newZ)
    {
        colorSpace = newColorSpace;
        z = newZ;

    }
}
