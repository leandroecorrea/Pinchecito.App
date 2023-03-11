using Pinchecito.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pinchecito.UI.ViewModel
{
    public class MainViewModel
    {
        private ILoginService _loginService;

        public MainViewModel(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<bool> VerifyLoginStatus()
        {            
            var result = await _loginService.GetUser();
            return result.IsSuccess;
        }
    }
}
