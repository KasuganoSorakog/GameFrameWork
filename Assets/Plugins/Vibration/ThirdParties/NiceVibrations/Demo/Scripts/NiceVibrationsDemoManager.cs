
/****************************************************
 * FileName:		NiceVibrationsDemoManager.cs
 * CompanyName:		
 * Author:			
 * Email:			
 * CreateTime:		2021-04-01-23:15:14
 * Version:			1.0
 * UnityVersion:	2019.4.8f1
 * Description:		Nothing
 * 
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.NiceVibrations
{
	public class NiceVibrationsDemoManager : MonoBehaviour 
	{

		protected string _debugString;
		protected string _platformString;

		protected virtual void Awake()
		{
			MMVibrationManager.iOSInitializeHaptics ();
		}

        protected virtual void OnDisable()
        {
            MMVibrationManager.iOSReleaseHaptics();
        }

        protected virtual void Start()
		{
			DisplayInformation ();
		}

		protected virtual void DisplayInformation()
		{
			if (MMVibrationManager.Android ())
			{
				_platformString = "API version " + MMVibrationManager.AndroidSDKVersion().ToString();
			} 
			else if (MMVibrationManager.iOS ())
			{
				_platformString = "iOS " + MMVibrationManager.iOSSDKVersion(); 
			}
		}




		public virtual void TriggerDefault()
		{
			#if UNITY_IOS || UNITY_ANDROID
				Handheld.Vibrate ();	
			#endif
		}

		public virtual void TriggerVibrate()
		{
			MMVibrationManager.Vibrate ();
		}

		/// <summary>
		/// Triggers the selection haptic feedback, a light vibration on Android, and a light impact on iOS
		/// </summary>
		public virtual void TriggerSelection()
		{
			MMVibrationManager.Haptic (HapticTypes.Selection);
		}

		/// <summary>
		/// Triggers the success haptic feedback, a light then heavy vibration on Android, and a success impact on iOS
		/// </summary>
		public virtual void TriggerSuccess()
		{
			MMVibrationManager.Haptic (HapticTypes.Success);
		}

		/// <summary>
		/// Triggers the warning haptic feedback, a heavy then medium vibration on Android, and a warning impact on iOS
		/// </summary>
		public virtual void TriggerWarning()
		{
			MMVibrationManager.Haptic (HapticTypes.Warning);
		}

		/// <summary>
		/// Triggers the failure haptic feedback, a medium / heavy / heavy / light vibration pattern on Android, and a failure impact on iOS
		/// </summary>
		public virtual void TriggerFailure()
		{
			MMVibrationManager.Haptic (HapticTypes.Failure);
		}

		/// <summary>
		/// Triggers a light impact on iOS and a short and light vibration on Android.
		/// </summary>
		public virtual void TriggerLightImpact()
		{
			MMVibrationManager.Haptic (HapticTypes.LightImpact);
		}

		/// <summary>
		/// Triggers a medium impact on iOS and a medium and regular vibration on Android.
		/// </summary>
		public virtual void TriggerMediumImpact()
		{
			MMVibrationManager.Haptic (HapticTypes.MediumImpact);
		}

		/// <summary>
		/// Triggers a heavy impact on iOS and a long and heavy vibration on Android.
		/// </summary>
		public virtual void TriggerHeavyImpact()
		{
			MMVibrationManager.Haptic (HapticTypes.HeavyImpact);
		}
	}
}