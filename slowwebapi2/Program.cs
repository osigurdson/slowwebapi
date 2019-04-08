using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Owin;

namespace slowwebapi2
{
   class MainClass
   {
      public static void Main(string[] args)
      {
         var baseAddress = args.Length == 0 ? "http://+:8000/" : args[0];

         using (WebApp.Start<Startup>(url: baseAddress))
         {
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:8000") };

            var response1 = client.GetAsync("api/values").Result;
            response1.EnsureSuccessStatusCode();
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {

               var response = client.GetAsync("api/values").Result;
               response.EnsureSuccessStatusCode();
            }

            Console.WriteLine(sw.Elapsed);


            Console.ReadLine();
         }
      }
   }

   class Startup
   {
      public void Configuration(IAppBuilder app)
      {
         // Configure Web API for self-host. 
         HttpConfiguration config = new HttpConfiguration();
         config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
         );

         app.UseWebApi(config);
      }
   }

   public class ValuesController : ApiController
   {
      // GET api/values 
      public IEnumerable<string> Get()
      {
         return new string[] { "value1", "value2" };
      }

      // GET api/values/5 
      public string Get(int id)
      {
         return "value";
      }

      // POST api/values 
      public void Post([FromBody]string value)
      {
      }

      // PUT api/values/5 
      public void Put(int id, [FromBody]string value)
      {
      }

      // DELETE api/values/5 
      public void Delete(int id)
      {
      }
   }
}
