## Overview of NBuilder ##

Through a fluent, extensible interface, NBuilder allows you to rapidly create test data, automatically assigning values to properties and public fields that are of type of the built in .NET data types (int, string etc). NBuilder allows you to override for properties you are interested in using lambda expressions.

This test data has a variety of uses. For example:

  * For automated functional and acceptance tests
  * Returning the data from a stubbed service
  * Creating test data for use when developing or testing an application

NBuilder's other major features are:

  * Persistence: Easily persist your objects using `Persist()`
  * Hierarchy generation: You can easily create hierarchies of objects simply by telling NBuilder how to add children to your object. You can even persist the hierarchies just by giving NBuilder create and update methods.
  * Configurability: NBuilder is highly configurable. Through the `BuilderSetup` class you can control how NBuilder names objects and disable naming for certain properties of certain types.
  * Extensibility: Through extension methods you can extend NBuilder's fluent interface to add custom building functionality. You can also create custom property namers globally or for specific types.

The best way of learning how to use NBuilder is to download the source and take a look at the functional tests but for some quick examples, see below.

## Creating Single Objects ##

### Creating a single object ###
```
var product = Builder<Product>.CreateNew().Build();
```

This will create a single `Product` object with default values like this:
```
   Product:
       Id              : 1
       Title           : "Title1"
       Description     : "Description1"
       QuantityInStock : 1       
```

### Passing in constructor args ###
```
    var basketItem = Builder<BasketItem>
        .CreateNew()
            .WithConstructorArgs(basket, product, quantity)
        .Build();
```

### Setting property values ###
```
var product = Builder<Product>
                .CreateNew()
                    .With(x => x.Description = "A custom description here")
                .Build();
```

This will give you:
```
   Product:
       Id              : 1
       Title           : "Title1"
       Description     : "A custom description here"
       QuantityInStock : 1       
```

### Setting multiple properties ###
```
var product = Builder<Product>
                .CreateNew()
                .With(x => x.Title = "Special title")
                .And(x => x.Description = "Special description") 
                .And(x => x.Id = 2)
                .Build();
```
Note: `With()` and `And()` do exactly the same thing, but `And()` reads a bit better when you're setting a few properties.

### Calling methods ###

You can use `Do()` to call methods.
```
var child = Builder<Category>.CreateNew().Build();

var category = Builder<Category>
               .CreateNew()
               .Do(x => x.AddChild(child))
               .Build();
```

### Calling a method multiple times ###

You can call a method multiple times for all the items of a list.

This example adds the product to five categories.

```
var categories = Builder<Category>.CreateListOfSize(5).Build();

var product = Builder<Product>
              .CreateNew()
              .DoForAll( (prod, cat) => prod.AddToCategory(cat), categories)
              .Build();
```

## Creating Lists ##

NBuilder allows you to create lists of objects giving the properties sequential values. This automatic naming of properties can be disabled for individual properties of individual types or completely disabled if need be.

### Creating a list ###
```
var products = Builder<Product>.CreateListOfSize(10).Build();
```

This would give you:

```
products[0]:
   Id - 1
   Title - "Title1"
   Description - "Description1"

products[1]:
   Id - 2
   Title - "Title2"
   Description - "Description2"

...

products[9]
   Id - 10
   Title - "Title10"
   Description - "Description10"

```

### Setting the value of a property for every item in the list ###
```
var products = Builder<Product>
                .CreateListOfSize(10)
                .WhereAll()
                    .Have(x => x.Title = "A special title")
                .Build();
```

### Setting a value of a property for just some of the items in a list ###

NBuilder has several methods which allow you to declare how certain parts of the list should have their properties set or how those objects should be constructed.

**`WhereTheFirst(x) & AndTheNext(x)`**

```
var items = Builder<BasketItem>
                .CreateListOfSize(4)
                .WhereTheFirst(2)
                    .AreConstructedWith(arg1, arg2, arg3)
                .AndTheNext(2)
                    .AreConstructedWith(arg4, arg5, arg6)
                .Build();
```

