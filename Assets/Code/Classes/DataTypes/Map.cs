using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataStructures
{
    public class Map : MonoBehaviour
    {
        [SerializeField]
        public List<Room> roomList;
        public double[,] travelMatrix;
        public object image; //placeholder, fill out later.

        /*public Map()
        {
            //roomList = new List<Room>();

            //-1 is same room, -2 is different room, no direct connection
            travelMatrix = new double[3, 3] {{-1, 1.5, -2 },
                                             { 1.5, -1, 2.3 },
                                             { -2, 2.3, -1} };

        }

        public Map(List<Room> iRoomList, double[,] iTravelMatrix)
        {
            roomList = iRoomList;
            travelMatrix = iTravelMatrix;
        }*/
        public void init()
        {
            GetRoomListFromChildren();

            //-1 is same room, -2 is different room, no direct connection
            travelMatrix = new double[3, 3] {{-1, 1.5, -2 },
                                             { 1.5, -1, 2.3 },
                                             { -2, 2.3, -1} };
        }
        void GetRoomListFromChildren()
        {
            roomList = new List<Room>( this.GetComponentsInChildren<Room>());
        }
        public Room GetRoom(int id)
        {
            return roomList[id];
        }

        public (bool, int, double) PerformCheck(int id, List<Stat> statList, int difficultyMod)
        {
            return roomList[id].check.PerformCheck(statList, difficultyMod);
        }
        public CheckType GetRoomCheckType(int id)
        {
            return roomList[id].check.type;
        }
        public double GetTimeBetweenRooms(int x, int y)
        {
            return travelMatrix[x,y];
        }

        internal bool IsRoomCheckComplete(int currentRoom)
        {
            if (currentRoom < 0)
            {
                return true;
            }
            else
            {
                return GetRoom(currentRoom).check.isComplete;
            }
        }
        public void UpdateRoomTooltips()
        {
            for(int i = 0; i< roomList.Count; i++)
            {
                roomList[i].UpdateTooltipText(i);
            }
        }
    }

    
}
