using BeanlancerAPI2.DTOs.Request;
using BeanlancerAPI2.Models;
using BeanlancerAPI2.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeanlancerAPI.Services
{
    public class RequestService : BaseService<Request,string>, IRequestService
    {

        public RequestService(BeanlancersContext context) : base(context)
        {
        }

        public Request CreateRequest(Request _entity)
        {
            Random rd = new Random();
            int random = 0;
            bool flag = false;
            string id = "";
            int budget = 0;
             do
            {
                random = rd.Next(10000, 100000);
                id = "REQ-" + random.ToString();
                if(GetRequestByID(id) == null)
                {
                    _entity.IdRequest = id;
                    flag = true;
                }
            } while (flag == false);
            if(_entity.BeanAmount >= 10 && _entity.BeanAmount < 30) budget = 1;
            if (_entity.BeanAmount >= 30 && _entity.BeanAmount < 250) budget = 2;
            if (_entity.BeanAmount >= 250 && _entity.BeanAmount < 750) budget = 3;
            if (_entity.BeanAmount >= 750 && _entity.BeanAmount < 1000) budget = 4;
            if (_entity.BeanAmount >= 1000 && _entity.BeanAmount < 3000) budget = 5;
            if (_entity.BeanAmount >= 3000 && _entity.BeanAmount < 5000) budget = 6;
            if (_entity.BeanAmount >= 5000 && _entity.BeanAmount < 10000) budget = 7;
            if (_entity.BeanAmount >= 10000) budget = 8;

            _entity.IdBudget = budget;
            _entity.Status = "Waiting";

            return Create(_entity);
        }

        public IEnumerable<Request> DisableAllRequestByUsername(int _username)
        {
            var list = GetAllNonPaging(a => a.Username.Equals(_username));
            for(int i = 0; i < list.Count(); i++){
                var req = GetRequestByID(list.ElementAt(i).IdRequest);
                if(req == null) return null;
                req.Status = "Disabled";
                Update(req.IdRequest,req);
            }
            return GetAllNonPaging();
        }

        public Request DeleteRequest(string _id)
        {
            return DeleteById(_id);
        }



        public IEnumerable<Request> GetAllByNameAndStatusAndCate(int _numpage, int _pagesize, string name, string status, int cate)
        {
            if(string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(status) && cate > 0){
                return GetAll(_numpage, _pagesize, a => a.Status == status && a.IdCategories == cate);
            }
            if(string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(name) && cate > 0){
                return GetAll(_numpage,_pagesize, a => a.NameRequest.Contains(name) && a.IdCategories == cate && a.Status != "Disabled");
            }
            if(cate == 0 && !string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(name)){
                return GetAll(_numpage,_pagesize, a => a.NameRequest.Contains(name) && a.Status == status);
            }
            if(string.IsNullOrEmpty(name) && cate == 0 && !string.IsNullOrEmpty(status)){
                return GetAll(_numpage,_pagesize, a => a.Status == status);
            }
            if(string.IsNullOrEmpty(status) && cate == 0 && !string.IsNullOrEmpty(name)){
                return GetAll(_numpage,_pagesize, a => a.NameRequest.Contains(name) && a.Status != "Disabled");
            }
            if(string.IsNullOrEmpty(name) && string.IsNullOrEmpty(status) && cate > 0){
                return GetAll(_numpage,_pagesize, a => a.IdCategories == cate && a.Status != "Disabled");
            }
            if(string.IsNullOrEmpty(name) && string.IsNullOrEmpty(status) && cate == 0){
                return GetAll(_numpage,_pagesize);
            }
            return GetAll(_numpage,_pagesize,a => a.NameRequest.Contains(name) && a.IdCategories == cate && a.Status == status);
        }

        public IEnumerable<Request> GetAllByUsername(int _numpage, int _pagesize, int username)
        {
            return GetAll(_numpage, _pagesize, a => a.Username == username);
        }

        public IEnumerable<Request> GetAllRequest(int numpage, int perpage)
        {
            return GetAll(numpage, perpage, a => a.Status != "Disabled");
        }

        public Request GetRequestByID(string id)
        {
            return GetById(id);
        }

        public IEnumerable<Request> GetRequestByUsernameAndStatus(int username, string status)
        {
            if(string.IsNullOrEmpty(status)) return GetAllNonPaging(a => a.Username == username);
            return GetAllNonPaging(a => a.Username == username && a.Status.Equals(status));
        }

        public IEnumerable<Request> GetRequestNonPaging()
        {
            return GetAllNonPaging(a => a.Status != "Disabled");
        }

        public Request UpdateRequest(Request _entity, string _id)
        {

                var req = GetRequestByID(_id);
                if(req == null) return null;
                _entity.IdRequest = req.IdRequest;
                _entity.Username = req.Username;
                _entity.Status = req.Status;
               if (_entity.BeanAmount >= 10 && _entity.BeanAmount < 30) _entity.IdBudget = 1;
                if (_entity.BeanAmount >= 30 && _entity.BeanAmount < 250) _entity.IdBudget = 2;
                if (_entity.BeanAmount >= 250 && _entity.BeanAmount < 750) _entity.IdBudget = 3;
                if (_entity.BeanAmount >= 750 && _entity.BeanAmount < 1000) _entity.IdBudget = 4;
                if (_entity.BeanAmount >= 1000 && _entity.BeanAmount < 3000) _entity.IdBudget = 5;
                if (_entity.BeanAmount >= 3000 && _entity.BeanAmount < 5000) _entity.IdBudget = 6;
                if (_entity.BeanAmount >= 5000 && _entity.BeanAmount < 10000) _entity.IdBudget = 7;
                if (_entity.BeanAmount >= 10000) _entity.IdBudget = 8;
            return Update(_id,_entity);
        }

        public IEnumerable<Request> GetByNameAndStatusAndCateNonPaging(string name, string status, int cate)
        {

            if (string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(status) && cate > 0)
            {
                return GetAllNonPaging(a => a.Status == status && a.IdCategories == cate);
            }
            if (string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(name) && cate > 0)
            {
                return GetAllNonPaging( a => a.NameRequest.Contains(name) && a.IdCategories == cate && a.Status != "Disabled");
            }
            if (cate == 0 && !string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(name))
            {
                return GetAllNonPaging( a => a.NameRequest.Contains(name) && a.Status == status);
            }
            if (string.IsNullOrEmpty(name) && cate == 0 && !string.IsNullOrEmpty(status))
            {
                return GetAllNonPaging( a => a.Status == status);
            }
            if (string.IsNullOrEmpty(status) && cate == 0 && !string.IsNullOrEmpty(name))
            {
                return GetAllNonPaging(a => a.NameRequest.Contains(name) && a.Status != "Disabled");
            }
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(status) && cate > 0)
            {
                return GetAllNonPaging( a => a.IdCategories == cate && a.Status != "Disabled");
            }
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(status) && cate == 0)
            {
                return GetAllNonPaging();
            }
            return GetAllNonPaging(a => a.NameRequest.Contains(name) && a.IdCategories == cate && a.Status == status);

        }

        
    }
}
