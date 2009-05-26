using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using UnfuddleToBasecamp;

namespace U2bcUnitTests
{
	[TestFixture]
	public class ChangesetParsingTests
	{

		[Test]
		public void ShouldParseBasicInformation()
		{
			string xml = 
				@"<changeset>
				  <committer-name>COMMITTER</committer-name>
				  <message>MESSAGE</message>
				  <repository-id type=""integer"">REPO</repository-id>
				  <revision>REVISION</revision>
				</changeset>";

			var ms = new MemoryStream(Encoding.UTF8.GetBytes(xml));
			var cs = UnfuddleChangesetHandler.GetIncomingChangeset(ms);

			Assert.AreEqual("COMMITTER", cs.Author);
			Assert.AreEqual("MESSAGE", cs.Message);
			Assert.AreEqual("REPO", cs.Repository);
			Assert.AreEqual("REVISION", cs.Revision);
		}
	}
}
