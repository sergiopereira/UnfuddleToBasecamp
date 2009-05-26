using System;

namespace UnfuddleToBasecamp
{
	public class BaseCampSettings
	{
		public string MainUrl { get; set; }
		public string CategoryId { get; set; }
		public string ProjectId { get; set; }
		public string UserId { get; set; }
		public string Password { get; set; }

		public string NewPostPath
		{
			get
			{
				return string.Format("/projects/{0}/posts.xml",
				                     ProjectId);
			}
		}		
		
		public string NewPostUrl
		{
			get
			{
				return MainUrl + NewPostPath;
			}
		}
	}
}