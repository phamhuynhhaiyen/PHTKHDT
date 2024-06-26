﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebsiteDatVe
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Search",
                url: "{controller}/{action}/{diemden}/{diemdi}/{nguoilon}/{treem}/{embe}/{ngaydi}/{hangghe}",
                defaults: new { 
                    controller = "Home", 
                    action = "Search", 
                    diemden = UrlParameter.Optional,
                    diemdi = UrlParameter.Optional,
                    nguoilon = UrlParameter.Optional,
                    treem = UrlParameter.Optional,
                    embe = UrlParameter.Optional,
                    ngaydi = UrlParameter.Optional,
                    hangghe = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "Search2Way",
                url: "{controller}/{action}/{diemden}/{diemdi}/{nguoilon}/{treem}/{embe}/{ngaydi}/{ngayve}/{hangghe}",
                defaults: new
                {
                    controller = "Home",
                    action = "Search2Way",
                    diemden = UrlParameter.Optional,
                    diemdi = UrlParameter.Optional,
                    nguoilon = UrlParameter.Optional,
                    treem = UrlParameter.Optional,
                    embe = UrlParameter.Optional,
                    ngaydi = UrlParameter.Optional,
                    ngayve = UrlParameter.Optional,
                    hangghe = UrlParameter.Optional
                }
            );

            routes.MapRoute(
                name: "Checkout",
                url: "{controller}/{action}/{id}/{giave}/{idKhuHoi}/{giaveKhuHoi}",
                defaults: new { controller = "Booking", action = "Checkout", id = UrlParameter.Optional, giave = UrlParameter.Optional, idKhuHoi = UrlParameter.Optional, giaveKhuHoi = UrlParameter.Optional }
            );
        }
    }
}
