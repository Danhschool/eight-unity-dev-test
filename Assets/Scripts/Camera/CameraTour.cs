using System.Collections;
using UnityEngine;

public class CameraTour : MonoBehaviour
{
    public static CameraTour Instance { get; private set; }

    [Header("Camera Configuration")]
    public Transform mainCamera;
    public Transform[] waypoints;
    public float moveSpeed = 15f;
    public float rotationSpeed = 5f;

    public static bool isTouring = false;

    private Transform originalParent;
    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;

    private void Awake()
    {
        Instance = this;
    }

    public void StartTour()
    {
        if (waypoints.Length > 0)
        {
            StartCoroutine(TourRoutine());
        }
    }

    private IEnumerator TourRoutine()
    {
        isTouring = true;

        originalParent = mainCamera.parent;
        originalLocalPosition = mainCamera.localPosition;
        originalLocalRotation = mainCamera.localRotation;

        GameObject tourDummy = new GameObject("TourDummy");
        tourDummy.transform.position = mainCamera.position;
        tourDummy.transform.rotation = mainCamera.rotation;

        mainCamera.SetParent(tourDummy.transform, true);

        mainCamera.localPosition = originalLocalPosition;
        mainCamera.localRotation = originalLocalRotation;

        for (int i = 0; i < waypoints.Length; i++)
        {
            Transform targetWaypoint = waypoints[i];

            while (Vector3.Distance(tourDummy.transform.position, targetWaypoint.position) > 0.01f)
            {
                tourDummy.transform.position = Vector3.MoveTowards(tourDummy.transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

                Vector3 directionToTarget = targetWaypoint.position - tourDummy.transform.position;
                if (directionToTarget != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                    tourDummy.transform.rotation = Quaternion.Slerp(tourDummy.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }

                yield return null;
            }
        }

        mainCamera.SetParent(null, true);
        Destroy(tourDummy);

        bool isReturning = true;
        while (isReturning)
        {
            Vector3 targetWorldPos = originalParent.TransformPoint(originalLocalPosition);
            Quaternion targetWorldRot = originalParent.rotation * originalLocalRotation;

            mainCamera.position = Vector3.MoveTowards(mainCamera.position, targetWorldPos, moveSpeed * Time.deltaTime);
            mainCamera.rotation = Quaternion.Slerp(mainCamera.rotation, targetWorldRot, rotationSpeed * Time.deltaTime);

            if (Vector3.Distance(mainCamera.position, targetWorldPos) < 0.01f && Quaternion.Angle(mainCamera.rotation, targetWorldRot) < 0.1f)
            {
                isReturning = false;
            }
            yield return null;
        }

        mainCamera.SetParent(originalParent);
        mainCamera.localPosition = originalLocalPosition;
        mainCamera.localRotation = originalLocalRotation;

        isTouring = false;
    }
}