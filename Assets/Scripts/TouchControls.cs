using UnityEngine;
using Mapbox.Unity.Location;
using UnityEngine.EventSystems;

public class TouchControls : MonoBehaviour
{
	[SerializeField]
	Camera _camera;

	[SerializeField]
	TransformLocationProvider _locationProvider;

	[SerializeField]
	Transform _ARPlayer;

	Quaternion _originalRotation;
	float _originalHeight;
	[SerializeField]
	float _zoomSpeed = 50f;

	[SerializeField]
	float _panSpeed = 20f;

	Vector2?[] oldTouchPositions = { null, null };

	Vector2 oldTouchVector;
	Vector3 _delta;
	float oldTouchDistance;
	Vector3 _origin;
	Vector3 _tempZoomPosition;
	bool _wasTouching;

	bool _shouldDrag;
	bool _wasRotating;
	[SerializeField]
	bool _followTarget=true;
	//void Update()
	//{
	//	if (Input.touchCount == 0)
	//	{
	//		oldTouchPositions[0] = null;
	//		oldTouchPositions[1] = null;
	//		_shouldDrag = false;
	//		if (_wasTouching)
	//		{
	//			if (_locationProvider != null)
	//			{
	//				_locationProvider.SendLocationEvent();
	//			}
	//			_wasTouching = false;
	//		}
	//	}
	//	else if (Input.touchCount == 1)
	//	{
	//		_wasTouching = true;
	//		if (oldTouchPositions[0] == null || oldTouchPositions[1] != null)
	//		{
	//			oldTouchPositions[0] = Input.GetTouch(0).position;
	//			oldTouchPositions[1] = null;
	//		}
	//		else
	//		{
	//			Vector3 newTouchPosition = Input.GetTouch(0).position;
	//			newTouchPosition.z = _camera.transform.localPosition.y;
	//			_delta = _camera.ScreenToWorldPoint(newTouchPosition) - _camera.transform.localPosition;
	//			if (_shouldDrag == false)
	//			{
	//				_shouldDrag = true;
	//				_origin = _camera.ScreenToWorldPoint(newTouchPosition);
	//			}

	//			oldTouchPositions[0] = newTouchPosition;
	//			_camera.transform.localPosition = _origin - _delta;
	//		}
	//	}
 //  //     else
 //  //     {
 //  //         _wasTouching = true;
	//		//_wasRotating = true;
 //  //         if (oldTouchPositions[1] == null)
 //  //         {
 //  //             oldTouchPositions[0] = Input.GetTouch(0).position;
 //  //             oldTouchPositions[1] = Input.GetTouch(1).position;
 //  //             oldTouchVector = (Vector2)(oldTouchPositions[0] - oldTouchPositions[1]);
 //  //             oldTouchDistance = oldTouchVector.magnitude;
 //  //         }
 //  //         else
 //  //         {
 //  //             Vector2[] newTouchPositions = { Input.GetTouch(0).position, Input.GetTouch(1).position };
 //  //             Vector2 newTouchVector = newTouchPositions[0] - newTouchPositions[1];
 //  //             float newTouchDistance = newTouchVector.magnitude;
 //  //             transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, Mathf.Asin(Mathf.Clamp((oldTouchVector.y * newTouchVector.x - oldTouchVector.x * newTouchVector.y) / oldTouchDistance / newTouchDistance, -1f, 1f)) / 0.0174532924f));
 //  //             oldTouchPositions[0] = newTouchPositions[0];
 //  //             oldTouchPositions[1] = newTouchPositions[1];
 //  //             oldTouchVector = newTouchVector;
 //  //             oldTouchDistance = newTouchDistance;
 //  //         }
 //  //     }
 //   }


	void HandleTouch()
	{
		//float zoomFactor = 0.0f;
		//pinch to zoom. 
		switch (Input.touchCount)
		{
			case 1:
				{
					HandleMouseAndKeyBoard();
				}
				break;
			//case 2:
			//	{
			//		// Store both touches.
			//		Touch touchZero = Input.GetTouch(0);
			//		Touch touchOne = Input.GetTouch(1);

			//		// Find the position in the previous frame of each touch.
			//		Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			//		Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			//		// Find the magnitude of the vector (the distance) between the touches in each frame.
			//		float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			//		float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			//		// Find the difference in the distances between each frame.
			//		zoomFactor = 0.05f * (touchDeltaMag - prevTouchDeltaMag);
			//	}
			//	ZoomMapUsingTouchOrMouse(zoomFactor);
			//	break;
			default:
				break;
		}
	}

	void ZoomMapUsingTouchOrMouse(float zoomFactor)
	{
		var y = zoomFactor * _zoomSpeed;
		transform.localPosition += (transform.forward * y);
	}

	void HandleMouseAndKeyBoard()
	{
		if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
		{
			var mousePosition = Input.mousePosition;
			mousePosition.z = _camera.transform.localPosition.y;
			_delta = _camera.ScreenToWorldPoint(mousePosition) - _camera.transform.localPosition;
			_delta.y = 0f;
			if (_shouldDrag == false)
			{
				_shouldDrag = true;
				_origin = _camera.ScreenToWorldPoint(mousePosition);
			}
		}
		else
		{
			_shouldDrag = false;
		}

		if (_shouldDrag == true)
		{
			var offset = _origin - _delta;
			offset.y = transform.localPosition.y;
			transform.localPosition = offset;
			_followTarget = false;
		}
		else
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				return;
			}

			var x = Input.GetAxis("Horizontal");
			var z = Input.GetAxis("Vertical");
			var y = Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
			if (!(Mathf.Approximately(x, 0) && Mathf.Approximately(y, 0) && Mathf.Approximately(z, 0)))
			{
				transform.localPosition += transform.forward * y + (_originalRotation * new Vector3(x * _panSpeed, 0, z * _panSpeed));
				//_map.UpdateMap();
			}
		}


	}

	void Awake()
	{
		_originalRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
		_originalHeight = transform.position.y;
		if (_camera == null)
		{
			_camera = GetComponent<Camera>();
			if (_camera == null)
			{
				throw new System.Exception("You must have a reference camera assigned!");
			}
		}

		//if (_map == null)
		//{
		//	_map = FindObjectOfType<AbstractMap>();
		//	if (_map == null)
		//	{
		//		throw new System.Exception("You must have a reference map assigned!");
		//	}
		//}
	}

	public void ZoomOut()
    {
		_followTarget = false;
		transform.localPosition -= (transform.forward * _zoomSpeed);
	}

	public void ZoomIn()
    {
		_followTarget = false;
		transform.localPosition += (transform.forward * _zoomSpeed);
	}

	public void Follow()
    {
		_followTarget = true;
    }
	public void ResetPosition()
    {

		Vector3 newPos = new Vector3(_ARPlayer.localPosition.x, _originalHeight, _ARPlayer.localPosition.z);
		_camera.transform.localPosition = newPos;
    }

	void LateUpdate()
	{

		if (Input.touchSupported && Input.touchCount > 0)
		{
			HandleTouch();
		}
		else
		{
			HandleMouseAndKeyBoard();
		}
        if (_followTarget)
        {
			ResetPosition();
        }
	}
}
