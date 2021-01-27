using System;
using UnityEngine;

namespace Packages.UpdateManagement
{
	public class ActionOverTime : IUpdateable
	{
		private float _currentTime = 0;
		public float TotalTime { get; }
		public bool IsRunning { get; private set; }
		private readonly Action _action;

		public ActionOverTime(float time, Action<float> action, bool giveLerp = false)
		{
			this.TotalTime = time;
			if (giveLerp)
			{
				this._action = () => action(_currentTime / TotalTime);
			}
			else
			{
				this._action = () => action(_currentTime);
			}
		}

		public ActionOverTime(float time, Action action)
		{
			this.TotalTime = time;
			this._action = action;
		}

		/// <summary>
		/// Subscribes to updateManager and starts ticking
		/// </summary>
		public void StartAction()
		{
			IsRunning = true;
			_currentTime = 0;
			UpdateManager.UnSubscribe(this);
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
			if (_currentTime > TotalTime) _currentTime = TotalTime;
			_action();
			if (_currentTime >= TotalTime)
			{
				_currentTime = 0;
				UpdateManager.UnSubscribe(this);
			}
		}
	}
}