using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moma.IwtForumConverter.WsIwt;

namespace Moma.IwtForumConverter
{
    class DataTransformationMgmt
    {

        public List<Message> CreatePost(List<Message> recoveredMessages)
        {
            List<Message> replies;

            foreach (Message message in recoveredMessages)
            {
                replies = (from a in recoveredMessages.AsEnumerable() where message.RepliesIds.Contains<int>(Convert.ToInt32(a.Id)) select a).ToList();

                foreach (Message replyMessage in replies)
                {
                     // Post retrievedPost = RetrievePostById(Convert.ToInt32(replyMessage.Id), recoveredMessages, message);
                    replyMessage.ReplyOf = message;
                 //   message.Replies.Add(replyMessage);
                }
            }

            return recoveredMessages;
        }

        private Post RetrievePostById(int messageId, List<Message> messages, Post higherLevelPost)
        {

            Message foundMessage;
            Post createdPost;

            try
            {

                foundMessage = (from a in messages where Convert.ToInt32(a.Id) == messageId select a).Single<Message>();

            }
            catch
            {

                foundMessage = null;
            }



            if (foundMessage != null)
            {
                createdPost = new Post
                {
                    Categories = foundMessage.Categories,
                    Content = foundMessage.Content,
                    CreationDate = foundMessage.CreationDate,
                    Creator = foundMessage.Creator,
                    Id = foundMessage.Id,
                   // Replies = null,
                    ReplyOf = higherLevelPost
                };

            }
            else
            {
                createdPost = null;
            }

            return createdPost;

        }
    }
}
