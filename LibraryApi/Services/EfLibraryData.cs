﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryApi.Domain;
using LibraryApi.Models.Books;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    //*This wouild be the class to unittest

    public class EfLibraryData : IQueryForBooks, IDoBookCommands
    {
        private readonly LibraryDataContext _context;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfig;

        public EfLibraryData(LibraryDataContext context, IMapper mapper, MapperConfiguration mapperConfig)
        {
            _context = context;
            _mapper = mapper;
            _mapperConfig = mapperConfig;
        }

        public async Task<GetBookDetailsResponse> AddBook(PostBookCreate bookToAdd)
        {
            // add it to the db context. (we need to make it a book)
            var book = _mapper.Map<Book>(bookToAdd); // PostBookCreate -> Book

            _context.Books.Add(book); // book.Id = 0;
                                      // save the changes to the database.
            await _context.SaveChangesAsync();
            // book.Id = 8;
            var response = _mapper.Map<GetBookDetailsResponse>(book); // Book -> GetBookDetailsResponse

            // return a 201, with location header, with a copy of what they'd get from that location
            return response;
        }

        public async Task<GetBooksResponse> GetAllBooks()
        {
            var response = new GetBooksResponse();

            var books = await _context.BooksInInventory()
                .ProjectTo<GetBooksResponseItem>(_mapperConfig)
                .ToListAsync();

            response.Data = books;
            return response;
        }

        public async Task<GetBookDetailsResponse> GetBooksById(int bookId)
        {
            var book = await _context.BooksInInventory()
                .Where(b => b.Id == bookId)
                .ProjectTo<GetBookDetailsResponse>(_mapperConfig)
                .SingleOrDefaultAsync();
            return book;
        }

        public async Task RemoveBooks(int bookId)
        {
            var book = await _context.BooksInInventory().SingleOrDefaultAsync(b => b.Id == bookId);
            if (book != null)
            {
                book.IsInInventory = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UpdateTitle(int bookId, string title)
        {
            var book = await _context.BooksInInventory()
            .Where(b => b.Id == bookId)
            .SingleOrDefaultAsync();
            if (book == null)
            {
                return false;
            }
            else
            {
                book.Title = title;
                await _context.SaveChangesAsync();
                return true;
            }
        }
    }
}
