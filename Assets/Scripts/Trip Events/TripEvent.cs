using UnityEngine;

public abstract class TripEvent : MonoBehaviour
{
    protected abstract void DoEvent();
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (LayerMask.GetMask(GlobalStrings.kPlayer) == (LayerMask.GetMask(GlobalStrings.kPlayer) | (1 << col.gameObject.layer)))
        {
            DoEvent();
            gameObject.SetActive(false);
        }
    }
}