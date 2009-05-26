using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NUnit.Framework;
using UnfuddleToBasecamp;

namespace U2bcUnitTests
{
	[TestFixture]
	public class BaseCampMessageTests
	{
		private BaseCampSettings _settings = new BaseCampSettings
			{
				CategoryId = "CAT",
				MainUrl = "MAINURL",
				Password = "PWD",
				UserId = "USR"
			};

		private Changeset _changeset = new Changeset
			{
				Author = "AUTH",
				Message = "MSG",
				Repository = "REPO",
				Revision = "RV"
			};
			
		[Test]
		public void TitleShouldContainAuthorRevisionAndRepo()
		{
			var m = UnfuddleChangesetHandler.CreateBasecampMessage(_changeset, _settings);

			StringAssert.Contains(_changeset.Author, m.Title);
			StringAssert.Contains(_changeset.Revision, m.Title);
			StringAssert.Contains(_changeset.Repository, m.Title);
		}

		[Test]
		public void BodyShouldBeOriginalMessage()
		{
			var m = UnfuddleChangesetHandler.CreateBasecampMessage(_changeset, _settings);

			Assert.AreEqual(_changeset.Message, m.Body);
		}
		[Test]
		public void ShouldUseConfiguredCategoryId()
		{
			var m = UnfuddleChangesetHandler.CreateBasecampMessage(_changeset, _settings);

			Assert.AreEqual(_settings.CategoryId, m.CategoryId);
		}

		[Test]
		public void ShouldConvertToProperXml()
		{
			var xml = new XmlDocument();
			var m = UnfuddleChangesetHandler.CreateBasecampMessage(_changeset, _settings);
			xml.LoadXml(m.ToXml(_settings));
			Assert.AreEqual(_settings.CategoryId, xml.SelectSingleNode("request/post/category-id").InnerText);
			Assert.AreEqual(m.Title, xml.SelectSingleNode("request/post/title").InnerText);
			Assert.AreEqual(m.Body, xml.SelectSingleNode("request/post/body").InnerText);
		}
	}
}
