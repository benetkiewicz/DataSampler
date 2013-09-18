## Introduction

__DataSampler__ is a simple framework that will get you up to speed while learning new data driven or presentation frameworks. With __DataSampler__ you'll be able to imediately focus on important stuff without worrying where to get sample data to work with.

## Usage
Suppose you want to learn how Knockout.JS data binding works with ASP.NET MVC WebAPI. You have the following class to play with.

    class Book
    {
        public int Id { get; set; }
        
        public string Author { get; set; }
        
        public string Title { get; set; }
        
        public DateTime ReleaseDate { get; set; }
    }

Now instead of worrying how to create a dozen of fake Books to play with, you can simply write the following:

    var sampleBooks = new DataSampler<Book>()
            .AddPropertyConfiguration(x => x.Id, () => 1)
            .AddPropertyConfiguration(x => x.Author, () => "author")
            .AddPropertyConfiguration(x => x.Title, () => "title")
            .AddPropertyConfiguration(x => x.ReleaseDate, () => DateTime.Now)
            .GenerateListOf(12);
That will give you a list of twelve sample books. A bit boring, I admit, so how about that:
* more complicated example of property generation
* nested class example