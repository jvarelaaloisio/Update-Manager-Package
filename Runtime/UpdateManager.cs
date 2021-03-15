using UnityEngine;
using System;

namespace VarelaAloisio.UpdateManagement.Runtime
{
	public class UpdateManager : MonoBehaviour
	{
		private bool _isPause;
		private Action _update;
		private Action _fixedUpdate;
		private Action _lateUpdate;
		private static UpdateManager _instance;
		private static bool _isQuittingApplication = false;
		private static UpdateManager Instance
		{
			get
			{
				if (_instance != null || FindObjectOfType<UpdateManager>() != null) return _instance;
				GameObject go = new GameObject("UpdateManager");
				_instance = go.AddComponent<UpdateManager>();
				return _instance;
			}
		}

		private void Awake()
		{
			if (_instance == null) _instance = this;
			else if (_instance != this) Destroy(this);
		}
		private void OnApplicationQuit()
		{
			_isQuittingApplication = true;
		}

		#region Update Messages
		private void Update()
		{
			if (_isPause)
				return;
			_update?.Invoke();
		}
		private void FixedUpdate()
		{
			if (_isPause)
				return;
			_fixedUpdate?.Invoke();
		}
		private void LateUpdate()
		{
			if (_isPause)
				return;
			_lateUpdate?.Invoke();
		}
		#endregion

		/// <summary>
		/// Resets the delegates.
		/// Use this when changing scenes
		/// </summary>
		public void ResetUpdateDelegates()
		{
			_update = null;
			_fixedUpdate = null;
			_lateUpdate = null;
		}

		/// <summary>
		/// Sets the Pause Flag (Does not affect physics, only the <b>OnUpdate</b> method)
		/// </summary>
		/// <param name="value">New Pause state</param>
		public static void SetPause(bool value) => Instance._isPause = value;
		
		#region Subscriptions
		/// <summary>
		/// Subscribes to Update
		/// </summary>
		/// <param name="updateable"></param>
		public static void Subscribe(IUpdateable updateable)
		{
			if (_isQuittingApplication)
				return;
			Instance._update += updateable.OnUpdate;
		}
		/// <summary>
		/// Unsubscribes from Update
		/// </summary>
		/// <param name="updateable"></param>
		public static void UnSubscribe(IUpdateable updateable)
		{
			if (_isQuittingApplication)
				return;
			Instance._update -= updateable.OnUpdate;
		}
		/// <summary>
		/// subscribes to FixedUpdate
		/// </summary>
		/// <param name="fixedUpdateable"></param>
		public static void Subscribe(IFixedUpdateable fixedUpdateable)
		{
			if (_isQuittingApplication)
				return;
			Instance._update += fixedUpdateable.OnFixedUpdate;
		}
		/// <summary>
		/// Unsubscribes from FixedUpdate
		/// </summary>
		/// <param name="fixedUpdateable"></param>
		public static void UnSubscribe(IFixedUpdateable fixedUpdateable)
		{
			if (_isQuittingApplication)
				return;
			Instance._update -= fixedUpdateable.OnFixedUpdate;
		}
		/// <summary>
		/// subscribes to LateUpdate
		/// </summary>
		/// <param name="lateUpdateable"></param>
		public static void Subscribe(ILateUpdateable lateUpdateable)
		{
			if (_isQuittingApplication)
				return;
			Instance._lateUpdate += lateUpdateable.OnLateUpdate;
		}
		/// <summary>
		/// Unsubscribes from LateUpdate
		/// </summary>
		/// <param name="lateUpdateable"></param>
		public static void UnSubscribe(ILateUpdateable lateUpdateable)
		{
			if (_isQuittingApplication)
				return;
			Instance._lateUpdate -= lateUpdateable.OnLateUpdate;
		}
		#endregion
	}
}