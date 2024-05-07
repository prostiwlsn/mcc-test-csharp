using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Blog;
using Microsoft.Extensions.Logging;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");

        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        var context = new MyDbContext(loggerFactory);
        context.Database.EnsureCreated();
        InitializeData(context);

        Console.WriteLine("All posts:");
        var data = context.BlogPosts.Select(x => x.Title).ToList();
        Console.WriteLine(JsonSerializer.Serialize(data));
            
            
        Console.WriteLine("How many comments each user left:");
        //ToDo: write a query and dump the data to console
        // Expected result (format could be different, e.g. object serialized to JSON is ok):
        // Ivan: 4
        // Petr: 2
        // Elena: 3
        /*

        var firstQuery = context.BlogComments
            .GroupBy(comment => comment.UserName)
            .Select(group => new { name = group.Key, count = group.Count()})
            .ToList();

        foreach (var item in firstQuery)
            Console.WriteLine(item.name + " " + item.count);
        */

        Console.WriteLine(
             JsonSerializer.Serialize(BlogService.NumberOfCommentsPerUser(context)));

        Console.WriteLine("Posts ordered by date of last comment. Result should include text of last comment:");
        //ToDo: write a query and dump the data to console
        // Expected result (format could be different, e.g. object serialized to JSON is ok):
        // Post2: '2020-03-06', '4'
        // Post1: '2020-03-05', '8'
        // Post3: '2020-02-14', '9'

        /*

        var secondQuery = context.BlogPosts
            .Select(post => new { 
                name = post.Title, 
                lastComment = post.Comments.OrderByDescending(comment => comment.CreatedDate).First()
            })
            .Select(post => new
            {
                post.name,
                lastCommentDate = post.lastComment.CreatedDate,
                lastCommentText = post.lastComment.Text
            })
            .OrderByDescending(post => post.lastCommentDate)
            .ToList();

        foreach (var item in secondQuery)
            Console.WriteLine(item.name + " " + item.lastCommentDate.ToShortDateString() + " " + item.lastCommentText);
        */

        Console.WriteLine(
             JsonSerializer.Serialize(BlogService.PostsOrderedByLastCommentDate(context)));

        Console.WriteLine("How many last comments each user left:");
        // 'last comment' is the latest Comment in each Post
        //ToDo: write a query and dump the data to console
        // Expected result (format could be different, e.g. object serialized to JSON is ok):
        // Ivan: 2
        // Petr: 1

        /*
        var thirdQuery = context.BlogPosts
            .Select(post => post.Comments
                .OrderByDescending(comment => comment.CreatedDate).First())
            .GroupBy(comment => comment.UserName)
            .Select(group => new { name = group.Key, count = group.Count() })
            .ToList();

        foreach (var item in thirdQuery)
            Console.WriteLine(item.name + " " + item.count);
        */

        Console.WriteLine(
             JsonSerializer.Serialize(BlogService.NumberOfLastCommentsLeftByUser(context)));

        // Console.WriteLine(
        //     JsonSerializer.Serialize(BlogService.NumberOfCommentsPerUser(context)));
        // Console.WriteLine(
        //     JsonSerializer.Serialize(BlogService.PostsOrderedByLastCommentDate(context)));
        // Console.WriteLine(
        //     JsonSerializer.Serialize(BlogService.NumberOfLastCommentsLeftByUser(context)));

    }

    private static void InitializeData(MyDbContext context)
    {
        context.BlogPosts.Add(new BlogPost("Post1")
        {
            Comments = new List<BlogComment>()
            {
                new BlogComment("1", new DateTime(2020, 3, 2), "Petr"),
                new BlogComment("2", new DateTime(2020, 3, 4), "Elena"),
                new BlogComment("8", new DateTime(2020, 3, 5), "Ivan"),
            }
        });
        context.BlogPosts.Add(new BlogPost("Post2")
        {
            Comments = new List<BlogComment>()
            {
                new BlogComment("3", new DateTime(2020, 3, 5), "Elena"),
                new BlogComment("4", new DateTime(2020, 3, 6), "Ivan"),
            }
        });
        context.BlogPosts.Add(new BlogPost("Post3")
        {
            Comments = new List<BlogComment>()
            {
                new BlogComment("5", new DateTime(2020, 2, 7), "Ivan"),
                new BlogComment("6", new DateTime(2020, 2, 9), "Elena"),
                new BlogComment("7", new DateTime(2020, 2, 10), "Ivan"),
                new BlogComment("9", new DateTime(2020, 2, 14), "Petr"),
            }
        });
        context.SaveChanges();
    }
}