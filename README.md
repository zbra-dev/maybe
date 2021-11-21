# Maybe

A maybe monad for C#.

# Introduction

A maybe monad is a good way to avoid nulls. So you avoid returning null by doing:

```
public Maybe<string> Foo()
{
    if (checkThis())
    {
        return "A string".ToMaybe();
    }
    return Maybe<string>.Nothing;
}
```

Because `Maybe<T>` is a struct this means it cannot have a null value. So instead of checking for null you'll have to verify if the returned object has a value before consuming it.

There are multiple ways to retrieve the value of a `Maybe<T>`.

```
var maybe = "some value".ToMaybe();
var v = maybe.Value; // this will throw if it has no value
v = maybe.Or("");
v = maybe.Or(() => FindAValue());
v = maybe.OrThrow(() => new ArgumentException());
v = maybe.OrNull(); // when T is a class
v = maybe.OrEmpty(); // when T is a string
var length = maybe.Select(s => s.Length); // length is a Maybe<int>

var s = maybe.ToNullable(); // s is a string?
```

There are other ways of using it in a more fluent-like API.

```
var maybe = "some value".ToMaybe();
maybe.Consume(s => CallMethod(s)); // CallMethod is only called if maybe has a value

var result = maybe.Where(s => s.StartsWith("some")); // result is a Maybe<string>
```

Comparing maybes is also supported.

```
var a = 10.ToMaybe();
var b = 0.ToMaybe();

if (a > b)
{
    // do stuff
}

a = Maybe<int>.Nothing;
b = int.MinValue;
if (a < b)
{
    // Nothing will always be less than any int value
}
```