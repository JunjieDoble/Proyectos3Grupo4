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
        public HashSet<Room> Rooms { get; private set; } = new();

        public static HashSet<Room> GetRooms()
        {
            if (s_instance == null) return null;
            return s_instance.Rooms;
        }

        public static void AddRoom(Room room)
        {
            if (s_instance == null) return;
            s_instance.Rooms.Add(room);
        }

        public static void RemoveRoom(Room room)
        {
            if (s_instance == null) return;
            s_instance.Rooms.Remove(room);
        }
        
    }
}