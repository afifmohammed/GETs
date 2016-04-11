# GETs

When all you wish to do is query from within a request and return the data back to the caller, all of the below is an over complication in design
  - Define an interface
  - Impelement a class
  - Ctor inject dependencies
  - Use an IOC container to resolve and handle the query with its dependencies injected

This library is meant to help write queries simply as static functions that
  - take in the criteria and the connection
  - return the results
  - done

###Limitations by Design
It disposes the connection as soon as the query completes. This is to ensure its only used inside requests that only perform the query. Hence the name, GETs