using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontEnd.Extensions;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TeamFinderDTO;

namespace FrontEnd.Pages
{
    public class MySessionsModel : IndexModel
    {

        public MySessionsModel(IApiClient apiClient) : base(apiClient)
        {
        }


        protected override  Task<List<SessionResponse>> GetSessionsAsync()
        {
         
            return _apiClient.GetSessionsByPlayerAsync(User.Identity.GetPlayerId());          
        }
    }
}
