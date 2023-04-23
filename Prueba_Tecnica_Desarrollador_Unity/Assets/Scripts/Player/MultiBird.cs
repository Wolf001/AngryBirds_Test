using System.Collections;
using UnityEngine;
using Assets.Scripts;

public class MultiBird : MonoBehaviour
{
    #region Variables
    public BirdState State { get; private set; }
    public SlingShot charpe;
    public GameObject BirdMulti;
    public AddScore addScor;
    #endregion
    #region Update
    void Update()
    {
        if (charpe.slingshotState == SlingshotState.BirdFlying)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MultiBirds();
            }
        }
    }
    #endregion
    #region Method
    void MultiBirds()
    {   
        //StartCoroutine(DestroyAfter());
    }

    IEnumerator DestroyAfter()
    {
        yield return null;
        Destroy(BirdMulti);
    }
    #endregion
}
