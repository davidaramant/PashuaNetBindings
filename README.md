# Pashua .NET Bindings

.NET binding library for [Pashua](https://www.bluem.net/en/projects/pashua/), a macOS tool for displaying dialog boxes.  The use case for this is small utilities where it would be nice to have a GUI but the overhead of using something like Xamarin Mac is not justified.

## Technology

.NET Standard 2.0, C#

## License

MIT

## Usage

This library follows the Pashua elements closely.  There is one class for every [element type described in the official documentation](https://www.bluem.net/pashua-docs-latest.html), each of which has properties that correspond to the documented fields.  One special case is the `Date` class, which has the two boolean options replaced with a `SelectionMode` enumeration property for whether times/dates/both can be selected.

The name of the element in the Pashua script is completely abstracted.  Once the script has been executed, any control type that returns a value will have a property set with the result.  This does mean that you will need to keep track of each control instance you want to capture output from.

Output properties:

* `Button.WasClicked`
* `CancelButton.WasClicked`
* `CheckBox.WasChecked`
* `ComboBox.SelectedOption`
* `Date.SelectedTimestamp`
* `DefaultButton.WasClicked`
* `OpenBrowser.SelectedPath`
* `Password.EnteredText`
* `Popup.SelectedOption`
* `RadioButton.SelectedOption`
* `SaveBrowser.SelectedPath`
* `TextBox.EnteredText`
* `TextField.EnteredText`

## Examples

See the [Demo project](src/PashuaNetBindings.Demo/Program.cs) for some examples.

--------------------

TODO:

* Search for Pashua in recommended locations
* Finish readme
* Create NuGet package

