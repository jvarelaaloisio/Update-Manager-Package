using System;
using UnityEngine;

namespace Packages.UpdateManagement
{
	public class ActionOverTime : IUpdateable
	{
		private float _currentTime = 0;

		public float Duration { get; }
		public bool IsRunning { get; private set; }
		private readonly Action _action;
		private readonly Action _onFinish;

		public ActionOverTime(float duration, Action<float> action, bool giveLerp = false)
		{
			this.Duration = duration;
			if (giveLerp)
			{
				this._action = () => action(_currentTime / Duration);
			}
			else
			{
				this._action = () => action(_currentTime);
			}
		}
		public ActionOverTime(float duration, Action<float> action, Action onFinish, bool giveLerp = false)
		{
			this.Duration = duration;
			if (giveLerp)
			{
				this._action = () => action(_currentTime / Duration);
			}
			else
			{
				this._action = () => action(_currentTime);
			}

			_onFinish = onFinish;
		}

		public ActionOverTime(float duration, Action action)
		{
			this.Duration = duration;
			this._action = action;
		}
		public ActionOverTime(float duration, Action action, Action onFinish)
		{
			this.Duration = duration;
			this._action = action;
			_onFinish = onFinish;
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
			if (_currentTime > Duration) _currentTime = Duration;
			_action();
			if (_currentTime >= Duration)
			{
				_currentTime = 0;
				_onFinish?.Invoke();
				UpdateManager.UnSubscribe(this);
			}
		}
	}
}