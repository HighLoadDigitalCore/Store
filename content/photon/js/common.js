$().ready(readyFunction);
$(window).resize(resizeFunction);
var widgetsLoaded = [];


function resizeFunction() {
    setSidebarHeight();
    setBlankWidgets();
    resizeEnd();

    // Main & User Menu
    $('.active-menu').removeClass('active-menu');
    $('.nav-active').removeClass('nav-active');
    $('.mobMMExpanded').removeClass('mobMMExpanded');
}

var chatTimer;
function loadAdminChat(first) {
    return;
    chatTimer = setTimeout(function () {
        $.post('/Master/ru/Chat/UnreadCount', {}, function (d) {
            if (d != '0') {
                var txt = 'Онлайн-чат содержит ' + d + ' непрочитанных сообщений. <a href="/Master/ru/Chat/Edit">Откройте</a> чат, чтобы прочитать их.';
                $.pnotify({
                    title: 'У вас есть непрочитанные сообщения',
                    type: 'info',
                    text: txt
                });
            }
            var nc = $('#chat-cell div');
            nc.remove();
            if (d != '0') {
                $('#chat-cell').append('<div class="notification-count">' + d + '</div>');
            }
            loadAdminChat(false);
        })
    }, first ? 1000 : 15000);

}
var dragObj1 = null;
var dragObj2 = null;
function resize() { //Ресайз боковой панели
    $('body').mouseup(function () {
        if (dragObj1)
            $.cookie('drag1', $('.side').width(), { expires: 365, path: '/' });
        dragObj1 = null //Сброс при отжатии мыши
        $('.resize').removeClass('resizeHover');
    });

    $('.resize')
	.mousedown(function () {
	    dragObj1 = $(this); //Присваиваем объект ресайза
	    $(this).addClass('resizeHover');
	    document.body.onselectstart = function () { return false }; //Убираем выделение текста при перемещении ресайза в IE...
	    return false; //...и в остальных браузерах
	})

    $('body').mousemove(
		function (e) {
		    var cursor = moveEvent(e);
		    if (dragObj1) {
		        //Приравниваем ширину к х-кординате курсора и преобразовывем ее в процентную
		        $('.side').width(((cursor.x - 4) * 100 / $('.container-tbl').width()).toFixed(1) + '%');
		    }
		});
}
function primaryNav() {
    $('.primary-nav li').click(function() {
        if ($(this).find('ul:first').is(':hidden')) {
            $(this).find('ul:first').fadeIn(200); //показываем подменю
            $('.primary-nav li ul').not($(this).find('ul:first')).fadeOut(200);
        } else {
            $(this).find('ul:first').hide(); //прячем его
        }
    });

    $('.primary-nav li').find('li').click(
		function () {
		    $(this).parent().fadeOut(200);
		}
	);
}

function stretchVertical() { //заполнение блоками до пределов контейнеров при загрузке, изменении размера и прокрутке
    if ($.cookie('drag1')) {
        $('.side').width($.cookie('drag1') + "px");
    }

    _stretch();
    $(window).resize(function () {
        _stretch();
    })
    $(window).scroll(function () {
        _stretch();
    })
    function _stretch() {
        //$('.side1').height($(window).height() - 97 + getBodyScrollTop());
        if (!isIE7()) {
            $('.content2').height($(window).height() - $('.title').height() - 65);
        } else {
            $('.content2').height($(document).height() - $('.title').height() - 65);
        }
    }
}

function moveEvent(e) {
    var x = 0;

    if (!e) e = window.event;
    if (e.pageX) {
        x = e.pageX;
    }
    else if (e.clientX) {
        x = e.clientX + (document.documentElement.scrollLeft || document.body.scrollLeft) - document.documentElement.clientLeft;
    }
    return { 'x': x }
}
function getBodyScrollTop() {
    return self.pageYOffset || (document.documentElement && document.documentElement.scrollTop) || (document.body && document.body.scrollTop);
}


