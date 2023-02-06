using System;
using System.Collections.Generic;

namespace Implementation.Data
{
    [Serializable]
    public class WholeDialogData : IWholeDialogBoxData
    {
        /// <inheritdoc />
        public string Id { get; set; }

        public Dictionary<int, ISingleDialogData> Dialog { get; set; }
		public List<DialogResponse> Responses { get; set; }

		public WholeDialogData()
        {
            this.Dialog = new Dictionary<int, ISingleDialogData>();
			this.Responses = new List<DialogResponse>();

			//DialogResponse dr = new DialogResponse();
			//dr.Text = "Hi";
			//dr.NextDialog = this;
			//Responses.Add(dr);

			DialogResponse drLong = new DialogResponse();
			drLong.Text = "Sorry, I got distracted for a bit and didn't catch that. Could we repeat this interaction?";
			drLong.NextDialog = this;
			Responses.Add(drLong);

			DialogResponse dr2 = new DialogResponse();
			dr2.Text = "Bye";
			Responses.Add(dr2);

			//DialogResponse dr3 = new DialogResponse();
			//dr3.Text = "Farewell. Hopefully we shall meet again one day, not so far away.";
			//Responses.Add(dr3);

			//DialogResponse dr4 = new DialogResponse();
			//dr4.Text = "Ciao";
			//Responses.Add(dr4);

			//DialogResponse dr5 = new DialogResponse();
			//dr5.Text = "Sayonara";
			//Responses.Add(dr5);

			//DialogResponse dr6 = new DialogResponse();
			//dr6.Text = "Pls werk as I'm too tired to keep doing this. I hate UI stuff.";
			//Responses.Add(dr6);

		}
    }
}
