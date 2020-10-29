# Pashua .NET Bindings

.NET binding library for [Pashua](https://www.bluem.net/en/projects/pashua/), a macOS tool for displaying dialog boxes.  The use case for this is small utilities where it would be nice to have a GUI but the overhead of using something like Xamarin Mac is not justified.

## Technology

.NET Standard 2.0, C#

## License

MIT

## Table of Contents

* [Usage](#usage)
* [Examples](#examples)
* [Source Code Notes](#source-code-notes)
* [Changelog](CHANGELOG.md)

## Usage

### Writing a Script

There is one class for every [element type described in the official Pashua documentation](https://www.bluem.net/pashua-docs-latest.html), each of which has properties that correspond to the documented fields.  One notable special case is the `Date` class, which has the two boolean options replaced with a `SelectionMode` enumeration property for whether times/dates/both can be selected.

A "script" for the purposes of this library is just a sequence of `IPashuaControl`. There is a helper extension method for `ICollection<IPashuaControl>` called `AddAndReturn` that, well, adds the given control to the script and returns a reference to it.  This is useful due to [how script output is handled](#reading-the-script-output).

### Running a Script

There is an extension method on `IEnumerable<IPashuaControl>` called `RunScript`.  This is a synchronous blocking call.

By default, the library will look in the following locations for Pashua:

* `/Applications/Pashua.app`
* `~/Applications/Pashua.app`
* `./Pashua.app`

However, the `RunScript` method takes an optional parameter for a custom location for `Pashua.app`.  This should be a path that includes the app, for example `~/PortableApp/Pashua.app`.

`RunScript` will check all the controls for validation issues before executing the script and will throw a `PashuaScriptException` that contains all the issues found.

### Reading the Script Output

The name of the element in the Pashua script is handled automatically.  You _can_ get access to it with the `Id` property, but there shouldn't be a reason to use it in normal use.

Once the script has been executed, any control type that returns a value will have a property set with the result.  This does mean that you will need to keep track of each control instance you want to capture output from.

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

### Other Functionality

All the controls implement `IPashuaControl` which has a `WriteTo(TextWriter)` method.  If you want to see the script that will be sent to Pashua, there is also an extension method on `IEnumerable<IPashuaScript>` called `WriteTo` that can be used:

```csharp
List<IPashuaControl> script = ...

script.WriteTo(Console.Out); // Or use a StreamWriter to a file or something
```

If for some reason you want to validate the script without running it, that can be done with `GetScriptValidationIssues`:

```csharp
var allIssues = script.GetScriptValidationIssues();
```

## Examples

See the [Demo project](src/PashuaNetBindings.Demo/Program.cs) for some examples of all the control types.  

The demo can be run with the following command:

`dotnet run --project src/PashuaNetBindings.Demo/PashuaNetBindings.Demo.csproj`

### Minimal Example

```csharp
var script = new List<IPashuaControl>
{
    new Window
    {
        Title = "Hello from Pashua!"
    },
    new Text
    {
        Default = "A simple GUI without having to deal with Xamarin.Mac"
    },
    new DefaultButton
    {
        Label = "Submit"
    }
};

var thoughts = script.AddAndReturn(
    new TextField { Label = "Your thoughts:" });

script.RunScript();

Console.Out.WriteLine(thoughts.EnteredText);

```

## Source Code Notes

Everything in the library is in one namespace for simplicity, however the controls are as organized in a subfolder.

Most the control classes were created with a code generator project (`src/DataModelGenerator`).  This is just a console app that spits out all the partial `.Generated.cs` files based on the contents of `Pashua Documentation.tsv`, which is a tweaked version of all the control documentation copied from the Pashua site.  Properties that were too much of a special case were just handled in a manual partial class.

## Changelog

See [CHANGELOG.md](CHANGELOG.md).

--------------------

TODO before release:

* Test the application finding thing
* Create NuGet package

