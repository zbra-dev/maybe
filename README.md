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

