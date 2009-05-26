using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Juicy.DirtCheapDaemons.Http;
using NUnit.Framework;
using UnfuddleToBasecamp;

namespace U2bcUnitTests
{
	[TestFixture]
	public class PostToBasecampTests
	{
			HttpServer _server;

		[TestFixtureSetUp]
		public void MasterSetup()
		{
			_server = new HttpServer();
			_server.Start();
		}

		[TestFixtureTearDown]
		public void MasterTearDown()
		{
			if(_server != null )
			{
				_server.Shutdown();
			}
		}

		[TearDown]
		public void TearDown()
		{
			_server.UnmountAll();
		}

		[Test]
		public void ShouldPostToBasecampUrl()
		{
			var settings = new BaseCampSettings
				{
					CategoryId = "cat",
					MainUrl = _server.RootUrl.TrimEnd('/'),
					UserId = "usr",
					Password = "pwd",
					ProjectId = "proj"
				};
			var message = new BaseCampMessage
				{
					Body = "message",
					CategoryId = settings.CategoryId,
					Title = "hi"
				};

			string body = null;
			string contentType = null;
			
			//TODO: the Juicy web server doesn't support the post bdy yet
			// (it's broken) so we will ignore that for now.

			_server.Mount(settings.NewPostPath, (req, resp) =>
				{
					//body = req.PostBody;
					contentType = req["Content-Type"];
				});

			UnfuddleChangesetHandler.PostNewMessageToBasecamp(message, settings);
			//Assert.AreEqual(message.ToXml(settings), body);
			Assert.AreEqual("application/xml", contentType);
		}
	}
}
