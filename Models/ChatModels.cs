
using System;
using System.Collections.Generic;
using System.Linq;
using Smoking.Extensions;

namespace Smoking.Models
{
    [Serializable]
    public class JChatMessage
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public string Author { get; set; }
    }

    public class ChatEditor
    {
        public ChatEditor()
        {

            InitProps();
        }
        public ChatEditor(bool skipInit)
        {
        }

        public void InitProps()
        {
            var db = new xChatDB();
            var chats = db.Chats.Where(x => x.IsClosed == IsArchive);
            if (StartDate.HasValue)
                chats = chats.Where(x => x.StartDate >= StartDate.Value);
            if (EndDate.HasValue)
                chats = chats.Where(x => x.StartDate <= EndDate.Value);
            ChatList = chats;

            if (ChatID.HasValue)
            {
                MessageList = db.ChatMessages.Where(x => x.ChatID == ChatID.Value).OrderBy(x => x.Date);
                Chat = db.Chats.FirstOrDefault(x => x.ID == ChatID);
                foreach (var message in MessageList)
                {
                    message.IsRead = true;
                }
                db.SubmitChanges();
            }
            else
            {
                MessageList = new List<ChatMessage>();
            }
            var mainDb = new DB();
            AnswerList = mainDb.CharAnswerTemplates.Select(x => x.Answer).OrderBy(x => x).ToList();
        }

        public bool IsArchive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ChatID { get; set; }
        public Chat Chat { get; set; }
        public IEnumerable<Chat> ChatList { get; set; }
        public IEnumerable<ChatMessage> MessageList { get; set; }
        public List<string> AnswerList { get; set; }
        public bool IsPost { get; set; }
    }


    public partial class ChatMessage
    {
        public string AuthorName
        {
            get
	        {
	            var db = new DB();
	            var user = db.Users.FirstOrDefault(x => x.UserId == UserID);
	            if (user == null)
	            {
	                return "Аноним";
	            }
	            return user.UserProfile.FullName;
	        }
        }

    }

    public partial class Chat
    {
        public string ChatName
        {
            get
            {
                var name = "<b>№{0}</b>, {1}, {2} сообщений".FormatWith(ID.ToString(),
                    StartDate.ToString("d MMMMM, HH:mm"), ChatMessages.Count);
                if (ChatMessages.Any(x => !x.IsRead))
                {
                    name += " <b>(" + ChatMessages.Count(x => !x.IsRead) + " непрочитанных)</b>";
                }
                return name;
            }
        }
    }
}