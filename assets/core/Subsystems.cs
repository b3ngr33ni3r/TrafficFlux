using UnityEngine;
using System.Collections;

namespace Core {
	public class Subsystems {
		
		private static SortedList _subsystems;
		
		/**
		 * an enum listing of the acceptable subsystem names (aka types)
		 */
		public enum TYPES {
			NETWORKING,
			LOGGING,
			ADVERTISING,
			THREADING,
			RENDERING,
			LOADING
		};
		
		/**
		 * define a base interface for all Subsystem architectures
		 */
		public interface Subsystem {
			void Send(params object[] data);
			object[] Receive(params object[] data);
			TYPES GetType();
		};
		
		/**
		 * get a Subsystem for the listing
		 */
		public static Subsystem GetSubsystem(TYPES typeid,int id = 0) {
			if (_subsystems == null)
				_subsystems = new SortedList();
			
			SortedList sync = SortedList.Synchronized(_subsystems);
			
			if (!sync.ContainsKey(typeid))
				return null;
			else
				if (sync[typeid] == null)
					return null;
				else
					return (Subsystem)((ArrayList)sync[typeid])[id];
		}
		
		/**
		 * register a subsystem into the listing.
		 * note that this doesn't verify integrity
		 * so duplicates are possible
		 */
		public static void RegisterSubsystem(Subsystem s) {
			if (_subsystems == null)
				_subsystems = new SortedList();
			
			SortedList sync = SortedList.Synchronized(_subsystems);
			
			if (sync.ContainsKey(s.GetType())) {
				if (sync[s.GetType()] == null)
					sync[s.GetType()] = new ArrayList();
				ArrayList.Synchronized(((ArrayList)sync[s.GetType()])).Add(s);
			}
		}
	}
}