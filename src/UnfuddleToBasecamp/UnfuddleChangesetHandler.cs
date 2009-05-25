using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace UnfuddleToBasecamp
{
	public class UnfuddleChangesetHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get { return true; }
		}

		public void ProcessRequest(HttpContext context)
		{
			HttpResponse response = context.Response;
			HttpRequest request = context.Request;
			BaseCampSettings bc = null; //TODO

			string changesetXml = GetIncomingChagesetXml(request);
			string messageXml = CreateBasecampMessageXml(changesetXml, bc);
			PostNewMessageToBasecamp(response, messageXml, bc);
		}

		private void PostNewMessageToBasecamp(HttpResponse response, string xml, BaseCampSettings settings)
		{


		}

		private string CreateBasecampMessageXml(string changesetXml, BaseCampSettings settings)
		{
			return "";

		}

		private string GetIncomingChagesetXml(HttpRequest request)
		{
			using(var sr = new StreamReader(request.InputStream))
			{
				return sr.ReadToEnd();
			}
		}
	}
}
