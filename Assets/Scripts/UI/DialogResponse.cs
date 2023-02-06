using System.Collections;
using System.Collections.Generic;
using Implementation.Data;

public class DialogResponse
{
	public string Text { get; set; }
	public IWholeDialogBoxData NextDialog { get; set; }
}
