using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParallaxLayer
{
	public string layerName = "ParallaxLayer";
	public Sprite layerSprite;
	public float speedX = 1f;
	public float speedY = 1f;
	public int orderInLayer = 0;
	public Vector2 position = new Vector2(0f, 0f);
	public bool verticalParallax = true;
	public bool horizontalRepeat = false;
	public float horizontalBoundsOffset = 10f;
	public bool horizontalLooping = false;
	public List<GameObject> loopingBackgrounds = new List<GameObject>();
	public bool verticalRepeat = false;
	public float verticalBoundsOffset = 10f;
	public bool showBounds = false;
	public Color boundsColor = Color.red;
	public GameObject layerGO;
}

[ExecuteInEditMode]
public class ParallaxCtrl : MonoBehaviour
{
	[SerializeField] public List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();
	[SerializeField] private Camera parallaxCam;
	private Vector2 oldCamPosition;
	private float verticalBound;
	private float horizontalBound;
	private Vector2 delta = new Vector2();

	// Start is called before the first frame update
	void Start()
	{
		if (parallaxCam == null)
		{
			parallaxCam = Camera.main;
		}

		oldCamPosition = parallaxCam.transform.position;
	}

	// Update is called once per frame
	void Update()
	{	
		if (oldCamPosition.x != parallaxCam.transform.position.x || oldCamPosition.y != parallaxCam.transform.position.y)
		{			
			delta.x = oldCamPosition.x - parallaxCam.transform.position.x;
			delta.y = oldCamPosition.y - parallaxCam.transform.position.y;
			oldCamPosition = parallaxCam.transform.position;
			verticalBound = parallaxCam.orthographicSize;
			horizontalBound = verticalBound * Screen.width / Screen.height;
			Move(delta);
		}

		foreach (ParallaxLayer paralLayer in parallaxLayers)
		{
			if (paralLayer.showBounds)
			{
				if (paralLayer.horizontalRepeat && !paralLayer.horizontalLooping)
				{
					Debug.DrawLine(new Vector2(parallaxCam.transform.position.x - horizontalBound - paralLayer.horizontalBoundsOffset, parallaxCam.transform.position.y - verticalBound),
						new Vector2(parallaxCam.transform.position.x - horizontalBound - paralLayer.horizontalBoundsOffset, parallaxCam.transform.position.y + verticalBound), paralLayer.boundsColor);
					Debug.DrawLine(new Vector2(parallaxCam.transform.position.x + horizontalBound + paralLayer.horizontalBoundsOffset, parallaxCam.transform.position.y - verticalBound),
						new Vector2(parallaxCam.transform.position.x + horizontalBound + paralLayer.horizontalBoundsOffset, parallaxCam.transform.position.y + verticalBound), paralLayer.boundsColor);
				}

				if (paralLayer.verticalRepeat)
				{

					Debug.DrawLine(new Vector2(parallaxCam.transform.position.x - horizontalBound, parallaxCam.transform.position.y - verticalBound - paralLayer.verticalBoundsOffset),
						new Vector2(parallaxCam.transform.position.x + horizontalBound, parallaxCam.transform.position.y - verticalBound - paralLayer.verticalBoundsOffset), paralLayer.boundsColor);
					Debug.DrawLine(new Vector2(parallaxCam.transform.position.x - horizontalBound, parallaxCam.transform.position.y + verticalBound + paralLayer.verticalBoundsOffset),
						new Vector2(parallaxCam.transform.position.x + horizontalBound, parallaxCam.transform.position.y + verticalBound + paralLayer.verticalBoundsOffset), paralLayer.boundsColor);
				}
			}
		}
	}

	private void Move(Vector2 delta)
	{
		foreach (ParallaxLayer paralLayer in parallaxLayers)
		{
			paralLayer.position = paralLayer.layerGO.transform.position;
			paralLayer.position.x -= delta.x * paralLayer.speedX;
			if (paralLayer.verticalParallax)
			{
				paralLayer.position.y += delta.y * paralLayer.speedY;
			}
			else
			{
				paralLayer.position.y -= delta.y;
			}

			if (paralLayer.horizontalRepeat && !paralLayer.horizontalLooping)
			{

				List<Transform> layerChildren = new List<Transform>(paralLayer.layerGO.GetComponentsInChildren<Transform>());
				layerChildren.Remove(paralLayer.layerGO.transform);
				//Transform[] layerChildren = paralLayer.layerGO.GetComponentsInChildren<Transform>();
				if (layerChildren.Count == 0)
				{
					if (paralLayer.position.x < parallaxCam.transform.position.x - horizontalBound - paralLayer.horizontalBoundsOffset)
					{
						paralLayer.position.x = parallaxCam.transform.position.x + horizontalBound + paralLayer.horizontalBoundsOffset;
					}
					else if (paralLayer.position.x > parallaxCam.transform.position.x + horizontalBound + paralLayer.horizontalBoundsOffset)
					{
						paralLayer.position.x = parallaxCam.transform.position.x - horizontalBound - paralLayer.horizontalBoundsOffset;
					}
				}
				else
				{					
					foreach (Transform child in layerChildren)
					{
						if (child.position.x < parallaxCam.transform.position.x - horizontalBound - paralLayer.horizontalBoundsOffset)
						{
							child.position = new Vector2(parallaxCam.transform.position.x + horizontalBound + paralLayer.horizontalBoundsOffset, child.position.y);
						}
						else if (child.position.x > parallaxCam.transform.position.x + horizontalBound + paralLayer.horizontalBoundsOffset)
						{
							child.position = new Vector2(parallaxCam.transform.position.x - horizontalBound - paralLayer.horizontalBoundsOffset, child.position.y);
						}
					}
				}
				
			}
			if (paralLayer.verticalRepeat)
			{
				if (paralLayer.position.y < parallaxCam.transform.position.y - verticalBound - paralLayer.verticalBoundsOffset)
				{
					paralLayer.position.y = parallaxCam.transform.position.y + verticalBound + paralLayer.verticalBoundsOffset;
				}
				else if (paralLayer.position.y > parallaxCam.transform.position.y + verticalBound + paralLayer.verticalBoundsOffset)
				{
					paralLayer.position.y = parallaxCam.transform.position.y - verticalBound - paralLayer.verticalBoundsOffset;
				}
			}

			paralLayer.layerGO.transform.position = paralLayer.position;

			//Repeat looping background
			if (paralLayer.horizontalLooping && paralLayer.loopingBackgrounds.Count == 3)
			{
				float diffFirst = parallaxCam.transform.position.x - paralLayer.loopingBackgrounds[0].transform.position.x;
				float diffLast = parallaxCam.transform.position.x - paralLayer.loopingBackgrounds[2].transform.position.x;

				if (diffFirst > paralLayer.loopingBackgrounds[0].GetComponent<SpriteRenderer>().size.x && delta.x < 0)
				{
					paralLayer.loopingBackgrounds = Shift(1, paralLayer.loopingBackgrounds);
				}
				else if (diffLast < -paralLayer.loopingBackgrounds[0].GetComponent<SpriteRenderer>().size.x && delta.x > 0)
				{
					paralLayer.loopingBackgrounds = Shift(-1, paralLayer.loopingBackgrounds);
				}
			}
		}
	}

