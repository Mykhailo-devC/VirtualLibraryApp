using Microsoft.Extensions.Primitives;
using System.Text;

namespace VirtualLibrary.Middleware
{
    public class IdentifierValidatorMiddleware
    {
        private RequestDelegate nextComponent;
        public IdentifierValidatorMiddleware(RequestDelegate nextComponent)
        {
            this.nextComponent = nextComponent;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Query.TryGetValue("id", out StringValues strId))
            {
                var id = strId.ToString();

                if (!int.TryParse(id, out _))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsJsonAsync(new Response
                    {
                        Message = $"Incorrect Id [Value = {id}]",
                        Success = false
                    });
                }
            }
            else
            {
                await nextComponent.Invoke(context);
            }


        }
    }
}