function isIE7() {
    return (navigator.userAgent.indexOf('MSIE 7.0') != -1)
}

function resizeLeft() {
    var hh = $('.header').height();
    var sc = $(this).scrollTop();
    if (sc < hh) {
        $('.side2').css('margin-top', '0px');
        $('.side2').height($(window).height() - hh + sc);
        
    } else {
        $('.side2').css('margin-top', (sc - hh) + 'px');
        $('.side2').height($(window).height());
    }
    $(".sidebarMenuHolder").height($('.side2').height());
    $(".sidebarMenuHolder").mCustomScrollbar('update');
}
function readyFunction() {
    primaryNav();
    resize();
    stretchVertical();
    $(window).scroll(function(e) {
        resizeLeft();
    });
    $(window).resize(function(e) {
        resizeLeft();
    });
    resizeLeft();
    
/*
    if ($(document).height() - $('.content2').position().top > $('.content2').height() + $('.footer').height()) {
        $('.content2').height($(document).height() - $('.content2').position().top - $('.footer').height());
    }
*/
    
    $.pnotify.defaults.delay -= 4500;
    $.pnotify.defaults.history = false;

    loadAdminChat(true);

    setSidebarHeight();
    mainMenuFunction();
    setBlankWidgets(true);
    jstreeHover();


    // INIT BREADCRUMBS

    $('.xbreadcrumbs').xBreadcrumbs();


    // TOOLTIP HOVER FOR .bootstrap-tooltip ELEMENTS

    $('.bootstrap-tooltip').tooltip();


    // PNOTIFY DEFAULT OPTIONS



    // JSTREE DUMMY THEME (REQUIRED TO AVOID CHANGING THE PLUGIN TO IGNORE THE ORIGINAL CSS SINCE LESS IS USED)
    $.jstree._themes = "/content/photon/css/plugins/jstree/";


    // CUSTOM SCROLLBAR FOR SIDE PANEL
    if ($(".content1").height() > $(".sidebarMenuHolder").height()) {
        $(".sidebarMenuHolder").height($(".content1").height());
    }
    $(".sidebarMenuHolder").mCustomScrollbar({
        mouseWheelPixels: 400,
        scrollButtons: {
            enable: true
        },
        advanced: {
            updateOnBrowserResize: true,
            updateOnContentResize: true
        },
        //height: 900
    });


    $(".sidebarMenuHolderRight").mCustomScrollbar({
        scrollButtons: {
            enable: true
        },
        advanced: {
            updateOnBrowserResize: true,
            updateOnContentResize: true
        },
        //height: 900
    });
    if ($(".chat-message-list").length) {
        $(".chat-message-list").mCustomScrollbar({
            scrollButtons: {
                enable: true
            },
            advanced: {
            },
            //height: 900
        });
        $(".chat-message-list").mCustomScrollbar("scrollTo", "bottom");
    }


    if ($(".chat-message-list").length && $('.chat-btn-send').length) {
        refreshChat();
    }


    function refreshChat() {
        setTimeout(function () {
            var t = $('.chat-btn-send');
            $.post('/Master/ru/Chat/PostMessage', { session: t.attr('session'), host: t.attr('host') }, function (d) {
                var hasNew = false;
                for (var i = 0; i < d.length; i++) {
                    var tmpl = '<li data-id="{id}"><div><b>{date}</b><i>{name}</i>:</div><span>{msg}</span></li>';
                    if (!$('.chat-message-list li[data-id="' + d[i].ID + '"]').length) {
                        hasNew = true;
                        var line = tmpl.replace('{id}', d[i].ID).replace('{msg}', d[i].Message).replace('{date}', d[i].Date).replace('{name}', d[i].Author);
                        $('.chat-message-list .last-line').before(line);

                    } else {
                        $('.chat-message-list li[data-id="' + d[i].ID + '"] span').html(d[i].Message);
                    }
                }
                if (hasNew) {
                    $(".chat-message-list").each(function () {
                        $(this).mCustomScrollbar('update');
                        $(this).mCustomScrollbar("scrollTo", "bottom");
                    });
                }
                refreshChat();

            });
        }, 2000);
    }

    //$(this).mCustomScrollbar('update');


    $('#AnsTemplate').change(function () {
        $('.chat-answer textarea').val($(this).val());
    })

    $('.chat-btn-send').click(function () {
        var t = $(this);
        var ans = $('.chat-answer textarea').val();
        if (ans.length) {
            $.post('/Master/ru/Chat/PostMessage', { session: t.attr('session'), host: t.attr('host'), message: ans }, function (d) {

                for (var i = 0; i < d.length; i++) {
                    var tmpl = '<li data-id="{id}"><div><b>{date}</b><i>{name}</i>:</div><span>{msg}</span></li>';
                    if (!$('.chat-message-list li[data-id="' + d[i].ID + '"]').length) {
                        var line = tmpl.replace('{id}', d[i].ID).replace('{msg}', d[i].Message).replace('{date}', d[i].Date).replace('{name}', d[i].Author);
                        $('.chat-message-list .last-line').before(line);

                    } else {
                        $('.chat-message-list li[data-id="' + d[i].ID + '"] span').html(d[i].Message);
                    }


                    $('.chat-answer textarea').val('');
                }

                $(".chat-message-list").each(function () {
                    $(this).mCustomScrollbar('update');
                    $(this).mCustomScrollbar("scrollTo", "bottom");
                });

            });
        }
        return false;
    });

    // SCROLLBAR SHADOWS

    $(".Jstree_shadow_top").prependTo(".sidebarMenuHolder");
    $(".Jstree_shadow_bottom").appendTo(".sidebarMenuHolder");


    // PANEL STATE CONTROL

    $('.panel-slider-arrow').on('click', function () {
        $('.panel, .main-content').toggleClass('retracted');

        if ($('.panel').is('.retracted')) {
            $.cookie('jsTreeMenu', 'retracted', {
                expires: 7,
                path: '/'
            });
        }
        else {
            $.cookie('jsTreeMenu', 'extended', {
                expires: 7,
                path: '/'
            });
        }

        /*
                if($.cookie('jsTreeMenuNotification')!='true') {
                    $.cookie('jsTreeMenuNotification', 'true', {
                        expires: 7,
                        path: '/'
                    });
                    $.pnotify({
                        title: 'Slide Menu Remembers It\'s State',
                        type: 'info',
                        text: 'Slide menu will remain closed when you browse other pages, until you open it again.'
                    });
                }
        */
    });

    // APPLY THEME COLOR
    if ($.cookie('themeColor') == 'light') {
        $('body').addClass('light-version');
    }
}


