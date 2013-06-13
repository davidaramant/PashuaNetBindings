using System;

namespace PashuaWrapper.BooleanExtensions {
	public static class Extensions {
		public static int ToInt( this bool value ) {
			return value ? 1 : 0;
		}

		public static int ToInt( this bool? value ) {
			return value.Value ? 1 : 0;
		}
	}
}

