using System.Collections.Generic;

namespace Implementation.Data
{
    [DbContextConfiguration("DialogBoxData")]
    public interface IWholeDialogBoxData : IUniqueIndex
    {
        Dictionary<int, ISingleDialogData> Dialog { get; set; }
		List<DialogResponse> Responses { get; set; }
    }
}
