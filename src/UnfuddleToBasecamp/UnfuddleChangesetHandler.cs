using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

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
			HttpRequest request = context.Request;
			BaseCampSettings bc = null; //TODO

			Changeset changeset = GetIncomingChangeset(request.InputStream);
			BaseCampMessage message = CreateBasecampMessage(changeset, bc);
			PostNewMessageToBasecamp(message, bc);
		}


		public static Changeset GetIncomingChangeset(Stream postBodyStream)
		{
			/* changeset xml sample
			<changeset>
			  <!--
			  Note that not all repositories systems distinguish between
			  the author and committer (i.e. Subversion does not)
			  -->
			  <author-id type="integer"> </author-id>
			  <author-name> </author-name>
			  <author-email> </author-email>
			  <author-date type="datetime"> <!-- the date the patch was authored --> </author-date>
			  <committer-id type="integer"> </committer-id>
			  <committer-name> </committer-name>
			  <committer-email> </committer-email>
			  <committer-date type="datetime"> <!-- the date of the commit --> </committer-date>
			  <created-at type="datetime"> </created-at>
			  <id type="integer"> </id>
			  <message> </message>
			  <message-formatted> <!-- only available if formatted=true --> </message-formatted>
			  <repository-id type="integer"> </repository-id>
			  <revision> </revision>
			</changeset>
			 */
			string xml;
			using(var sr = new StreamReader(postBodyStream))
			{
				xml = sr.ReadToEnd();
			}

			if (string.IsNullOrEmpty(xml))
			{
				throw new HttpException("Invalid request");
			}

			var cs = new XmlDocument();
			cs.LoadXml(xml);

			return new Changeset
				{
					Author = cs.SelectSingleNode("changeset/committer-name").InnerText,
					Message = cs.SelectSingleNode("changeset/message").InnerText,
					Repository = cs.SelectSingleNode("changeset/repository-id").InnerText,
					Revision = cs.SelectSingleNode("changeset/revision").InnerText
				};
		}


		public static BaseCampMessage CreateBasecampMessage(Changeset changeset, BaseCampSettings settings)
		{
			/*
			POST /projects/#{project_id}/posts.xml
				<request>
				  <post>
					<category-id>#{category_id}</category-id>
					<title>#{title}</title>
					<body>#{body}</body>
					<extended-body>#{extended_body}</extended-body>
					<private>1</private> <!-- only for firm employees -->
				  </post>
				  <notify>#{person_id}</notify>
				  <notify>#{person_id}</notify>
				</request>
			 */
			return new BaseCampMessage
				{
					Title = string.Format("{0} committed revision {1} in {2}",
					                      changeset.Author,
					                      changeset.Revision,
					                      changeset.Repository),
					Body = changeset.Message,
					CategoryId = settings.CategoryId
				};
		}

		public static void PostNewMessageToBasecamp(BaseCampMessage message, BaseCampSettings settings)
		{
			string url = settings.NewPostUrl;

			var messageXml = message.ToXml(settings);
			GetResponseBodyFromUrlViaPost(url, messageXml, "application/xml");
		}

		private static string GetResponseBodyFromUrlViaPost(string url, string postBody, string contentType)
		{
			WebRequest request = WebRequest.Create(url);
			request.Method = "POST";
			request.ContentType = contentType;
			request.ContentLength = postBody.Length;
			byte[] buffer = Encoding.ASCII.GetBytes(postBody);
			request.GetRequestStream().Write(buffer, 0, buffer.Length);

			using (WebResponse response = request.GetResponse())
			using (var sr = new StreamReader(response.GetResponseStream()))
			{
				return sr.ReadToEnd();
			}
		}
	}
}