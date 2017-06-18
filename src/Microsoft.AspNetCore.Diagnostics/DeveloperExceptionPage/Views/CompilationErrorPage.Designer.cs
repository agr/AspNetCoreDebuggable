namespace Microsoft.AspNetCore.Diagnostics.RazorViews
{
#line 1 "CompilationErrorPage.cshtml"
using System

#line default
#line hidden
    ;
#line 2 "CompilationErrorPage.cshtml"
using System.Globalization

#line default
#line hidden
    ;
#line 3 "CompilationErrorPage.cshtml"
using System.Linq

#line default
#line hidden
    ;
#line 4 "CompilationErrorPage.cshtml"
using System.Net

#line default
#line hidden
    ;
#line 5 "CompilationErrorPage.cshtml"
using Microsoft.AspNetCore.Diagnostics

#line default
#line hidden
    ;
#line 6 "CompilationErrorPage.cshtml"
using Microsoft.AspNetCore.Diagnostics.RazorViews

#line default
#line hidden
    ;
    using System.Threading.Tasks;

    internal class CompilationErrorPage : Microsoft.Extensions.RazorViews.BaseView
    {
#line 8 "CompilationErrorPage.cshtml"

    public CompilationErrorPageModel Model { get; set; }

#line default
#line hidden
        #line hidden
        public CompilationErrorPage()
        {
        }

        #pragma warning disable 1998
        public override async Task ExecuteAsync()
        {
#line 11 "CompilationErrorPage.cshtml"
  
    Response.StatusCode = 500;
    Response.ContentType = "text/html; charset=utf-8";
    Response.ContentLength = null; // Clear any prior Content-Length

#line default
#line hidden

            WriteLiteral("<!DOCTYPE html>\r\n<html>\r\n    <head>\r\n        <meta charset=\"utf-8\" />\r\n        <title>");
#line 20 "CompilationErrorPage.cshtml"
          Write(Resources.ErrorPageHtml_Title);

#line default
#line hidden
            WriteLiteral(@"</title>
        <style>
            body {
    font-family: 'Segoe UI', Tahoma, Arial, Helvetica, sans-serif;
    font-size: .813em;
    color: #222;
    background-color: #fff;
}

h1, h2, h3, h4, h5 {
    /*font-family: 'Segoe UI',Tahoma,Arial,Helvetica,sans-serif;*/
    font-weight: 100;
}

h1 {
    color: #44525e;
    margin: 15px 0 15px 0;
}

h2 {
    margin: 10px 5px 0 0;
}

h3 {
    color: #363636;
    margin: 5px 5px 0 0;
}

code {
    font-family: Consolas, ""Courier New"", courier, monospace;
}

body .titleerror {
    padding: 3px 3px 6px 3px;
    display: block;
    font-size: 1.5em;
    font-weight: 100;
}

body .location {
    margin: 3px 0 10px 30px;
}

#header {
    font-size: 18px;
    padding: 15px 0;
    border-top: 1px #ddd solid;
    border-bottom: 1px #ddd solid;
    margin-bottom: 0;
}

    #header li {
        display: inline;
        margin: 5px;
        padding: 5px;
        color: #a0a0a0;
        cursor: pointer;
    }

    #h");
            WriteLiteral(@"eader .selected {
        background: #44c5f2;
        color: #fff;
    }

#stackpage ul {
    list-style: none;
    padding-left: 0;
    margin: 0;
    /*border-bottom: 1px #ddd solid;*/
}

#stackpage .details {
    font-size: 1.2em;
    padding: 3px;
    color: #000;
}

#stackpage .stackerror {
    padding: 5px;
    border-bottom: 1px #ddd solid;
}


#stackpage .frame {
    padding: 0;
    margin: 0 0 0 30px;
}

    #stackpage .frame h3 {
        padding: 2px;
        margin: 0;
    }

#stackpage .source {
    padding: 0 0 0 30px;
}

    #stackpage .source ol li {
        font-family: Consolas, ""Courier New"", courier, monospace;
        white-space: pre;
        background-color: #fbfbfb;
    }

