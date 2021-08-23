using UnityEngine;

public struct Inputs
{
    public delegate void AttackInput();
    public static event AttackInput OnAttackInput;

    private const float _kHoldThreshold = 0.1f;
    private static float _holdTime;
    
    public static float Horizontal => Input.GetAxisRaw("Horizontal");
    public static float Vertical => Input.GetAxisRaw("Vertical");
    public static bool Attack
    {
        get
        {
            if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.J) || Input.GetMouseButtonUp(0))
            {
                OnAttackInput?.Invoke();
                return true;
            }
            return false;
        }
    }
    public static bool AttackHold
    {
        get
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
                _holdTime = 0f;
            if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.J) || Input.GetMouseButton(0))
            {
                _holdTime += Time.deltaTime;
                if (_holdTime >= _kHoldThreshold)
                    return true;
            }
            return false;
        }
    }
}
