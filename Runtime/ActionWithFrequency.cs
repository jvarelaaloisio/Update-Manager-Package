using System;
using UnityEngine;

namespace Packages.UpdateManagement
{
	public class ActionWithFrequency : IUpdateable
	{
		private float _currentTime = 0;
		public float Period { get; }
		public bool IsRunning { get; private set; }
		private readonly Action _action;

		public ActionWithFrequency(Action action, float period)
		{
			Period = period;
			_action = action;
		}

		/// <summary>
		/// Subscribes to updateManager and starts ticking
		/// </summary>
		public void StartAction()
		{
			IsRunning = true;
			_currentTime = 0;
			if (!IsRunning)
				UpdateManager.Subscribe(this);
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