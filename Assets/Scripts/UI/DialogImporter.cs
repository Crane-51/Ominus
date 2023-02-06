using UnityEngine;
using Implementation.Data;

public class DialogImporter : MonoBehaviour
{
	[SerializeField] private TextAsset[] textFiles;
	private string[] textLines;
	private string[] rawLine;
	private SingleDialogData singleDialog;
	private WholeDialogData wholeDialog;

	public static DialogImporter instance;

    void Awake()
    {
		instance = this;
    }

	public WholeDialogData ImportDialog(string dialogId, int startLine = 0, int endLine = 0)
	{
		foreach (var textFile in textFiles)
		{
			if (textFile.name == dialogId)
			{
				wholeDialog = new WholeDialogData();
				wholeDialog.Id = textFile.name;

				textLines = textFile.text.Split('\n');
				int index = 1;
				if (startLine > endLine || startLine == 0 || endLine == 0)
				{
					startLine = 1;
					endLine = textLines.Length;
				}
				//foreach (string line in textLines)
				for (int i = startLine - 1; i < endLine; i++)
				{
					//if (i >= startLine - 1 && i < endLine)
					//{
						rawLine = textLines[i].Split('#');
						singleDialog = new SingleDialogData
						{
							CharacterIcon = rawLine[0],
							CharacterName = rawLine[1],
							Text = rawLine[2]
						};

						wholeDialog.Dialog.Add(index++, singleDialog); 
					//}
				}
				//foreach (string line in textLines)
				//{
				//	rawLine = line.Split('#');
				//	singleDialog = new SingleDialogData
				//	{
				//		CharacterIcon = rawLine[0],
				//		CharacterName = rawLine[1],
				//		Text = rawLine[2]
				//	};

				//	wholeDialog.Dialog.Add(i++, singleDialog);
				//}

				return wholeDialog;
			} 
		}

		return null;
	}
}
