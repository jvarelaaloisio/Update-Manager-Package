using System;
using UnityEngine;

namespace VarelaAloisio.UpdateManagement.Runtime
{
	public class ActionOverTime : IUpdateable
	{
		private readonly int _sceneIndex;
		private float _currentTime = 0;
		private readonly Action _action;
		private readonly Action _onFinish;

		public float Duration { get; }
		public bool IsRunning { get; private set; }

		public ActionOverTime(float duration, Action<float> action, int sceneIndex, bool giveLerp = false)
		{
			Duration = duration;
			_sceneIndex = sceneIndex;

			if (giveLerp)
				_action = () => action(_currentTime / Duration);
			else
				_action = () => action(_currentTime);
		}

		public ActionOverTime(float duration, Action<float> action, Action onFinish, int sceneIndex,
			bool giveLerp = false)
		{
			Duration = duration;
			_onFinish = onFinish;
			_sceneIndex = sceneIndex;

			if (giveLerp)
				_action = () => action(_currentTime / Duration);
			else
				_action = () => action(_currentTime);
		}

		public ActionOverTime(float duration, Action action, int sceneIndex)
		{
			Duration = duration;
			_action = action;
			_sceneIndex = sceneIndex;
		}

		public ActionOverTime(float duration, Action action, Action onFinish, int sceneIndex)
		{
			Duration = duration;
			_action = action;
			_onFinish = onFinish;
			_sceneIndex = sceneIndex;
		}

		/// <summary>
		/// Subscribes to updateManager and starts ticking
		/// </summary>
		public void StartAction()
		{
			_currentTime = 0;
			if (!IsRunning)
				UpdateManager.Subscribe(this, _sceneIndex);
			IsRunning = true;
		}

		/// <summary>
		/// Stops ticking and unsubscribes from updateManager
		/// </summary>
		public void StopAction()
		{
			IsRunning = false;
			UpdateManager.UnSubscribe(this, _sceneIndex);
		}

		/// <summary>
		/// Method used for UpdateManager. Don't Call.
		/// </summary>
		public void OnUpdate()
		{
			_currentTime += Time.deltaTime;
			if (_currentTime > Duration)
				_currentTime = Duration;
			_action();
			if (!(_currentTime >= Duration))
				return;
			_currentTime = 0;
			_onFinish?.Invoke();
			UpdateManager.UnSubscribe(this, _sceneIndex);
			IsRunning = false;
		}
	}
}