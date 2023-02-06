using System.Collections.Generic;
using UnityEngine;
using DiContainerLibrary.DiContainer;
using Implementation.Data;
using UnityEngine.SceneManagement;
using System.Linq;


public class LevelSummary : MonoBehaviour
{
	[InjectDiContainter]
	private IGameInformation gameInformation { get; set; }

	//private Dictionary<string, Counter> secrets = new Dictionary<string, Counter>();
	private List<LevelData> levelData = new List<LevelData>();
	private string currentLevel;

	private void Start()
	{
		DiContainerInitializor.RegisterObject(this);
		currentLevel = SceneManager.GetActiveScene().name;
	}

	public void SaveSecrets(int collected, int total)
	{
		//secrets.Add(currentLevel, new Counter(collected, total));
		levelData.Add(new LevelData
		{
			LevelName = currentLevel,
			SecretsFound = collected,
			TotalNumberOfSecrets = total
		});
	}

	public LevelData GetSecrets(string name)
	{
		return levelData.ToArray().FirstOrDefault(x => x.LevelName == name);
	}

	public List<LevelData> GetAllSecrets()
	{
		return levelData;
	}

	public void ClearAllSecrets()
	{
		levelData.Clear();
	}

}

//public class Counter
//{
//	int collected;
//	int total;

//	public Counter(int coll, int tot)
//	{
//		collected = coll;
//		total = tot;
//	}
//}