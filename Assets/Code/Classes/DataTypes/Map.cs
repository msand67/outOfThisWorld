using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataStructures
{
    public class Map
    {
        public List<Room> roomList;
        public double[,] travelMatrix;
        public object image; //placeholder, fill out later.

        public Map()
        {
            Room room1 = new Room();
            Room room2 = new Room(new Check(CheckType.SmoothTalk), true, 1);
            Room room3 = new Room(new Check(CheckType.Egghead), false, 2);
            roomList = new List<Room>();
            roomList.Add(room1);
            roomList.Add(room2);
            roomList.Add(room3);

            //-1 is same room, -2 is different room, no direct connection
            travelMatrix = new double[3, 3] {{-1, 1.5, -2 },
                                             { 1.5, -1, 2.3 },
                                             { -2, 2.3, -1} };

        }

        public Map(List<Room> iRoomList, double[,] iTravelMatrix)
        {
            roomList = iRoomList;
            travelMatrix = iTravelMatrix;
        }
        public Room GetRoom(int id)
        {
            return roomList[id];
        }

        public (bool, int) PerformCheck(int id, Dictionary<StatType, int> statList, int difficultyMod)
        {
            return roomList[id].check.PerformCheck(statList, difficultyMod);
        }
        public CheckType GetRoomCheckType(int id)
        {
            return roomList[id].check.type;
        }

    }

    public class Room
    {
        public Check check;
        public bool isEntrance;
        object nodeLocation; //used to display location on UI and move characters to it.
        public Room()
        {
            check = new Check(CheckType.Breach);
            isEntrance = false;
        }
        public Room(Check iMyCheck, bool iIsEntrance, object iNodeLocation)
        {
            check = iMyCheck;
            isEntrance = iIsEntrance;
            nodeLocation = iNodeLocation;
        }


    }
}
