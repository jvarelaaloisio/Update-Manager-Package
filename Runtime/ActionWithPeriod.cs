using System;
using UnityEngine;

namespace VarelaAloisio.UpdateManagement.Runtime
{
	public class ActionWithPeriod : IUpdateable
	{
		private float _currentTime = 0;
		public float Period { get; }
		public bool IsRunning { get; private set; }
		private readonly Action _action;

		public ActionWithPeriod(Action action, float period)
		{
			Period = period;
			_action = action;
		}

		/// <summary>
		/// Subscribes to updateManager and starts ticking
		/// </summary>
		public void StartAction()
		{
			_currentTime = 0;
			if (!IsRunning)
				UpdateManager.Subscribe(this);
			IsRunning = true;
		}

		/// <summary>
		/// Stops ticking and unsubscribes from updateManager
		/// </summary>
		public void StopAction()
		{
			IsRunning = false;
			UpdateManager.UnSubscribe(this);
		}

		/// <summary>
		/// Method used for UpdateManager. Don't Call.
		/// </summary>
		public void OnUpdate()
		{
			_currentTime += Time.deltaTime;
			if (_currentTime < Period)
				return;
			_currentTime = 0;
			_action();
		}
	}
}