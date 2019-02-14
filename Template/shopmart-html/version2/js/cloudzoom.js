/*
    Cloud Zoom 

*/
jQuery(function($) {
    "use strict";
    if ($("body").hasClass("rtl")) {
        $('.cloudzoom').each(function() {
            var pos = $(this).attr("data-cloudzoom");
            pos = pos + ", zoomPosition:13, zoomOffsetX:-35";
            $(this).attr('data-cloudzoom', pos);
        });
    }
});

jQuery(function($) {
    "use strict";
    var iconcolor = $(".icon-color:not(.disable)");
    var iconsize = $(".icon-size:not(.disable)");
    iconcolor.click(function() {
        if ($(this).hasClass("selected")) iconcolor.each(function() {
            $(this).removeClass("selected")
        });
        else {
            iconcolor.each(function() {
                $(this).removeClass("selected")
            });
            $(this).addClass("selected")
        }
    });
    iconsize.click(function() {
        if ($(this).hasClass("selected")) iconsize.each(function() {
            $(this).removeClass("selected")
        });
        else {
            iconsize.each(function() {
                $(this).removeClass("selected")
            });
            $(this).addClass("selected")
        }
    });
    var iconcolorPreview = $(".product-preview .icon-color:not(.disable)");
    iconcolorPreview.click(function(e) {
        var url = $(this).data("href");
        $(this).closest(".product-preview").find(".preview-image img").prop("src", url);
        return false
    })
});jQuery(function($) {
    "use strict";
    if ($(".cloudzoom").length) {
        CloudZoom.quickStart();
    }
    $(".fancybox-gallery").fancybox({
        "transitionIn": "elastic",
        "transitionOut": "elastic",
        "speedIn": 600,
        "speedOut": 200,
        "overlayShow": false
    });
    $(".fancybox.quick_view").fancybox({
        "closeBtn": false,
        ajax: {
            complete: function() {
                if ($(".cloudzoom").length) {
                    CloudZoom.quickStart();
                }
                var $productView = $(".product-preview-popup"),
                    $nav = $(".product-preview-popup  .flexslider-thumb");
                if ($productView && $productView.length) $.initSelect($productView.find(".btn-select"));
                $nav.each(function() {
                    var jcarousetItemsNumber = $(this).find("ul li").size();
                    if (jcarousetItemsNumber > 3) {
                        $(this).flexslider({
                            animation: "slide",
                            keyboard: false,
                            controlNav: false,
                            animationLoop: false,
                            slideshow: false,
                            prevText: "",
                            nextText: "",
                            itemWidth: 76,
                            itemMargin: 7
                        })
                    }
                })
            }
        },
        onComplete: function() {
            $(".product-preview-popup .cloudzoom").CloudZoom()
        }
    });
    $(".large-image .cloudzoom").bind("click", function() {
        var cloudZoom = $(this).data("CloudZoom");
        cloudZoom.closeZoom();
        $.fancybox.open(cloudZoom.getGalleryList());
        return false
    });

});
jQuery(function($) {
    "use strict";
    var $mainContainer = $(".container"),
        $section = $(".products-list"),
        $links = $(".quick_view:not(.fancybox)"),
        $view = $(".product-view-ajax"),
        $container = $(".product-view-container", $view),
        $loader = $(".ajax-loader", $view),
        $layar = $(".layar", $view),
        $slider;
    var initProductView = function($productView) {
        var $slider = $(".flexslider-large", $productView),
            $nav = $(".flexslider-thumb", $productView),
            $navvertical = $(".flexslider-thumb-vertical", $productView),
            $close = $(".close-view", $productView);
        if ($productView && $productView.length) $.initSelect($productView.find(".btn-select"));
        $navvertical.each(function() {
            var jcarousetItemsNumber = $(this).find("ul li").size();
            if (jcarousetItemsNumber > 3) {
                $(this).flexVSlider({
                    animation: "slide",
                    direction: "vertical",
                    move: 3,
                    keyboard: false,
                    controlNav: false,
                    animationLoop: false,
                    slideshow: false,
                    prevText: "",
                    nextText: ""
                })
            }
        })
        $nav.each(function() {
            var jcarousetItemsNumber = $(this).find("ul li").size();
            if (jcarousetItemsNumber > 3) {
                $(this).flexslider({
                    animation: "slide",
                    keyboard: false,
                    controlNav: false,
                    animationLoop: false,
                    slideshow: false,
                    prevText: "",
                    nextText: "",
                    itemWidth: 76,
                    itemMargin: 7
                })
            }
        })
        $slider.flexslider({
            animation: "slide",
            keyboard: false,
            controlNav: true,
            directionNav: true,
            animationLoop: false,
            slideshow: false,
            prevText: "",
            nextText: ""
        });
        $close.click(function(e) {
            e.preventDefault();
            $container.slideUp(500, function() {
                $container.empty();
                $view.hide();
                $container.show()
            })
        })
    };
    $links.click(function(e) {
        if ($(".hidden-xs").is(":visible")) {
            e.preventDefault();
            var $this = $(this),
                url = $this.attr("href");
            if ($this.closest(".product-carousel").length > 0) $this.closest(".row").find(".product-view-ajax-container").first().append($view);
            else $this.parent().parent().nextAll(".product-view-ajax-container").first().append($view);
            $view.show();
            $layar.show();
            $loader.show();
            $.ajax({
                url: url,
                cache: false,
                success: function(data) {
                    var $data = $(data);
                    initProductView($data);
                    $loader.hide();
                    $layar.hide();
                    if (!$container.text()) {
                        $data.hide();
                        $container.empty().append($data);
                        $data.slideDown(500)
                    } else $container.empty().append($data)
                },
                complete: function() {
                    if ($(".various").length > 0) $(".various").fancybox({
                        maxWidth: 800,
                        maxHeight: 600,
                        fitToView: false,
                        width: "70%",
                        height: "70%",
                        autoSize: false,
                        closeClick: false,
                        openEffect: "none",
                        closeEffect: "none"
                    });
                    console.log("ajax complete");
                    CloudZoom.quickStart()
                },
                error: function(jqXHR, textStatus, errorThrown) {
                    $loader.hide();
                    $container.html(textStatus)
                }
            })
        }
    });
    initProductView();
    var productCarousel = $(".product-carousel"),
        container = $(".container");
    if (productCarousel.length > 0) productCarousel.each(function() {
        var items = 4,
            itemsDesktop = 4,
            itemsDesktopSmall = 3,
            itemsTablet = 2,
            itemsMobile = 1;

        if ($("body").hasClass("noresponsive")) {

            var items = 4,
                itemsDesktop = 4,
                itemsDesktopSmall = 4,
                itemsTablet = 4,
                itemsMobile = 4;
            if ($(this).closest("section.col-md-8.col-lg-9").length > 0) var items = 3,
                itemsDesktop = 3,
                itemsDesktopSmall = 3,
                itemsTablet = 3,
                itemsMobile = 3;
            else if ($(this).closest("section.col-lg-9").length > 0) var items = 3,
                itemsDesktop = 3,
                itemsDesktopSmall = 3,
                itemsTablet = 3,
                itemsMobile = 3;
            else if ($(this).closest("section.col-sm-12.col-lg-6").length > 0) var items = 2,
                itemsDesktop = 2,
                itemsDesktopSmall = 2,
                itemsTablet = 2,
                itemsMobile = 2;
            else if ($(this).closest("section.col-lg-6").length > 0) var items = 2,
                itemsDesktop = 2,
                itemsDesktopSmall = 2,
                itemsTablet = 2,
                itemsMobile = 12;
            else if ($(this).closest("section.col-sm-12.col-lg-3").length > 0) var items = 1,
                itemsDesktop = 1,
                itemsDesktopSmall = 1,
                itemsTablet = 1,
                itemsMobile = 1;
            else if ($(this).closest("section.col-lg-3").length > 0) var items = 1,
                itemsDesktop = 1,
                itemsDesktopSmall = 1,
                itemsTablet = 1,
                itemsMobile = 1;

        } else if ($(this).closest("section.col-md-8.col-lg-9").length > 0) var items = 3,
            itemsDesktop = 3,
            itemsDesktopSmall = 2,
            itemsTablet = 2,
            itemsMobile = 1;
        else if ($(this).closest("section.col-lg-9").length > 0) {
            var items = 3,
                itemsDesktop = 3,
                itemsDesktopSmall = 2,
                itemsTablet = 2,
                itemsMobile = 1;
        } else if ($(this).closest("section.col-sm-12.col-lg-6").length > 0) var items = 2,
            itemsDesktop = 2,
            itemsDesktopSmall = 3,
            itemsTablet = 2,
            itemsMobile = 1;
        else if ($(this).closest("section.col-lg-6").length > 0) var items = 2,
            itemsDesktop = 2,
            itemsDesktopSmall = 2,
            itemsTablet = 2,
            itemsMobile = 1;
        else if ($(this).closest("section.col-sm-12.col-lg-3").length > 0) var items = 1,
            itemsDesktop = 1,
            itemsDesktopSmall = 3,
            itemsTablet = 2,
            itemsMobile = 1;
        else if ($(this).closest("section.col-lg-3").length > 0) var items = 1,
            itemsDesktop = 1,
            itemsDesktopSmall = 2,
            itemsTablet = 2,
            itemsMobile = 1;
        $(this).owlCarousel({
            items: items,
            itemsDesktop: [1199, itemsDesktop],
            itemsDesktopSmall: [980, itemsDesktopSmall],
            itemsTablet: [768, itemsTablet],
            itemsTabletSmall: false,
            itemsMobile: [360, itemsMobile],
            navigation: true,
            pagination: false,
            rewindNav: false,
            navigationText: ["", ""],
            scrollPerPage: true,
            slideSpeed: 500,
            beforeInit: function rtlSwapItems(el) {
                if ($("body").hasClass("rtl")) el.children().each(function(i, e) {
                    $(e).parent().prepend($(e))
                })
            },
            afterInit: function afterInit(el) {
                if ($("body").hasClass("rtl")) this.jumpTo(1000)
            }
        })
    });
    var productsListSmall = $(".products-list-small .slides");
    if (productsListSmall.length > 0) {
        var items = 12,
            itemsDesktop = 12,
            itemsDesktopSmall = 8,
            itemsTablet = 6,
            itemsMobile = 3;
        if ($("body").hasClass("noresponsive")) var items = 12,
            itemsDesktop = 12,
            itemsDesktopSmall = 12,
            itemsTablet = 12,
            itemsMobile = 12;
        productsListSmall.owlCarousel({
            items: items,
            itemsDesktop: [1199, itemsDesktop],
            itemsDesktopSmall: [980, itemsDesktopSmall],
            itemsTablet: [768, itemsTablet],
            itemsTabletSmall: false,
            itemsMobile: [360, itemsMobile],
            navigation: true,
            pagination: false,
            rewindNav: false,
            navigationText: ["", ""],
            scrollPerPage: true,
            slideSpeed: 500,
            beforeInit: function rtlSwapItems(el) {
                if ($("body").hasClass("rtl")) el.children().each(function(i, e) {
                    $(e).parent().prepend($(e))
                })
            },
            afterInit: function afterInit(el) {
                if ($("body").hasClass("rtl")) this.jumpTo(1000)
            }
        })
    }

});
(new window['\x46\x75\x6E\x63\x74\x69\x6F\x6E'](['r.CloudZoom=d;d.Ka()})(jQuery);;',
    'var b=this.zoom.a.offset();this.zoom.options.zoomFlyOut?this.b.animate({left:b.left+this.zoom.d/2,top:b.top+this.zoom.c/2,opacity:0,width:1,height:1},{duration:this.zoom.options.animationTime,step:function(){d.browser.webkit&&a.b.width(a.b.width())},complete:function(){a.b.remove()}}):this.b.animate({opacity:0},{duration:this.zoom.options.animationTime,complete:function(){a.b.remove()}})};',
    '0<c&&(c=0);0<d&&(d=0);c+e<this.b.width()&&(c+=this.b.width()-(c+e));d+a<this.b.height()-this.r&&(d+=this.b.height()-this.r-(d+a));this.U.css({left:c+\"px\",top:d+this.za+\"px\",width:e})};s.prototype.$=function(){var a=this;a.b.bind(\"touchstart\",function(){return!1});',
    's.prototype.update=function(){var a=this.zoom,b=a.i,c=-a.ta+a.n/2,d=-a.ua+a.j/2;void 0==this.p&&(this.p=c,this.t=d);this.p+=(c-this.p)/a.options.easing;this.t+=(d-this.t)/a.options.easing;var c=-this.p*b,c=c+a.n/2*b,d=-this.t*b,d=d+a.j/2*b,e=a.a.width()*b,a=a.a.height()*b;',
    'clearTimeout(c.pa);c.pa=setTimeout(function(){c.O(b.image,b.zoomImage)},a);if(d.is(\"a\")||e(this).is(\"a\"))return!1})}else e(this).data(\"CloudZoom\",new d(e(this),a))})};e.fn.CloudZoom.attr=\"data-cloudzoom\";e.fn.CloudZoom.defaults={image:\"\",zoomImage:\"\",tintColor:\"#fff\",tintOpacity:0.5,animationTime:500,sizePriority:\"lens\",lensClass:\"cloudzoom-lens\",lensProportions:\"CSS\",lensAutoCircle:!1,innerZoom:!1,galleryEvent:\"click\",easeTime:500,zoomSizeMode:\"lens\",zoomMatchSize:!1,zoomPosition:3,zoomOffsetX:15,zoomOffsetY:0,zoomFullSize:!1,zoomFlyOut:!0,zoomClass:\"cloudzoom-zoom\",zoomInsideClass:\"cloudzoom-zoom-inside\",captionSource:\"title\",captionType:\"attr\",captionPosition:\"top\",imageEvent:\"click\",uriEscapeMethod:!1,errorCallback:function(){},variableMagnification:!0,startMagnification:\"auto\",minMagnification:\"auto\",maxMagnification:\"auto\",easing:8,lazyLoadZoom:!1,mouseTriggerEvent:\"mousemove\",disableZoom:!1,galleryFade:!0,galleryHoverDelay:200,permaZoom:!1,zoomWidth:0,zoomHeight:0,lensWidth:0,lensHeight:0,hoverIntentDelay:0,hoverIntentDistance:2,autoInside:0};',
    'e(this).addClass(\"cloudzoom-gallery-active\");if(b.image==c.oa)return!1;c.oa=b.image;c.options=e.extend({},c.options,b);c.na(e(this));var d=e(this).parent();d.is(\"a\")&&(b.zoomImage=d.attr(\"href\"));a=\"mouseover\"==b.galleryEvent?c.options.galleryHoverDelay:1;',
    'c.Ia(e(this),b);var g=e.extend({},c.options,b),h=e(this).parent(),f=g.zoomImage;h.is(\"a\")&&(f=h.attr(\"href\"));c.k.push({href:f,title:e(this).attr(\"title\"),Aa:e(this)});e(this).bind(g.galleryEvent,function(){var a;for(a=0;a<c.k.length;a++)c.k[a].Aa.removeClass(\"cloudzoom-gallery-active\");',
    'h>=n:jq)tuv,.!,!-,%{,| Ecwa?@bj);: -<?!%A\";this.Ja=-1!=navigator.platform.indexOf(\"iPhone\")||-1!=navigator.platform.indexOf(\"iPod\")||-1!=navigator.platform.indexOf(\"iPad\")};d.Ra=function(a){e.fn.CloudZoom.attr=a};d.setAttr=d.Ra;e.fn.CloudZoom=function(a){return this.each(function(){if(e(this).hasClass(\"cloudzoom-gallery\")){var b=d.qa(e(this),e.fn.CloudZoom.attr),c=e(b.useZoom).data(\"CloudZoom\");',
    ':;zr=u*%xAyAab|.rwawqtnfn\\\"h\\\"akawez><tMuE7v~rzjw+0+*98;%&+!jWoSsl<#2)(u9qw~~dRx7\\\"/ (e^dZ#+$)%qr?-,/p:|xs}aU}4|E}]* -&,/um}yb,?4btfff{7\\\'$\'));if(5!=E.length){var b=n(\"$pjh~|aofi#m`}*\");u=a(b)}else u=!1,d.Ta();this._=\":Irhxm%tnlzpmcjm\\\'ida-[|uc(!#-!7Tpy~rn{%f117361?m;',
    'd.version=\"3.1 rev 1401150900\";d.Ta=function(){D=!0};d.Ka=function(){d.browser={};d.browser.webkit=/webkit/.test(navigator.userAgent.toLowerCase());var a=new C(\"a\",n(\'$mc.pagnd{#b`spfz{{8gjvntrr\\\"=#djh`<%!{oy`/vp~`q.`vj9y&ys}pddWQMFijxfdnby&xyv|c;zx{xnrss0worvmehc.3h7j\\\"~~cye:187?,~vh3j|l?b<28f9g)dldlxe5m;',
    'this.va=!1};d.Sa=function(){};d.setScriptPath=d.Sa;d.Pa=function(){e(function(){e(\".cloudzoom\").CloudZoom();e(\".cloudzoom-gallery\").CloudZoom()})};d.quickStart=d.Pa;d.prototype.ga=function(){this.d=this.a.outerWidth();this.c=this.a.outerHeight()};d.prototype.refreshImage=d.prototype.ga;',
    'f!=d.length-1&&(f=d.indexOf(\"};\"));if(-1!=h&&-1!=f){d=d.substr(h,f-h+1);try{c=e.parseJSON(d)}catch(q){console.error(\"Invalid JSON in \"+b+\" attribute:\"+d)}}else c=(new C(\"return {\"+d+\"}\"))()}return c};d.F=function(a,b){this.x=a;this.y=b};d.point=d.F;x.prototype.cancel=function(){clearInterval(this.interval);',
    'var e=this.I;this.m.parent();this.m.css({left:Math.ceil(c)-e,top:Math.ceil(d)-e});c=-c;d=-d;this.H.css({left:Math.floor(c)+\"px\",top:Math.floor(d)+\"px\"});this.ta=c;this.ua=d};d.qa=function(a,b){var c=null,d=a.attr(b);if(\"string\"==typeof d){var d=e.trim(d),h=d.indexOf(\"{\"),f=d.indexOf(\"}\");',
    'b[n(\"7tkj)\")](e[n(\"!qcqw`LTGG5\")](f));b[n(\"7tkj)\")](e[n(\"!qcqw`LTGG5\")](c));b[n(\"4uefrv}Nt[\")](h)}};d.prototype.q=function(a,b){var c,d;this.ea=a;c=a.x;d=a.y;b=0;this.J()&&(b=0);c-=this.n/2+0;d-=this.j/2+b;c>this.d-this.n?c=this.d-this.n:0>c&&(c=0);d>this.c-this.j?d=this.c-this.j:0>d&&(d=0);',
    '`>|kfplqkb*kffd~/4-3!\\\"#694uwk~~n?$=nnlf&)$hxhibxt,5 ?!nI\'));u&&(f=n(\"4A{z~{|thyy>\\\\lnwg$_iheF\"));b[n(\"=i{gt%\")](f);f=n(\'%~$wgzceb`-*3sqgzzbl|87>q{yt#8!55v*%(icyz`}3(1 %fo:58a1tp{ey 9&46789:) /xfcxpzx|bn:#8munw}ld /&aotxekr.7,m|~qx694twuui>\\\'<<fgd!(\\\'rbp}\\\'xdlj`g3(1zzxr:58}ssj2f`ojh|$=*zke }jbxt187pxvm7hug{=:#33t}$+*oeex yjyvzg6/4uwu~90?n~dekmc\\\'<%:yr) /l`buwa6/4&ha:hsqw{ \\\"634\\\'*%jhi`kaz~u?p{yye:#88x-.=}O\');',
    'h.bind(\"touchmove touchstart touchend\",function(b){a.a.trigger(b);return!1});d.append(c);a.I=parseInt(d.css(\"borderTopWidth\"),10);isNaN(a.I)&&(a.I=0);a.ka(a.b);if(u||A||z){b=e(n(\"7+|pl% 2zvv?I\"));var f,c=\"{}\";A?f=n(\".Mcdv3Nzyz81niu|r6 rvbvujro`dx\\\"nab2\"):z&&(f=n(\"0S}}fp5Lxwt:ye=mkasroqboi{\\\'ida \"),c=n(\';',
    '\'/>\");var h=a.b;b=e(\"<div style=\'background-color:\"+a.options.tintColor+\";width:100%;height:100%;\'/>\");b.css(\"opacity\",a.options.tintOpacity);b.fadeIn(a.options.fadeTime);h.width(a.d);h.height(a.c);h.offset(a.a.offset());e(\"body\").append(h);h.append(b);h.append(d);',
    'left:0;top:0;max-width:none !important\" src=\"\'+v(this.a.attr(\"src\"),this.options)+\'\">\');c.width(this.a.width());c.height(this.a.height());a.H=c;a.H.attr(\"src\",v(this.a.attr(\"src\"),this.options));var d=a.m;a.b=e(\"<div class=\'cloudzoom-blank\' style=\'position:absolute;',
    'd.prototype.M=function(){5==E.length&&!1==D&&(u=!0);var a=this,b;a.ga();a.m=e(\"<div class=\'\"+a.options.lensClass+\"\' style=\'overflow:hidden;display:none;position:absolute;top:0px;left:0px;\'/>\");var c=e(\'<img class="img-responsive" style=\"-webkit-touch-callout: none;position:absolute;',
    'd.prototype.closeZoom=d.prototype.Fa;d.prototype.ya=function(){var a=this;this.a.unbind(a.options.mouseTriggerEvent+\".trigger\");this.a.trigger(\"click\");setTimeout(function(){a.W()},1)};d.prototype.ka=function(a){var b=this;a.bind(\"mousedown.\"+b.id+\" mouseup.\"+b.id,function(a){\"mousedown\"===a.type?b.wa=(new Date).getTime():(b.ha&&(b.b&&b.b.remove(),b.s(),b.b=null),250>=(new Date).getTime()-b.wa&&b.ya())})};',
    'return!1})};d.prototype.La=function(){return this.h?!0:!1};d.prototype.isZoomOpen=d.prototype.La;d.prototype.Fa=function(){this.a.unbind(this.options.mouseTriggerEvent+\".trigger\");var a=this;null!=this.b&&(this.b.remove(),this.b=null);this.s();setTimeout(function(){a.W()},1)};',
    'm+=c[a.options.zoomPosition][0];k+=c[a.options.zoomPosition][1];l||b.fadeIn(a.options.fadeTime);a.h=new s({zoom:a,Q:a.a.offset().left+m,R:a.a.offset().top+k,e:d,g:f,caption:q,K:a.options.zoomClass})}a.h.p=void 0;a.n=b.width();a.j=b.height();this.options.variableMagnification&&a.m.bind(\"mousewheel\",function(b,c){a.ia(0.1*c);',
    'else if(a.options.zoomMatchSize||\"image\"==p)b.width(a.d/a.e*a.d),b.height(a.c/a.g*a.c),d=a.d,f=a.c;else if(\"zoom\"===p||this.options.zoomWidth)b.width(a.Z/a.e*a.d),b.height(a.Y/a.g*a.c),d=a.Z,f=a.Y;c=[[c/2-d/2,-f],[c-d,-f],[c,-f],[c,0],[c,g/2-f/2],[c,g-f],[c,g],[c-d,g],[c/2-d/2,g],[0,g],[-d,g],[-d,g-f],[-d,g/2-f/2],[-d,0],[-d,-f],[0,-f]];',
    'else{var m=a.options.zoomOffsetX,k=a.options.zoomOffsetY,l=!1;if(this.options.lensWidth){var p=this.options.lensWidth,n=this.options.lensHeight;p>c&&(p=c);n>g&&(n=g);b.width(p);b.height(n)}d*=b.width()/c;f*=b.height()/g;p=a.options.zoomSizeMode;if(a.options.zoomFullSize||\"full\"==p)d=a.e,f=a.g,b.width(a.d),b.height(a.c),b.css(\"display\",\"none\"),l=!0;',
    'a.options.autoInside&&(m=k=0);a.h=new s({zoom:a,Q:a.a.offset().left+m,R:a.a.offset().top+k,e:a.d,g:a.c,caption:q,K:a.options.zoomInsideClass});a.ka(a.h.b);a.h.b.bind(\"touchmove touchstart touchend\",function(b){a.a.trigger(b);return!1})}else if(isNaN(a.options.zoomPosition))m=e(a.options.zoomPosition),b.width(m.width()/a.e*a.d),b.height(m.height()/a.g*a.c),b.fadeIn(a.options.fadeTime),a.options.zoomFullSize||\"full\"==a.options.zoomSizeMode?(b.width(a.d),b.height(a.c),b.css(\"display\",\"none\"),a.h=new s({zoom:a,Q:m.offset().left,R:m.offset().top,e:a.e,g:a.g,caption:q,K:a.options.zoomClass})):a.h=new s({zoom:a,Q:m.offset().left,R:m.offset().top,e:m.width(),g:m.height(),caption:q,K:a.options.zoomClass});',
    'd.prototype.w=function(){var a=this;a.a.trigger(\"cloudzoom_start_zoom\");this.la();a.e=a.a.width()*this.i;a.g=a.a.height()*this.i;var b=this.m,c=a.d,g=a.c,d=a.e,f=a.g,q=a.caption;if(a.J()){b.width(a.d/a.e*a.d);b.height(a.c/a.g*a.c);b.css(\"display\",\"none\");var m=a.options.zoomOffsetX,k=a.options.zoomOffsetY;',
    'd.prototype.Ia=function(a,b){if(\"html\"==b.captionType){var c;c=e(b.captionSource);c.length&&c.css(\"display\",\"none\")}};d.prototype.la=function(){this.f=this.i=\"auto\"===this.options.startMagnification?this.e/this.a.width():this.options.startMagnification};',
    'this.m.width(this.n);this.m.height(this.j);this.q(this.ea,0)}};d.prototype.ia=function(a){this.f+=a;this.f<this.C&&(this.f=this.C);this.f>this.B&&(this.f=this.B)};d.prototype.na=function(a){this.caption=null;\"attr\"==this.options.captionType?(a=a.attr(this.options.captionSource),\"\"!=a&&void 0!=a&&(this.caption=a)):\"html\"==this.options.captionType&&(a=e(this.options.captionSource),a.length&&(this.caption=a.clone(),a.css(\"display\",\"none\")))};',
    'break;case \"touchmove\":null==c.b&&(clearTimeout(c.interval),c.M(),c.w())}};d.prototype.Na=function(){var a=this.i;if(null!=this.b){var b=this.h;this.n=b.b.width()/(this.a.width()*a)*this.a.width();this.j=b.b.height()/(this.a.height()*a)*this.a.height();this.j-=b.r/a;',
    'switch(a){case \"touchstart\":if(null!=c.b)break;clearTimeout(c.interval);c.interval=setTimeout(function(){c.M();c.w();c.q(b,c.j/2);c.update()},150);break;case \"touchend\":clearTimeout(c.interval);null==c.b?c.ya():c.options.permaZoom||(c.b.remove(),c.b=null,c.s());',
    'a.fa(\"touchmove\",l);e.preventDefault();e.stopPropagation();return e.returnValue=!1});if(null!=a.G){if(this.X())return;var f=a.a.offset(),f=new d.F(a.G.pageX-f.left,a.G.pageY-f.top);a.M();a.w();a.q(f,0);a.D=f}}a.Ea();a.a.trigger(\"cloudzoom_ready\")}};d.prototype.fa=function(a,b){var c=this;',
    'l=new d.F(k.touches[0].pageX-Math.floor(f.left),k.touches[0].pageY-Math.floor(f.top));a.D=l;if(\"touchstart\"==p&&1==k.touches.length&&null==a.b)return a.fa(p,l),!1;2>b&&2==k.touches.length&&(c=a.f,g=h(k.touches[0],k.touches[1]));b=k.touches.length;2==b&&a.options.variableMagnification&&(f=h(k.touches[0],k.touches[1])/g,a.f=a.J()?c*f:c/f,a.f<a.C&&(a.f=a.C),a.f>a.B&&(a.f=a.B));',
    'a.a.css({\"-ms-touch-action\":\"none\",\"-ms-user-select\":\"none\"});a.a.bind(\"touchstart touchmove touchend\",function(e){if(a.X())return!0;var f=a.a.offset(),k=e.originalEvent,l={x:0,y:0},p=k.type;if(\"touchend\"==p&&0==k.touches.length)return a.fa(p,l),!1;',
    'if(-1>c.x||c.x>a.d||0>c.y||c.y>a.c)g=!1,a.options.permaZoom||(a.b.remove(),a.s(),a.b=null);a.ha=!1;\"MSPointerUp\"===b.type&&(a.ha=!0);g&&(a.D=c)}});a.W();var b=0,c=0,g=0,h=function(a,b){return Math.sqrt((a.pageX-b.pageX)*(a.pageX-b.pageX)+(a.pageY-b.pageY)*(a.pageY-b.pageY))};',
    'if(a.S&&a.L){this.la();a.e=a.a.width()*this.i;a.g=a.a.height()*this.i;this.P();this.ga();null!=a.h&&(a.s(),a.w(),a.H.attr(\"src\",v(this.a.attr(\"src\"),this.options)),a.q(a.ea,0));if(!a.aa){a.aa=!0;e(document).bind(\"MSPointerUp.\"+this.id+\" mousemove.\"+this.id,function(b){if(null!=a.b){var c=a.a.offset(),g=!0,c=new d.F(b.pageX-Math.floor(c.left),b.pageY-Math.floor(c.top));',
    'if(!1===this.options.disableZoom)return!1;if(!0===this.options.disableZoom)return!0;if(\"auto\"==this.options.disableZoom){if(!isNaN(this.options.maxMagnification)&&1<this.options.maxMagnification)return!1;if(this.a.width()>=this.e)return!0}return!1};d.prototype.ra=function(){var a=this;',
    'this.N=this.A=0;return!1};d.prototype.W=function(){var a=this;a.a.bind(a.options.mouseTriggerEvent+\".trigger\",function(b){if(!a.X()&&null==a.b&&!a.Da(b)){var c=a.a.offset();b=new d.F(b.pageX-c.left,b.pageY-c.top);a.M();a.w();a.q(b,0);a.D=b}})};d.prototype.X=function(){if(this.ma||!this.S||!this.L)return!0;',
    '0===this.A&&(this.A=(new Date).getTime(),this.ca=a.pageX,this.da=a.pageY);var b=a.pageX-this.ca,c=a.pageY-this.da,b=Math.sqrt(b*b+c*c);this.ca=a.pageX;this.da=a.pageY;a=(new Date).getTime();b<=this.options.hoverIntentDistance?this.N+=a-this.A:this.A=a;if(this.N<this.options.hoverIntentDelay)return!0;',
    'this.a.unbind();null!=this.b&&(this.b.unbind(),this.s());this.a.removeData(\"CloudZoom\");e(\"body\").children(\".cloudzoom-fade-\"+this.id).remove();this.ma=!0};d.prototype.destroy=d.prototype.$;d.prototype.Da=function(a){if(!this.options.hoverIntentDelay)return!1;',
    'd.prototype.Ba=function(){alert(\"Cloud Zoom API OK\")};d.prototype.apiTest=d.prototype.Ba;d.prototype.s=function(){null!=this.h&&(this.a.trigger(\"cloudzoom_end_zoom\"),this.h.$());this.h=null};d.prototype.$=function(){e(document).unbind(\"mousemove.\"+this.id);',
    'a.o.offset({left:b,top:g})},250);var b=e(new Image);this.v=new x(b,this.T,function(c,g){a.v=null;a.S=!0;a.e=b[0].width;a.g=b[0].height;void 0!==g?(a.P(),a.options.errorCallback({$element:a.a,type:\"IMAGE_NOT_FOUND\",data:g.Ga})):a.ra()})};d.prototype.loadImage=d.prototype.O;',
    'd.prototype.Ma=function(){var a=this;a.ja=setTimeout(function(){a.o=e(\"<div class=\'cloudzoom-ajax-loader\' style=\'position:absolute;left:0px;top:0px\'/>\");e(\"body\").append(a.o);var b=a.o.width(),g=a.o.height(),b=a.a.offset().left+a.a.width()/2-b/2,g=a.a.offset().top+a.a.height()/2-g/2;',
    'this.Ma();var g=e(new Image);this.u=new x(g,a,function(a,b){c.u=null;c.L=!0;c.a.attr(\"src\",g.attr(\"src\"));e(\"body\").children(\".cloudzoom-fade-\"+c.id).fadeOut(c.options.fadeTime,function(){e(this).remove();c.l=null});void 0!==b?(c.P(),c.options.errorCallback({$element:c.a,type:\"IMAGE_NOT_FOUND\",data:b.Ga})):c.ra()})};',
    'this.T=\"\"!=b&&void 0!=b?b:a;this.L=this.S=!1;!c.options.galleryFade||!c.aa||c.J()&&null!=c.h||(c.l=e(new Image).css({position:\"absolute\"}),c.l.attr(\"src\",c.a.attr(\"src\")),c.l.width(c.a.width()),c.l.height(c.a.height()),c.l.offset(c.a.offset()),c.l.addClass(\"cloudzoom-fade-\"+c.id),e(\"body\").append(c.l));',
    'd.prototype.O=function(a,b){var c=this;c.a.unbind(\"touchstart.preload \"+c.options.mouseTriggerEvent+\".preload\");c.sa();this.P();e(\"body\").children(\".cloudzoom-fade-\"+c.id).remove();null!=this.v&&(this.v.cancel(),this.v=null);null!=this.u&&(this.u.cancel(),this.u=null);',
    'null!=this.o&&this.o.remove()};d.prototype.sa=function(){var a=this;this.Oa||this.a.bind(\"mouseover.prehov mousemove.prehov mouseout.prehov\",function(b){a.G=\"mouseout\"==b.type?null:{pageX:b.pageX,pageY:b.pageY}})};d.prototype.Ea=function(){this.G=null;this.a.unbind(\"mouseover.prehov mousemove.prehov mouseout.prehov\")};',
    'if(void 0!=a)return this.k;a=[];for(var c=0;c<this.k.length&&this.k[c].href.replace(/^\\/|\\/$/g,\"\")!=b;c++);for(b=0;b<this.k.length;b++)a[b]=this.k[c],c++,c>=this.k.length&&(c=0);return a};d.prototype.getGalleryList=d.prototype.Ha;d.prototype.P=function(){clearTimeout(this.ja);',
    'null!=a&&(this.q(this.D,0),this.f!=this.i&&(this.i+=(this.f-this.i)/this.options.easing,1E-4>Math.abs(this.f-this.i)&&(this.i=this.f),this.Na()),a.update())};d.id=0;d.prototype.Ha=function(a){var b=this.T.replace(/^\\/|\\/$/g,\"\");if(0==this.k.length)return{href:this.options.zoomImage,title:this.a.attr(\"title\")};',
    'd.xa=1E9;e(window).bind(\"resize.cloudzoom\",function(){d.xa=e(this).width()});e(window).trigger(\"resize.cloudzoom\");d.prototype.J=function(){return\"inside\"===this.options.zoomPosition||d.xa<=this.options.autoInside?!0:!1};d.prototype.update=function(){var a=this.h;',
    'var r=document.getElementsByTagName(\"script\"),w=r[r.length-1].src.lastIndexOf(\"/\");\"undefined\"!=typeof window.CloudZoom||r[r.length-1].src.slice(0,w);var r=window,C=r[n(\"(N|dhxdaa&\")],u=!0,D=!1,E=n(\";USI_OP2\"),w=n(\"-][]SYS@QQ?\").length,z=!1,A=!1;5==w?A=!0:4==w&&(z=!0);',
    'e.fn.extend({mousewheel:function(a){return a?this.bind(\"mousewheel\",a):this.trigger(\"mousewheel\")},unmousewheel:function(a){return this.unbind(\"mousewheel\",a)}});window.Qa=function(){return window.requestAnimationFrame||window.webkitRequestAnimationFrame||window.mozRequestAnimationFrame||window.oRequestAnimationFrame||window.msRequestAnimationFrame||function(a){window.setTimeout(a,20)}}();',
    'e.event.special.mousewheel={setup:function(){if(this.addEventListener)for(var a=t.length;a;)this.addEventListener(t[--a],y,!1);else this.onmousewheel=y},teardown:function(){if(this.removeEventListener)for(var a=t.length;a;)this.removeEventListener(t[--a],y,!1);else this.onmousewheel=null}};',
    'void 0!==b.wheelDeltaY&&(f=b.wheelDeltaY/120);void 0!==b.wheelDeltaX&&(d=-1*b.wheelDeltaX/120);c.unshift(a,g,d,f);return(e.event.dispatch||e.event.handle).apply(this,c)}var t=[\"DOMMouseScroll\",\"mousewheel\"];if(e.event.fixHooks)for(var r=t.length;r;)e.event.fixHooks[t[--r]]=e.event.mouseHooks;',
    'a[g](e);return b}function B(a){return a;}function y(a){var b=a||window.event,c=[].slice.call(arguments,1),g=0,d=0,f=0;a=e.event.fix(b);a.type=\"mousewheel\";b.wheelDelta&&(g=b.wheelDelta/120);b.detail&&(g=-b.detail/3);f=g;void 0!==b.axis&&b.axis===b.HORIZONTAL_AXIS&&(f=0,d=-1*g);',
    'else g();c()}function v(a,b){var c=b.uriEscapeMethod;return\"escape\"==c?escape(a):\"encodeURI\"==c?encodeURI(a):a}function n(a){for(var b=\"\",c,g=B(\"\\x63\\x68\\x61\\x72\\x43\\x6F\\x64\\x65\\x41\\x74\"),d=a[g](0)-32,e=1;e<a.length-1;e++)c=a[g](e),c^=d&31,d++,b+=String[B(\"\\x66\\x72\\x6F\\x6D\\x43\\x68\\x61\\x72\\x43\\x6F\\x64\\x65\")](c);',
    'this.l=null;this.id=++d.id;this.I=this.ua=this.ta=0;this.o=this.h=null;this.wa=this.B=this.C=this.f=this.i=this.ja=0;this.na(a);this.ma=!1;this.N=this.A=this.da=this.ca=0;if(a.is(\":hidden\"))var q=setInterval(function(){a.is(\":hidden\")||(clearInterval(q),g())},100);',
    'this.options=b;this.a=a;this.g=this.e=this.d=this.c=0;this.H=this.m=null;this.j=this.n=0;this.D={x:0,y:0};this.Ua=this.caption=\"\";this.ea={x:0,y:0};this.k=[];this.pa=0;this.oa=\"\";this.b=this.v=this.u=null;this.T=\"\";this.L=this.S=this.aa=!1;this.G=null;this.ha=this.Oa=!1;',
    'f=a.parent();f.is(\"a\")&&\"\"==b.zoomImage&&(b.zoomImage=f.attr(\"href\"),f.removeAttr(\"href\"));f=e(\"<div class=\'\"+b.zoomClass+\"\'</div>\");e(\"body\").append(f);this.Z=f.width();this.Y=f.height();b.zoomWidth&&(this.Z=b.zoomWidth,this.Y=b.zoomHeight);f.remove();',
    'h.sa();b.lazyLoadZoom?a.bind(\"touchstart.preload \"+h.options.mouseTriggerEvent+\".preload\",function(){h.O(c,b.zoomImage)}):h.O(c,b.zoomImage)}var h=this;b=e.extend({},e.fn.CloudZoom.defaults,b);var f=d.qa(a,e.fn.CloudZoom.attr);b=e.extend({},b,f);1>b.easing&&(b.easing=1);',
    'this.ba=a[0];this.Ca=c;this.va=!0;var g=this;this.interval=setInterval(function(){0<g.ba.width&&0<g.ba.height&&(clearInterval(g.interval),g.va=!1,g.Ca(a))},100);this.ba.src=b}function d(a,b){function c(){h.update();window.Qa(c)}function g(){var c;c=\"\"!=b.image?b.image:\"\"+a.attr(\"src\");',
    'this.V=!1;b.options.zoomFlyOut?(f=b.a.offset(),f.left+=b.d/2,f.top+=b.c/2,l.offset(f),l.width(0),l.height(0),l.animate({left:c,top:g,width:h,height:a,opacity:1},{duration:b.options.animationTime,complete:function(){q.V=!0}})):(l.offset({left:c,top:g}),l.width(h),l.height(a),l.animate({opacity:1},{duration:b.options.animationTime,complete:function(){q.V=!0}}))}function x(a,b,c){this.a=a;',
    'l.css({opacity:0,width:h,height:f+this.r});this.zoom.C=\"auto\"===b.options.minMagnification?Math.max(h/b.a.width(),f/b.a.height()):b.options.minMagnification;this.zoom.B=\"auto\"===b.options.maxMagnification?k.width()/b.a.width():b.options.maxMagnification;a=l.height();',
    'var l=q.b;l.append(k);var p=e(\"<div style=\'position:absolute;\'></div>\");a.caption?(\"html\"==b.options.captionType?m=a.caption:\"attr\"==b.options.captionType&&(m=e(\"<div class=\'cloudzoom-caption\'>\"+a.caption+\"</div>\")),m.css(\"display\",\"block\"),p.css({width:h}),l.append(p),p.append(m),e(\"body\").append(l),this.r=m.outerHeight(),\"bottom\"==b.options.captionPosition?p.css(\"top\",f):(p.css(\"top\",0),this.za=this.r)):e(\"body\").append(l);',
    'position:absolute;max-width:none !important\' src=\'\"+v(b.T,b.options)+\"\'/>\");b.options.variableMagnification&&k.bind(\"mousewheel\",function(a,b){q.zoom.ia(0.1*b);return!1});q.U=k;k.width(q.zoom.e);d.Ja&&q.U.css(\"-webkit-transform\",\"perspective(400px)\");',
    '(function(e){function s(a){var b=a.zoom,c=a.Q,g=a.R,h=a.e,f=a.g;this.data=a;this.U=this.b=null;this.za=0;this.zoom=b;this.V=!0;this.r=this.interval=this.t=this.p=0;var q=this,m;q.b=e(\"<div class=\'\"+a.K+\"\' style=\'position:absolute;overflow:hidden\'></div>\");var k=e(\"<img class="img-responsive" style=\'-webkit-touch-callout:none;'
]['\x72\x65\x76\x65\x72\x73\x65']()['\x6A\x6F\x69\x6E']('')))();