using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog
{
    public static class BlogService
    {
        public static List<KeyCount> NumberOfCommentsPerUser(MyDbContext context) => context.BlogComments
            .GroupBy(comment => comment.UserName)
            .Select(group => new KeyCount { name = group.Key, count = group.Count() })
            .ToList();

        public static List<PostLastComment> PostsOrderedByLastCommentDate(MyDbContext context) => context.BlogPosts
            .Select(post => new {
                name = post.Title,
                lastComment = post.Comments.OrderByDescending(comment => comment.CreatedDate).First()
            })
            .Select(post => new PostLastComment {
                name = post.name,
                lastCommentDate = post.lastComment.CreatedDate,
                lastCommentText = post.lastComment.Text
            })
            .OrderByDescending(post => post.lastCommentDate)
            .ToList();

        public static List<KeyCount> NumberOfLastCommentsLeftByUser(MyDbContext context) => context.BlogPosts
            .Select(post => post.Comments
                .OrderByDescending(comment => comment.CreatedDate).First())
            .GroupBy(comment => comment.UserName)
            .Select(group => new KeyCount { name = group.Key, count = group.Count() })
            .ToList();
    }

    public class KeyCount 
    {
        public string name { get; set; }
        public int count { get; set; }
    }
    public class PostLastComment 
    {
        public string name { get; set; }
        public DateTime lastCommentDate { get; set; }
        public string lastCommentText { get; set; }
    }
}
