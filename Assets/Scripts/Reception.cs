using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reception : MonoBehaviour
{

    public ApplianceGroup tableGroup;
    List<Customer> queue = new List<Customer>();

    // Use this for initialization
    void Start()
    {
        tableGroup = GameObject.Find("Table Group").GetComponent<ApplianceGroup>();
    }

    public int CheckSeats()
    {
        if (queue.Count > tableGroup.appliance.Length) return -1;
        for (int i = 0; i < tableGroup.appliance.Length; i++)
        {
            if (!((Table)tableGroup.appliance[i]).occupied)
            {
                return i;
            }
        }
        return -1;
    }

    public int RequestSeat(Customer c)
    {
        if (!queue.Contains(c)) queue.Add(c);
        int seat = CheckSeats();
        if (seat >= 0) ((Table)tableGroup.appliance[seat]).occupied = true;
        return seat;
    }

    public void LeaveSeat(Customer c)
    {
        int seat = c.seat;
        ((Table)tableGroup.appliance[seat]).occupied = false;
        ((Table)tableGroup.appliance[seat]).slot = null;
        queue.Remove(c);
        foreach (Customer cs in queue)
        {
            if (cs.seat == -1)
            {
                cs.seat = seat;
                cs.SetSeat(seat);
                ((Table)tableGroup.appliance[seat]).occupied = true;
                break;
            }
        }
    }
}
