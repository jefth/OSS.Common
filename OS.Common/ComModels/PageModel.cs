﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using OS.Common.ComModels.Enums;
using OS.Common.Extention;

namespace OS.Common.ComModels
{

    /// <summary>
    ///  
    /// </summary>
    public static class SearchModelExtention
    {
        /// <summary>
        /// 添加过滤条件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="columns"></param>
        public static void AddFilterColumn(this SearchModel model, NameValueCollection columns)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                if (model.FilterDics==null)
                {
                    model.FilterDics=new Dictionary<string, string>(columns.Count);
                }
                model.FilterDics.Add(columns.GetKey(i),columns[i]);
            }
        }

        public static SearchModel ToSearchModel(this NameValueCollection collections)
        {
            var model = new SearchModel();
            model.CurrentPage = collections["CurrentPage"].ToInt32();
            model.PageSize = collections["PageSize"].ToInt32();
            return model;
        }
    }

    /// <summary>
    /// 搜索实体
    /// </summary>
    public class SearchModel
    {
        public SearchModel()
        {
            FilterDics = new Dictionary<string, string>();
            OrderDics=new Dictionary<string, SortType>();
        }

        private int _curntPage = 1;

        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage
        {
            get
            {
                if (_curntPage<=0)
                {
                    return 1;
                }
                return _curntPage;
            }
            set { _curntPage = value; }
        }

        private int _pageSize = 20;

        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_pageSize <= 0)
                {
                    return 20;
                }
                return _pageSize;
            }
            set { _pageSize = value; }
        }
        
        /// <summary>
        ///    起始行 -只读属性
        /// </summary>
        public int StartRow
        {
            get { return (CurrentPage - 1)*PageSize; }
        }

        /// <summary>
        /// 排序集合      适用于多个查询条件
        /// </summary>
        public Dictionary<string, SortType> OrderDics { get; set; }

        /// <summary>
        /// 搜索关键字集合      适用于多个查询条件
        /// </summary>
        public Dictionary<string, string> FilterDics { get; set; }
    }



    /// <summary>
    ///  分页实体
    /// </summary>
    public class PageListModel<TModel> : ResultModel where TModel : class, new()
    {

        /// <summary>
        ///    空构造函数  照顾  json序列化 
        /// </summary>
        public PageListModel()
        {
        }


        /// <summary>
        ///   出错时  构造函数    
        /// </summary>
        /// <param name="ret"></param>
        /// <param name="message"></param>
        public PageListModel(ResultTypes ret, string message = "")
            : base(ret, message)
        {
        }

        /// <summary>
        ///   正常赋值时的实体
        /// </summary>
        /// <param name="list"></param>
        /// <param name="searchMo"></param>
        /// <param name="total"></param>
        public PageListModel(long total, List<TModel> list, SearchModel searchMo)
        {

            if (searchMo == null)
                throw new ArgumentNullException("searchMo", "searchmodel 不能为空");

            Search = searchMo;
            Data = list;
            Total = total;

            _pageSize = searchMo.PageSize;
        }

        /// <summary>
        ///   正常赋值时的实体
        /// </summary>
        /// <param name="list"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        public PageListModel(long total, int pageSize, List<TModel> list)
        {
            if (pageSize < 1)
            {
                throw new ArgumentNullException("pageSize", "pageSize 必须大于0");
            }
            Total = total;
            Data = list;
            _pageSize = pageSize;
        }

        /// <summary>
        /// 实体列表
        /// </summary>
        public List<TModel> Data { get; set; }

        /// <summary>
        /// 搜索对象
        /// </summary>
        public SearchModel Search { get; set; }


        /// <summary>
        /// 总数
        /// </summary>
        public long Total { get; set; }

        private int _pageSize;
        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_pageSize == 0)
                {
                        return 1;
                }
                return _pageSize;
            }
            set { _pageSize = value; }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get { return (int)Math.Ceiling((double)Total / PageSize); }
        }
    }


    ///// <summary>
    ///// 
    ///// </summary>
    ///// <typeparam name="TModel"></typeparam>
    //public class SimpListModel<TModel> 
    //{
    //    public SimpListModel(long total,List<TModel> list)
    //    {
    //        Data = list;
    //        Total = total;
    //    }

    //    /// <summary>
    //    /// 总数
    //    /// </summary>
    //    public long Total { get; set; }

    //    /// <summary>
    //    /// 实体列表
    //    /// </summary>
    //    public List<TModel> Data { get; private set; }

    //}


    /// <summary>
    /// 
    /// </summary>
    public static class PageListMap
    {
        /// <summary>
        ///   转化pageList列表实体
        /// </summary>
        /// <typeparam name="TPara"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="pageList"></param>
        /// <param name="convertFun"></param>
        /// <returns></returns>
        public static PageListModel<TResult> ConvertToPageList<TPara, TResult>(this PageListModel<TPara> pageList,
            Func<TPara, TResult> convertFun)
            where TResult : class, new()
            where TPara : class, new()
        {
            if (convertFun == null)
            {
                throw new ArgumentNullException("convertFun", "转化方法不能为空！");
            }

            List<TResult> resultList = null;
            if (pageList.Data != null)
            {
                resultList = pageList.Data.ConvertAll(e => convertFun(e));
            }
            return new PageListModel<TResult>(pageList.Total, resultList, pageList.Search);
        }
    }

}
