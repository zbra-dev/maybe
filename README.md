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


## MaybeExtensions.ToMaybe
### Remarks
Converts the value to a Maybe.

If the value is null, so it returns a Maybe.Nothing for that specific type.
For structs it always returns the Maybe.

### Method definition
```
public static Maybe<T> ToMaybe<T>(this T value)
```
### Overloads
Overload to convert structs.
```
public static Maybe<S> ToMaybe<S>(this S? value) where S : struct
```

This overload will always return the original Maybe, preveting the user to create a Maybe of a Maybe.
```
public static Maybe<T> ToMaybe<T>(this Maybe<T> value)
```

### Examples
Transform an object to a maybe and access its properties
```
var myObject = new { PropertyA = "Value Of Property A", PropertyB = 66 };
var maybe = myObject.ToMaybe();
Console.WriteLine($"Print Value: {maybe}");
// Print Value: { PropertyA = Value Of Property A, PropertyB = 66 }
Console.WriteLine($"Maybe PropertyA value: {maybe.Select(o => o.PropertyA)}");
// Maybe PropertyA value: Value Of Property A
Console.WriteLine($"Maybe PropertyB value: {maybe.Select(o => o.PropertyB)}");
// Maybe PropertyB value: 66
```
Transform an int to a maybe and access it value.
```
var myInt = 66;
var maybe = myInt.ToMaybe();
Console.WriteLine($"Print Value: {maybe}");
// Print Value: 66
Console.WriteLine($"Print Maybe value: {maybe.Select(i => i)}");
//Print Maybe value: 66
```
Try to transform a Maybe object to another Maybe. 
```
var myObject = new { PropertyA = "Value Of Property A", PropertyB = 66 };
var maybe = myObject.ToMaybe();
var secondMaybe = maybe.ToMaybe();
Console.WriteLine($"First Maybe Print Value: {maybe}");
// First Maybe Print Value: { PropertyA = Value Of Property A, PropertyB = 66 }
Console.WriteLine($"Second Maybe Print Value: {secondMaybe}");
// Second Maybe Print Value: { PropertyA = Value Of Property A, PropertyB = 66 }

```
Try to transform a empty Maybe to another Maybe
```
var maybe = Maybe<string>.Nothing;
var secondMaybe = maybe.ToMaybe();
Console.WriteLine($"First Maybe Print Value: {maybe}");
// First Maybe Print Value: 
Console.WriteLine($"Second Maybe Print Value: {secondMaybe}");
// Second Maybe Print Value: 
```

## MaybeExtensions.OrEmpty

### Remarks
Allow to get the encapsulated value or an empty string.

Returns the maybe value if HasValue is true, otherwise `string.Empty`.

### Method definition
```
public static string OrEmpty(this Maybe<string> subject);
```

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
### Remarks
Allow to get the encapsulated value or null.

Returns the maybe value if HasValue is true, otherwise `null`.

### Method definition
```
public static T OrNull<T>(this Maybe<T> subject) where T : class;
```

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
### Remarks
Allow to get the boolean maybe encapsulated value or True.

Returns the maybe value if HasValue is true, otherwise `true`.
### Method definition
```
public static bool OrTrue(this Maybe<bool> subject);
```
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
### Remarks
Allow to get the boolean maybe encapsulated value or False.

Returns the maybe value if HasValue is true, otherwise `false`.

### Method defintion
```
public static bool OrFalse(this Maybe<bool> subject);
```
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


## MaybeExtensions.ToNullable
### Remarks
Convert a Maybe to Nullable. 

If the Maybe.HasValue is true, return its `value` otherwise a `null`.

### Method definition
```
public static T? ToNullable<T>(this Maybe<T> value) where T : struct
```
### Examples
```
var maybe = 1234.ToMaybe();
var v = maybe.ToNullable();
Console.WriteLine($"Print Value: {v}");
// Print Value: 1234
```
```
var maybe = Maybe<int>.Nothing;
var v = maybe.ToNullable();
Console.WriteLine($"Print Value: {v}");
// Print Value: 
```


# RAW DATA

## MaybeExtensions.ToMaybe
overloads

