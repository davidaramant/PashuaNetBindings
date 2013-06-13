using System;
using PashuaWrapper;

namespace Runner
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			PashuaDialog.Create( title:"Hello" ).Show();
		}
	}
}
