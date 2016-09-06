## Introduction

__DataSampler__ is a simple framework that will get you up to speed while learning new data driven or presentation frameworks. With __DataSampler__ you'll be able to imediately focus on important stuff without worrying where to get sample data to work with.

## Usage
Suppose you want to learn how Knockout.JS data binding works with ASP.NET MVC WebAPI. You have the following class to play with.

```csharp
    class Book
    {
        public int Id { get; set; }
        
        public string Author { get; set; }
        
        public string Title { get; set; }
        
        public DateTime ReleaseDate { get; set; }
    }
```

Now instead of worrying how to create a dozen of fake Books, you can simply write the following:

    var sampleBooks = new DataSampler<Book>()
            .AddPropertyConfiguration(x => x.Id, () => 1)
            .AddPropertyConfiguration(x => x.Author, () => "author")
            .AddPropertyConfiguration(x => x.Title, () => "title")
            .AddPropertyConfiguration(x => x.ReleaseDate, () => DateTime.Now)
            .GenerateListOf(12);
That will give you a list of twelve sample books. A bit boring, I admit, so how about that:

    int i, j = 0;
    var sampleBooks = new DataSampler<Book>()
            .AddPropertyConfiguration(x => x.Id, () => i++)
            .AddPropertyConfiguration(x => x.Author, () => Guid.NewGuid().ToString())
            .AddPropertyConfiguration(x => x.Title, () => Guid.NewGuid().ToString())
            .AddPropertyConfiguration(x => x.ReleaseDate, () => DateTime.Now.AddDays(j++))
            .GenerateListOf(12);

That's cool but what if you have Author modelled as a sub class? 

    class Author
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
    }
No problem, use AddKnownType()

    var sampleBooks = new DataSampler<Book>()
            .AddPropertyConfiguration(x => x.Id, () => 1)
            .AddPropertyConfiguration(x => x.Author.FirstName, () => Guid.NewGuid().ToString())
            .AddPropertyConfiguration(x => x.Author.LastName, () => Guid.NewGuid().ToString())
            .WithKnownType(typeof(Author))
            .GenerateListOf(12);
