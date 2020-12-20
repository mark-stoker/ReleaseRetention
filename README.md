## ReleaseRetention
Class library to keep N of the most recent Releases based on the the most recent Deployment dates.

## Design Decisions and Assumption

* I have created the main project as a Class library, this fits the brief of it being a reusable and testable component. It can also then
be reused as a Nuget package.
* I have injected ILogger into the the Project Class to log out why a Release is kept, I have not wired up the Dependency Injection as this would
be passed by the consumer of the Library.
* The brief stated no new UI and I have tried to prove it works through the Unit Tests. However, I have added a small Console App to help give a better visual
idea of how the Library works.

## Ideas for Improvements

* Check for invlaid (e.g. 1.0.1-ci1) and null version numbers.
* Validation on datetime, the next release must not be before the previous one.
* Currently the Retention algorithm does not take into account Environment, this could be extended to be included.
** This would break all the tests.
** Consider abstracting _releasesToKeep to own Class with reference to Environment.
* Create an Indexer on the Retention Class
* Remove filter in Retention Class to include orphan objects e.g. 'Release-8' and 'Deployment-10'
