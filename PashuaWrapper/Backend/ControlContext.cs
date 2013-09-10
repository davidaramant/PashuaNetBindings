using System;
using System.Collections.Generic;
using PashuaWrapper.ListExtensions;
using PashuaWrapper.BooleanExtensions;

namespace PashuaWrapper {
	public sealed class ControlContext {
		private static int _count;
		private readonly List<string> _script;
		private readonly string _id;

		public ControlContext( List<string> script, string id, string type ) {
			_script = script;
			_id = id ?? GetNextId();
			_script.AddFormat( "{0}.type = {1}", _id, type );
		}

		public ControlContext( List<string> script ) {
			_script = script;
			_id = "*";
		}

		private string GetNextId() {
			return "unnamedControl" + _count++;
		}

		public void Set( string propertyName, string propertyValue ) {
			if( !String.IsNullOrWhiteSpace( propertyValue ) ) {
				_script.AddFormat( "{0}.{1} = {2}", _id, propertyName, propertyValue );
			}
		}

		public void Set( string propertyName, bool? propertyValue, bool defaultValue ) {
			if( propertyValue.HasValue && propertyValue != defaultValue ) {
				_script.AddFormat( "{0}.{1} = 1", _id, propertyName );
			}
		}

		public void Set( string propertyName, int? propertyValue ) {
			if( propertyValue.HasValue ) {
				_script.AddFormat( "{0}.{1} = {2}", _id, propertyName, propertyValue.Value );
			}
		}

		public void Set( string propertyName, double? propertyValue ) {
			if( propertyValue.HasValue ) {
				_script.AddFormat( "{0}.{1} = {2}", _id, propertyName, propertyValue.Value );
			}
		}
	}
}

