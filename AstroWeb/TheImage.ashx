<%@ webhandler language="C#" class="TheImage" %>
using System; 
using System.Web; 

public class TheImage : IHttpHandler 
{ 
    public void ProcessRequest(HttpContext context)
    {
        using(Image image = GetImage(context.Request.QueryString["ID"]))
        {    
            context.Response.ContentType = "image/jpeg";
            image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
        }
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }
} 