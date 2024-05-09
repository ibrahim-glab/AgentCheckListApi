// add Service Result Class

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace AgentCheckListApi.Helper
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
    }
}