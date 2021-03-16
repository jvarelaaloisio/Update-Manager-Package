using UnityEngine;
using System;
using System.Collections.Generic;

namespace VarelaAloisio.UpdateManagement.Runtime
{
	public class UpdateManager : MonoBehaviour
	{
		private bool _isPause;
		private readonly Dictionary<int, Action> _updateDictionary = new Dictionary<int, Action>();
		private readonly Dictionary<int, Action> _fixedUpdateDictionary = new Dictionary<int, Action>();
		private readonly Dictionary<int, Action> _lateUpdateDictionary = new Dictionary<int, Action>();
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
			var enumerator = _updateDictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Value?.Invoke();
			}
			enumerator.Dispose();
		}

		private void FixedUpdate()
		{
			if (_isPause)
				return;
			_fixedUpdate?.Invoke();
			var enumerator = _fixedUpdateDictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Value?.Invoke();
			}
			enumerator.Dispose();
		}

		private void LateUpdate()
		{
			if (_isPause)
				return;
			_lateUpdate?.Invoke();
			var enumerator = _lateUpdateDictionary.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Value?.Invoke();
			}
			enumerator.Dispose();
		}

		#endregion

		/// <summary>
		/// (DEPRECATED) Resets the delegates.
		/// Use this when changing scenes
		/// </summary>
		public void ResetUpdateDelegates()
		{
			_update = null;
			_fixedUpdate = null;
			_lateUpdate = null;
		}

		/// <summary>
		/// Removes the scene from the updates
		/// </summary>
		/// <param name="sceneIndex"></param>
		public static void FlushScene(int sceneIndex)
		{
			if (Instance._updateDictionary.ContainsKey(sceneIndex))
				Instance._updateDictionary.Remove(sceneIndex);
			if (Instance._fixedUpdateDictionary.ContainsKey(sceneIndex))
				Instance._fixedUpdateDictionary.Remove(sceneIndex);
			if (Instance._lateUpdateDictionary.ContainsKey(sceneIndex))
				Instance._lateUpdateDictionary.Remove(sceneIndex);
		}
		/// <summary>
		/// Sets the Pause Flag (Does not affect physics, only the <b>OnUpdate</b> method)
		/// </summary>
		/// <param name="value">New Pause state</param>
		public static void SetPause(bool value) => Instance._isPause = value;

		#region Subscriptions

		/// <summary>
		/// (DEPRECATED) Subscribes to Update
		/// </summary>
		/// <param name="updateable"></param>
		public static void Subscribe(IUpdateable updateable)
		{
			if (_isQuittingApplication)
				return;
			Instance._update += updateable.OnUpdate;
		}

		/// <summary>
		/// (DEPRECATED) Unsubscribes from Update
		/// </summary>
		/// <param name="updateable"></param>
		public static void UnSubscribe(IUpdateable updateable)
		{
			if (_isQuittingApplication)
				return;
			Instance._update -= updateable.OnUpdate;
		}

		/// <summary>
		/// Subscribes to Update
		/// </summary>
		/// <param name="updateable"></param>
		/// <param name="sceneIndex"></param>
		public static void Subscribe(IUpdateable updateable, int sceneIndex)
		{
			if (_isQuittingApplication)
				return;
			if(!Instance._updateDictionary.ContainsKey(sceneIndex))
				Instance._updateDictionary.Add(sceneIndex, null);
			Instance._updateDictionary[sceneIndex] += updateable.OnUpdate;
		}
		/// <summary>
		/// Unsubscribes from Update
		/// </summary>
		/// <param name="sceneIndex">index of the scene in the build settings</param>
		/// <param name="updateable"></param>
		public static void UnSubscribe(IUpdateable updateable, int sceneIndex)
		{
			if (_isQuittingApplication || !Instance._updateDictionary.ContainsKey(sceneIndex))
				return;
			Instance._updateDictionary[sceneIndex] -= updateable.OnUpdate;
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