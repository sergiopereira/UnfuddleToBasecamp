# Unfuddle To Basecamp

This is a bridging ASP.NET application to receive [Unfuddle](http://unfuddle.com/) commit notifications and post a new message to a [Basecamp](http://basecamphq.com/) project.

## Usage

Create a new ASP.NET application directory in IIS (let's call it BRIDGE) and put the UnfuddleToBasecamp.dll file in the /bin subdirectory. Copy the file rename.Web.config to the application directory and rename it to Web.config.

Edit the Web.config to reflect your Basecamp settings. The comments in that file should be enough help. You'll probably need to create a new user in Basecamp just to act as a proxy for the commit messages.

In your Unfuddle repository settings, put the URL to the new application like this:
 http://yourserver.IP.or.domain/BRIDGE/tobasecamp.ashx


## LICENSE:

This software is hereby placed in the public domain.
    Sergio Pereira, the author, May 25th 2009
