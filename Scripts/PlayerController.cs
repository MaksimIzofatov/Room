using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _distanceRay;
    [SerializeField] private int _force;
    private InteractableItem _item;
    private InteractableItem _oldItem;
    [SerializeField]
    private Transform _hand;
    private Camera _camera;


    private void Start()
    {
        _camera = Camera.main;
    }

    
    private void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

		Physics.Raycast(ray, out var info, _distanceRay);
		if (info.collider.TryGetComponent<InteractableItem>(out var item))
		{
			var isNewObj = item != _oldItem;
            if (isNewObj)
            {
				item.SetFocus();

				if (_oldItem != null)
                {
                    _oldItem.RemoveFocus();
                }

				_oldItem = item;
			}

            if (Input.GetKeyDown(KeyCode.E))
            {
                if(_item != null)
                {
					_item.GetComponent<Rigidbody>().isKinematic = false;
					_item.transform.parent = null;
				}
                    _item = item;
                    _item.transform.position = _hand.position;
                    _item.transform.rotation = _hand.rotation;
                    _item.transform.parent = _hand;
                    _item.GetComponent<Rigidbody>().isKinematic = true;
                
            }
		}
        else if(_oldItem  != null)
        {
            _oldItem.RemoveFocus();
            _oldItem = null;
        }

		if (info.collider.TryGetComponent<Door>(out var door)) 
        {

			if(Input.GetKeyDown(KeyCode.E))
			{
                door.SwitchDoorState();
            }
        }

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			if (_item != null)
			{
				_item.GetComponent<Rigidbody>().isKinematic = false;
				_item.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, _force), ForceMode.Force);
				_item.transform.parent = null;
			}
		}
	}

    
}
