using UnityEngine;

public class Torchlight : MonoBehaviour {

    [SerializeField] private float SpeedOfStrenghtChange;

	[SerializeField] private float MaxStrenght;
	[SerializeField] private float MinStrenght;
	[SerializeField] private Light lightEmission { get; set; }

    private float NextIntensity { get; set; }

    // Use this for initialization
    void Start () {
        lightEmission = GetComponent<Light>();
        lightEmission.intensity= NextIntensity = Random.Range(MinStrenght, MaxStrenght);
	}

    // Update is called once per frame
    void Update()
    {
        UpdateLightProperties();
    }

    private void UpdateLightProperties()
    {
        if (lightEmission.intensity < NextIntensity)
        {
            lightEmission.intensity += SpeedOfStrenghtChange * Time.deltaTime;  
            
            if(lightEmission.intensity > NextIntensity)
            {
                NextIntensity = Random.Range(MinStrenght, MaxStrenght);
            }
        }
        else
        {
            lightEmission.intensity -= SpeedOfStrenghtChange * Time.deltaTime;

            if (lightEmission.intensity < NextIntensity)
            {
                NextIntensity = Random.Range(MinStrenght, MaxStrenght);
            }
        }
    }
}