#stackpage .frame .source .highlight li span {
    color: #FF0000;
}

#stackpage .source ol.collapsible li {
    color: #888;
}

    #stackpage .source ol.collapsible li span {
        color: #606060;
    }

.page table {
    border-collapse: separate;
    bo");
            WriteLiteral(@"rder-spacing: 0;
    margin: 0 0 20px;
}

.page th {
    vertical-align: bottom;
    padding: 10px 5px 5px 5px;
    font-weight: 400;
    color: #a0a0a0;
    text-align: left;
}

.page td {
    padding: 3px 10px;
}

.page th, .page td {
    border-right: 1px #ddd solid;
    border-bottom: 1px #ddd solid;
    border-left: 1px transparent solid;
    border-top: 1px transparent solid;
    box-sizing: border-box;
}

    .page th:last-child, .page td:last-child {
        border-right: 1px transparent solid;
    }

.page .length {
    text-align: right;
}

a {
    color: #1ba1e2;
    text-decoration: none;
}

    a:hover {
        color: #13709e;
        text-decoration: underline;
    }

.showRawException {
    cursor: pointer;
    color: #44c5f2;
    background-color: transparent;
    font-size: 1.2em;
    text-align: left;
    text-decoration: none;
    display: inline-block;
    border: 0;
    padding: 0;
}

.rawExceptionStackTrace {
    font-size: 1.2em;
");
            WriteLiteral(@"}

.rawExceptionBlock {
    border-top: 1px #ddd solid;
    border-bottom: 1px #ddd solid;
}

.showRawExceptionContainer {
    margin-top: 10px;
    margin-bottom: 10px;
}

.expandCollapseButton {
    cursor: pointer;
    float: left;
    height: 16px;
    width: 16px;
    font-size: 10px;
    position: absolute;
    left: 10px;
    background-color: #eee;
    padding: 0;
    border: 0;
    margin: 0;
}

        </style>
    </head>
    <body>
        <h1>");
#line 222 "CompilationErrorPage.cshtml"
       Write(Resources.ErrorPageHtml_CompilationException);

#line default
#line hidden
            WriteLiteral("</h1>\r\n");
#line 223 "CompilationErrorPage.cshtml"
        

#line default
#line hidden

#line 223 "CompilationErrorPage.cshtml"
          
            var exceptionDetailId = "";
        

#line default
#line hidden

            WriteLiteral("        ");
#line 226 "CompilationErrorPage.cshtml"
         for (var i = 0; i < Model.ErrorDetails.Count; i++)
        {
            var errorDetail = Model.ErrorDetails[i];
            exceptionDetailId = "exceptionDetail" + i;


#line default
#line hidden

            WriteLiteral("            <div id=\"stackpage\" class=\"page\">\r\n");
#line 232 "CompilationErrorPage.cshtml"
                

#line default
#line hidden

#line 232 "CompilationErrorPage.cshtml"
                  
                    var stackFrameCount = 0;
                    var frameId = "";
                 

#line default
#line hidden

            WriteLiteral("                ");
#line 236 "CompilationErrorPage.cshtml"
                  
                    var fileName = errorDetail.StackFrames.FirstOrDefault()?.File;
                    if (!string.IsNullOrEmpty(fileName))
                    {

#line default
#line hidden

            WriteLiteral("                        <div class=\"titleerror\">");
#line 240 "CompilationErrorPage.cshtml"
                                           Write(fileName);

#line default
#line hidden
            WriteLiteral("</div>\r\n");
#line 241 "CompilationErrorPage.cshtml"
                    }
                

#line default
#line hidden

            WriteLiteral("                ");
#line 243 "CompilationErrorPage.cshtml"
                 if (!string.IsNullOrEmpty(errorDetail.ErrorMessage))
                {

#line default
#line hidden

            WriteLiteral("                    <div class=\"details\">");
#line 245 "CompilationErrorPage.cshtml"
                                    Write(errorDetail.ErrorMessage);

#line default
#line hidden
            WriteLiteral("</div>\r\n");
#line 246 "CompilationErrorPage.cshtml"
                }

#line default
#line hidden

            WriteLiteral("                <br />\r\n                <ul>\r\n");
#line 249 "CompilationErrorPage.cshtml"
                

#line default
#line hidden

#line 249 "CompilationErrorPage.cshtml"
                 foreach (var frame in errorDetail.StackFrames)
                {
                    

#line default
#line hidden

#line 251 "CompilationErrorPage.cshtml"
                      
                        stackFrameCount++;
                        frameId = "frame" + stackFrameCount;
                    

#line default
#line hidden

#line 254 "CompilationErrorPage.cshtml"
                     

#line default
#line hidden

            WriteLiteral("                    <li class=\"frame\"");
            BeginWriteAttribute("id", " id=\"", 5361, "\"", 5374, 1);
#line 255 "CompilationErrorPage.cshtml"
WriteAttributeValue("", 5366, frameId, 5366, 8, false);

#line default
#line hidden
            EndWriteAttribute();
            WriteLiteral(">\r\n");
#line 256 "CompilationErrorPage.cshtml"
                        

#line default
#line hidden

#line 256 "CompilationErrorPage.cshtml"
                         if (!string.IsNullOrEmpty(frame.ErrorDetails))
                        {

#line default
#line hidden

            WriteLiteral("                            <h3>");
#line 258 "CompilationErrorPage.cshtml"
                           Write(frame.ErrorDetails);

#line default
#line hidden
            WriteLiteral("</h3>\r\n");
#line 259 "CompilationErrorPage.cshtml"
                        }

#line default
#line hidden

            WriteLiteral("\r\n");
#line 261 "CompilationErrorPage.cshtml"
                        

#line default
#line hidden

#line 261 "CompilationErrorPage.cshtml"
                         if (frame.Line != 0 && frame.ContextCode.Any())
                        {

#line default
#line hidden

            WriteLiteral("                            <button class=\"expandCollapseButton\" data-frameId=\"");
#line 263 "CompilationErrorPage.cshtml"
                                                                          Write(frameId);

#line default
#line hidden
            WriteLiteral("\">+</button>\r\n                            <div class=\"source\">\r\n");
#line 265 "CompilationErrorPage.cshtml"
                                

#line default
#line hidden

#line 265 "CompilationErrorPage.cshtml"
                                 if (frame.PreContextCode.Any())
                                {

#line default
#line hidden

            WriteLiteral("                                    <ol");
            BeginWriteAttribute("start", " start=\"", 5957, "\"", 5986, 1);
#line 267 "CompilationErrorPage.cshtml"
WriteAttributeValue("", 5965, frame.PreContextLine, 5965, 21, false);

#line default
#line hidden
            EndWriteAttribute();
            WriteLiteral(" class=\"collapsible\">\r\n");
#line 268 "CompilationErrorPage.cshtml"
                                        

#line default
#line hidden

#line 268 "CompilationErrorPage.cshtml"
                                         foreach (var line in frame.PreContextCode)
                                        {

#line default
#line hidden

            WriteLiteral("                                            <li><span>");
#line 270 "CompilationErrorPage.cshtml"
                                                 Write(line);

#line default
#line hidden
            WriteLiteral("</span></li>\r\n");
#line 271 "CompilationErrorPage.cshtml"
                                        }

#line default
#line hidden

            WriteLiteral("                                    </ol>\r\n");
#line 273 "CompilationErrorPage.cshtml"
                                }

#line default
#line hidden

            WriteLiteral("                                <ol");
            BeginWriteAttribute("start", " start=\"", 6367, "\"", 6386, 1);
#line 274 "CompilationErrorPage.cshtml"
WriteAttributeValue("", 6375, frame.Line, 6375, 11, false);

#line default
#line hidden
            EndWriteAttribute();
            WriteLiteral(" class=\"highlight\">\r\n");
#line 275 "CompilationErrorPage.cshtml"
                                    

#line default
#line hidden

#line 275 "CompilationErrorPage.cshtml"
                                     foreach (var line in frame.ContextCode)
                                    {

#line default
#line hidden

            WriteLiteral("                                        <li><span>");
#line 277 "CompilationErrorPage.cshtml"
                                             Write(line);

#line default
#line hidden
            WriteLiteral("</span></li>\r\n");
#line 278 "CompilationErrorPage.cshtml"
                                    }

#line default
#line hidden

            WriteLiteral("                                </ol>\r\n");
#line 280 "CompilationErrorPage.cshtml"
                                

#line default
#line hidden

#line 280 "CompilationErrorPage.cshtml"
                                 if (frame.PostContextCode.Any())
                                {

#line default
#line hidden

            WriteLiteral("                                    <ol");
            BeginWriteAttribute("start", " start=\'", 6813, "\'", 6838, 1);
#line 282 "CompilationErrorPage.cshtml"
WriteAttributeValue("", 6821, frame.Line + 1, 6821, 17, false);

#line default
#line hidden
            EndWriteAttribute();
            WriteLiteral(" class=\"collapsible\">\r\n");
#line 283 "CompilationErrorPage.cshtml"
                                        

#line default
#line hidden

#line 283 "CompilationErrorPage.cshtml"
                                         foreach (var line in frame.PostContextCode)
                                        {

#line default
#line hidden

            WriteLiteral("                                            <li><span>");
#line 285 "CompilationErrorPage.cshtml"
                                                 Write(line);

#line default
#line hidden
            WriteLiteral("</span></li>\r\n");
#line 286 "CompilationErrorPage.cshtml"
                                        }

#line default
#line hidden

            WriteLiteral("                                    </ol>\r\n");
#line 288 "CompilationErrorPage.cshtml"
                                }

#line default
#line hidden

            WriteLiteral("                            </div>\r\n");
#line 290 "CompilationErrorPage.cshtml"
                        }

#line default
#line hidden

            WriteLiteral("                    </li>\r\n");
#line 292 "CompilationErrorPage.cshtml"
                }

#line default
#line hidden

            WriteLiteral("                </ul>\r\n                <br />\r\n            </div>\r\n");
#line 296 "CompilationErrorPage.cshtml"
            

#line default
#line hidden

#line 296 "CompilationErrorPage.cshtml"
             if (!string.IsNullOrEmpty(Model.CompiledContent[i]))
            { 

#line default
#line hidden

            WriteLiteral("                <div class=\"rawExceptionBlock\">\r\n                    <div class=\"showRawExceptionContainer\">\r\n                        <button class=\"showRawException\" data-exceptionDetailId=\"");
#line 300 "CompilationErrorPage.cshtml"
                                                                            Write(exceptionDetailId);

#line default
#line hidden
            WriteLiteral("\">Show compilation source</button>\r\n                    </div>\r\n                    <div");
            BeginWriteAttribute("id", " id=\"", 7741, "\"", 7764, 1);
#line 302 "CompilationErrorPage.cshtml"
WriteAttributeValue("", 7746, exceptionDetailId, 7746, 18, false);

#line default
#line hidden
            EndWriteAttribute();
            WriteLiteral(" class=\"rawExceptionDetails\">\r\n                        <pre class=\"rawExceptionStackTrace\">");
#line 303 "CompilationErrorPage.cshtml"
                                                       Write(Model.CompiledContent[i]);

#line default
#line hidden
            WriteLiteral("</pre>\r\n                    </div>\r\n                </div>\r\n");
#line 306 "CompilationErrorPage.cshtml"
            }

#line default
#line hidden

#line 306 "CompilationErrorPage.cshtml"
             
        }

#line default
#line hidden

            WriteLiteral(@"
        <script>
            //<!--
            (function (window, undefined) {
    ""use strict"";

    function ns(selector, element) {
        return new NodeCollection(selector, element);
    }

    function NodeCollection(selector, element) {
        this.items = [];
        element = element || window.document;

        var nodeList;

        if (typeof (selector) === ""string"") {
            nodeList = element.querySelectorAll(selector);
            for (var i = 0, l = nodeList.length; i < l; i++) {
                this.items.push(nodeList.item(i));
            }
        }
    }

    NodeCollection.prototype = {
        each: function (callback) {
            for (var i = 0, l = this.items.length; i < l; i++) {
                callback(this.items[i], i);
            }
            return this;
        },

        children: function (selector) {
            var children = [];

            this.each(function (el) {
                children = children.concat(ns(selector, e");
            WriteLiteral(@"l).items);
            });

            return ns(children);
        },

        hide: function () {
            this.each(function (el) {
                el.style.display = ""none"";
            });

            return this;
        },

        toggle: function () {
            this.each(function (el) {
                el.style.display = el.style.display === ""none"" ? """" : ""none"";
            });

            return this;
        },

        show: function () {
            this.each(function (el) {
                el.style.display = """";
            });

            return this;
        },

        addClass: function (className) {
            this.each(function (el) {
                var existingClassName = el.className,
                    classNames;
                if (!existingClassName) {
                    el.className = className;
                } else {
                    classNames = existingClassName.split("" "");
                    if (classNames.indexOf(classNa");
            WriteLiteral(@"me) < 0) {
                        el.className = existingClassName + "" "" + className;
                    }
                }
            });

            return this;
        },

        removeClass: function (className) {
            this.each(function (el) {
                var existingClassName = el.className,
                    classNames, index;
                if (existingClassName === className) {
                    el.className = """";
                } else if (existingClassName) {
                    classNames = existingClassName.split("" "");
                    index = classNames.indexOf(className);
                    if (index > 0) {
                        classNames.splice(index, 1);
                        el.className = classNames.join("" "");
                    }
                }
            });

            return this;
        },

        attr: function (name) {
            if (this.items.length === 0) {
                return null;
            }

       ");
            WriteLiteral(@"     return this.items[0].getAttribute(name);
        },

        on: function (eventName, handler) {
            this.each(function (el, idx) {
                var callback = function (e) {
                    e = e || window.event;
                    if (!e.which && e.keyCode) {
                        e.which = e.keyCode; // Normalize IE8 key events
                    }
                    handler.apply(el, [e]);
                };

                if (el.addEventListener) { // DOM Events
                    el.addEventListener(eventName, callback, false);
                } else if (el.attachEvent) { // IE8 events
                    el.attachEvent(""on"" + eventName, callback);
                } else {
                    el[""on"" + type] = callback;
                }
            });

            return this;
        },

        click: function (handler) {
            return this.on(""click"", handler);
        },

        keypress: function (handler) {
            return this.o");
            WriteLiteral(@"n(""keypress"", handler);
        }
    };

    function frame(el) {
        ns("".source .collapsible"", el).toggle();
    }

    function expandCollapseButton(el) {
        var frameId = el.getAttribute(""data-frameId"");
        frame(document.getElementById(frameId));
        if (el.innerText === ""+"") {
            el.innerText = ""-"";
        }
        else {
            el.innerText = ""+"";
        }
    }

    function tab(el) {
        var unselected = ns(""#header .selected"").removeClass(""selected"").attr(""id"");
        var selected = ns(""#"" + el.id).addClass(""selected"").attr(""id"");

        ns(""#"" + unselected + ""page"").hide();
        ns(""#"" + selected + ""page"").show();
    }

    ns("".rawExceptionDetails"").hide();
    ns("".collapsible"").hide();
    ns("".page"").hide();
    ns(""#stackpage"").show();

    ns("".expandCollapseButton"")
        .click(function () {
            expandCollapseButton(this);
        })
        .keypress(function (e) {
            if (e.which === 13)");
            WriteLiteral(@" {
                expandCollapseButton(this);
            }
        });

    ns(""#header li"")
        .click(function () {
            tab(this);
        })
        .keypress(function (e) {
            if (e.which === 13) {
                tab(this);
            }
        });

    ns("".showRawException"")
        .click(function () {
            var exceptionDetailId = this.getAttribute(""data-exceptionDetailId"");
            ns(""#"" + exceptionDetailId).toggle();
        });
})(window);
            //-->
        </script>
    </body>
</html>
");
        }
        #pragma warning restore 1998
    }
}
