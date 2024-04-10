using System;
using System.Linq;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using FixMath.NET;
using UnityEngine;
using UnityEngine.Serialization;

namespace AE_BEPUPhysics_Addition
{
    public class AEMeshVolumnBaseCollider : BaseVolumnBaseCollider
    {
        private ConvexHull m_convexHull;
        [SerializeField] private Mesh m_mesh;

        protected override Entity OnCreateEnity()
        {
            if (IsStatic)
            {
                m_convexHull = new ConvexHull(transform.position.ToFix64(), m_mesh.vertices.ToFix64());
            }
            else
            {
                m_convexHull = new ConvexHull(transform.position.ToFix64(), m_mesh.vertices.ToFix64(), Mass.ToFix64());
            }

            return m_convexHull;
        }

        protected override Entity GetEntity()
        {
            return m_convexHull;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            // 获取原始变换的位置、旋转和缩放信息
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3 scale = new Vector3(1, 1, 1); // 将缩放设置为 (1, 1, 1)
            // 构建一个新的局部到世界的变换矩阵，缩放为 (1, 1, 1)
            Matrix4x4 newLocalToWorldMatrix = Matrix4x4.TRS(position, rotation, scale);
            Gizmos.matrix = newLocalToWorldMatrix;
            Gizmos.DrawWireMesh(m_mesh);
        }
#endif
    }
}