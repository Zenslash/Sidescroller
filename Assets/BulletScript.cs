using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnEnable()
    {
        StartCoroutine(Death());
    }


   

    IEnumerator Death()
    {
        yield return new WaitForSecondsRealtime(5);
        ObjectPoolManager.ReturnCopy(gameObject);
    }
}
