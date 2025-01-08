using System.Net;
using LoginDemoApp;
using Microsoft.AspNetCore.Diagnostics;
var builder = WebApplication.CreateBuilder(args);


// 配置Kestrel服务器监听端口
builder.WebHost.ConfigureKestrel((context, options) =>
{
      options.AddServerHeader = false;
  //  options.Listen(IPAddress.Any, 8080); // 将监听端口设置为8080，可按需替换端口号
   options.Limits.MaxRequestHeadersTotalSize = 32768;
    options.Limits.MaxRequestBodySize = 1024 * 1024;
});


builder.Services.AddSession(options =>
{
    // 配置Session选项，如设置过期时间等
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 注册服务类
builder.Services.AddScoped<MyController>();
// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseSession();

app.Use((context, next) =>
{
    context.Response.Headers.Add("Author","wzlv");
    return next();
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

//app.UseExceptionHandler("/Error");
app.MapGet("/error", (HttpContext context) =>
{
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
    var exception = exceptionHandlerPathFeature.Error;
    return $"发生了一个错误：{exception.Message}";
});


app.MapGet("/api", MyController.HandleGetRequest);



// 配置MapGet端点，调用服务类中的方法
app.MapGet("/message", (MyController myService) =>
{
    return myService.GetMessage();
});



app.MapGet("/{id}", (int id) => {return $"Hello World!{id}";});

app.MapGet("/", (HttpContext context) =>
{

    var user= context.Session.GetString("UserName");

    string id = context.Request.Query["id"].FirstOrDefault();
    if (id!= null)
    {
        // 根据id进行相关处理，这里假设只是简单返回一个包含id的消息
        return $"你传入的id是{id}{user}";
    }
    else
    {
        return "未传入id参数";
    }
});

app.MapGet("/get", async (HttpContext context) =>
{
    // 执行异步操作
   
  
    // 返回结果
       return TypedResults.BadRequest(new { message = "请求成功", status = 200 });
    return Results.Ok();
});


app.MapPost("/register",   (HttpContext context) =>
{


 

// 将用户名保存到Session中
        context.Session.SetString("UserName", "guest");


 string contentType = context.Request.ContentType;
    if (contentType == null)
    {
      
        return  "请求缺少Content-Type头";
    }


    var formData = new List<string>();
    foreach (var key in context.Request.Form.Keys)
    {
        var values = context.Request.Form[key];
        foreach (var value in values)
        {
            formData.Add($"{key}: {value}");
        }
    }
    return $"收到的表单数据：{string.Join(", ", formData)}";
});





app.Run();
