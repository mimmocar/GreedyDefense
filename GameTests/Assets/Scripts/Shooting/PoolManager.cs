using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour {

	private Dictionary<int,Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>> ();

	
	public void CreatePool(GameObject prefab, int poolSize) {
		int poolKey = prefab.GetInstanceID ();

		if (!poolDictionary.ContainsKey (poolKey)) {
			poolDictionary.Add (poolKey, new Queue<ObjectInstance> ());

			GameObject poolHolder = GameObject.Find("PoolManager");

			for (int i = 0; i < poolSize; i++) {
				ObjectInstance newObject = new ObjectInstance(Instantiate (prefab) as GameObject);
				poolDictionary [poolKey].Enqueue (newObject);
				newObject.SetParent (poolHolder.transform);
			}
		}
	}

	public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation, Transform target) {
		int poolKey = prefab.GetInstanceID ();

		if (poolDictionary.ContainsKey (poolKey)) {
			ObjectInstance objectToReuse = poolDictionary [poolKey].Dequeue ();
            if (objectToReuse.IsBusy())
            {
				poolDictionary[poolKey].Enqueue(objectToReuse);
				objectToReuse = new ObjectInstance(Instantiate(prefab) as GameObject);

			}
			poolDictionary [poolKey].Enqueue (objectToReuse);

			objectToReuse.Reuse (position, rotation, target);
		}
	}

	public class ObjectInstance {

		GameObject gameObject;
		Transform transform;

		bool isBusy;
		bool hasPoolObjectComponent;
		PoolObject poolObjectScript;

		public ObjectInstance(GameObject objectInstance) {
			gameObject = objectInstance;
			transform = gameObject.transform;
			gameObject.SetActive(false);

			if (gameObject.GetComponent<PoolObject>()) {
				hasPoolObjectComponent = true;
				poolObjectScript = gameObject.GetComponent<PoolObject>();
			}
		}

		public bool IsBusy()
        {
			if (hasPoolObjectComponent)
			{
				return poolObjectScript.IsBusy;
			}
			return false;
		}

		public void Reuse(Vector3 position, Quaternion rotation, Transform target) {
			gameObject.SetActive (true);
			Debug.Log("BULLET11 RIATTIVATO");
			transform.position = position;
			transform.rotation = rotation;

			if (hasPoolObjectComponent) {
				poolObjectScript.OnObjectReuse (target);
			}
		}

		public void SetParent(Transform parent) {
			transform.parent = parent;
		}
	}
}
