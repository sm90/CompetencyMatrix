using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace CompetencyMatrix.Infrastructure
{
    public class ViewRender
    {
        private IRazorViewEngine _viewEngine;
        private ITempDataProvider _tempDataProvider;
        private IServiceProvider _serviceProvider;

        public ViewRender(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public string Render<TModel>(string name, TModel model, ViewDataDictionary viewData)
        {
            var actionContext = GetActionContext();

            var viewEngineResult = _viewEngine.FindView(actionContext, name, false);

            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException(string.Format("Couldn't find view '{0}'", name));
            }

            var view = viewEngineResult.View;

            viewData["IsJsonResult"] = true;

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary<TModel>(viewData, model)
                        
                        //metadataProvider: new EmptyModelMetadataProvider(),
                        //modelState: new ModelStateDictionary()
                        //)
                    {
                        //Model = model
                    }
                    ,
                    new TempDataDictionary(
                        actionContext.HttpContext,
                        _tempDataProvider),
                    output,
                    new HtmlHelperOptions());

                view.RenderAsync(viewContext).GetAwaiter().GetResult();

                return output.ToString();
            }
        }

        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.RequestServices = _serviceProvider;
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }


    //public class ViewRenderService
    //{
    //    IRazorViewEngine _viewEngine;
    //    IHttpContextAccessor _httpContextAccessor;

    //    public ViewRenderService(IRazorViewEngine viewEngine, IHttpContextAccessor httpContextAccessor)
    //    {
    //        _viewEngine = viewEngine;
    //        _httpContextAccessor = httpContextAccessor;
    //    }

    //    //public string Render(string viewPath)
    //    //{
    //    //    return Render(viewPath, string.Empty);
    //    //}

    //    public string Render<TModel>(string viewPath, TModel model, ViewDataDictionary viewData)
    //    {
    //        var viewEngineResult = _viewEngine.GetView("~/", viewPath, false);

    //        viewData.Model = model;

    //        if (!viewEngineResult.Success)
    //        {
    //            throw new InvalidOperationException($"Couldn't find view {viewPath}");
    //        }

    //        var view = viewEngineResult.View;

    //        using (var output = new StringWriter())
    //        {
    //            var viewContext = new ViewContext();
    //            viewContext.HttpContext = _httpContextAccessor.HttpContext;
    //            viewContext.ViewData = viewData;
    //            //new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary()){ Model = model };
    //            viewContext.Writer = output;

    //            view.RenderAsync(viewContext).GetAwaiter().GetResult();

    //            return output.ToString();
    //        }
    //    }
    //}

    //public class RazorViewToStringRenderer
    //{
    //    private IRazorViewEngine _viewEngine;
    //    private ITempDataProvider _tempDataProvider;
    //    private IServiceProvider _serviceProvider;

    //    public RazorViewToStringRenderer(
    //        IRazorViewEngine viewEngine,
    //        ITempDataProvider tempDataProvider,
    //        IServiceProvider serviceProvider)
    //    {
    //        _viewEngine = viewEngine;
    //        _tempDataProvider = tempDataProvider;
    //        _serviceProvider = serviceProvider;
    //    }

    //    public async Task<string> RenderViewToStringAsync<TModel>(string name, TModel model)
    //    {
    //        var actionContext = GetActionContext();

    //        var viewEngineResult = _viewEngine.FindView(actionContext, name, false);

    //        if (!viewEngineResult.Success)
    //        {
    //            throw new InvalidOperationException(string.Format("Couldn't find view '{0}'", name));
    //        }

    //        var view = viewEngineResult.View;

    //        using (var output = new StringWriter())
    //        {
    //            var viewContext = new ViewContext(
    //                actionContext,
    //                view,
    //                new ViewDataDictionary<TModel>(
    //                    metadataProvider: new EmptyModelMetadataProvider(),
    //                    modelState: new ModelStateDictionary())
    //                {
    //                    Model = model
    //                },
    //                new TempDataDictionary(
    //                    actionContext.HttpContext,
    //                    _tempDataProvider),
    //                output,
    //                new HtmlHelperOptions());

    //            await view.RenderAsync(viewContext);

    //            return output.ToString();
    //        }
    //    }

    //    private ActionContext GetActionContext()
    //    {
    //        var httpContext = new DefaultHttpContext();
    //        httpContext.RequestServices = _serviceProvider;
    //        return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    //    }
    //}
}