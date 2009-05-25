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

			var changeset = GetIncomingChangeset(request);
			var message = CreateBasecampMessage(changeset, bc);
			PostNewMessageToBasecamp(response, message, bc);
		}

		private void PostNewMessageToBasecamp(HttpResponse response, BaseCampMessage message, BaseCampSettings settings)
		{


		}

		private BaseCampMessage CreateBasecampMessage(Changeset changeset, BaseCampSettings settings)
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

		private Changeset GetIncomingChangeset(HttpRequest request)
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
			string xml = null;
			using(var sr = new StreamReader(request.InputStream))
			{
				xml = sr.ReadToEnd();
			}

			return new Changeset();
		}
	}
}
