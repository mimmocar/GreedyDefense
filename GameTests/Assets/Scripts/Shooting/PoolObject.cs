using UnityEngine;
using System.Collections;

public class PoolObject : MonoBehaviour {
	private bool isBusy;

	public bool IsBusy
    {
        get
        {
			return isBusy;
        }
    }
	public virtual void OnObjectReuse(Transform target) {
		isBusy = true;
	}

	public void Destroy() {
		isBusy = false;
		gameObject.SetActive (false);
		Debug.Log("BULLET11 DISATTIVATO");
	}
}
