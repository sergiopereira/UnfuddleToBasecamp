using System;
using System.Collections.Generic;
using System.Xml;

namespace UnfuddleToBasecamp
{
	public class BaseCampMessage
	{
		public string CategoryId { get; set; }
		public string Title { get; set; }
		public string Body { get; set; }

		public string ToXml(BaseCampSettings settings)
		{
			var msg = new XmlDocument();
			msg.LoadXml(
				@"<request>
				  <post>
					<category-id></category-id>
					<title></title>
					<body></body>
				  </post>
				</request>"
				);

			msg.SelectSingleNode("request/post/category-id").InnerText = settings.CategoryId;
			msg.SelectSingleNode("request/post/title").InnerText = Title;
			msg.SelectSingleNode("request/post/body").InnerText = Body;

			return msg.OuterXml;
		}
	}
}