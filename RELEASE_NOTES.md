### 2.0.0-alpha.15

* Allow more control over async calls of the API
* Remove auto authentication from core methods
* Add a new wrapper function which can handle authentication refresh without
  polluting the other function signatures
* Change used library: Http.Fs -> FsHttp
* For most functions you need a valid Authentication now
* Restructure stuff to better resemble the api calls
* Expand response data and available API functions
* Generate documentation with Fornax
* Introduce Cli in project to test API

### 1.0.4 (2020-02-18)

* Updated dependencies

### 1.0.3 (2019-11-22)

* Changed internal workflow: introduced Paket and FAKE

### 1.0.2 (2019-09-26)

* Changed FSharp.Core dependency (4.7.0 -> 4.5.4)

### 1.0.1 (2019-09-25)

* Updated dependencies

### 1.0.0 (2019-09-25)

This release is the initial release after moving this part from Andromeda-for-Gog into an own project.
