using System;
 

namespace LoginDemoApp;

public class MyController
{
    public static async Task<IResult> HandleGetRequest(HttpContext context)
    {

    context.Response.StatusCode = 200;
   context.Response.ContentType = "text/plain";
   await context.Response.WriteAsync("Hello, World!");

         return TypedResults.Ok(new { message = "请求成功", status = 200 });
       // return "Hello from MyController";
    }
     public string GetMessage()
    {
        return "Hello from the service method!";
    }
}