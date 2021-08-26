using UnityEngine;

public abstract class TripEvent : MonoBehaviour
{
    private enum TripTarget { Player, Camera }
    [SerializeField] private TripTarget _tripObject;

    protected abstract void DoEvent();
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (_tripObject)
        {
        case TripTarget.Player:
            if (LayerMask.GetMask(GlobalStrings.kPlayer) == (LayerMask.GetMask(GlobalStrings.kPlayer) | (1 << col.gameObject.layer)))
            {
                DoEvent();
                gameObject.GetComponent<Collider2D>().enabled = false;
            }
            break;
        case TripTarget.Camera:
            if (LayerMask.GetMask(GlobalStrings.kCamera) == (LayerMask.GetMask(GlobalStrings.kCamera) | (1 << col.gameObject.layer)))
            {
                DoEvent();
                gameObject.GetComponent<Collider2D>().enabled = false;
            }
            break;
        }
    }
}