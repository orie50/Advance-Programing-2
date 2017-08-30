using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;

namespace WebServer
{
	/// <summary>
	/// SignalR startup point
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// Configurations the specified application.
		/// </summary>
		/// <param name="app">The application.</param>
		public void Configuration(IAppBuilder app)
		{
			app.MapSignalR();
		}
	}
}