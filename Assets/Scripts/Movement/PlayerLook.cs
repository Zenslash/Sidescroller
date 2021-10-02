using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerLook : MonoBehaviour
{
    [SerializeField]private GameObject lookTarget;
    private Transform lookTargetTransform;
    private PlayerStatsManager playerStatsManager;
    private Transform playerTransform;
    private Vector2 inputVector;
    [SerializeField] [Range(0, .2f)] private float timeToResetPosition;
    [SerializeField] [Range(0, .2f)] private float timeToAim;
    [SerializeField]private Vector3 defaultTargetPosition;

    public Vector2 InputVector
    {
        get => inputVector;
        set => inputVector = value;
    }
    void Awake()
    {
        lookTargetTransform = lookTarget.transform;
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerTransform = playerStatsManager.Movements.PlayerLocalTransform;
        defaultTargetPosition = new Vector3(playerTransform.position.x*100, playerTransform.position.y + 100f,
            playerTransform.position.z);
        lookTargetTransform.position = defaultTargetPosition;
    }
    
    
    void Update()
    {
        if (lookTargetTransform.position.x < playerTransform.position.x)
        {
            Debug.Log("rotate");
        }
        if(inputVector != Vector2.zero)
        {
                lookTargetTransform.position = Vector3.Lerp(lookTargetTransform.position, new Vector3(inputVector.x +playerTransform.position.x, inputVector.y + playerTransform.position.y, 0), timeToAim);
        }
        else
        {
                lookTargetTransform.position = Vector3.Lerp(lookTargetTransform.position, defaultTargetPosition, timeToResetPosition);
                Debug.Log(lookTargetTransform.position);
        }
    }
}
