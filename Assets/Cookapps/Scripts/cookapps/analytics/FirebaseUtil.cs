using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;

namespace Cookapps.Analytics {
public class FirebaseUtil {

	public static Parameter[] ParseParams(Dictionary<string, object> param = null) 
	{
		if ((param != null) && (param.Count > 0)) 
		{
			List<Parameter> _params = new List<Parameter>();
			int _paramLength = param.Count;
			foreach (KeyValuePair<string, object> _p in param) 
			{
				if (_params.Count < 25)
				{
					_params.Add(new Parameter(_p.Key, _p.Value.ToString()));
				}
			}
			return _params.ToArray();
		}
		else 
		{
			return null;
		}
	}
}
}