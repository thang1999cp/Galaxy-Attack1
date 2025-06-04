using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpLineMove : MonoBehaviour
{
    [Header("Path Settings")]
    public Transform[] waypoints;
    public bool autoStart = true;
    public bool useLocalSpace = false;
    public bool closeLoop = false;

    [Header("Movement Settings")]
    public SpeedMode speedMode = SpeedMode.Speed;
    public float speed = 5f;
    public AnimationCurve easeCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Loop Settings")]
    public LoopMode loopMode = LoopMode.None;
    public bool reverse = false;
    public int startPoint = 0;

    [Header("Rotation Settings")]
    public RotationMode rotationMode = RotationMode.None;
    public float lookAhead = 0.1f;
    public Transform rotationTarget;

    [Header("Events")]
    public List<UnityEvent> waypointEvents = new List<UnityEvent>();
    public UnityEvent onPathComplete;

    // Private variables
    private Vector3[] splinePoints;
    private float currentTime = 0f;
    private float totalDuration = 1f;
    private bool isMoving = false;
    private bool isReversed = false;
    private int currentWaypointIndex = 0;
    private Coroutine moveCoroutine;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public enum SpeedMode
    {
        Time,
        Speed
    }

    public enum LoopMode
    {
        None,
        Loop,
        PingPong,
        Random
    }

    public enum RotationMode
    {
        None,
        LookAtNext,
        LookAtDirection,
        UseWaypointRotation
    }

    private void Start()
    {
        InitializeSpline();
        if (autoStart)
        {
            StartMovement();
        }
    }

    private void InitializeSpline()
    {
        if (waypoints == null || waypoints.Length < 2)
        {
            Debug.LogError($"{gameObject.name}: Cần ít nhất 2 waypoints để tạo spline!");
            return;
        }

        CreateSplinePoints();

        CalculateDuration();

        InitializeEvents();

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        if (rotationTarget == null)
            rotationTarget = transform;
    }

    private void CreateSplinePoints()
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] != null)
            {
                Vector3 pos = useLocalSpace ? waypoints[i].localPosition : waypoints[i].position;
                points.Add(pos);
            }
        }

        splinePoints = points.ToArray();
    }

    private void CalculateDuration()
    {
        if (speedMode == SpeedMode.Speed)
        {
            float totalDistance = CalculatePathLength();
            totalDuration = totalDistance / speed;
        }
        else
        {
            totalDuration = speed;
        }
    }

    private float CalculatePathLength()
    {
        float length = 0f;
        for (int i = 0; i < splinePoints.Length - 1; i++)
        {
            length += Vector3.Distance(splinePoints[i], splinePoints[i + 1]);
        }
        if (closeLoop && splinePoints.Length > 0)
        {
            length += Vector3.Distance(splinePoints[splinePoints.Length - 1], splinePoints[0]);
        }
        return length;
    }

    private void InitializeEvents()
    {
        while (waypointEvents.Count < waypoints.Length)
        {
            waypointEvents.Add(new UnityEvent());
        }
    }

    public void StartMovement()
    {
        if (splinePoints == null || splinePoints.Length < 2)
        {
            Debug.LogWarning($"{gameObject.name}: Không thể bắt đầu di chuyển - spline chưa được khởi tạo!");
            return;
        }

        StopMovement();

        currentTime = (float)startPoint / (splinePoints.Length - 1);
        if (isReversed) currentTime = 1f - currentTime;

        moveCoroutine = StartCoroutine(MoveAlongSpline());
    }

    public void StopMovement()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
        isMoving = false;
    }

    public void PauseMovement()
    {
        isMoving = false;
    }

    public void ResumeMovement()
    {
        isMoving = true;
    }

    public void ReverseDirection()
    {
        isReversed = !isReversed;
        reverse = isReversed;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        CalculateDuration();
    }

    public void GoToWaypoint(int index)
    {
        if (index >= 0 && index < splinePoints.Length)
        {
            currentTime = (float)index / (splinePoints.Length - 1);
            if (isReversed) currentTime = 1f - currentTime;
            UpdatePosition();
        }
    }

    public void ResetToStart()
    {
        currentTime = 0f;
        currentWaypointIndex = 0;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        StopMovement();
    }

    private IEnumerator MoveAlongSpline()
    {
        isMoving = true;

        while (isMoving)
        {
            if (!isMoving)
            {
                yield return null;
                continue;
            }

            float deltaTime = Time.deltaTime / totalDuration;
            if (isReversed) deltaTime = -deltaTime;

            currentTime += deltaTime;

            CheckWaypointReached();

            UpdatePosition();
            UpdateRotation();

            if ((!isReversed && currentTime >= 1f) || (isReversed && currentTime <= 0f))
            {
                HandlePathComplete();
            }

            yield return null;
        }
    }

    private void UpdatePosition()
    {
        if (splinePoints.Length < 2) return;

        float t = Mathf.Clamp01(currentTime);
        t = easeCurve.Evaluate(t);

        Vector3 position = GetPositionOnSpline(t);

        if (useLocalSpace)
            transform.localPosition = position;
        else
            transform.position = position;
    }

    private Vector3 GetPositionOnSpline(float t)
    {
        if (splinePoints.Length == 2)
        {
            return Vector3.Lerp(splinePoints[0], splinePoints[1], t);
        }

        int pointCount = closeLoop ? splinePoints.Length : splinePoints.Length - 1;
        float scaledT = t * pointCount;
        int index = Mathf.FloorToInt(scaledT);
        float localT = scaledT - index;

        if (closeLoop)
        {
            index = index % splinePoints.Length;
            Vector3 p0 = splinePoints[(index - 1 + splinePoints.Length) % splinePoints.Length];
            Vector3 p1 = splinePoints[index];
            Vector3 p2 = splinePoints[(index + 1) % splinePoints.Length];
            Vector3 p3 = splinePoints[(index + 2) % splinePoints.Length];

            return CatmullRomInterpolate(p0, p1, p2, p3, localT);
        }
        else
        {
            index = Mathf.Clamp(index, 0, splinePoints.Length - 2);

            Vector3 p0 = index > 0 ? splinePoints[index - 1] : splinePoints[index];
            Vector3 p1 = splinePoints[index];
            Vector3 p2 = splinePoints[index + 1];
            Vector3 p3 = index + 2 < splinePoints.Length ? splinePoints[index + 2] : splinePoints[index + 1];

            return CatmullRomInterpolate(p0, p1, p2, p3, localT);
        }
    }

    private Vector3 CatmullRomInterpolate(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * ((2f * p1) +  (-p0 + p2) * t +  (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +  (-p0 + 3f * p1 - 3f * p2 + p3) * t3);
    }

    private void UpdateRotation()
    {
        if (rotationMode == RotationMode.None) return;

        Vector3 lookDirection = Vector3.forward;

        switch (rotationMode)
        {
            case RotationMode.LookAtNext:
                lookDirection = GetLookDirection();
                break;
            case RotationMode.LookAtDirection:
                lookDirection = GetMovementDirection();
                break;
            case RotationMode.UseWaypointRotation:
                ApplyWaypointRotation();
                return;
        }

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            rotationTarget.rotation = targetRotation;
        }
    }

    private Vector3 GetLookDirection()
    {
        float lookT = Mathf.Clamp01(currentTime + lookAhead);
        Vector3 futurePosition = GetPositionOnSpline(lookT);
        Vector3 currentPosition = GetPositionOnSpline(currentTime);

        return (futurePosition - currentPosition).normalized;
    }

    private Vector3 GetMovementDirection()
    {
        float deltaT = 0.01f;
        float t1 = Mathf.Clamp01(currentTime - deltaT);
        float t2 = Mathf.Clamp01(currentTime + deltaT);

        Vector3 pos1 = GetPositionOnSpline(t1);
        Vector3 pos2 = GetPositionOnSpline(t2);

        return (pos2 - pos1).normalized;
    }

    private void ApplyWaypointRotation()
    {
        int waypointIndex = GetCurrentWaypointIndex();
        if (waypointIndex >= 0 && waypointIndex < waypoints.Length && waypoints[waypointIndex] != null)
        {
            rotationTarget.rotation = waypoints[waypointIndex].rotation;
        }
    }

    private void CheckWaypointReached()
    {
        int newWaypointIndex = GetCurrentWaypointIndex();
        if (newWaypointIndex != currentWaypointIndex && newWaypointIndex >= 0 && newWaypointIndex < waypointEvents.Count)
        {
            waypointEvents[newWaypointIndex]?.Invoke();
            currentWaypointIndex = newWaypointIndex;
        }
    }

    private int GetCurrentWaypointIndex()
    {
        float t = Mathf.Clamp01(currentTime);
        return Mathf.RoundToInt(t * (splinePoints.Length - 1));
    }

    private void HandlePathComplete()
    {
        onPathComplete?.Invoke();

        switch (loopMode)
        {
            case LoopMode.None:
                isMoving = false;
                break;
            case LoopMode.Loop:
                currentTime = isReversed ? 1f : 0f;
                break;
            case LoopMode.PingPong:
                ReverseDirection();
                currentTime = Mathf.Clamp01(currentTime);
                break;
            case LoopMode.Random:
                RandomizeWaypoints();
                currentTime = 0f;
                break;
        }
    }

    private void RandomizeWaypoints()
    {
        // Shuffle waypoints array
        for (int i = 0; i < waypoints.Length; i++)
        {
            Transform temp = waypoints[i];
            int randomIndex = UnityEngine.Random.Range(i, waypoints.Length);
            waypoints[i] = waypoints[randomIndex];
            waypoints[randomIndex] = temp;
        }

        CreateSplinePoints();
        CalculateDuration();
    }

    public float CurrentProgress => currentTime;
    public bool IsMoving => isMoving;
    public int CurrentWaypointIndex => currentWaypointIndex;
}
