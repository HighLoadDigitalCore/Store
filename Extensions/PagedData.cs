using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Smoking.Extensions.Helpers;
using Smoking.Models;

namespace Smoking.Extensions
{


    public class PagedData<T> : List<T>
    {
        public PagedData(IQueryable<T> source, int pageIndex, int pageSize)
            : this(source, pageIndex, pageSize, "MasterListPaged", new RouteValueDictionary())
        {
        }
        public PagedData(IQueryable<T> source, int pageIndex, int pageSize, RouteValueDictionary dictionary, int totalCount)
            : this(source, pageIndex, pageSize, "MasterListPaged", dictionary, totalCount)
        {
        }
        public PagedData(IQueryable<T> source, int pageIndex, int pageSize, RouteValueDictionary dictionary)
            : this(source, pageIndex, pageSize, "MasterListPaged", dictionary)
        {
        }
        public PagedData(IQueryable<T> source, int pageIndex, int pageSize, string mapRuleName)
            : this(source, pageIndex, pageSize, mapRuleName, new RouteValueDictionary())
        {
        }

        public PagedData(IQueryable<T> source, int pageIndex, int pageSize, string mapRuleName,
            IEnumerable<FilterConfiguration> filters): this(source, pageIndex, pageSize, mapRuleName, filters, false)
        {
            
        }
        public PagedData(IQueryable<T> source, int pageIndex, int pageSize, string mapRuleName, IEnumerable<FilterConfiguration> filters, bool IsPartial)
        {
            MapRuleName = mapRuleName;
            PageIndex = pageIndex;
            PageSize = pageSize;
            this.IsPartial = IsPartial;
            var dictionary = new RouteValueDictionary();
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    var val = filter.ValueFromQuery == null ? "" : filter.ValueFromQuery.ToString();
                    if (val.IsFilled())
                        dictionary.Add(filter.QueryKey, filter.ValueFromQuery);
                }
            }
            DefaultRoutes = dictionary;
            Filters = filters;
            var filtered = AddRangeFiltered(source);
            TotalCount = filtered.Count();
            TotalPages = (int)Math.Ceiling((double)(((double)TotalCount) / ((double)PageSize)));

        }

        public PagedData(IQueryable<T> source, int pageIndex, int pageSize, string mapRuleName, RouteValueDictionary dictionary, int totalCount)
        {
            DefaultRoutes = dictionary;
            MapRuleName = mapRuleName;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling((double)(((double)TotalCount) / ((double)PageSize)));
            AddRangeFiltered(source);

        }

        private IQueryable<T> AddRangeFiltered(IQueryable<T> source)
        {
            var target = source;
            if (Filters != null && Filters.Any())
            {
                target = Filters.Where(x => !x.SkipInQuery).Aggregate(target, (current, filter) => filter.ApplyToQuery(current));
            }
            var data = target.Skip((PageIndex*PageSize)).Take(PageSize);
            if (!data.Any())
            {
                PageIndex = 0;
                data = target.Skip((PageIndex*PageSize)).Take(PageSize);
            }
            AddRange(data.ToList());
            return target;
        }



        public PagedData(IQueryable<T> source, int pageIndex, int pageSize, string mapRuleName, RouteValueDictionary dictionary)
        {
            DefaultRoutes = dictionary;
            MapRuleName = mapRuleName;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling((double)(((double)TotalCount) / ((double)PageSize)));
            AddRangeFiltered(source);
        }

        public PagedData(IQueryable<T> source, int pageIndex, int pageSize, string mapRuleName, HttpRequestBase request, IEnumerable<string> queryFilter)
            : this(source, pageIndex, pageSize, mapRuleName)
        {
            var dictionary = new RouteValueDictionary();
            NameValueCollection query = request.QueryString;
            foreach (string key in query.Keys.Cast<string>().Where(queryFilter.Contains))
            {
                dictionary.Add(key, query[key]);
            }

            DefaultRoutes = dictionary;
        }

        public MvcHtmlString PagerMenu(HtmlHelper hhelper, RouteValueDictionary add = null)
        {
            if (AccessHelper.IsMasterPage)
            {
                var content = "";

                content =
                    string.Concat(new object[]
                        {content, "<input type=\"hidden\" value=\"", PageIndex, "\" id=\"page\"/>"});
                if (TotalPages > 1)
                {

                    content = content + "<div class=\"pager\">Страницы:&nbsp;";


                    if (add != null)
                    {
                        foreach (var pair in add)
                        {
                            if (DefaultRoutes.ContainsKey(pair.Key))
                                DefaultRoutes[pair.Key] = pair.Value;
                            else DefaultRoutes.Add(pair.Key, pair.Value);
                        }
                    }

                    if (!DefaultRoutes.ContainsKey("page"))
                        DefaultRoutes.Add("page", 0);



                    if (HasPreviousPage)
                    {
                        DefaultRoutes["page"] = PageIndex - 1;
                        content = AddLink("<<", PageIndex - 1, content, hhelper);


                        /*
                                                (hhelper.RouteLink("<<", MapRuleName, DefaultRoutes,
                                                                                   (AjaxPager
                                                                                        ? ((object)
                                                                                           new
                                                                                               {
                                                                                                   onclick =
                                                                                               "return {0}({1})".FormatWith(PageJSFunction,
                                                                                                                            (PageIndex - 1).
                                                                                                                                ToString())
                                                                                               })
                                                                                        : ((object)new { })).ToDictionary())) + "&nbsp;";
                        */
                    }
                    string links = "";
                    bool begin = false;
                    bool end = false;
                    string fmtInactive = "<span>{0}</span>&nbsp;&nbsp;&nbsp;";
                    /*
                                    List<int> allowed = new List<int>();
                                    if (TotalPages < 15)
                                        for (int i = 0; i < 15; i++)
                                            allowed.Add(i);
                                    else
                                    {
                    */
                    int PageWidth = 5;
                    for (int i = 0; TotalPages > i; i++)
                    {
                        int offset = 0;
                        if (PageIndex < (PageWidth - 1))
                            offset = PageWidth - PageIndex - 1;

                        if ((i > (PageIndex - PageWidth)) && (i < (PageIndex + PageWidth + offset)))
                        {
                            if (PageIndex == i)
                            {
                                links = links + string.Format(fmtInactive, i + 1);
                            }
                            else
                            {

                                string linkText = (i + 1).ToString();
                                if ((i - 1) == (PageIndex - PageWidth))
                                {
                                    if (i > 0)
                                    {
                                        begin = true;
                                        linkText = "...";
                                    }
                                }

                                if ((i + 1) == (PageIndex + PageWidth + offset))
                                {
                                    if ((i + 1) != TotalPages)
                                    {
                                        end = true;
                                        linkText = "...";
                                    }
                                }

                                DefaultRoutes["page"] = i;
                                links = AddLink(linkText, i, links, hhelper);
                            }
                        }
                    }

                    if (begin)
                    {
                        links = AddLink("1", 0, links, hhelper, false);
                    }
                    if (end)
                    {
                        links = AddLink(TotalPages.ToString(), TotalPages - 1, links, hhelper);
                    }
                    content = content + links;
                    if (HasNextPage)
                    {
                        content = AddLink(">>", PageIndex + 1, content, hhelper);
                    }
                    content = content + "</div>";
                }
                return new MvcHtmlString(content);
            }
            else
            {
                string prevSpan = "<span><i class=\"icon_c_mark-lb\"></i> Назад</span>";
                string nextSpan = "<span>Вперед <i class=\"icon_c_mark-rb\"></i></span>";
                string prevLink = "<a href=\"{0}\"><i class=\"icon_c_mark-lb\"></i> Назад</a>";
                string nextLink = "<a href=\"{0}\"> Вперед <i class=\"icon_c_mark-rb\"></i></a>";
                string indexSpan = "<span class=\"selected\">{0}</span>";
                string indexLink = "<a href=\"{0}\">{1}</a>";
                string ctx = "<div class=\"el_paginate\"><div class=\"num_line bradius_20 gradient_white-gray\">{0}</div><div class=\"signature\">Страница: {1} из {2}</div></div>";

                string sLinks = "";
                if (TotalPages > 1)
                {
                    if (!DefaultRoutes.ContainsKey("page"))
                        DefaultRoutes.Add("page", 0);




                    bool begin = false;
                    bool end = false;
                    int PageWidth = 5;
                    for (int i = 0; TotalPages > i; i++)
                    {
                        int offset = 0;
                        if (PageIndex < (PageWidth - 1))
                            offset = PageWidth - PageIndex - 1;

                        if ((i > (PageIndex - PageWidth)) && (i < (PageIndex + PageWidth + offset)))
                        {
                            if (PageIndex == i)
                            {
                                sLinks += string.Format(indexSpan, i + 1);
                            }
                            else
                            {

                                string linkText = (i + 1).ToString();
                                if ((i - 1) == (PageIndex - PageWidth))
                                {
                                    if (i > 0)
                                    {
                                        begin = true;
                                        linkText = "...";
                                    }
                                }

                                if ((i + 1) == (PageIndex + PageWidth + offset))
                                {
                                    if ((i + 1) != TotalPages)
                                    {
                                        end = true;
                                        linkText = "...";
                                    }
                                }

                                DefaultRoutes["page"] = i;
                                sLinks += indexLink.FormatWith(CreateLink(i), linkText);
                            }
                        }
                    }

                    if (begin)
                    {
                        sLinks = indexLink.FormatWith(CreateLink(0), "1") + sLinks;
                    }
                    if (end)
                    {
                        sLinks += indexLink.FormatWith(CreateLink(TotalPages-1), TotalPages.ToString());
                    }

                    if (HasPreviousPage)
                    {
                        DefaultRoutes["page"] = PageIndex - 1;
                        sLinks = prevLink.FormatWith(CreateLink(PageIndex - 1)) + sLinks;
                    }
                    else
                    {
                        sLinks = prevSpan + sLinks;
                    }

                    if (HasNextPage)
                    {
                        DefaultRoutes["page"] = PageIndex + 1;
                        sLinks += nextLink.FormatWith(CreateLink(PageIndex + 1));
                    }
                    else
                    {
                        sLinks += nextSpan;
                    }
                    return new MvcHtmlString(ctx.FormatWith(sLinks, PageIndex + 1, TotalPages));   
                }
                return new MvcHtmlString("");
                

            }
        }

        private string CreateLink(int arg)
        {
            var myRoutes = DefaultRoutes.Where(
                x => !x.Key.StartsWith("url") && !x.Key.ToLower().StartsWith("page")).ToList();
            myRoutes.Add(new KeyValuePair<string, object>("page", arg));
            var routes = myRoutes.
                Select(x => string.Format("{0}={1}", x.Key, x.Value))
                                 .ToList();

            string paramList = string.Join("&", routes);

            var text = string.Format("{0}?{1}",
                                   AccessHelper.BaseURL, paramList);
            return text;

        }
        private string AddLink(string linkText, int arg, string links, HtmlHelper hhelper, bool toEnd = true)
        {
            var myRoutes = DefaultRoutes.Where(
                x => !x.Key.StartsWith("url") && !x.Key.ToLower().StartsWith("page")).ToList();

            myRoutes.Add(new KeyValuePair<string, object>("page", arg));

            if (!AccessHelper.IsMasterPage)
            {
                var info = AccessHelper.CurrentPageInfo;
                if (info != null)
                {

                    var routes = myRoutes.
                        Select(x => string.Format("{0}={1}", x.Key, x.Value))
                                         .ToList();

                    string paramList = string.Join("&", routes);

                    var text = string.Format("<a href=\"{0}?{1}\">{2}</a>",
                                           AccessHelper.BaseURL, paramList, linkText);
                    if (toEnd)
                        links = links + text + "&nbsp;&nbsp;&nbsp;";
                    else links = text + "&nbsp;&nbsp;&nbsp;" + links;
                    return links;
                }
            }
            else
            {
                var rd = new RouteValueDictionary();
                foreach (var pair in myRoutes)
                {
                    if(!rd.ContainsKey(pair.Key))
                        rd.Add(pair.Key, pair.Value);
                }
                
                var text = hhelper.RouteLink(linkText,
                                             MapRuleName, rd
                                             ,
                                             (AjaxPager
                                                  ? ((object)
                                                     new
                                                         {
                                                             onclick =
                                                         "return {0}({1})".FormatWith(new string[]
                                                             {
                                                                 PageJSFunction
                                                                 , arg.ToString()
                                                             })
                                                         })
                                                         : (IsPartial ? ((object)new { onclick = "loadByLink(this); return false;" }) : ((object)new { })) /*((object) new {})*/).ToDictionary());

                if (toEnd)
                    links = links + text + "&nbsp;&nbsp;&nbsp;";
                else links = text + "&nbsp;&nbsp;&nbsp;" + links;

                return links;


            }
            return links;
        }

        public bool AjaxPager { get; set; }

        public RouteValueDictionary DefaultRoutes { get; set; }

        public bool HasNextPage
        {
            get
            {
                return ((PageIndex + 1) < TotalPages);
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        public string MapRuleName { get; set; }

        public bool IsPartial { get; set; }
        public int PageIndex { get; set; }

        public string PageJSFunction { get; set; }


        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        IEnumerable<FilterConfiguration> Filters { get; set; }
    }
}

