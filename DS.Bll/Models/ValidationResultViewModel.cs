using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Bll.Models
{
    public class ValidationResultViewModel
    {
        public ValidationResultViewModel()
        {
            ErrorFlag = false;
            Message = "Success";
            ModelStateErrorList = new List<ModelStateError>();
        }
        public bool ErrorFlag { get; set; }
        public string Message { get; set; }
        public List<ModelStateError> ModelStateErrorList { get; set; }
    }
}
