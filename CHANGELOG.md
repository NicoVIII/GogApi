# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.1] - 2020-06-01

### Fixed

* Make endpoints User/getData and Account/getFilteredGames functional again

## [2.0.0] - 2020-05-31

### Added
* Add a new wrapper function which can handle authentication refresh without
  polluting the other function signatures
* Expand response data and available API functions
* Generate documentation with Fornax
* Introduce Cli in project to test API

### Changed
* Change used library: Http.Fs -> FsHttp
* For most functions you need a valid Authentication now
* Restructure stuff to better resemble the api calls
* Allow more control over async calls of the API

### Removed
* Remove auto authentication from core methods

## [1.0.4] - 2020-02-18

* Updated dependencies

## [1.0.3] - 2019-11-22

* Changed internal workflow: introduced Paket and FAKE

## [1.0.2] - 2019-09-26

* Changed FSharp.Core dependency (4.7.0 -> 4.5.4)

## [1.0.1] - 2019-09-25

* Updated dependencies

## [1.0.0] - 2019-09-25

This release is the initial release after moving this part from Andromeda-for-Gog into an own project.
