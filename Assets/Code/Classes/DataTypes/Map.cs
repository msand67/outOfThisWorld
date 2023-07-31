using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dataStructures
{
    public class Map : MonoBehaviour
    {
        [SerializeField]
        public List<Room> roomList;
        [SerializeField]
        public List<List<double>> travelMatrix;
        public object image; //placeholder, fill out later.
        public string mapFolder;

        string roomLocFile = "RoomLocData_";
        string mapImageLocation = "mapImage.png";
        string roomDataFile = "roomData_";

        string mapDataPath = "Assets/MapData/";



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
            Debug.Log(GetRoomData());
            Debug.Log(GetRoomLocations());
            //PopulateRoomsByMapId();
            //-1 is same room, -2 is different room, no direct connection
            //                   0   1   2   3   4   5   6   7   8
            double[] array0 = { -1, 4, 3, -2, 4, -2, -2, -2, -2 };
            double[] array1 = { 4, -1, 4, -2, 6, -2, -2, -2, -2 };
            double[] array2 = { 3, 4, -1, 5, 4, -2, -2, -2, -2 };
            double[] array3 = { -2, -2, 5, -1, -2, -2, -2, -2, -2 };
            double[] array4 = { 4, 6, 4, -2, -1, 6, 4, 3, -2 };
            double[] array5 = { -2, -2, -2, -2, 6, -1, 2, -2, -2 };
            double[] array6 = { -2, -2, -2, -2, 4, 2, -1, -2, 3 };
            double[] array7 = { -2, -2, -2, -2, 3, -2, -2, -1, -2 };
            double[] array8 = { -2, -2, -2, -2, -2, -2, 3, -2, -1 };
            travelMatrix = new List<List<double>>();
            travelMatrix.Add(array0.ToList());
            var foo = array1.ToList();
            travelMatrix.Add(array1.ToList());
            travelMatrix.Add(array2.ToList());
            travelMatrix.Add(array3.ToList());
            travelMatrix.Add(array4.ToList());
            travelMatrix.Add(array5.ToList());
            travelMatrix.Add(array6.ToList());
            travelMatrix.Add(array7.ToList());
            travelMatrix.Add(array8.ToList());
            Debug.Log(GetTravelMatrixAsJSON());
            Debug.Log(GetTravelMatrixFromJSON(GetTravelMatrixAsJSON()));

            //SaveRoomsToFile();
            FetchDataFromFile(0, 9);
        }

        internal List<(int, bool)> GetRequiredRooms()
        {
            List<(int, bool)> myList = new List<(int, bool)>();
            foreach (Room r in roomList)
            {
                if (r.check.isRequired)
                {
                    myList.Add((r.id, false));
                }
            }
            return myList;
        }

        internal void UpdateRoomTextBoxes(List<(int, bool)> requiredRoomsAssigned, List<(int, bool)> hiddenRoomsRevealed)
        {
            foreach (Room r in roomList)
            {
                bool isHidden = r.check.isHidden;
                foreach ((int, bool) set in hiddenRoomsRevealed)
                {
                    if (r.id == set.Item1)
                    {
                        isHidden = set.Item2;

                    }
                }
                r.UpdatePlanningDescription(isHidden);

                foreach ((int, bool) set in requiredRoomsAssigned)
                {
                    if (r.id == set.Item1)
                    {
                        r.DisplayRequiredStatus(set.Item2);
                    }
                }
            }
        }
        public string GetRoomDescription(int roomId, bool isStillHidden)
        {
            return roomList[roomId].GetRoomDescription(isStillHidden);
        }

        internal List<(int, bool)> GetHIddenRooms()
        {
            List<(int, bool)> myList = new List<(int, bool)>();
            foreach (Room r in roomList)
            {
                if (r.check.isHidden)
                {
                    myList.Add((r.id, true));
                }
            }
            return myList;
        }

        private void FetchDataFromFile(int id, int roomCount)
        {
            string myFolder = mapDataPath + $"map_{id}/";
            //fetch image here
            //read from file into strings to populate through functions.
            PopulateRoomsFromJson(myFolder, roomCount);
            //PlaceRoomsFromFile(roomLocReader.ReadToEnd());
            Debug.Log(GetRoomData());
            InsertRoomDataInChildren();
        }
        void InsertRoomDataInChildren()
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                this.GetComponentsInChildren<Room>()[i] = roomList[i];
                roomList[i].textBox = roomList[i].GetComponentInChildren<TMPro.TextMeshProUGUI>();
            }
        }

        private void PlaceRoomsFromFile(string v)
        {
            //throw new NotImplementedException();
        }

        private void PopulateRoomsFromJson(string myFolder, int roomCount)
        {
            for (int i = 0; i < roomCount; i++)
            {
                using (System.IO.StreamReader myReader = new System.IO.StreamReader(myFolder + $"RoomData_{i}.json"))
                {
                    JsonUtility.FromJsonOverwrite(myReader.ReadToEnd(), roomList[i]);
                }

            }
        }

        void GetRoomListFromChildren()
        {
            roomList = new List<Room>(this.GetComponentsInChildren<Room>());
            foreach (Room r in roomList)
            {
                for (int i = 0; i < roomList.Count; i++)
                {
                    if (r.gameObject.name.Contains(i.ToString()))
                    {
                        r.id = i;
                    }
                }
            }
        }

        private List<string> GetRoomData()
        {
            List<string> roomData = new List<string>();
            for (int i = 0; i < roomList.Count; i++)
            {
                roomData.Add(JsonUtility.ToJson(roomList[i]));
            }
            return roomData;
        }

        private List<string> GetRoomLocations()
        {
            List<string> locData = new List<string>();
            for (int i = 0; i < roomList.Count; i++)
            {
                locData.Add(JsonUtility.ToJson(roomList[i].gameObject.transform.ToString()));
            }
            return locData;
        }

        public void SaveRoomsToFile()
        {
            string myFolder = mapDataPath + $"map_{0}/";
            List<string> myLocations = GetRoomLocations();
            List<string> myRoomData = GetRoomData();
            for (int i = 0; i < roomList.Count; i++)
            {

                using (System.IO.StreamWriter myWriter = new System.IO.StreamWriter(myFolder + $"RoomData_{i}.json"))
                {

                    myWriter.Write(myRoomData[i]);
                    myWriter.Close();
                }
                using (System.IO.StreamWriter myWriter = new System.IO.StreamWriter(myFolder + $"RoomLocData_{i}.json"))
                {

                    myWriter.Write(myLocations[i]);
                    myWriter.Close();
                }
            }
        }

        public string GetTravelMatrixAsJSON()
        {
            string matrixData = "{";
            for (int i = 0; i < travelMatrix.Count; i++)
            {
                matrixData += $"\"{i}\": {{";
                for (int j = 0; j < travelMatrix[i].Count; j++)
                {
                    matrixData += travelMatrix[i][j].ToString();
                    if (j != travelMatrix[i].Count - 1)
                    {
                        matrixData += ",";
                    }
                }
                matrixData += "}";
                if (i != travelMatrix.Count - 1)
                {
                    matrixData += ",";
                }
            }

            matrixData += "}";
            return matrixData;
        }

        public List<List<double>> GetTravelMatrixFromJSON(string json)
        {
            return new List<List<double>>();
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
            return travelMatrix[x][y];
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
            for (int i = 0; i < roomList.Count; i++)
            {
                roomList[i].UpdateTooltipText(i);
            }
        }
    }


}
