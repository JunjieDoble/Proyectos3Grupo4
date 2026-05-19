using _Code.Scripts.Rooms;
using UnityEngine;
using Interactions;
using System.Collections;
using UnityEditor;

namespace Rooms
{
    public class RoomRotator : MonoBehaviour, IHoldInteractable, ILockable
    {
        [SerializeField] public Room targetRoom;
        [SerializeField] public float rotationCooldown = 1.5f;
        [SerializeField] public MeshFilter hologramTarget;
        
        private IInteractor _currentInteractor;

        public void Awake()
        {
            if (targetRoom == null) Debug.LogWarning("RoomRotator does not have a targetRoom", this);
            if (hologramTarget == null) Debug.LogWarning("RoomRotator does not have a hologramPivot", this);
            if (hologramTarget.sharedMesh == null) CreateHologramMesh();
        }

        public void CreateHologramMesh(bool saveAsset = true, bool makeNewInstance = true, bool optimize = true)
        {
            if (targetRoom == null) return;
            if (hologramTarget == null) return;
            
            var meshFilters = targetRoom.gameObject.GetComponentsInChildren<MeshFilter>();
            if (meshFilters.Length == 0) return;
            var combine = new CombineInstance[meshFilters.Length];
            for (int i = 0; i < meshFilters.Length; i++)
            {
                if (meshFilters[i].sharedMesh == null) continue;
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform =targetRoom.transform.worldToLocalMatrix *
                                       meshFilters[i].transform.localToWorldMatrix;
            }

            var mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mesh.CombineMeshes(combine, true, true);

            var center = mesh.bounds.center;
            var verts = mesh.vertices;
            for (int i = 0; i < verts.Length; i++)
                verts[i] -= center;
            mesh.vertices = verts;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            
            hologramTarget.mesh = mesh;
            hologramTarget.transform.rotation = targetRoom.transform.rotation;
            
            if (saveAsset)
                SaveMesh(hologramTarget.sharedMesh, targetRoom.name + "_Hologram", makeNewInstance, optimize);
            
            Debug.Log("Hologram mesh created");
        }

        public void SaveMesh(Mesh mesh, string meshName, bool makeNewInstance, bool optimize)
        {
            string path = EditorUtility.SaveFilePanel("Save Hologram Mesh Asset", "Assets/", meshName, "asset");
            if (string.IsNullOrEmpty(path)) return;
            
            path = FileUtil.GetProjectRelativePath(path);
            
            Mesh meshToSave = (makeNewInstance) ? Instantiate(mesh) : mesh;
            
            if (optimize) meshToSave.Optimize();
            
            AssetDatabase.CreateAsset(meshToSave, path);
            AssetDatabase.SaveAssets();
        }

        public void RotateRoom()
        {
            targetRoom?.StartRotate();
        }

        public void Interact(IInteractor interactor)
        {
            if (IsLocked()) return;
            OnHoldStarted(interactor);
        }

        public void OnHoldStarted(IInteractor interactor)
        {
            if (_currentInteractor != null && _currentInteractor != interactor) return;

            _currentInteractor = interactor;
            RotateRoom();
            _currentInteractor = null;
        }

        public void OnHoldCanceled(IInteractor interactor)
        {
            if (_currentInteractor != null && _currentInteractor != interactor) return;
            targetRoom?.CancelRotate();

            _currentInteractor = null;
        }

        public void OnHoldCompleted(IInteractor interactor)
        {
            if (_currentInteractor != null && _currentInteractor != interactor) return;

            _currentInteractor = null;
        }

        public bool IsLocked()
        {
            return false;
        }

        public void Lock()
        {
            // TODO: Implement lock
        }

        public void Unlock()
        {
            // TODO: Implement unlock
        }
    }
}