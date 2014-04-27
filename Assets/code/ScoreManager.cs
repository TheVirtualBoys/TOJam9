using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	public float maxLife;

	public float penaltyPointsPerSecond;

	public float pointsForResync;


	private class DesyncedObject {
		public int id;
		public float stime;
		public int numPointsSoFar;
	}


	private ArrayList desyncedObjs;

	public float curLife;

	// Use this for initialization
	void Start () {
		curLife = maxLife;
		desyncedObjs = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
		foreach (DesyncedObject obj in desyncedObjs)
		{
			//if the desync object is over the wait time then count points against it
			float desyncTime = Time.time - obj.stime;
			float timeOver = desyncTime - (float)obj.numPointsSoFar;
			if (timeOver > 1f)
			{
				obj.numPointsSoFar += (int)timeOver;
				curLife -= (int)timeOver * penaltyPointsPerSecond;
			}
		}

		if (curLife <= 0)
		{
			//TODO: game over
		}
	}

	public void onGotResync(int instanceId)
	{
		//remove the desynced object with the same id
		DesyncedObject obj = removeDesyncedObject(instanceId);

		if (obj != null)
		{
			curLife = Mathf.Min(curLife + pointsForResync, maxLife);
		}
	}

	public void onGotDesync(int instanceId)
	{
		DesyncedObject obj = new DesyncedObject();
		obj.id = instanceId;
		obj.stime = Time.time;
		obj.numPointsSoFar = 0;
		desyncedObjs.Add(obj);
	}

	private DesyncedObject removeDesyncedObject(int id)
	{
		DesyncedObject ret = null;

		int count = desyncedObjs.Count;
		int i = 0;
		for (; i < count; ++i)
		{
			DesyncedObject obj = (DesyncedObject)desyncedObjs[i];
			if (obj.id == id)
			{
				ret = obj;
				break;
			}
		}

		if (ret != null)
		{
			//found the object, so remove it from that last index in the list
			desyncedObjs.RemoveAt(i);
		}

		return ret;
	}

}
