using System;
using UnityEngine;

namespace Packages.UpdateManagement
{
	public class CountDownTimer : IUpdateable
	{
		private float _currentTime = 0;
		public float TotalTime { get; }
		private readonly Action onFinished;
		public bool IsTicking { get; private set; }

		public CountDownTimer(float time, Action onFinished)
		{
			this.TotalTime = time;
			this.onFinished = onFinished;
		}

		/// <summary>
		/// Subscribes to updateManager and starts timer
		/// </summary>
		public void StartTimer()
		{
			_currentTime = 0;
			if (!IsTicking)
				UpdateManager.Subscribe(this);
			IsTicking = true;
		}

		/// <summary>
		/// Stops timer and unsubscribes from updateManager
		/// </summary>
		public void StopTimer()
		{
			IsTicking = false;
			UpdateManager.UnSubscribe(this);
		}

		/// <summary>
		/// Method used for UpdateManager. Don't Call.
		/// </summary>
		public void OnUpdate()
		{
			_currentTime += Time.deltaTime;
			if (_currentTime >= TotalTime)
			{
				StopTimer();
				onFinished?.Invoke();
			}
		}
	}
}