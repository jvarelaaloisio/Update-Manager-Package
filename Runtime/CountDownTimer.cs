using System;
using UnityEngine;

namespace VarelaAloisio.UpdateManagement.Runtime
{
	public class CountDownTimer : IUpdateable
	{
		private float _currentTime = 0;
		public float TotalTime { get; }
		private readonly Action _onFinished;
		public bool IsTicking { get; private set; }

		private readonly int _sceneIndex;

		public CountDownTimer(float time, Action onFinished, int sceneIndex)
		{
			TotalTime = time;
			_onFinished = onFinished;
			_sceneIndex = sceneIndex;
		}

		/// <summary>
		/// Subscribes to updateManager and starts timer
		/// </summary>
		public void StartTimer()
		{
			_currentTime = 0;
			if (!IsTicking)
				UpdateManager.Subscribe(this, _sceneIndex);
			IsTicking = true;
		}

		/// <summary>
		/// Stops timer and unsubscribes from updateManager
		/// </summary>
		public void StopTimer()
		{
			IsTicking = false;
			UpdateManager.UnSubscribe(this, _sceneIndex);
		}

		/// <summary>
		/// Method used for UpdateManager. Don't Call.
		/// </summary>
		public void OnUpdate()
		{
			_currentTime += Time.deltaTime;
			if (!(_currentTime >= TotalTime))
				return;
			StopTimer();
			_onFinished?.Invoke();
		}
	}
}