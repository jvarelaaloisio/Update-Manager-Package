using System;

namespace Packages.UpdateManagement
{
	public class ActionWithFrequency : ActionWithPeriod
	{
		public ActionWithFrequency(Action action, float frequency) : base(action, 1/frequency) { }
	}
}