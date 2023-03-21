# Maybe Extension Documentation

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