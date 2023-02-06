using System.Linq;
using General.State;
using UnityEngine;

[ExecuteInEditMode]
public class RuntimeStateCompiler : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var allStates = gameObject.GetComponents<State>();
        var child = gameObject.transform.GetChild(0);
        if (allStates.Length > 0)
        {
            var allSoundData = child.GetComponents<StateSoundData>().ToList();
            foreach (var state in allStates)
            {
                allSoundData.ForEach(x => { if (x.State == null) { DestroyImmediate(x); } });
                if (allSoundData.FirstOrDefault(x => x.State == state) == null)
                {
                    var soundData = child.gameObject.AddComponent<StateSoundData>();
                    soundData.State = state;
                }
            }

            var emptyData = allSoundData.Where(x => x.State == null);

            foreach(var item in emptyData)
            {
                Destroy(item);
            }

        }
    }
}
