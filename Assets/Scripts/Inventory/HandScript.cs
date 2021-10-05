
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    private static HandScript instance;

    public static HandScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }
            return instance;
        }
    }

    public IMoveable MyMoveable { get; set; }
    private Image icon;
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<Image>();
        icon.transform.position = Input.mousePosition;  // !!!!!!! remake for our input !!!!!!!!!!!!!!!!
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeMoveable(IMoveable moveable)
    {
        this.MyMoveable = moveable;
        icon.sprite = moveable.MyIcon;
        icon.color = Color.white;
    }
}
