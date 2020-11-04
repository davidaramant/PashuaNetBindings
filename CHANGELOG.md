# Pashua .NET Bindings Changelog

This project uses [Semantic Versioning 2.0.0](https://semver.org).

Format loosely based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## 2.0.0 (2020-11-03)

The 1.0.0 release was a bit rushed to meet an arbitrary Hacktoberfest deadline.  As such, the interface of the library was not properly thought through.  In keeping with SemVer, the library has been bumped to 2.0 since it breaks compatibility.

### Changed

- Output is reported through properties with `Action` types.  This allows the script to be written in "one shot" without having to keep track of a bunch of control instances to read the output from.

### Removed

- `AddAndReturn` extension method

## 1.0.0 (2020-10-29)

Initial release.
