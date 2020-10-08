using System.Collections.Generic;
using Pashua.ListExtensions;

namespace Pashua
{
    public sealed class ControlContext
    {
        // HACK: This seems a bit gross, but shouldn't cause any problems and does guarantee that the ids are unique.
        private static int _unnamedControlCount;
        private readonly string _id;
        private readonly List<string> _script;

        public ControlContext(List<string> script, string id, string type)
        {
            _script = script;
            _id = id ?? GetNextId();
            _script.AddFormat("{0}.type = {1}", _id, type);
        }

        public ControlContext(List<string> script)
        {
            _script = script;
            _id = "*";
        }

        private string GetNextId()
        {
            return "unnamedControl" + _unnamedControlCount++;
        }

        public void Set(string propertyName, string propertyValue)
        {
            if (!string.IsNullOrWhiteSpace(propertyValue))
                _script.AddFormat("{0}.{1} = {2}", _id, propertyName, propertyValue);
        }

        public void Set(string propertyName, bool? propertyValue, bool defaultValue)
        {
            if (propertyValue.HasValue && propertyValue != defaultValue)
                _script.AddFormat("{0}.{1} = 1", _id, propertyName);
        }

        public void Set(string propertyName, int? propertyValue)
        {
            if (propertyValue.HasValue) _script.AddFormat("{0}.{1} = {2}", _id, propertyName, propertyValue.Value);
        }

        public void Set(string propertyName, double? propertyValue)
        {
            if (propertyValue.HasValue) _script.AddFormat("{0}.{1} = {2}", _id, propertyName, propertyValue.Value);
        }
    }
}