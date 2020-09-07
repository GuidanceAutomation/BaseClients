# Service Call Result Guide

The client wrapper libraries allow the user to make remote procedure calls on the server, without the need to understand or implement and of the underlying communications layer.

Remote calls are categorized either as:

* ```void``` calls, i.e. calls that do not return a value.
* value calls, i.e. calls that return a value.

The wrapper implementation also provides functionality to diagnose faults; determine if the fault is server or client side and capture caught exceptions. This is all wrapped up in the ```IServiceCallResult``` object.

## Methods returning ```void```

The most basic calls takes the format:

* ```IServiceCallResult DoThing();```
* ```IServiceCallResult DoThingWithThisValue(T value);```

These calls call a method remotely on the server and optionally pass in a parameter. ```IServiceCallResult``` contains the result of the call with:

```
public interface IServiceCallResult
{
    string ExceptionMessage { get; }

    string ExceptionSource { get; }

    string ExceptionStackTrace { get; }

    int ServiceCode { get; }
}
```

### ServiceCode

This value represents the success of the call without

* No Error = 0
* Unknown Failure = 1
* Unknown Exception = 2
* Service Not Configured = 3
* Client Exception = 4

#### No Error

This indicates the call was successfully, your method has been called on the server and the server effectively returned void.

#### Unknown Failure

This indicates the call has failed and no diagnosis is available.

##### Unknown Exception

This indicates the method has been called on the server, and the method has thrown an exception. There is no explicit handler setup for this exception, but the ```ExceptionMessage```, ```ExceptionSource``` and ```ExceptionStackTrace``` properties will contain information on the caught exception for further analysis.

#### Service Not Configured

This indicates the service is running on the server, but the service itself is not yet ready for communications. Caused by the endpoint being hosted before the object it exposes is in a usable state. Indicates the problem is server side.

#### Client Exception

This indicates the problem is client side, i.e. the call has not reached the server. Typically caused by incorrect endpoint settings (i.e. the server IP address is wrong, or the server is not running).

### Custom Service Codes

If the method call on the server fails for a known reason, a custom service code (a value of 10 or greater) may be returned instead. Exception data (```ExceptionMessage```, ```ExceptionSource``` and ```ExceptionStackTrace```) may or may not be provided. Using the [ServiceCodeSupport](https://github.com/GuidanceAutomation/ServiceCodeSupport) package this can be diagnosed further.

E.g. using the job builder client call:

```
IServiceCallResult IssueIPAddressDirective(int taskId, string parameterAlias, IPAddress value);
```

with an invalid taskId, returns the Service Code 1001, which enumerates to ```JobBuilder_Task_Id_Invalid```.

## Methods returning a value

Value calls take the form:

* ```IServiceCallResult<T> GetThing();```
* ```IServiceCallResult<T> DoThingAndReturnResult(U value);```

Where ```IServiceCallResult<T>``` is a generic:

```
public interface IServiceCallResult<T> : IServiceCallResult
{
    T Value { get; }
}
```

If the call is successful (i.e. ```ServiceCode == 0```) then the ```Value``` property contains the returned value. If the call is unsuccessful the value property with be ```default(T)```.

The success of the call (i.e. the service code) should always be validated before using the returned value.
