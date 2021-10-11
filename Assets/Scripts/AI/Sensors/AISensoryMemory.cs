using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AIMemory
{
    public float Age => Time.time - lastSeen;

    public GameObject   gameObject;
    public Vector3      position;
    public Vector3      direction;
    public float        angle;
    public float        distance;
    public float        lastSeen;
    public float        score;
}

public class AISensoryMemory
{
    private List<AIMemory> memories = new List<AIMemory>();
    private GameObject[] buffer;
    private string layerToRemember = "Player";

    public List<AIMemory> Memories
    {
        get => memories;
    }
    
    public AISensoryMemory(int maxPlayers)
    {
        buffer = new GameObject[maxPlayers];
    }
    
    [Server]
    public void UpdateSenses(AISensor sensor)
    {
        int targets = sensor.Filter(buffer, layerToRemember);
        for (int i = 0; i < targets; i++)
        {
            GameObject go = buffer[i];
            RefreshMemory(sensor.gameObject, go);
        }
    }
    
    [Server]
    public void RefreshMemory(GameObject agent, GameObject target)
    {
        AIMemory memory = FetchMemory(target);
        memory.gameObject = target;
        memory.position = target.transform.position;
        memory.direction = target.transform.position - agent.transform.position;
        memory.distance = memory.direction.magnitude;
        memory.angle = Vector3.Angle(agent.transform.forward, memory.direction);
        memory.lastSeen = Time.time;
    }
    
    [Server]
    public void ForgetMemories(float olderThan)
    {
        memories.RemoveAll(m => m.Age > olderThan);
        memories.RemoveAll(m => (m.gameObject == null));
        memories.RemoveAll(m => !m.gameObject.GetComponent<HealthComponent>().IsAlive());
    }
    
    [Server]
    public AIMemory FetchMemory(GameObject go)
    {
        AIMemory memory = memories.Find(x => x.gameObject == go);
        if (memory == null)
        {
            memory = new AIMemory();
            memories.Add(memory);
        }

        return memory;
    }
}
