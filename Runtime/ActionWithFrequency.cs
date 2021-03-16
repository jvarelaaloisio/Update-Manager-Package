using System;

namespace VarelaAloisio.UpdateManagement.Runtime
{
	public class ActionWithFrequency : ActionWithPeriod
	{
		public ActionWithFrequency(Action action, float frequency, int sceneIndex) : base(action, 1/frequency, sceneIndex) { }
	}
}