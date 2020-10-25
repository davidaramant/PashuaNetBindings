# Pashua .NET Bindings

.NET Standard wrapper for [Pashua](https://www.bluem.net/en/projects/pashua/), a macOS utility for displaying simple dialog boxes through scripts.  The use case for this is small utilities where it would be nice to have a GUI but the overhead of using something like Xamarin Mac is not justified.

## Technology

.NET Standard 2.0, C#

## License

MIT

## Usage

This library follows the Pashua elements closely.  There is one class for every [element type described in the official documentation](https://www.bluem.net/pashua-docs-latest.html), each of which has properties that correspond to the documented fields.  One special case is the `Date` class, which has the two boolean options replaced with a `SelectionMode` enumeration property for whether times/dates/both can be selected.

The name of the element in the Pashua script is completely abstracted.  Once the script has been executed, there is a `Result` property on the object that is in the data type relevant to the type of control.  This does mean that you will need to keep track of each control instance you want to capture output from.

## Examples

--------------------



TODO:
* Finish readme
* Create NuGet package
* Magic to parse output
* Validation of properties when writing