// MAIN MENU FUNCTION

function mainMenuFunction() {
    setTimeout(function () {
        $('.nav-fixed-left').removeAttr('style');
    }, 300);
    $('.nav-side-menu > li').on('mouseleave', function () {
        if ($('.nav-fixed-left').css('top') != '0px') return;
        var $subMenu = $(this).find('ul');
        $subMenu.fadeOut(100);
        setTimeout(function () {
            $subMenu.removeAttr('style');
        }, 200);
    });
    $('.nav-side-menu > li').on('click', function () {
        //console.log('test');
        var $touchedMenu = $(this).find('ul');
        var $subMenu = $('.sub-nav ul').not($touchedMenu);
        $('.nav-side-menu > li').not(this).removeClass('nav-active');
        $(this).toggleClass('nav-active');
        if ($('.nav-fixed-left').css('top') == '0px') $subMenu.fadeOut(100);
        setTimeout(function () {
            $subMenu.removeAttr('style');
        }, 200);
    });
    $('.btn-mobile-menus .btn').on('click', function () {
        if ($(this).is('.active-menu')) $('.active-menu').removeClass('active-menu');
        else {
            $('.active-menu').removeClass('active-menu');
            $(this).addClass('active-menu');
        }
        if ($('.btn-main-menu').is('.active-menu')) {
            $('.mobMMExpanded').removeClass('mobMMExpanded');
            $('.nav-fixed-left').addClass('mobMMExpanded');
        }
        else if ($('.btn-user-menu').is('.active-menu')) {
            $('.mobMMExpanded').removeClass('mobMMExpanded');
            $('.nav-fixed-topright').addClass('mobMMExpanded');
        }
        else $('.mobMMExpanded').removeClass('mobMMExpanded');
    });
}