	private List<GameObject> Shift(int dir, List<GameObject> backgrounds)
	{
		GameObject tmp = dir == 1 ? backgrounds[0] : backgrounds[2];
		tmp.GetComponent<SpriteRenderer>().flipX = !tmp.GetComponent<SpriteRenderer>().flipX;
		if (dir == 1)
		{
			tmp.transform.position = new Vector3(tmp.transform.position.x + 3 * tmp.GetComponent<SpriteRenderer>().size.x, tmp.transform.position.y, tmp.transform.position.z);
			backgrounds[0] = backgrounds[1];
			backgrounds[1] = backgrounds[2];
			backgrounds[2] = tmp;
		}
		else
		{
			tmp.transform.position = new Vector3(tmp.transform.position.x - 3 * tmp.GetComponent<SpriteRenderer>().size.x, tmp.transform.position.y, tmp.transform.position.z);
			backgrounds[2] = backgrounds[1];
			backgrounds[1] = backgrounds[0];
			backgrounds[0] = tmp;
		}

		return backgrounds;
	}

	public GameObject CreateObject()
	{
		GameObject go = new GameObject();
		go.AddComponent<SpriteRenderer>();
		go.transform.parent = transform;
		go.name = "ParallaxLayer";

		return go;
	}

	public void ChangedName(int index)
	{
		parallaxLayers[index].layerGO.name = parallaxLayers[index].layerName;
	}

	public void ChangedOrderInLayer(int index)
	{
		parallaxLayers[index].layerGO.GetComponent<SpriteRenderer>().sortingOrder = parallaxLayers[index].orderInLayer;
	}

	public void ChangedSprite(int index)
	{
		parallaxLayers[index].layerGO.GetComponent<SpriteRenderer>().sprite = parallaxLayers[index].layerSprite;
	}

	public void ChangedPosition(int index)
	{
		Vector2 newPosition = parallaxLayers[index].layerGO.transform.localPosition;
		newPosition = parallaxLayers[index].position;
		parallaxLayers[index].layerGO.transform.localPosition = newPosition;
	}

	public bool CheckLoopingBackgrounds(int index)
	{
		for (int i = 0; i < 2; i++)
		{
			if (parallaxLayers[index].loopingBackgrounds[i] != parallaxLayers[index].layerGO.transform.GetChild(0).gameObject
				&& parallaxLayers[index].loopingBackgrounds[i] != parallaxLayers[index].layerGO.transform.GetChild(1).gameObject
				&& parallaxLayers[index].loopingBackgrounds[i] != parallaxLayers[index].layerGO.transform.GetChild(2).gameObject)
			{
				return false;
			}
		}

		return true;
	}

	public void DisableLooping(int index)
	{
		while (parallaxLayers[index].layerGO.transform.childCount > 1)
		{
			DestroyImmediate(parallaxLayers[index].layerGO.transform.GetChild(1).gameObject);
		}
		
		parallaxLayers[index].loopingBackgrounds.Clear();
	}

	public void EnableLooping(int index)
	{
		if (parallaxLayers[index].loopingBackgrounds.Count <= 1)
		{
			GameObject go;
			if (parallaxLayers[index].layerGO.transform.childCount == 1)
			{
				go = parallaxLayers[index].layerGO.transform.GetChild(0).gameObject; 
			}
			else
			{
				go = parallaxLayers[index].layerGO;
			}
			parallaxLayers[index].loopingBackgrounds.Add(go);
			for (int i = 1; i < 3; i++)
			{
				GameObject newChild = Instantiate(go, 
					(parallaxLayers[index].loopingBackgrounds[0].transform.position + new Vector3(parallaxLayers[index].loopingBackgrounds[0].GetComponent<SpriteRenderer>().size.x * i, 0f, 0f)),
					Quaternion.identity, parallaxLayers[index].layerGO.transform);
				newChild.GetComponent<SpriteRenderer>().flipX = i % 2 == 0;
				newChild.name = parallaxLayers[index].loopingBackgrounds[0].name + (i + 1);
				parallaxLayers[index].loopingBackgrounds.Add(newChild);
			}
		}
		}

}
