using System;
using System.Collections.Generic;

namespace UnfuddleToBasecamp
{
	public class Changeset
	{
		public string Author { get; set; }
		public string Message { get; set; }
		public string Revision { get; set; }
		public string Repository { get; set; }
	}
}