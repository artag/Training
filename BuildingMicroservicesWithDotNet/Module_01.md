# Module 1. Introduction

## Lesson 5. What's wrong with the monolith?

### PROS

* Convenient for new projects
* Tools mostly focused on them
* Great code reuse
* Easier to run locally
* Easier to debug and troubleshoot
* One thing to build
* One thing to deploy
* One thing to test end to end
* One thing to scale

### CONS

* Easily gets too complex to understand
* Merging code can be challenging
* Slows down IDEs
* Long build times
* Slow and infrequent deployments
* Long testing and stabilization periods
* Rolling back is all or nothing
* No isolation between modules
* Can be hard to scale
* Hard to adopt new tech

## Lesson 6. What are microservices?

*Microservices* - an architectural style that structures an application as a collection
of independently deployable services that are modeled around a business domain and are usually
owned by a small team.

### PROS

* Small, easier to understand code base
* Quicker to build
* Independent, faster deployments and rollbacks
* Independently scalable
* Much better isolation from failures
* Designed for continuous delivery
* Easier to adopt new, varied tech
* Grants autonomy to teams and lets them work in parallel

### CONS

* Not easy to find the right set of services
* Adds the complexity of distributed systems
* Shared code moves to separate libraries
* No good tooling for distributed apps
* Releasing features across services is hard
* Hard to troubleshoot issues across services
* Can't use transactions across services
* Raises the re quired skillset for the team

### When to use microservices?

* It's perfectly fine to start woth a monolith, then move to microservices

* Start looking at microservices when:
  * The code base size is more than what a small team can mantain
  * Teams can't move fast anymore
  * Builds become too slow due to large code base
  * Time to market is compromised due to infrequent deployments and long verification times

* It's all about team autonomy!
