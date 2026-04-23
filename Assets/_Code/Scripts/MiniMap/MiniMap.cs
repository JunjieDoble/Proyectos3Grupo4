using UnityEngine;

namespace MiniMap
{
    public class MiniMap : MonoBehaviour
    {
        [SerializeField] private GameObject MiniMapObject;
        private bool _isMapVisible;

        private void Awake()
        {
            _isMapVisible = false;
            MiniMapObject.SetActive(_isMapVisible);
        }

        public void OnShowMap()
        {
            _isMapVisible = !_isMapVisible;
            MiniMapObject.SetActive(_isMapVisible);
        }
    }
}

