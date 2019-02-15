using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Rest;

namespace CognitiveServiceAPI_TextAnalytics.Controllers
{
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private const string SubscriptionKey = "0a3dfcab84184000a8d1a57238068a6c"; //Insert your Text Anaytics subscription key
        class ApiKeyServiceClientCredentials : ServiceClientCredentials
        {
            
            public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);
                return base.ProcessHttpRequestAsync(request, cancellationToken);
            }
        }
        ITextAnalyticsClient client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials())
        {
            Endpoint = "https://eastus.api.cognitive.microsoft.com"
        };
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            string value = "Languages -- KeyPhrases -- Sentiments -- Entities";

            return Ok(value);
        }

        // GET api/values/5
        [HttpGet("Languages")]
        public ActionResult<string> GetLanguages()
        {
            List<Models.LanguageDetection> obj = new List<Models.LanguageDetection>();
            var result = client.DetectLanguageAsync(new BatchInput(
                     new List<Input>()
                         {
                          new Input("1", "This is a document written in English."),
                          new Input("2", "Este es un document escrito en Español."),
                          new Input("3", "这是一个用中文写的文件")
                     })).Result;

            // Printing language results.

            foreach (var document in result.Documents)
            {
                Models.LanguageDetection temp = new Models.LanguageDetection();
                temp.id = document.Id;
                temp.Language = document.DetectedLanguages[0].Name;
                obj.Add(temp);
            }

            return Ok(obj);
        }

        [HttpGet("Sentiments")]
        public ActionResult<string> GetSentiments()
        {
            List<Models.Sentiments> obj = new List<Models.Sentiments>();
            SentimentBatchResult result3 = client.SentimentAsync(
                   new MultiLanguageBatchInput(
                       new List<MultiLanguageInput>()
                       {
                          new MultiLanguageInput("en", "0", "I had the best day of my life."),
                          new MultiLanguageInput("en", "1", "This was a waste of my time. The speaker put me to sleep."),
                          new MultiLanguageInput("es", "2", "No tengo dinero ni nada que dar..."),
                          new MultiLanguageInput("it", "3", "L'hotel veneziano era meraviglioso. È un bellissimo pezzo di architettura."),
                       })).Result;


            // Printing sentiment results
            foreach (var document in result3.Documents)
            {
                Models.Sentiments temp = new Models.Sentiments();
                temp.id = document.Id;
                temp.sentiment = document.Score.ToString();
                obj.Add(temp);
            }
            return Ok(obj);
        }
        [HttpGet("KeyPhrases")]
        public ActionResult<string> GetKeyPhrases(int id)
        {
            List<Models.KeyPhrase> obj = new List<Models.KeyPhrase>();
            KeyPhraseBatchResult result2 = client.KeyPhrasesAsync(new MultiLanguageBatchInput(
                        new List<MultiLanguageInput>()
                        {
                          new MultiLanguageInput("ja", "1", "猫は幸せ"),
                          new MultiLanguageInput("de", "2", "Fahrt nach Stuttgart und dann zum Hotel zu Fu."),
                          new MultiLanguageInput("en", "3", "My cat is stiff as a rock."),
                          new MultiLanguageInput("es", "4", "A mi me encanta el fútbol!")
                        })).Result;

            // Printing keyphrases
            foreach (var document in result2.Documents)
            {
                Models.KeyPhrase temp = new Models.KeyPhrase();
                //List<string> keys = new List<string>();
                temp.id = document.Id; 

                foreach (string keyphrase in document.KeyPhrases)
                {
                    temp.KeyPhrases.Add(keyphrase);
                }
                obj.Add(temp);
            }
            return Ok(obj);
        }
        [HttpGet("Entities")]
        public ActionResult<string> GetEntities(int id)
        {
            List<Models.Entities> obj = new List<Models.Entities>();
            EntitiesBatchResultV2dot1 result4 = client.EntitiesAsync(
                   new MultiLanguageBatchInput(
                       new List<MultiLanguageInput>()
                       {
                          new MultiLanguageInput("en", "0", "The Great Depression began in 1929. By 1933, the GDP in America fell by 25%.")
                       })).Result;

            // Printing entities results
            foreach (var document in result4.Documents)
            {
                Models.Entities temp = new Models.Entities();
                temp.id = document.Id;

                foreach (EntityRecordV2dot1 entity in document.Entities)
                {
                    temp.entity.Add(entity);
                }
                obj.Add(temp);
            }

            return Ok(obj);
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
