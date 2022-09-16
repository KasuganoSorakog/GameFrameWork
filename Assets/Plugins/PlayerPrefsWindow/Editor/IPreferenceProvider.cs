
/****************************************************
 * FileName:		IPreferenceProvider.cs
 * CompanyName:		
 * Author:			
 * Email:			
 * CreateTime:		2021-04-01-23:09:13
 * Version:			1.0
 * UnityVersion:	2019.4.8f1
 * Description:		Nothing
 * 
*****************************************************/
using System.Collections.Generic;

public interface IPreferenceProvider
{
	void SetKeyValue(string valueName, object value);
	void FetchKeyValues (IDictionary<string, object> prefsLookup);
	object ValueField(string valueName, object value);
}