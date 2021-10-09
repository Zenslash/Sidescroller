using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AITargetingSystem : MonoBehaviour
{
    /**
     * How much time we store memories?
     * In seconds
     */
    [SerializeField] private float memorySpan = 3.0f;

    [SerializeField] private float distanceWeight = 1.0f;
    [SerializeField] private float angleWeight = 1.0f;
    [SerializeField] private float ageWeight = 1.0f;

    /**
     * Max players in target
     */
    private static int maxPlayers = 4;

    private AISensoryMemory memory = new AISensoryMemory(maxPlayers);
    private AISensor        sensor;
    private AIMemory        bestMemory;

    public bool HasTarget
    {
        get => bestMemory != null;
    }

    public GameObject Target
    {
        get => bestMemory.gameObject;
    }

    public Vector3 TargetPosition
    {
        get => bestMemory.position;
    }

    public bool TargetInSight
    {
        get => bestMemory.Age < 0.5f; //seconds
    }

    public float TargetDistance
    {
        get => bestMemory.distance;
    }
    
    private void Awake()
    {
        sensor = GetComponent<AISensor>();
        if (sensor == null)
        {
            Debug.LogError("AITargetingSystem: Attack AI Sensor to Agent!!!");
        }
    }

    private void Update()
    {
        memory.UpdateSenses(sensor);
        memory.ForgetMemories(memorySpan);
        
        EvaluateScore();
    }

    private void EvaluateScore()
    {
        foreach (var mem in memory.Memories)
        {
            mem.score = CalculateScore(mem);

            if (bestMemory == null ||
                bestMemory.score < mem.score)
            {
                bestMemory = mem;
            }
        }
    }

    private float Normalize(float value, float maxValue)
    {
        return 1.0f - (value / maxValue);
    }
    
    private float CalculateScore(AIMemory memory)
    {
        float distanceScore = Normalize(memory.distance, sensor.Distance) * distanceWeight;
        float angleScore = Normalize(memory.angle, sensor.Angle) * angleWeight;
        float ageScore = Normalize(memory.Age, memorySpan) * ageWeight;
        return distanceScore + angleScore + ageScore;
    }

    private void OnDrawGizmos()
    {
        float maxScore = float.MinValue;
        foreach (var memory in memory.Memories)
        {
            maxScore = Mathf.Max(maxScore, memory.score);
        }
        
        Color color = Color.red;
        foreach (AIMemory memory in memory.Memories)
        {
            color.a = memory.score / maxScore;
            Gizmos.color = color;
            if (memory == bestMemory)
            {
                Gizmos.color = Color.yellow;
            }
            Gizmos.DrawSphere(memory.position, 0.2f);
        }
    }
}
