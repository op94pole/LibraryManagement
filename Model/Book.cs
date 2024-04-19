﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Model
{
    public class Book
    {
        //public Book(string title, string authorName, string authorSurname, string publishingHouse, int quantity)
        //{
        //    BookId = _bookId++;
        //    Title = title;
        //    AuthorName = authorName;
        //    AuthorSurname = authorSurname;
        //    PublishingHouse = publishingHouse;
        //    Quantity = quantity;
        //}

        public Book()
        {
            BookId = _bookId++;            
        }

        private static int _bookId = 1;         
        public int BookId { get; set; }       
        public string? Title { get; set; }      
        public string? AuthorName { get; set; }        
        public string? AuthorSurname { get; set; }        
        public string? PublishingHouse { get; set; }       
        public int Quantity { get; set; }
    }
}