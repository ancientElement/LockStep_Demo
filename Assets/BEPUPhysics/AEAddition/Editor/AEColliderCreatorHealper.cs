using AE_BEPUPhysics_Addition;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

public static class AEColliderCreatorHealper
{
    // 在 Hierarchy 窗口中添加一个右键菜单选项
    [MenuItem("GameObject/AE 3D Obejct/Box", false, 1)]
    private static void CreatBox()
    {
        GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject.DestroyImmediate(newObject.GetComponent<BoxCollider>());
        var box = newObject.AddComponent<AEBoxVolumnBaseCollider>();
        box.Height = 1f;
        box.Width = 1f;
        box.Length = 1f;
        newObject.transform.position = GetPositionForwdSceneCamera();
        //保存在场景中
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    [MenuItem("GameObject/AE 3D Obejct/Capsule", false, 1)]
    public static void CreateCapsule()
    {
        GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        GameObject.DestroyImmediate(newObject.GetComponent<CapsuleCollider>());
        var capsule = newObject.AddComponent<AECapsuleVolumnBaseCollider>();
        capsule.HalfLength = 1f;
        capsule.Radius = 0.5f;
        newObject.transform.position = GetPositionForwdSceneCamera();
        //保存在场景中
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
    
    [MenuItem("GameObject/AE 3D Obejct/Cylinder", false, 1)]
    public static void CreateCylinder()
    {
        GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        GameObject.DestroyImmediate(newObject.GetComponent<CapsuleCollider>());
        var cylinder = newObject.AddComponent<AECylinderVolumnBaseCollider>();
        cylinder.Height = 2f;
        cylinder.Radius = 0.5f;
        newObject.transform.position = GetPositionForwdSceneCamera();
        //保存在场景中
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
    
    [MenuItem("GameObject/AE 3D Obejct/Sphere", false, 1)]
    public static void CreateSphere()
    {
        GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject.DestroyImmediate(newObject.GetComponent<SphereCollider>());
        var sphere = newObject.AddComponent<AESphereVolumnBaseCollider>();
        sphere.Radius = 0.5f;
        newObject.transform.position = GetPositionForwdSceneCamera();
        //保存在场景中
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
    
    [MenuItem("GameObject/AE 3D Obejct/Plane", false, 1)]
    public static void CreatePlane()
    {
        GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
        GameObject.DestroyImmediate(newObject.GetComponent<MeshCollider>());
        var plane = newObject.AddComponent<AEMeshVolumnBaseCollider>();
        newObject.transform.position = GetPositionForwdSceneCamera();
        //保存在场景中
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    private static Vector3 GetPositionForwdSceneCamera()
    {
        // 获取当前 Scene 视图的相机对象
        SceneView sceneView = SceneView.lastActiveSceneView;
        if (sceneView == null)
        {
            return Vector3.zero;
        }

        // 获取相机的位置和朝向
        Vector3 cameraPosition = sceneView.camera.transform.position;
        Vector3 cameraForward = sceneView.camera.transform.forward;
        // 在相机前方 2 米处创建一个物体
        var newPosition = cameraPosition + 5f * cameraForward;
        return newPosition;
    }
}