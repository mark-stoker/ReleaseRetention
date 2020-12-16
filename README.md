## ReleaseRetention
Class library to keep N of the most recent Releases/Deployments

## Design Decisions and Assumption

* I have created the main project as a Class library, this fits the brief of it being a reusable and testable component. It can also then
be reused as a Nuget package.
* I have injected Ilogger into the the Projects class to log out why a release is kept, I have not wired up the Dependency Injection as this would
be passed by the consumer of the library.
* The brief stated no new UI and I have tried to prove it works through the Unit Tests. Howeve, I have added a small Console App to help give a better visual
idea of how the Library works.

## Ideas for Improvements

* Check a release is for a valid project and environment, the test data contained Deployments -> Project-3 and Releases -> Envrionment-3.
* Check for invlaid (e.g. 1.0.1-ci1) and null version numbers
* Validation on datetime, the next release must not be before the previous one

