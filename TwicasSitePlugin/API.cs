﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;

namespace TwicasSitePlugin
{
    static class API
    {
        /// <summary>
        /// コメント一覧を取得する（非公開API）
        /// </summary>
        /// <param name="live_id"></param>
        /// <param name="lastCommentId"></param>
        /// <param name="f"></param>
        /// <param name="count"></param>
        public static async Task<LowObject.Comment[]> GetListAll(IDataServer dataSource, string broadcasterName, long live_id, long lastCommentId, int from, int count, CookieContainer cc)
        {
            var url = $"http://twitcasting.tv/{broadcasterName}/userajax.php?c=listall&m={live_id}&k={lastCommentId}&f={from}&n={count}";
            var str = await dataSource.GetAsync(url, cc);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<LowObject.Comment[]>(str);
            return obj;
        }
        public static async Task<LowObject.StreamChecker> GetUtreamChecker(IDataServer dataServer, string broadcasterId)
        {
            var url = $"https://twitcasting.tv/streamchecker.php?u={broadcasterId}&v=999";
            var str = await dataServer.GetAsync(url);
            var s = new LowObject.StreamChecker(str);
            return s;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="broadcaster"></param>
        /// <returns></returns>
        /// <exception cref="WebException"></exception>
        public static async Task<LowObject.LiveContext> GetLiveContext(IDataServer dataSource, string broadcaster)
        {
            var url = "http://twitcasting.tv/" + broadcaster;
            var str = await dataSource.GetAsync(url);
            var context = new LowObject.LiveContext();
            var match0 = Regex.Match(str, "var movie_cnum = (?<cnum>[\\d]+);");
            if (match0.Success)
            {
                context.MovieCnum = int.Parse(match0.Groups["cnum"].Value);
            }
            else
            {
                throw new InvalidBroadcasterIdException(broadcaster);
            }
            var match1 = Regex.Match(str, "var movieid = \"(\\d+)\"");
            if (match1.Success)
            {
                context.MovieId = long.Parse(match1.Groups[1].Value);
            }
            else
            {
                throw new InvalidBroadcasterIdException(broadcaster);
            }
            return context;
        }
        /// <summary>
        /// （非公開API）
        /// </summary>
        /// <param name="live_id"></param>
        /// <param name="n"></param>
        /// <param name="lastCommentId"></param>
        public static async Task<(List<LowObject.Comment> LowComments, int Cnum)> GetListUpdate(IDataServer dataSource, string broadcaster, long live_id, int cnum, long lastCommentId, CookieContainer cc)
        {
            var url = $"http://twitcasting.tv/{broadcaster}/userajax.php?c=listupdate&m={live_id}&n={cnum}&k={lastCommentId}";

            var str = await dataSource.GetAsync(url, cc);

            if (str == "[]")
            {
                return (new List<LowObject.Comment>(), cnum);
            }
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<LowObject.ListUpdate>(str);

            var comments = obj.comment;
            var newCnum = obj.cnum;
            return (comments, newCnum);
        }
    }
}