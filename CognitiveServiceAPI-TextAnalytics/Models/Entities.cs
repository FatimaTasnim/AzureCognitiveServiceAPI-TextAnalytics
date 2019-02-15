using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CognitiveServiceAPI_TextAnalytics.Models
{
    public class Entities
    {
        public string id;
        public List<EntityRecordV2dot1> entity= new List<EntityRecordV2dot1>();
    }
}
