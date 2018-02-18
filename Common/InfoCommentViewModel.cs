﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using SitePlugin;
namespace Common
{
    public class InfoCommentViewModel : CommentViewModelBase
    {
        public override string UserId => "-";
        public InfoCommentViewModel(ConnectionName connectionName, IOptions options, string message)
            :base(connectionName, options)
        {
            IsInfo = true;
            MessageItems = new List<IMessagePart>
            {
                new MessageText(message),
            };
        }
    }
}