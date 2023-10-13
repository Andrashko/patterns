using System.Collections.Generic;
using System;
using System.Xml;

namespace Structural.Facade
{

    class ArtItem 
    {
        public int Id;
        public string Title;
        public ArtItem (int Id, string Title)
        {
            this.Title = Title;
            this.Id = Id;
        }
        override public string ToString()
        {
            return $"{Id}: '{Title}' ";
        }
    }

    class FetchMusic 
    {
        private static List<ArtItem> Resources = new List<ArtItem>()
        {
            new ArtItem(1, "The Fragile"),
            new ArtItem(2, "Alladin Sane"),
            new ArtItem(3, "OK Computer")
        };

        public static ArtItem Fetch (int Id)
        {
            return Resources.Find(item => item.Id == Id);
        }
    }


    class GetMovie
    {
        private int Id;
        public GetMovie(int Id)
        {
            this.Id = Id;
        }
        private List<ArtItem> Resources = new List<ArtItem>()
        {
            new ArtItem(1, "Apocalypse Now"),
            new ArtItem(2, "Die Hard"),
            new ArtItem(3, "Big Lebowski")
        };

        public ArtItem Value {
            get
            {
                return Resources.Find(item => item.Id == Id);
            }
        }

    }

    class TvShowResource
    {
        public static List<ArtItem> Get() {
            return new List<ArtItem>()
            {
                new ArtItem(1, "Twin Peaks"),
                new ArtItem(2, "Luther" ),
                new ArtItem(3, "The Simpsons")
            };
        }
    }


    class ArtFacade
    {
        private static List<ArtItem> Books 
        {
            get 
            {
                var result = new List<ArtItem> ();
                using (XmlTextReader reader = new XmlTextReader("Books.xml"))
                {
                    reader.ReadStartElement("BookList");
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            reader.ReadStartElement("Book");
                            result.Add(new ArtItem( Convert.ToInt32(reader.ReadElementString("Id")), reader.ReadElementString("Title")));
                        }
                    }
                }    
                return result;
            }
        }

        static public ArtItem Get (string Type, int Id) 
        {
            switch (Type)
            {
                case "Music":
                    return FetchMusic.Fetch(Id);
                case "Movie":
                    return new GetMovie(Id).Value;
                case "TVShow":
                    return TvShowResource.Get().Find(item => item.Id == Id);
                case "Book":
                    return Books.Find(item => item.Id == Id);
                default:
                    throw new Exception("Type error");
            }
        }
    }


}