**`WhereTheLast(x) & AndThePrevious(x)`**
```
var list = Builder<Product>
                .CreateListOfSize(30)
                .WhereTheLast(10)
                    .Have(x => x.Title = "Special Title 1")
                .AndThePrevious(10)
                    .Have(x => x.Title = "Special Title 2")
                .Build();
```

**`WhereRandom(x)`**
```
var products = Builder<Product>.CreateListOfSize(10)
                .WhereRandom(5)
                    .Have(x => x.Description = specialdescription)
                    .And(x => x.PriceBeforeTax = specialPrice)
                .Build();
```


**`WhereSection(x, y)`**
```
var list = Builder<Product>
                .CreateListOfSize(30)
                .WhereSection(12, 14)
                    .Have(x => x.Title = "Special Title 2")
                .Build();
```


### Calling methods on your objects ###
```
var categories = Builder<Category>
                .CreateListOfSize(10)
                .WhereTheFirst(2)
                    .HaveDoneToThem(x => x.AddChild(children[0]))
                .Build();
```

and

```
var products = Builder<Product>
           .CreateListOfSize(10)
           .WhereAll().HaveDoneToThemForAll((product, category) => product.AddToCategory(category), categories)
           .Build();
```

## The **`Pick`** class ##

The `Pick` class allows you to add random elements from another list.

### Picking one random item from a secondary list ###

```
var children = Builder<Category>.CreateListOfSize(10).Build();

var categories = Builder<Category>
                .CreateListOfSize(10)
                .WhereTheFirst(2)
                    .HaveDoneToThem(x => x.AddChild(Pick<Category>.RandomItemFrom(children)))
                .Build();
```

### Picking multiple random items ###

This would add every one of the 500 products to between 5 and 10 of 50 categories.

```
var categories = Builder<Category>.CreateListOfSize(50).Build();

            var products = Builder<Product>
                            .CreateListOfSize(500)
                            .WhereAll()
                                .Have(x => x.Categories = Pick<Category>.UniqueRandomList(With.Between(5, 10).Elements).From(categories))
                            .Build();
```

## Generators ##

You can use generators for repeat assignments.

### Sequential generator ###

This would assign 10000, 11000, 12000 to `Id`

```
var generator = new SequentialGenerator<int> { Direction = GeneratorDirection.Ascending, Increment = 1000 };
generator.StartingWith(10000);

var products = Builder<Product>
               .CreateListOfSize(3)
               .WhereAll()
                   .Have(x => x.Id = generator.Generate())
               .Build();
```

### Random generator ###
```
var generator = new RandomGenerator();

            var products = Builder<Product>
                .CreateListOfSize(10)
                .WhereAll()
                    .Have(x => x.PriceBeforeTax = generator.Next(50, 1000))
                .Build();
```

## Persistence ##

NBuilder also allows you to easily set up persistence. You do this by telling NBuilder how to persist your objects. The most convenient place to do this would be in an NUnit `SetUpFixture` class.
```
var repository = new ProductRepository();

BuilderSetup.SetPersistenceCreateMethod<IList<Product>>(repository.CreateAll);
```

Once you have done this, it's simply a case of calling Persist() instead of Build():
```
Builder<Product>.CreateListOfSize(100).Persist();
```

You can do this for single objects and lists


## Building and persisting hierarchies ##

You can easily create a random hierarchy by first creating an initial list and then calling `BuildHierarchy()`, and passing in a specification.

```
var hierarchySpec = Builder<HierarchySpec<Category>>.CreateNew()
                .With(x => x.AddMethod = (parent, child) => parent.AddChild(child))
                .With(x => x.Depth = 5)
                .With(x => x.MaximumChildren = 10)
                .With(x => x.MinimumChildren = 5)
                .With(x => x.NamingMethod = (cat, title) => cat.Title = "Category " + title)
                .With(x => x.NumberOfRoots = 10).Build();

            Builder<Category>.CreateListOfSize(2500).BuildHierarchy(hierarchySpec);
```

