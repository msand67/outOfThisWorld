using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using dataStructures;
public class RoomClickHandler : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    public PlanningManager manager;
    [SerializeField]
    public Room room;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //right click for plan creation
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            manager.ConstructPlanStep(room);
        }
        //left click for room details
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            manager.DisplayRoomDetails(room);
        }
    }
}
