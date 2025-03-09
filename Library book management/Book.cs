using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
public class Book
{
    public string Author { get; set; }
    public string Title { get; set; }
    public string Year { get; set; }
    public Book(string author, string title, string year)
    {
        Author = author;
        Title = title;
        Year = year;
    }
}