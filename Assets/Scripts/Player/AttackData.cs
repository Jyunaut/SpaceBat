using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "Player Attack Data")]
    public class AttackData : ScriptableObject
    {
        public AnimationClip animation;
        public int hitFrame;
        public int damage;
        public Vector2 knockbackMagnitude;
        public float staggerDuration;
        public Vector2 hitBoxOrigin;
        public Vector2 hitBoxSize;
        public EffectsManager.ScreenShakeData screenShakeData;
        public EffectsManager.TimeSlowData timeSlowData;
    }
}