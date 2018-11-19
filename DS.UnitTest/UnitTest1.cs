using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DS.Bll.Models;
using DS.Data.Pocos;
using Xunit;

namespace DS.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var model = new Ca { Id = 1003 };
            var property = model.GetType().GetProperty(nameof(model.RequestFor));
            StringLengthAttribute strLenAttr = typeof(Ca).GetProperty("ObjectiveDesc").GetCustomAttributes(typeof(StringLengthAttribute), false).Cast<StringLengthAttribute>().SingleOrDefault();
            var id = model.GetType().GetProperty("Id").GetValue(model, null);
            if (strLenAttr != null)
            {
                int maxLen = strLenAttr.MaximumLength;
            }
        }
    }
}
