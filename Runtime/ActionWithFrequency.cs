using System;

namespace Packages.UpdateManagement
{
	class ActionWithFrequency : ActionWithPeriod
	{
		public ActionWithFrequency(Action action, float frequency) : base(action, 1/frequency) { }
	}
}