using UnityEngine;
using System.Collections.Generic;

namespace Rooms
{
    public class RoomRegistry
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (s_instance != null) return;
            s_instance = new RoomRegistry();
        }

        private static RoomRegistry s_instance;
        private readonly HashSet<Room> _rooms = new();

        public static IReadOnlyCollection<Room> GetRooms()
        {
            Initialize();
            return s_instance._rooms;
        }

        public static void AddRoom(Room room)
        {
            if (room == null) return;
            Initialize();
            s_instance._rooms.Add(room);
        }

        public static void RemoveRoom(Room room)
        {
            if (room == null) return;
            if (s_instance == null) return;
            s_instance._rooms.Remove(room);
        }
        
    }
}