/// Converts the value to Maybe&lt;S&gt;.
/// Maybe&lt;<typeparamref name="S"/>&gt;.Nothing if value.HasValue is false,
/// otherwise new Maybe&lt;<typeparamref name="S"/>&gt;(value.Value)
/// <param name="value"> The value to be converted.</param>

Maybe&lt;S&gt; ToMaybe&lt;S&gt;(this S? value) where S : struct

/// <summary>
/// This methods prevents stacking maybes, it just returns value
/// </summary>
/// <returns>
/// value
/// </returns>
/// <param name="value"> The value to be converted.</param>

Maybe<T> ToMaybe<T>(this Maybe<T> value)

/// <summary>
/// Converts the value to Maybe&lt;<typeparamref name="T"/>&gt;.
/// </summary>
/// <returns>
/// Maybe&lt;<typeparamref name="T"/>&gt;.Nothing if value is null,
/// otherwise new Maybe&lt;<typeparamref name="T"/>&gt;(value)
/// </returns>
/// <param name="value"> The value to be converted.</param>
Maybe<T> ToMaybe<T>(this T value)


## MaybeExtensions.Select

/// <summary>
/// Projects the value according to the selector.
/// Analogous to Linq's Select.
/// </summary>
/// <returns>
/// Maybe&lt;<typeparamref name="V"/>&gt;.Nothing if subject.HasValue is false,
/// otherwise returns an instance of Maybe&lt;<typeparamref name="V"/>&gt; with the projected value
/// </returns>
/// <param name="subject"> The subject that will be projected.</param>
/// <param name="selector"> The selector to be applied.</param>
Maybe<V> Select<T, V>(this Maybe<T> subject, Func<T, V> selector)

/// <summary>
/// Projects the value according to the selector.
/// Analogous to Linq's Select.
/// </summary>
/// <returns>
/// Maybe&lt;<typeparamref name="V"/>&gt;.Nothing if subject.HasValue is false,
/// otherwise returns an instance of Maybe&lt;<typeparamref name="V"/>&gt; with the projected value
/// </returns>
/// <param name="subject"> The subject that will be projected.</param>
/// <param name="selector"> The selector to be applied.</param>
Maybe<V> Select<T, V>(this Maybe<T> subject, Func<T, V?> selector)

## MaybeExtensions.SelectMany
/// <summary>
/// Projects the value according to the selector and flatten it.
/// Analogous to Linq's SelectMany.
/// </summary>
/// <returns>
/// Maybe&lt;<typeparamref name="V"/>&gt;.Nothing if subject.HasValue is false,
/// otherwise returns an instance of Maybe&lt;<typeparamref name="V"/>&gt; with the projected value
/// </returns>
/// <param name="subject"> The subject that will be projected.</param>
/// <param name="selector"> The selector to be applied.</param>
Maybe<V> SelectMany<T, V>(this Maybe<T> subject, Func<T, Maybe<V>> selector)


## MaybeExtensions.Zip
### Overloads 

/// <summary>
/// Zips two maybes together. Analogous to Linq's Zip.
/// </summary>
/// <returns>
/// The zipped maybe
/// </returns>
/// <param name="subject"> The subject that will be projected.</param>
/// <param name="other"> The other maybe to be zipped.</param>
/// <param name="transformer"> The transformer function to be applied.</param>
Maybe<R> Zip<T, U, R>(this Maybe<T> subject, Maybe<U> other, Func<T, U, R> transformer)

/// <summary>
/// Zips two maybes together. Analogous to Linq's Zip.
/// </summary>
/// <returns>
/// The zipped maybe
/// </returns>
/// <param name="subject"> The subject that will be projected.</param>
/// <param name="other"> The other maybe to be zipped.</param>
/// <param name="transformer"> The transformer function to be applied.</param>
Maybe<R> Zip<T, U, R>(this Maybe<T> subject, Maybe<U> other, Func<T, U, Maybe<R>> transformer)

## MaybeExtensions.ZipAndConsume
/// <summary>
/// Zips and consumes two maybes.
/// </summary>
/// <param name="subject"> The subject that will be projected.</param>
/// <param name="other"> The other maybe to be zipped.</param>
/// <param name="consumer"> The action to be applied to both maybes.</param>
void ZipAndConsume<T, U>(this Maybe<T> subject, Maybe<U> other, Action<T, U> consumer)

# End of  Methods Extensions

