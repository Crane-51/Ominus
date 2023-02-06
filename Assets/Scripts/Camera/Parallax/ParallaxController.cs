using System.Collections.Generic;
using System.Linq;
using DiContainerLibrary.DiContainer;
using General.State;
using Implementation.Data;
using UnityEngine;

/***************************************************************************
	DEPRECATED: Use parallax controller prefab (which uses ParallaxCtrl)
****************************************************************************/

public class ParallaxController : HighPriorityState {

    private const float borderTolerance = 0.25f;

    /// <summary>
    /// Speed of following.
    /// </summary>
    public float Speed;

    /// <summary>
    /// List of sprites that will be used for parallax.
    /// </summary>
    public List<GameObject> ParallaxSpriteList;

    /// <summary>
    /// Parallax used for sorting.
    /// </summary>
    private Dictionary<int, GameObject> OrderedParallax { get; set; }

    /// <summary>
    /// Gets or sets enemy data.
    /// </summary>
    [InjectDiContainter]
    private IGameInformation gameInformation { get; set; }

    /// <summary>
    /// Parallax sprite that is closest to player.
    /// </summary>
    private GameObject currentClosest { get; set; }

    /// <summary>
    /// Screen height in world space.
    /// </summary>
    private float WorldScreenHeight { get; set; }

    /// <summary>
    /// Screen width in world space.
    /// </summary>
    private float WorldScreenWidth { get; set; }

    /// <summary>
    /// Last position of player (before refresh).
    /// </summary>
    private float LastPosition { get; set; }

    protected override void Initialization_State()
    {
        base.Initialization_State();
        WorldScreenHeight = gameInformation.Camera.orthographicSize * 4;
        WorldScreenWidth = WorldScreenHeight / Screen.height * Screen.width;
        OrderedParallax = new Dictionary<int, GameObject>();

        for (int i = 0; i<ParallaxSpriteList.Count; i++)
        {
            OrderedParallax.Add(i-1, ResizeSpriteToScreen(Instantiate(ParallaxSpriteList[i], transform), i));
            OrderedParallax[i-1].layer = LayerMask.NameToLayer("Parallax");
            OrderedParallax[i - 1].transform.localPosition = new Vector2((WorldScreenWidth - borderTolerance) * (i-1), 0);
        }
    }

    public void LateUpdate()
    {
        var getClosestPoint = OrderedParallax.Values.Min(x => Vector2.Distance(x.transform.position, gameInformation.Camera.transform.position));
        var closestSprite = OrderedParallax.Values.FirstOrDefault(x => Vector2.Distance(x.transform.position, gameInformation.Camera.transform.position) == getClosestPoint);

        if(closestSprite != OrderedParallax[0])
        {
            OrderedParallax = ReOrderParallax(closestSprite);
        }

        transform.position = new Vector2(transform.position.x, gameInformation.Camera.transform.position.y);
        if ( Mathf.Abs(gameInformation.Camera.transform.position.x - LastPosition) > 0.1f)
        {
            LastPosition = gameInformation.Camera.transform.position.x;
            transform.position = Vector2.Lerp(transform.position, new Vector2(gameInformation.Camera.transform.position.x, transform.position.y), Speed * Time.deltaTime);
        }
    }


    private Dictionary<int, GameObject> ReOrderParallax(GameObject closestParallax)
    {
        var newDict = new Dictionary<int, GameObject>();
        var farestSprite = OrderedParallax.FirstOrDefault(x => x.Value != closestParallax  && x.Key != 0).Value;

        newDict.Add(0, closestParallax);

        if(closestParallax.transform.position.x - farestSprite.transform.position.x > 0)
        {
            farestSprite.transform.localPosition = new Vector2(OrderedParallax[1].transform.localPosition.x + (WorldScreenWidth - borderTolerance), 0);
            newDict.Add(1, farestSprite);
            newDict.Add(-1, OrderedParallax[0]);
        }
        else
        {
            farestSprite.transform.localPosition = new Vector2(OrderedParallax[-1].transform.localPosition.x - (WorldScreenWidth - borderTolerance), 0);
            newDict.Add(-1, farestSprite);
            newDict.Add(1, OrderedParallax[0]);
        }

        return newDict;
    }

    private GameObject ResizeSpriteToScreen(GameObject paralaxSprtie, int position)
    {
        var sr = paralaxSprtie.GetComponent<SpriteRenderer>();
        if (sr == null) return null;

        transform.localScale = new Vector3(1, 1, 1);

        var width = sr.sprite.bounds.size.x;
        var height = sr.sprite.bounds.size.y;

        paralaxSprtie.transform.localScale = new Vector2(WorldScreenWidth / width, WorldScreenHeight / height);

        paralaxSprtie.transform.position = new Vector2(WorldScreenWidth * position, transform.position.y);

        return paralaxSprtie;
    }
}
