using System.Collections;
using System.Linq;
using UnityEngine;

public class CaptureRestore : MonoBehaviour
{
    public Transform captureRoot;
    public Camera viewCamera;

    private Plane[] frustumPlanes = new Plane[6];

    private void CaptureObjectsInFrustum()
    {
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(viewCamera);

        IEnumerable capturableObjects =
            FindObjectsOfType<Capturable>().Where(a =>
            {
                return a.GetComponent<Capturable>().isCapturable &&
                a.TryGetComponent(out Renderer renderer) &&
                renderer != null &&
                GeometryUtility.TestPlanesAABB(frustumPlanes, renderer.bounds);
            });

        foreach (Capturable capturableObject in capturableObjects)
        {
            GameObject temp = Instantiate(capturableObject.gameObject, captureRoot, true);
            temp.transform.position = capturableObject.transform.position;
            temp.transform.rotation = capturableObject.transform.rotation;
            temp.GetComponent<Capturable>().isCapturable = false;

            Mesh mesh = temp.GetComponent<MeshFilter>().mesh;
            foreach (Plane plane in frustumPlanes)
            {
                // CutMesh(mesh, plane, capturableObject.transform);
            }
        }
    }
}
