using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Machine_GUI
{
	class Solvers
	{
		public List<string> SolverIDs { get; set; }

		/// <summary>
		/// Get available solver IDs to this.SolverIDs.
		/// </summary>
		public void GetIDs()
		{
			Machine machine = new Machine();
			this.SolverIDs = machine.AwakeMachine("-l");
		}
	}
}
