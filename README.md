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

// this will throw if it has no value
var v = maybe.Value;

v = maybe.Or("");
v = maybe.Or(() => FindAValue());
v = maybe.OrThrow(() => new ArgumentException());

// when T is a class
v = maybe.OrNull();

// when T is a string
v = maybe.OrEmpty();

// returns a Maybe<int>
var length = maybe.Select(s => s.Length);

// returns a string?
var s = maybe.ToNullable();
```

There are other ways of using it in a more fluent-like API.

```
var maybe = "some value".ToMaybe();

// CallMethod is only called if maybe has a value
maybe.Consume(s => CallMethod(s));

// returns a Maybe<string>
var result = maybe.Where(s => s.StartsWith("some"));
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

# Documentation

## MaybeExtensions.OrEmpty
Allow to get the encapsulated value or an empty string.

Returns the maybe value if HasValue is true, otherwise `string.Empty`.

### Examples
```
var maybe = "some value".ToMaybe();
var v = maybe.OrEmpty();
Console.WriteLine($"Print Value: {v}");
// Print Value: some value
```
```
var maybe = Maybe<string>.Nothing;
var v = maybe.OrEmpty();
Console.WriteLine($"Print Value: {v}");
// Print Value: 
```

## MaybeExtensions.OrNull
Allow to get the encapsulated value or null.

Returns the maybe value if HasValue is true, otherwise `null`.

### Examples
```
var maybe = "some value".ToMaybe();
var v = maybe.OrNull();
Console.WriteLine($"Print Value: {v}");
// Print Value: some value
```
```
var maybe = Maybe<string>.Nothing;
var v = maybe.OrNull();
Console.WriteLine($"Print Value: {v}");
// Print Value: 
```

## MaybeExtensions.OrTrue
Allow to get the boolean maybe encapsulated value or True.

Returns the maybe value if HasValue is true, otherwise `true`.

### Examples
```
var maybe = false.ToMaybe();
var v = maybe.OrTrue();
Console.WriteLine($"Print Value: {v}");
// Print Value: False
```
```
var maybe = Maybe<bool>.Nothing;
var v = maybe.OrTrue();
Console.WriteLine($"Print Value: {v}");
// Print Value: True
```

## MaybeExtensions.OrFalse
Allow to get the boolean maybe encapsulated value or False.

Returns the maybe value if HasValue is true, otherwise `false`.

### Examples
```
var maybe = false.ToMaybe();
var v = maybe.OrFalse();
Console.WriteLine($"Print Value: {v}");
// Print Value: False
```
```
var maybe = Maybe<bool>.Nothing;
var v = maybe.OrFalse();
Console.WriteLine($"Print Value: {v}");
// Print Value: False
```
