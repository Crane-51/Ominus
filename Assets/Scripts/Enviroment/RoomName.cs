using System.Collections;
using UnityEngine;

public class RoomName : MonoBehaviour
{
    public float displayForSeconds = 2;
    public float dissolveForSeconds = 2;

    private float dissolveValue = 0;
    private float solveValue = 1;
    private float dissolveCounter = 0f;
    
    private Material mat;

    // Start is called before the first frame update
    void Awake()
    {
        mat = GetComponent<Renderer>().material;
        mat.SetFloat("_Fade", dissolveValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(Solve());
        }
    }

    IEnumerator Solve()
    {
        while (dissolveValue < 1f)
        {
            dissolveValue += Time.deltaTime / dissolveForSeconds;
            dissolveValue = Mathf.Clamp01(dissolveValue);

            if (dissolveValue == 1)
            {
                StartCoroutine(Hold());
            }

            mat.SetFloat("_Fade", dissolveValue);
            yield return null;
        }
    }

    IEnumerator Hold()
    {
        while (displayForSeconds > 0f)
        {
            displayForSeconds -= Time.deltaTime;
            if (displayForSeconds <= 0)
            {
                StartCoroutine(Dissolve());
            }
            yield return null;
        }
    }


    IEnumerator Dissolve()
    {
        while (solveValue >0f)
        {
            solveValue -= Time.deltaTime / dissolveForSeconds;
            solveValue = Mathf.Clamp01(solveValue);

            mat.SetFloat("_Fade", solveValue);
            yield return null;
        }
    }
}
