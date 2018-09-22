using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ETVManager : MonoBehaviour
{
    private static bool OCCUPIED = false;
    private static bool VACANT = true;

    [SerializeField]
    public GameObject AutoPositioningSlots;

    private bool initialized = false;
    private IDictionary<GameObject, bool> slots;

    void Awake()
    {
        
    }

    // Use this for initialization
    void Start ()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    /// <summary>
    /// Takes an ET visualization object and puts it at an empty
    /// circularily ordered slot.
    /// </summary>
    /// <param name="ETV"></param>
    /// <returns>Whether there was an empty slot available</returns>
    public bool AutoPlaceETV(GameObject ETV)
    {
        if(!initialized)
        {
            Init(2);
            Init(3);
        }

        bool success = false;

        
        foreach(var slot in slots.Keys)
        {
            if(slots[slot] == VACANT)
            {
                ETV.transform.position = slot.transform.position;
                ETV.transform.rotation = slot.transform.rotation;
                slots[slot] = OCCUPIED;
                return success = true;
            }
        }

        return success;
    }

    public void Init(int ring)
    {
        if(!initialized)
        {
            slots = new Dictionary<GameObject, bool>();
            initialized = true;
        }

        // Initialize 12 slots in the first circle (3m distance)
        float radAngle;
        for(float deg = 0; deg < 360; deg += (360f / (3*ring)))
        {
            radAngle = deg * Mathf.Deg2Rad;
            var slot = new GameObject("AutoVisSlot");
            slot.transform.parent = AutoPositioningSlots.transform;
            slot.transform.position = new Vector3(Mathf.Cos(radAngle)*ring, 0, Mathf.Sin(radAngle) *ring);
            slot.transform.rotation = Quaternion.Euler(0, -(deg - 90), 0);
            // since most ETVs are 1 UE wide, move it by .5f to the left to center it
            slot.transform.Translate(new Vector3(-.5f, 0, 0), Space.Self);
            slots.Add(slot, VACANT);
        }
    }

    public void Reset()
    {
        initialized = false;
    }
}