// SETS SIDEBAR HEIGHT BASED ON TOTAL WINDOW HEIGHT

function setSidebarHeight() {
    if ($('.panel-slider-center .panel-slider-arrow').css('position') != 'absolute') {
        $win_hei = $(window).height();
        $(".sidebarMenuHolder").height($win_hei - 185);
    }
    else {
        $(".sidebarMenuHolder").height(300);
    }
}


// BALANCE NUMBER OF DASHBOARD WIDGETS

var lastWidgetPerRow;
function setBlankWidgets(onReady) {
    if (!$('body').is('.body-dashboard')) return;
    var widgetPerRow;

    widgetPerRow = parseInt($('.dashboard-widget-group').css('background-position'), 10);
    if (widgetPerRow == lastWidgetPerRow) return;

    $('.blank-widget').remove();
    var currentWidgetCount = $('.widget-holder').length;
    var finalWidgetCount = Math.ceil(currentWidgetCount / widgetPerRow) * widgetPerRow;

    for (var i = finalWidgetCount - currentWidgetCount; i > 0; i--) {
        $.get('./library/widgets/blank-widget.php', createBlankWidget);
    }
    lastWidgetPerRow = widgetPerRow;
}


// GENERATES BLANK WIDGET

function createBlankWidget(data) {
    var html = $(data).appendTo('#photon_widgets');
}


// RESIZABLE TEXT FIELDS FUNCTION

function autoGrowField(f, max) {
    /* Default max height */
    max = (typeof max == 'undefined') ? 1000 : max;
    /* Don't let it grow over the max height */
    if (f.scrollHeight > max) {
        /* Add the scrollbar back and bail */
        if (f.style.overflowY != 'scroll') {
            f.style.overflowY = 'scroll';
        }
        return;
    }
    /* Make sure element does not have scroll bar to prevent jumpy-ness */
    if (f.style.overflowY != 'hidden') {
        f.style.overflowY = 'hidden';
    }
    /* Now adjust the height */
    var scrollH = f.scrollHeight;
    // console.log(scrollH);
    if (scrollH > f.style.height.replace(/[^0-9]/g, '')) {
        f.style.height = scrollH + 20 + 'px';
    }
}

function jstreeHover() {
    $('.overflowing').removeClass('overflowing');
    if ($('.panel-slider-center .panel-slider-arrow').css('position') != 'absolute') {
        setTimeout(function () {
            $('.jstree li').each(function () {
                if (isTextOverflowing($(this))) {
                    var title = $('>a', this).text();
                    $(this).tooltip({
                        title: title,
                        placement: 'right',
                        container: '.panel'
                    });
                }
            });
        }, 500);
    }
}

function showColorChanger() {

}

// CHECK IF JSTREE LINK TEXT IS OVERFLOWING (NOTE THAT CLONE AND ORIGINAL ELEMENT MUST HAVE THE SAME TYPOGRAFY FOR THIS TO WORK, USE CSS)

function isTextOverflowing($elem) {
    returnVal = false;
    if ($elem.get(0).offsetWidth < $elem.get(0).scrollWidth) {
        returnVal = true;
        $elem.addClass('overflowing');
    }
    return returnVal;
}


// ON RESIZE END FUNCTION

var isResizing;
function resizeEnd() {
    clearTimeout(isResizing);
    isResizing = setTimeout(function () {
        // RESIZE END LOGIC BELOW:
        jstreeHover();
    }, 300);
}

// FLIP WIDGET

function flipit(elem) {
    $(elem).parents('.widget-holder').toggleClass('flip-it');
}