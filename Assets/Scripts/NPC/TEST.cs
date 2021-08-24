using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public List<NPCa.Action> phase;
    public List<NPCa.Action> queue;
    public int currentAction;
    public void Start()
    {
        phase = new List<NPCa.Action>();
        phase.Add(new Wait(this, 3f));
        phase.Add(new Attack(this, 3f));
        queue = phase;
        currentAction = 0;

    }

    public void Update()
    {
        if (queue[currentAction].OnUpdate())
        {
            CompleteAction();
            if(currentAction != queue.Count - 1)
            {
                Debug.Log("Timed Out");
                NextAction();
            }
            else
            {
                Debug.Log("Loop finished");
            }
        }
    }

    public void CompleteAction()
    {
        if(queue[currentAction].OnUpdate())
            queue[currentAction]?.OnComplete();
    }

    public void NextAction()
    {
        queue[++currentAction]?.OnEnter();
    }

}

public class Wait : NPCa.Action
{
    public TEST controller;
    private float timer;
    public Wait(TEST controller, float time)
    {
        this.controller = controller;
        timer = time;
    }
    public override void OnEnter()
    {
        Debug.Log("Wait - OnEnter");
    }
    public override bool OnUpdate()
    {
        if (timer > 0.1f)
        {
            Debug.Log("Wait - OnUpdate");
            timer -= Time.deltaTime;
            return false;
        }
        return true;
    }
    public override void OnComplete()
    {
        Debug.Log("Wait - OnComplete");
    }
}

public class Attack : NPCa.Action
{
    public TEST controller;
    private float timer;
    public Attack(TEST controller, float time)
    {
        this.controller = controller;
        timer = time;
    }
    public override void OnEnter()
    {
        Debug.Log("Attack - OnEnter");
    }
    public override bool OnUpdate()
    {
        if (timer > 0.1f)
        {
            Debug.Log("Attack - OnUpdate");
            timer -= Time.deltaTime;
            return false;
        }
        return true;
    }
    public override void OnComplete()
    {
        Debug.Log("Attack - OnComplete");
    }
}