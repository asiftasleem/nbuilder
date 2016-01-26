# NBuilder #

Please see the [official website: http://nbuilder.org](http://nbuilder.org)

## Update 4 June 2012 ##

Source has been moved to GitHub under: https://github.com/garethdown44/nbuilder/

## Update 14 August 2011: Version 3.0 Release ##

Some bug fixes and patches, courtesy of Joshua Roth.

Also revised the syntax. Now building lists and building single objects share the same method names.

For example, where it used to be:
```
Builder<Product>.CreateNew().With(x => x.Title = "some title").Build();

Builder<Product>.CreateListOfSize(10).WhereAll().Have(x => x.Title = "some title").Build();
```

Now the syntax for building a list is:

```
Builder<Product>.CreateListOfSize(10).All().With(x => x.Title = "some title").Build();
```

The old syntax methods are still there but have been marked `[Obsolete]`. Please note though that a build is included where these methods are not obsolete. This is for cases where people don't want compiler warnings about using `[Obsolete]` methods.

The recommended approach though is to find-and-replace and convert the old syntax to the new. The changes are as follows:

```
WhereAll()  ->  All()
WhereTheFirst()  ->  TheFirst()
WhereTheLast()  ->  TheLast()
WhereSection()  ->  Section()
WhereRandom()  ->  Random()
AndTheNext()  ->  TheNext()
AndThePrevious()  ->  ThePrevious()
AndTheLast()  ->  TheLast()
Have()  ->  With()
Has()  ->  With()
```

I'll be posting up a script in the next few days to find and replace all method calls within a codebase.

[Download 3.0 here](http://code.google.com/p/nbuilder/downloads)

## Update 29 August 2010: Version 2.2.0 Release ##

This adds support for silverlight. Both the 3.5 CLR and a Silverlight 3 version are included in the zip file. Also includes some other very minor bug fixes.

[Download 2.2.0 here](http://code.google.com/p/nbuilder/downloads)

## Update 20 October 2009: Version 2.1.9 beta DLL available ##

Since there are a few patches and new features all at once, I thought it would be best to issue a beta release. Please [report any issues](http://code.google.com/p/nbuilder/issues/list).

  * Support for Guids and sbytes - the property value assigners (property namers) now all support guids and sbytes.

  * New constructor syntax:
```
Builder<Product>.CreateNew()
    .WithConstructor( () => new Product("title", 5.5m) )
    .Build()
```

  * Extensible Random Value Property Namer. No documentation at the moment but see [issue 21](http://code.google.com/p/nbuilder/issues/detail?id=21&can=1) for details. Many thanks to Tim Scott for providing this patch.

  * [Issue 23](http://code.google.com/p/nbuilder/issues/detail?id=23&can=1) fixed. (Thanks to  Jan Van Ryswyck for this patch.

  * [Issue 20](http://code.google.com/p/nbuilder/issues/detail?id=20&can=1) fixed. (Thanks to Lukasz Podolak and Bobby Johnson for this patch)

  * A few other minor bug fixes - some wrong or misleading error and warning messages have been fixed.


## Update 12 June 2009: Version 2.1.8 DLL and source code now available ##

  * Fixes issues [14](http://code.google.com/p/nbuilder/issues/detail?id=14&can=1) and [15](http://code.google.com/p/nbuilder/issues/detail?id=15&can=1)

[Download NBuilder 2.1.8 now](http://code.google.com/p/nbuilder/downloads/list)

## Update 6 June 2009: Version 2.1.7 DLL and source code now available ##

  * Fluent dates - See [this blog post](http://shouldbeableto.wordpress.com/2009/06/06/fluent-dates-added-to-nbuilder/) for more details.

  * `WhereRandom()` bug fixed - [Issue 11](http://code.google.com/p/nbuilder/issues/detail?id=11&can=1)

  * Addition of singular methods - e.g. `Has(x => ...), HasDoneToIt(), HasDoneToItForAll(), IsConstructedWith()` [Issue 8](http://code.google.com/p/nbuilder/issues/detail?id=8&can=1)

## Update 30 May 2009: Version 2.1.6 DLL and source code now available ##

  * Fix for [issue 5](http://code.google.com/p/nbuilder/issues/detail?id=5&can=1) - `Pick<>.RandomItemFrom()` picks the same item when creating a list
  * Fix for [issue 6](http://code.google.com/p/nbuilder/issues/detail?id=6&can=1) - `RandomItemPicker` will never choose last item in list
  * New feature detailed in [issue 7](http://code.google.com/p/nbuilder/issues/detail?id=7&can=1) - NBuilder will now assign values to enum properties

Thanks to Kevin Kuebler for finding the defects, fixing both of them and implementing enum support!

To see it in action please see the [Overview/HowTo page](Overview_HowTo.md)