This will create a category tree and by supplying a naming method, will even name your categories with their path in the tree. For example:

```
  Category - Title = "1"
      Category - Title = "1.1"
      Category - Title = "1.2"
  Category - Title = "2"
      Category - Title = "2.1"
          Category - Title = "2.1.1"
      Category - Title = "2.2"
      Category - Title = "2.3"
```

This allows you to easily identify your hierarchical object's position in relation to the whole hierarchy.

You can even persist this hierarchy by calling `PersistHierarchy()` instead.

```
Builder<Category>.CreateListOfSize(2500).PersistHierarchy(hierarchySpec);
```

## Configuration ##

NBuilder allows you to change its default behaviour using the `BuilderSetup` class.

### Custom persistence service ###

```
BuilderSetup.SetPersistenceService(new MyCustomPersistenceService());

Builder<Product>.CreateNew().Persist();
```

### Turning off automatic property naming ###

If you don't want properties to be automatically given values, you can simply turn it off.

```
BuilderSetup.AutoNameProperties = false;
```

### Changing the default property namer ###

You can change the default property namer to use the random value property namer, or you can create your own either from scratch implementing the `IPropertyNamer` interface, or by extending one of the classes, for example to add support

for more types.

```
BuilderSetup.SetDefaultPropertyNamer(new RandomValuePropertyNamer());
```

### Adding a property namer for a specific type ###

If, for example, you have a class that has a custom struct, NBuilder will ignore this property because it doesn't know how to set it. You could overcome this by adding a special property namer, just for `Product`s.

```
BuilderSetup.SetPropertyNamerFor<Product>(new CustomProductPropertyNamer(new ReflectionUtil()));
```

### Disabling automatic property naming for a specific property of a specific type ###

If you don't want values to automatically be assigned to certain properties, you can disable it like this:

```
BuilderSetup.DisablePropertyNamingFor<Product, int>(x => x.Id);
```


## Extensibility ##

### Custom declarations ###

In NBuilder nearly all of the public interface is implemented with extension methods. This of course means it's possible to add your own.

For example, out of the box the list builder has seven 'declarations' `WhereAll(), WhereRandom(n), WhereRandom(n, start, end), WhereTheFirst(n), WhereTheLast(n), AndTheNext(n), AndThePrevious(n)`. However if you wanted to add your own,

e.g. to return all the even or odd items, all you need to do is write a new extension method - `WhereAllEven()`

### "Operable" extensions ###

If, for example, you find yourself repeating yourself when creating test data and you want to wrap something up in a method, you can do this by extending `IOperable<T>`. You can do this generically or per-type.

For example say if rather than saying:

```
`Builder<Product>.CreateListOfSize(10).WhereAll().Have(x => x.Title = "12345....[LongString].....12345").Build();`
```

You could instead create an extension method:

```
public static IOperable<Product> HaveLongTitles(this IOperable<Product> operable)
{
    ((IDeclaration<Product>) operable).ObjectBuilder.With(x => x.Title = "12345....[LongString].....12345");
    return operable;
}
```

Giving you the ability to say:

```
Builder<Product>
    .CreateListOfSize(10)
    .WhereAll()
        .HaveLongTitles()
    .Build();
```

You could of course make it even more succinct by adding an extension method to `IListBuilder<Product>`

```
public static IListBuilder<Product> WhereAllHaveLongTitles(this IListBuilder<Product> listBuilder)
{
    var listBuilderImpl = (IListBuilderImpl<Product>) listBuilder;
    var declaration = new GlobalDeclaration<Product>(listBuilderImpl, listBuilderImpl.CreateObjectBuilder());
    declaration.Have(x => x.Title = "12345....[LongString].....12345");

    return declaration;
}
```

This would allow you to say:

```
Builder<Product>.CreateListOfSize(10).WhereAllHaveLongTitles();
```


## For more examples, please check the functional tests ##

Until the full documentation is available please have a look at the functional tests in the source code. These explain how to do everything that's currently possible in NBuilder.


