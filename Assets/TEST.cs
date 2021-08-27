using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    /* Finding a structure for non-looping moves */
    public struct NeedyInts
    {
        public int myint;
        public bool loop;
        public NeedyInts(int a = 1, bool b = true) { myint = a; loop = b;}

        public override string ToString() => $"PROCESSING\nValue: {myint}, Loop: {loop}";
    }
     public List<NeedyInts> list;
     public int intTraverse = 0;

     private void Start()
     {
         List<NeedyInts> list = new List<NeedyInts> {new NeedyInts(1, false), new NeedyInts(2, true), new NeedyInts(3, true)};
         StartCoroutine(wait(list));
     }

     IEnumerator wait(List<NeedyInts> a)
     {
         Debug.Log($"Traversing list @ {intTraverse}");
         Debug.Log(a[intTraverse].ToString());
         yield return new WaitForSeconds(1f);
         if(!a[intTraverse].loop)
         {
             a.RemoveAt(intTraverse);
         }
         else
         {
             intTraverse++;
         }
         ShowList(a);
     }

     private void ShowList(List<NeedyInts> a)
     {
         for(int i = 0; i < a.Count; i++)
         {
             Debug.Log($"POSITION#{i} showing {a[i]}");
         }
     }
}
