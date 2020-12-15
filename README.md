# ReleaseRetention
Class library to keep N of the most recent Releases/Deployments

# Design Decisions

I have created the main project as a Class library, this fits the brief of it being a reusable and testable component.

# Ideas and Improvements

* Check a release is for a valid project, the test data contained Deployments -> Project-3 and Releases -> Envrionment-3.
* Check for invlaid (e.g. 1.0.1-ci1) and null version numbers
* Validation on datetime, the next release must not be before the previous one

