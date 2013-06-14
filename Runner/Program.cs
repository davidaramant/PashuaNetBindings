using System;
using PashuaWrapper;

namespace Runner
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			PashuaDialog.
				Create( title:"Hello" ).
				WithButton( id:"one", label:"Another option" ).
				WithDefaultButton( label: "Yup" ).
				Show();
		}
	}
}
