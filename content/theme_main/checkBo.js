(function (b, s, t, u) {
    b.fn.checkBo = function (c) {
        c = b.extend({}, {
            checkAllButton: null,
            checkAllTarget: null,
            checkAllTextDefault: null,
            checkAllTextToggle: null
        }, c);
        return this.each(function () {
            function f(a) {
                this.input = a
            }
            function k() {
                var a = b(this).is(":checked");
                b(this).closest("label").toggleClass("checked", a)
            }
            function l(a, b, c) {
                a.parent(g).hasClass("checked") ? a.text(c) : a.text(b)
            }
            function m(a) {
                var c = a.attr("data-show");
                a = a.attr("data-hide");
                b(c).removeClass("is-hidden");
                b(a).addClass("is-hidden")
            }
            var e = b(this),
                g = e.find(".cb-checkbox"),
                h = e.find(".cb-radio"),
                n = g.find("input:checkbox"),
                p = h.find("input:radio");
            n.wrap('<span class="cb-inner"><i></i></span>');
            p.wrap('<span class="cb-inner"><i></i></span>');
            var q = new f("input:checkbox"),
                r = new f("input:radio");
            f.prototype.checkbox = function (a) {
                var b = a.find(this.input).is(":checked");
                a.find(this.input).prop("checked", !b).trigger("change")
            };
            f.prototype.radiobtn = function (a, c) {
                var d = b('input:radio[name="' + c + '"]');
                d.prop("checked", !1);
                d.closest(d.closest(h)).removeClass("checked");
                a.addClass("checked");
                a.find(this.input).get(0).checked = a.hasClass("checked");
                a.find(this.input).change()
            };
            n.on("change", k);
            p.on("change", k);
            g.on("click", function (a) {
                a.preventDefault();
                q.checkbox(b(this));
                a = b(this).attr("data-toggle");
                b(a).toggleClass("is-hidden");
                m(b(this))
            });
            h.on("click", function (a) {
                a.preventDefault();
                r.radiobtn(b(this), b(this).find("input:radio").attr("name"));
                m(b(this))
            });
            if (c.checkAllButton && c.checkAllTarget) {
                var d = b(this);
                d.find(b(c.checkAllButton)).on("click", function () {
                    d.find(c.checkAllTarget).find("input:checkbox").each(function () {
                        d.find(b(c.checkAllButton)).hasClass("checked") ? d.find(c.checkAllTarget).find("input:checkbox").prop("checked", !0).change() : d.find(c.checkAllTarget).find("input:checkbox").prop("checked", !1).change();
                    });
                    l(d.find(b(c.checkAllButton)).find(".toggle-text"), c.checkAllTextDefault, c.checkAllTextToggle)
                });
                d.find(c.checkAllTarget).find(g).on("click", function () {
                    d.find(c.checkAllButton).find("input:checkbox").prop("checked", !1).change().removeClass("checked");
                    l(d.find(b(c.checkAllButton)).find(".toggle-text"), c.checkAllTextDefault, c.checkAllTextToggle)
                })
            }
            e.find('[checked="checked"]').closest("label").addClass("checked");
            e.find("input").is("input:disabled") && e.find("input:disabled").closest("label").off().addClass("disabled")
        })
    }
})(jQuery, window, document);

$(document).ready(function () {
    if ($(window).width() < '784') {
        $('.container_12 .main_navigate').hide();
        $('#navigation .menu').hide();
    }
    $(document).ready(function () { //новый дизайн шапки
        $('.sfy').prependTo('#navigation');
    });
    $.getScript('https://use.fontawesome.com/releases/v5.0.8/js/all.js');
    //иконка "на главную"
    $('#navigation').prepend('<div class="firstHomeIconNav"><div title="На главную страницу" class="homeIconNav"><a class="homeIconA" href="/"></a></div></div>');
    $('#header').append('<div class="phoneMail"><div class="headPhone"><span id="firstNumbP">+7(495)</span><span id="secondNumbP">120-08-22</span></div><div class="headMail thisMcopy"><span id="MailP1">info@boiler-gas.ru</span></div></div>');
    $('#user-menu').append('<svg class="svg-inline--fa fa-user fa-w-16" aria-hidden="true" data-prefix="fas" data-icon="user" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" data-fa-i2svg=""><path fill="currentColor" d="M256 0c88.366 0 160 71.634 160 160s-71.634 160-160 160S96 248.366 96 160 167.634 0 256 0zm183.283 333.821l-71.313-17.828c-74.923 53.89-165.738 41.864-223.94 0l-71.313 17.828C29.981 344.505 0 382.903 0 426.955V464c0 26.51 21.49 48 48 48h416c26.51 0 48-21.49 48-48v-37.045c0-44.052-29.981-82.45-72.717-93.134z"></path></svg>');
    $('#header #cart-block .cart-link').append('<i class="fas fa-shopping-cart"></i>');
    $('#header').append('<div class="bigNameG"><span id="bigNameL">BOILER-GAS.RU</span><span id="bigNameS">Только из Европы и России</span></div><div class="bigTimeO"><div id="bigTimeIc"></div><div id="bigTimeT">Контактный центр работает круглосуточно</div><div>');
    $('.el_form.btn.bradius_c-l', this).attr('id', 'bchange');
    $('#bchange').css('display', 'none');
    $('.el_form.bradius_c-r.text.blur-box.ui-autocomplete-input').attr('def', 'Название или код товара');
    if ($('#user-menu li a').text() != ('Вход' && 'Регистрация')) {
        $('#user-menu').css('color', '#ff7901');
        $('.cart-link').addClass('fChahgeCount');
    }
    if ($('#header #user-menu li:first-child a').text() == 'Регистрация') {
        $('#user-menu').addClass('newP');
        $('.svg-inline--fa.fa-user.fa-w-16').wrap('<a href="#nogo" id="reg-link" class="regIcon" onclick="showRegPopup();"></a>')
    }
    else {
        $('.svg-inline--fa.fa-user.fa-w-16').wrap('<a href="/cabinet?view=profile"></a>')
    }
    if ($('#header #user-menu li:first-child a').text() != 'Регистрация') {
        $('#header #user-menu li:first-child').css('margin-bottom', '15px');
        x = document.location.href;
        if (x != 'https://boiler-gas.ru/cabinet?view=profile') {
            $('#header #user-menu li:first-child').css({ 'margin-bottom': '-3px' })
        }
        $('svg.svg-inline--fa.fa-user.fa-w-16').css({ 'left': '7px', 'top': '13px' });
    }
    $.getScript('https://boiler-gas.ru/content/theme_main/js/JS_calc_boiler-gas.js');//подключение скрипта калькулятора
    $.getScript('https://boiler-gas.ru/content/theme_main/js/hot_floor_calc.js');
    $.getScript('https://boiler-gas.ru/content/theme_main/js/jquery-ui-1.10.4.custom.js');
    try {
        var s = $('.signature').text();
        s = s.replace('Артикул: ', '');
        //v = $(s).text();
        $('.goods_item_properties').children().prepend('<div style="border-bottom: none;" class="char-odd"><span class="mySpanFirst firs1" style="color: rgb(135, 135, 135);">Артикул</span><span class="mySpanX mySpanFirst" style="color: rgb(135, 135, 135);">' + s + '</span></div>');
    }
    catch (err) {
        return;
    }
    $('meta:eq(0)').attr('name', 'viewport');
    $('meta:eq(0)').attr('content', 'width=device-width, initial-scale=1');
    $('body').find('strong').css('font-weight', 'bold');
    var scount = $('body').find('.shcount').text();
    var scou = $('body').find('.shcount');
    for (var n = 0; n < scount.length; n++) {
        $(scou[n]).text(''+ $(scou[n]).text().replace('[', '') +'');
        var gg = $(scou[n]).text();
        $(scou[n]).text(''+ $(scou[n]).text().replace(']', '') +'');
    }

});


$(function () {
    $('body').on('mouseover', '.cat-item', function (event) {
        $(this).find('span').css({ 'background': 'rgba(242, 122, 36, 0.9)'});
        $(this).find('span').addClass('hvr');
        $(this).find('.size100, a, span, img').addClass('hvvr');
    });
    $('body').on('mouseout', '.cat-item', function (event) {
        $(this).find('span').css({'background': 'rgba(242,122,36,.6)', 'margin-left': '0px'});
        $(this).find('span').removeClass('hvr');
        $(this).find('.size100, a, span, img').removeClass('hvvr');
    });
    $('body').on('mouseover', '.firstHomeIconNav', function (event) {
        $(this).find('.homeIconNav').css('background-position', '-56px -80px');
    });
    $('body').on('mouseout', '.firstHomeIconNav', function (event) {
        $(this).find('.homeIconNav').css('background-position', '-38px -80px');
    });
    /*$('body').on('mouseover', '.category', function (event) {
        $(this).find('.close').css('transform', 'scale(1.5) translate(-2px)');
    });*/
    $('body').on('mouseout', '.category', function (event) {
        $(this).find('.close').css('transform', 'none');
    });
});

$(function () {
    $('.goods_sort').on('click', 'a.act', function (event) {
        $('body').find('.sort-block').css('background-color', '#dfdfdf');
        $(this).css('background-color', '#59BBE5');

    });
});

$(function () {
    $('body').on('mouseover', '.char-odd', function (event) {
        $(this).find('span').css({ 'color': '#FFFFFF !important' });
        $(this).find('a').css({ 'color': '#FFFFFF !important' });
    });
    $('body').on('mouseleave', '.char-odd', function (event) {
        $(this).find('span').css({ 'color': '#878787' });
        $(this).find('a').css({ 'color': '#1686b6' });
    });
    $('body').on('mouseover', '.char-even', function (event) {
        $(this).find('span').css({ 'color': '#FFFFFF !important' });
        $(this).find('a').css({ 'color': '#FFFFFF !important' });
    });
    $('body').on('mouseleave', '.char-even', function (event) {
        $(this).find('span').css({ 'color': '#878787' });
        $(this).find('a').css({ 'color': '#1686b6' });
    });
});

$(document).ready(function () {
    $('body').find('.char-odd>a').css({ 'color': '#1686b6' });
    $('body').find('.char-even>a').css({ 'color': '#1686b6' });
    $('html').find('head').append('<link href="https://fonts.googleapis.com/css?family=Open+Sans" rel="stylesheet">');
    $('.ui-autocomplete.ui-front.ui-menu.ui-widget.ui-widget-content.ui-corner-all').prependTo('article');
});

$(document).ready(function () {
    var tttx1 = '\u0414\u043b\u044f\u0020\u0441\u0440\u0430\u0432\u043d\u0435\u043d\u0438\u044f';
    $('.compare-head').text(tttx1);
    $('p').css({ 'font-size': '16px !important', 'color': '#4A4A4A !important', 'font-family': 'Segoe UI !important' });
});

$(function () {
    var chekt = 0;
    $('body').on('mousedown', '.el_form-search>.el_form.btn.bradius_c-l', function (event) {
        $(this).css({ 'box-shadow': 'inset 1px 2px 4px 2px #186d0e' });
    });
    $('body').on('mouseup', '.el_form-search>.el_form.btn.bradius_c-l', function (event) {
        $(this).css({ 'box-shadow': '0px 1.5px 0px #186d0e' });
    });
    $('body').on('mouseleave', '.el_form-search>.el_form.btn.bradius_c-l', function (event) {
        $(this).css({ 'box-shadow': '0px 1.5px 0px #186d0e' });
    });
    $('body').find('#SearchQuery').focus(function () {
        if (chekt == 0) {
            $('.ui-autocomplete.ui-front.ui-menu.ui-widget.ui-widget-content.ui-corner-all').prependTo('article');
            chekt = 1;
        }
        $(this).attr('placeholder', '');
        $(this).css({ 'width': '248px !important;' });
        if ($(this).parent().parent().parent().hasClass('sfy')) {
            $('.ui-autocomplete.ui-front.ui-menu.ui-widget.ui-widget-content.ui-corner-all').removeClass('fixedAutocomplete');
        }
    });
    $('body').find('#SearchQuery').focus(function () {
        b = $('article').children();
        setTimeout(function () {
            $(b[0]).css({ 'display': 'block' })
        }, 1000);
    });
    $('body').find('#SearchQuery').blur(function () {
        b = $('article').children();
        setTimeout(function () {
            $(b[0]).css({ 'display': 'none' })
        }, 1000);
        setTimeout(function () {
            $('b').removeClass('thisit');
        }, 1000);
    });
});



$(document).ready(function () {
    var tttx = '\u041d\u0430\u0439\u0442\u0438';
    $('.el_form-search').find('.el_form.btn.bradius_c-l').text(tttx);

    var tttx2 = '\u0412 \u043a\u043e\u0440\u0437\u0438\u043d\u0443';
    $('body').find('.el_form.btn.btn_green.btn_toBasket').text(tttx2);


    var tttx3 = '\u0412 \u043a\u043e\u0440\u0437\u0438\u043d\u0435';
    $('body').find('.el_form.btn.btn_orange.btn_toBasket').text(tttx3);

    //$('body').find('.char-odd span, .char-even span').append("<p></p>");
});


$(document).ready(function () {
    $('body').append('<a href="#" title="ВВЕРХ" id="go-top"><i class="fas fa-angle-double-up"></i></a>');
    $('#header #user-menu li .go_enter_byerd :not(script)').contents().filter(function () {
        return this.nodeType === 3;
    }).replaceWith(function () {
        return this.nodeValue.replace('Выход', 'выход');
    });
    if (window.location.href != 'https://boiler-gas.ru/cabinet?view=profile') {
        var x = $('#header #user-menu li .go_enter_byerd').text();
        if (x == 'Выход') {
            $('body').find('.go_enter_byerd').css({ 'display': 'none' });
        }
        else {
            $('body').find('.go_enter_byerd').css({ 'display': 'block' });
        }
    }
});




$(function () {
    $.fn.scrollToTop = function () {
        $(this).hide().removeAttr("href");
        if ($(window).scrollTop() >= "250") $(this).fadeIn("slow")
        var scrollDiv = $(this);
        $(window).scroll(function () {
            if ($(window).scrollTop() <= "250") $(scrollDiv).fadeOut("slow")
            else $(scrollDiv).fadeIn("slow")
        });
        $(this).click(function () {
            $("html, body").animate({ scrollTop: 0 }, "slow")
        })
    }
});

$(function () {
    $("#go-top").scrollToTop();
});


$(document).ready(function () {//перемещение блока "состав поставки"
    $('.element_tabs_wrapper.item_other_links').before('<div id="divTabs-product"></div>');
    $('#tabs-product').prependTo('#divTabs-product');
    $('.text-truncate.seoTextOpenButton.stTruncate.stTruncate-close div:first-child').css('border-top', 'none');
    $('.goods_view_item.clearfix h5').addClass('charA');
    $('.goods_view_item.clearfix h5').after('<h5 id="sPostavki">Состав поставки</h5>');
    $('#tabs-product .ui-tabs-nav li:last-child ').css('display', 'none');
    $('#tabs-2').css('display', 'none');
    $('#tabs-2').prependTo('.side_right .goods_item_properties');
    $('#sPostavki').click(function () {
        $(this).addClass('actPo');
        $('.charA').addClass('dontActPo');
        $('.text-truncate.seoTextOpenButton.stTruncate.stTruncate-close').css('display', 'none');
        $('.side_right .myMainImg.ui-state-default.ui-corner-top').css('display', 'block');
        $('.main_content .goods_item_properties').css('background', 'none');
        $('.main_content .goods_item_properties').children().css('display', 'none');
        $('.goods_item_properties #tabs-2').css('display', 'block');
    });
    $('.charA').click(function () {
        $('#sPostavki').removeClass('actPo');
        $('.charA').removeClass('dontActPo');
        $('.text-truncate.seoTextOpenButton.stTruncate.stTruncate-close').css('display', 'block');
        $('.side_right .myMainImg.ui-state-default.ui-corner-top').css('display', 'none');
        $('.main_content .goods_item_properties').css('background', 'linear-gradient(to right, #dbe5f1 54%, white 50%)');
        $('.main_content .goods_item_properties').children().css('display', 'block');
        $('.goods_item_properties #tabs-2').css('display', 'none');
    });
    $('body').on('click', '.sort-block', function (event) {
        $(this).css({ 'background-color': '#cceddc' });
    });
    if ($('#tabs-2 p').text() != "Состав поставки" || $('#tabs-2 p').text() != "") {
        setTimeout(function () {
            var x = $('#sPostavki');
            x.detach();
        }, 500);
    }
});


$(document).ready(function () {
    $('#header').append('<div class="diChangesity"><div class="changesity"><div class="icon_change_sity"></div><span class="p_change_sity"><p>Ваш регион</p></span></div></div>');
    $('#header').append('<div class="changesity_tooltip"><div class="changesity-tooltip__tail"></div><div class="changesity-tooltip__close"></div><div class="changesity-tooltip__content"><div class="changesity-no-modal changesity-no-modal_location"><div class="changesity-no-modal__wrap"><div class="changesity-no-modal__head">\u0412\u044b\u0431\u0435\u0440\u0438\u0442\u0435 \u0441\u0432\u043e\u0439 \u0440\u0435\u0433\u0438\u043e\u043d</div><ul class="changesity-city-list changesity-city-list_geo mCustomScrollbar _mCS_1"><div class="changesitymCustomScrollBox mCS-3d-blue mCSB_vertical mCSB_inside" tabindex="0"><div class="changesity_mCSB_container" style="position:relative; top:0; left:0;" dir="ltr"><li data-location-id="2097" class="changesity-city-list__item b-city-list__item_success"><span>\u041c\u043e\u0441\u043a\u0432\u0430</span></li><li data-location-id="2287" class="changesity-city-list__item"><span>СПБ</span></li><li data-location-id="1427" class="changesity-city-list__item"><span>\u041a\u0440\u0430\u0441\u043d\u043e\u0434\u0430\u0440</span></li><li data-location-id="1283" class="changesity-city-list__item"><span>\u041a\u0430\u0437\u0430\u043d\u044c</span></li><li data-location-id="676" class="changesity-city-list__item"><span>\u0410\u0431\u0430\u043a\u0430\u043d</span></li><li data-location-id="873" class="changesity-city-list__item"><span>\u0410\u043d\u0430\u0434\u044b\u0440\u044c</span></li><li data-location-id="764" class="changesity-city-list__item"><span>\u0410\u0440\u0445\u0430\u043d\u0433\u0435\u043b\u044c\u0441\u043a</span></li><li data-location-id="982" class="changesity-city-list__item"><span>\u0410\u0441\u0442\u0440\u0430\u0445\u0430\u043d\u044c</span></li><li data-location-id="693" class="changesity-city-list__item"><span>\u0411\u0430\u0440\u043d\u0430\u0443\u043b</span></li><li data-location-id="1064" class="changesity-city-list__item"><span>\u0411\u0435\u043b\u0433\u043e\u0440\u043e\u0434</span></li><li data-location-id="800" class="changesity-city-list__item"><span>\u0411\u0438\u0440\u043e\u0431\u0438\u0434\u0436\u0430\u043d</span></li><li data-location-id="786" class="changesity-city-list__item"><span>\u0411\u043b\u0430\u0433\u043e\u0432\u0435\u0449\u0435\u043d\u0441\u043a</span></li><li data-location-id="780" class="changesity-city-list__item"><span>\u0411\u0440\u044f\u043d\u0441\u043a</span></li><li data-location-id="1069" class="changesity-city-list__item"><span>Вел. Новгород</span></li><li data-location-id="794" class="changesity-city-list__item"><span>\u0412\u043b\u0430\u0434\u0438\u0432\u043e\u0441\u0442\u043e\u043a</span></li><li data-location-id="795" class="changesity-city-list__item"><span>\u0412\u043b\u0430\u0434\u0438\u043a\u0430\u0432\u043a\u0430\u0437</span></li><li data-location-id="796" class="changesity-city-list__item"><span>\u0412\u043b\u0430\u0434\u0438\u043c\u0438\u0440</span></li><li data-location-id="908" class="changesity-city-list__item"><span>\u0412\u043e\u043b\u0433\u043e\u0433\u0440\u0430\u0434</span></li><li data-location-id="917" class="changesity-city-list__item"><span>\u0412\u043e\u043b\u043e\u0433\u0434\u0430</span></li><li data-location-id="889" class="changesity-city-list__item"><span>\u0412\u043e\u0440\u043e\u043d\u0435\u0436</span></li><li data-location-id="887" class="changesity-city-list__item"><span>\u0413\u043e\u0440\u043d\u043e-\u0410\u043b\u0442\u0430\u0439\u0441\u043a</span></li><li data-location-id="756" class="changesity-city-list__item"><span>\u0413\u0440\u043e\u0437\u043d\u044b\u0439</span></li><li data-location-id="2732" class="changesity-city-list__item"><span>\u0415\u043a\u0430\u0442\u0435\u0440\u0438\u043d\u0431\u0443\u0440\u0433</span></li><li data-location-id="1277" class="changesity-city-list__item"><span>\u0418\u0432\u0430\u043d\u043e\u0432\u043e</span></li><li data-location-id="1721" class="changesity-city-list__item"><span>\u0418\u0436\u0435\u0432\u0441\u043a</span></li><li data-location-id="1442" class="changesity-city-list__item"><span>\u0418\u0440\u043a\u0443\u0442\u0441\u043a</span></li><li data-location-id="1605" class="changesity-city-list__item"><span>\u0419\u043e\u0448\u043a\u0430\u0440-\u041e\u043b\u0430</span></li><li data-location-id="1316" class="changesity-city-list__item"><span>\u041a\u0430\u043b\u0438\u043d\u0438\u043d\u0433\u0440\u0430\u0434</span></li><li data-location-id="1325" class="changesity-city-list__item"><span>\u041a\u0430\u043b\u0443\u0433\u0430</span></li><li data-location-id="1706" class="changesity-city-list__item"><span>\u041a\u0435\u043c\u0435\u0440\u043e\u0432\u043e</span></li><li data-location-id="1463" class="changesity-city-list__item"><span>\u041a\u0438\u0440\u043e\u0432</span></li><li data-location-id="1587" class="changesity-city-list__item"><span>\u041a\u043e\u0441\u0442\u0440\u043e\u043c\u0430</span></li><li data-location-id="1428" class="changesity-city-list__item"><span>\u041a\u0440\u0430\u0441\u043d\u043e\u044f\u0440\u0441\u043a</span></li><li data-location-id="1635" class="changesity-city-list__item"><span>\u041a\u0443\u0440\u0433\u0430\u043d</span></li><li data-location-id="1643" class="changesity-city-list__item"><span>\u041a\u0443\u0440\u0441\u043a</span></li><li data-location-id="1678" class="changesity-city-list__item"><span>\u041a\u044b\u0437\u044b\u043b</span></li><li data-location-id="1499" class="changesity-city-list__item"><span>\u041b\u0438\u043f\u0435\u0446\u043a</span></li><li data-location-id="1737" class="changesity-city-list__item"><span>\u041c\u0430\u0433\u0430\u0434\u0430\u043d</span></li><li data-location-id="1792" class="changesity-city-list__item"><span>\u041c\u0430\u0439\u043a\u043e\u043f</span></li><li data-location-id="1827" class="changesity-city-list__item"><span>\u041c\u0430\u0445\u0430\u0447\u043a\u0430\u043b\u0430</span></li><li data-location-id="2148" class="changesity-city-list__item"><span>\u041c\u0443\u0440\u043c\u0430\u043d\u0441\u043a</span></li><li data-location-id="1753" class="changesity-city-list__item"><span>\u041d\u0430\u0437\u0440\u0430\u043d\u044c</span></li><li data-location-id="1808" class="changesity-city-list__item"><span>\u041d\u0430\u043b\u044c\u0447\u0438\u043a</span></li><li data-location-id="1956" class="changesity-city-list__item"><span>Ниж. Новгород</span></li><li data-location-id="2012" class="changesity-city-list__item"><span>\u041d\u043e\u0432\u043e\u0441\u0438\u0431\u0438\u0440\u0441\u043a</span></li><li data-location-id="2096" class="changesity-city-list__item"><span>\u041e\u043c\u0441\u043a</span></li><li data-location-id="1878" class="changesity-city-list__item"><span>\u041e\u0440\u0435\u043b</span></li><li data-location-id="1881" class="changesity-city-list__item"><span>\u041e\u0440\u0435\u043d\u0431\u0443\u0440\u0433</span></li><li data-location-id="2211" class="changesity-city-list__item"><span>\u041f\u0435\u043d\u0437\u0430</span></li><li data-location-id="2190" class="changesity-city-list__item"><span>\u041f\u0435\u0440\u043c\u044c</span></li><li data-location-id="2230" class="changesity-city-list__item"><span>\u041f\u0435\u0442\u0440\u043e\u0437\u0430\u0432\u043e\u0434\u0441\u043a</span></li><li data-location-id="2233" class="changesity-city-list__item"><span>\u041f\u0435\u0442\u0440\u043e\u043f\u0430\u0432\u043b\u043e\u0432\u0441\u043a-\u041a\u0430\u043c\u0447\u0430\u0442\u0441\u043a\u0438\u0439</span></li><li data-location-id="2130" class="changesity-city-list__item"><span>\u041f\u0441\u043a\u043e\u0432</span></li><li data-location-id="1235" class="changesity-city-list__item"><span>\u0420\u043e\u0441\u0442\u043e\u0432-\u043d\u0430-\u0414\u043e\u043d\u0443</span></li><li data-location-id="1268" class="changesity-city-list__item"><span>\u0420\u044f\u0437\u0430\u043d\u044c</span></li><li data-location-id="2282" class="changesity-city-list__item"><span>\u0421\u0430\u043b\u0435\u0445\u0430\u0440\u0434</span></li><li data-location-id="2284" class="changesity-city-list__item"><span>\u0421\u0430\u043c\u0430\u0440\u0430</span></li><li data-location-id="2272" class="changesity-city-list__item"><span>\u0421\u0430\u0440\u0430\u043d\u0441\u043a</span></li><li data-location-id="2275" class="changesity-city-list__item"><span>\u0421\u0430\u0440\u0430\u0442\u043e\u0432</span></li><li data-location-id="491" class="changesity-city-list__item"><span>\u0421\u0438\u043c\u0444\u0435\u0440\u043e\u043f\u043e\u043b\u044c</span></li><li data-location-id="2365" class="changesity-city-list__item"><span>\u0421\u043c\u043e\u043b\u0435\u043d\u0441\u043a</span></li><li data-location-id="2378" class="changesity-city-list__item"><span>\u0421\u0442\u0430\u0432\u0440\u043e\u043f\u043e\u043b\u044c</span></li><li data-location-id="2428" class="changesity-city-list__item"><span>\u0421\u044b\u043a\u0442\u044b\u0432\u043a\u0430\u0440</span></li><li data-location-id="2509" class="changesity-city-list__item"><span>\u0422\u0430\u043c\u0431\u043e\u0432</span></li><li data-location-id="2519" class="changesity-city-list__item"><span>\u0422\u0432\u0435\u0440\u044c</span></li><li data-location-id="2601" class="changesity-city-list__item"><span>\u0422\u043e\u043c\u0441\u043a</span></li><li data-location-id="2656" class="changesity-city-list__item"><span>\u0422\u0443\u043b\u0430</span></li><li data-location-id="2695" class="changesity-city-list__item"><span>\u0422\u044e\u043c\u0435\u043d\u044c</span></li><li data-location-id="2551" class="changesity-city-list__item"><span>\u0423\u043b\u0430\u043d-\u0423\u0434\u044d</span></li><li data-location-id="2571" class="changesity-city-list__item"><span>\u0423\u043b\u044c\u044f\u043d\u043e\u0432\u0441\u043a</span></li><li data-location-id="2644" class="changesity-city-list__item"><span>\u0423\u0444\u0430</span></li><li data-location-id="2471" class="changesity-city-list__item"><span>\u0425\u0430\u0431\u0430\u0440\u043e\u0432\u0441\u043a</span></li><li data-location-id="2511" class="changesity-city-list__item"><span>\u0425\u0430\u043d\u0442\u044b-\u041c\u0430\u043d\u0441\u0438\u0439\u0441\u043a</span></li><li data-location-id="2883" class="changesity-city-list__item"><span>\u0427\u0435\u0431\u043e\u043a\u0441\u0430\u0440\u044b</span></li><li data-location-id="2910" class="changesity-city-list__item"><span>\u0427\u0435\u043b\u044f\u0431\u0438\u043d\u0441\u043a</span></li><li data-location-id="2887" class="changesity-city-list__item"><span>\u0427\u0435\u0440\u043a\u0435\u0441\u0441\u043a</span></li><li data-location-id="2870" class="changesity-city-list__item"><span>\u0427\u0438\u0442\u0430</span></li><li data-location-id="2711" class="changesity-city-list__item"><span>\u042d\u043b\u0438\u0441\u0442\u0430</span></li><li data-location-id="2974" class="changesity-city-list__item"><span>\u042e\u0436\u043d\u043e-\u0421\u0430\u0445\u0430\u043b\u0438\u043d\u0441\u043a</span></li><li data-location-id="2994" class="changesity-city-list__item"><span>\u042f\u043a\u0443\u0442\u0441\u043a</span></li><li data-location-id="2988" class="changesity-city-list__item"><span>\u042f\u0440\u043e\u0441\u043b\u0430\u0432\u043b\u044c</span></li></div></div></div></div><input type="text" class="form-control pull-right" id= "search" placeholder= "\u041f\u043e\u0438\u0441\u043a \u0433\u043e\u0440\u043e\u0434\u043e\u0432:"></div>');
    var sityv = $('.p_change_sity').find('p');
    var i = localStorage.SityCng;
    sityv.text(i);
});


$(function () {
    $('body').on('click', '.changesity', function (event) {
        $('.changesity_tooltip').css({ 'display': 'block', '': '' });
    });

});

$(function () {
    $('body').on('click', '.changesity-tooltip__close', function (event) {
        var nulltext = '';
        $('body').find('.form-control').val(nulltext);
        $('.changesity_tooltip').css({ 'display': 'none', '': '' });
        $.each($('.changesity-city-list__item'), function () {
            $(this).show();
        });
    });
});


$(function () {
    $('body').on('click', '.changesity-city-list__item', function (event) {
        var sityC = $(this).text();
        localStorage.SityCng = sityC;
        $('.p_change_sity').find('p').text($(this).find('span').text());
        $('.changesity-city-list__item').css({ 'background': 'white', 'color': '#0081b5' });
        $(this).css({ 'background': 'hsla(199, 72%, 88%, 1)', 'color': 'white' });
        $('.changesity_tooltip').css({ 'display': 'none', '': '' });

    });
});

$(document).mouseup(function (e) {
    var container = $('.changesity_tooltip');
    if (container.has(e.target).length === 0) {
        var nulltext = '';
        $('body').find('.form-control').val(nulltext);
        container.hide();
        $.each($('.changesity-city-list__item'), function () {
            $(this).show();
        });
    }
});


$(document).ready(function () {
    $('.form-control').keyup(function () {
        _this = this;

        $.each($('.changesity-city-list__item'), function () {
            if ($(this).text().toLowerCase().indexOf($(_this).val().toLowerCase()) === -1) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    });
});



$(document).ready(function () {
    $('.goods_view_box').find('.el_form.btn.btn_orange.btn_toBasket').css({ 'padding': '0em 0em 0em', 'font-size': '12px', 'width': '88px' });
    $('.goods_view_row').find('.el_form.btn.btn_orange.btn_toBasket').css({ 'top': '-21px;', 'font-size': '14px', 'width': '85px', 'margin-top': '17px' });
});


$(document).ready(function () {
    $('.irs-slider.to').css({ 'left': '95.6%' });
    $('.irs-slider.to:hover').css({ 'left': '95.6%' });
    var ref13 = $('body').find('.goods_pic');
    (ref13).css({ 'top': '0px', '': '' });
});



$(document).ready(function () {
    $('body').find('.filter-cbx-group').parent('.category-filter-col').css('width', '18%');
    $('.b-breadcrumb :not(script)').contents().filter(function () {
        return this.nodeType === 3;
    }).replaceWith(function () {
        return this.nodeValue.replace('/', '>');
    });
});

$(function () {
    $('body').on('mousedown', '.ye_result', function (event) {
        $(this).css({ 'box-shadow': 'inset 1px 2px 4px 2px #186d0e' });
    });
    $('body').on('mouseup', '.ye_result', function (event) {
        $(this).css({ 'box-shadow': '0px 1.5px 0px #186d0e' });
    });
    $('body').on('mouseleave', '.ye_result', function (event) {
        $(this).css({ 'box-shadow': '0px 1.5px 0px #186d0e' });
    });

});

function cngOrangeBut2() {
    var x1 = $('body').find('.btn_orange');
    var x2 = x1.parent().parent('.goods_view_row');
    var y1 = x2.children('.goods_form');
    y1.css('top', '15px');
};

function cngOrangeBut() {
    var x1 = $('body').find('.btn_orange');
    var x2 = x1.parent().parent('.goods_view_box');
    var y1 = x2.children('.goods_form');
    y1.css('top', '50px');
};

function cngGreenBut() {
    var x1 = $('body').find('.btn_green');
    var x2 = x1.parent().parent('.goods_view_row');
    var y1 = x2.children('.goods_form').find('.price');
    var y2 = x2.children('.goods_form').find('.el_form-quantity');
    y1.css('margin-top', '17px');
    y2.css('margin-top', '44px');
};

function dobKsr() {
    $('body :not(script)').contents().filter(function () {
        return this.nodeType === 3;
    }).replaceWith(function () {
        return this.nodeValue.replace('сравнить', 'Добавить к сравнению');
    });
}

function sravn() {
    $('body :not(script)').contents().filter(function () {
        return this.nodeType === 3;
    }).replaceWith(function () {
        return this.nodeValue.replace('Добавить к сравнению', 'сравнить');
    });
}

$(document).ready(function () { //замена текста кнопки
    $('.ye_result :not(script)').contents().filter(function () {
        return this.nodeType === 3;
    }).replaceWith(function () {
        return this.nodeValue.replace('Выбрано: ', 'Найдено: ');
    });
});

function newView1() {
    localStorage.setCng2 = "0";
    var ref18 = $('body').find('.Cng0');
    (ref18).removeClass('activeB');
    localStorage.setCng0 = "1";
    var ref19 = $('body').find('.Cng1');
    (ref19).removeClass('activeB');
    localStorage.setCng1 = "0";
    var ref31 = $('body').find('.Cng0');
    (ref31).addClass('activeB');
    var ref2 = $('body').find('.goods-cell');
    var ref1 = $('body').find('.goods-cell');
    (ref1).addClass('big-cell goods_view_line');
    (ref1).removeClass('goods_view_box goods_view_row');
    var ref11 = $('body').find('.goods_addition_chars');
    (ref11).css('display', 'inline-block');
    var ref12 = $('body').find('.exist');
    (ref12).css('display', 'inline-block');
    var ref13 = $('body').find('.goods_pic');
    (ref13).css({ 'top': '0px', '': '' });
    var ref14 = $('body').find('.goods_form');
    (ref14).css({ 'top': '0px', '': '' });
    var ref15 = $('body').find('.goods_data');
    (ref15).css({ 'height': '210px', '': '' });
    var ref16 = $('body').find('.main-img');
    (ref16).css({ 'height': '180px', '': '' });
    var ref17 = $('body').find('.compare');
    (ref17).css({ 'top': '-110px !important', 'left': '507px' });
    var ref38 = $('body').find('.btn_toBasket');
    (ref38).css({ 'width': '100px', 'height': '25px', 'margin-left': '0px', 'margin-top': '0px', 'padding': '.1em .1em .4em .1em' });
    ref38.find('.svg-inline--fa fa-search fa-w-16').remove();
    thisText = 'В корзину';
    ref38.text(thisText);
    var ref39 = $('body').find('.price');
    (ref39).css({ 'position': 'inherit', 'margin-left': '0px', 'margin-top': '0px' });
    var ref40 = $('body').find('.el_form-quantity');
    (ref40).css({ 'margin-top': '0px' });
    var ref41 = $('body').find('.exist');
    (ref41).css({ 'visibility': 'hidden' });
    var ref43 = $('body').find('.gallery');
    (ref43).css({ 'display': 'none' });
    str = $('body').find('#chgid1');
    srt1 = str.children();
    $(srt1).eq($(srt1).length - 1).css({ 'border-bottom': '1px solid #DCDCDC', 'border-radius': '0px 0px 5px 5px' });
    $(srt1).eq(0).css({ 'border-top': '1px solid #DCDCDC', 'border-radius': '5px 0px 0px 0px' });
    sravn();
}

function newView2() {
    localStorage.setCng2 = "0";
    var ref18 = $('body').find('.Cng0');
    (ref18).removeClass('activeB');
    localStorage.setCng0 = "0";
    var ref19 = $('body').find('.Cng1');
    (ref19).removeClass('activeB');
    localStorage.setCng1 = "1";
    var ref5 = $('body').find('.Cng1');
    (ref5).addClass('activeB');
    var ref2 = $('body').find('.goods-cell');
    (ref2).addClass('goods_view_box');
    (ref2).removeClass('goods_view_line goods_view_row big-cell');
    var ref11 = $('body').find('.goods_addition_chars');
    (ref11).css('display', 'none');
    var ref12 = $('body').find('.exist');
    (ref12).css('display', 'none');
    var ref13 = $('body').find('.goods_pic');
    (ref13).css({ 'top': '45px', '': '' });
    var ref14 = $('body').find('.goods_form');
    (ref14).css('top', '50px');
    var ref15 = $('body').find('.goods_data');
    (ref15).css({ 'height': '40px', '': '' });
    var ref16 = $('body').find('.main-img');
    (ref16).css({ 'height': '100px', '': '' });
    var ref17 = $('body').find('.compare');
    (ref17).css({ 'top': '0px', 'left': '0px' });
    var ref38 = $('body').find('.btn_toBasket');
    (ref38).css({ 'width': '90px', 'height': '20px', 'margin-left': '0px', 'margin-top': '0px', 'padding': '0px' });
    ref38.find('.svg-inline--fa fa-search fa-w-16').remove();
    thisText = 'В корзину';
    ref38.text(thisText);
    var ref39 = $('body').find('.price');
    (ref39).css({ 'position': 'inherit', 'margin-left': '0px', 'margin-top': '0px' });
    var ref40 = $('body').find('.el_form-quantity');
    (ref40).css({ 'margin-top': '0px' });
    var ref41 = $('body').find('.exist');
    (ref41).css({ 'visibility': 'hidden' });
    var ref43 = $('body').find('.gallery');
    (ref43).css({ 'display': 'none' });
    sravn();
    cngOrangeBut();
}

function newView3() {
    var ref20 = $('body').find('.Cng2');
    (ref20).addClass('activeB');
    localStorage.setCng2 = "1";
    var ref18 = $('body').find('.Cng0');
    (ref18).removeClass('activeB');
    localStorage.setCng0 = "0";
    var ref19 = $('body').find('.Cng1');
    (ref19).removeClass('activeB');
    localStorage.setCng1 = "0";
    var ref3 = $('body').find('.goods-cell');
    (ref3).addClass('goods_view_row goods_view_line');
    (ref3).removeClass('goods_view_box big-cell');
    var ref11 = $('body').find('.goods_addition_chars');
    (ref11).css('display', 'none');
    var ref12 = $('body').find('.exist');
    (ref12).css('display', 'inline-block');
    var ref13 = $('body').find('.goods_pic');
    (ref13).css({ 'top': '0px', '': '' });
    var ref14 = $('body').find('.goods_form');
    (ref14).css({ 'top': '0px', '': '' });
    var ref15 = $('body').find('.goods_data');
    (ref15).css({ 'height': '0px;', '': '' });
    var ref16 = $('body').find('.main-img');
    (ref16).css({ 'height': '50px', '': '' });
    var ref37 = $('body').find('.compare');
    (ref37).css({ 'top': '-23px !important', 'left': '764px !important' });
    //document.getElementById("chgid1").innerHTML = document.getElementById("chgid1").innerHTML.replace(/Добавить к сравнению/ig, 'сравнить');
    var ref38 = $('body').find('.btn_toBasket');
    (ref38).css({ 'width': '58px', 'height': '25px', 'margin-left': '15px', 'margin-top': '-1px', 'padding-left': '0px', 'padding-right': '2px' });
    ref38.text('');
    ref38.append('<svg class="svg-inline--fa fa-search fa-w-16" aria-hidden="true" data-prefix="fas" data-icon="search" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" data-fa-i2svg=""><path fill="currentColor" d="M 551.991 64 H 129.28 l -8.329 -44.423 C 118.822 8.226 108.911 0 97.362 0 H 12 C 5.373 0 0 5.373 0 12 v 8 c 0 6.627 5.373 12 12 12 h 78.72 l 69.927 372.946 C 150.305 416.314 144 431.42 144 448 c 0 35.346 28.654 64 64 64 s 64 -28.654 64 -64 a 63.681 63.681 0 0 0 -8.583 -32 h 145.167 a 63.681 63.681 0 0 0 -8.583 32 c 0 35.346 28.654 64 64 64 c 35.346 0 64 -28.654 64 -64 c 0 -17.993 -7.435 -34.24 -19.388 -45.868 C 506.022 391.891 496.76 384 485.328 384 H 189.28 l -12 -64 h 331.381 c 11.368 0 21.177 -7.976 23.496 -19.105 l 43.331 -208 C 578.592 77.991 567.215 64 551.991 64 Z M 240 448 c 0 17.645 -14.355 32 -32 32 s -32 -14.355 -32 -32 s 14.355 -32 32 -32 s 32 14.355 32 32 Z m 224 32 c -17.645 0 -32 -14.355 -32 -32 s 14.355 -32 32 -32 s 32 14.355 32 32 s -14.355 32 -32 32 Z m 38.156 -192 H 171.28 l -36 -192 h 406.876 l -40 192 Z"></path></svg>');
    var ref39 = $('body').find('.price');
    (ref39).css({ 'position': 'absolute', 'margin-left': '-90px', 'margin-top': '0px' });
    var ref40 = $('body').find('.el_form-quantity');
    (ref40).css({ 'margin-top': '26px' });
    var ref41 = $('body').find('.exist');
    (ref41).css({ 'visibility': 'hidden' });
    var ref42 = $('body').find('.element_tabs_btn');
    (ref42).css({ 'display': 'none' });
    var ref43 = $('body').find('.gallery');
    (ref43).css({ 'display': 'none' });
    str = $('body').find('#chgid1');
    var ref44 = $('body').find('.signature');
    (ref44).css({ 'display': 'none' });
    srt1 = str.children();
    $(srt1).eq($(srt1).length - 1).css({ 'border-bottom': '1px solid #DCDCDC', 'border-radius': '0px 0px 5px 5px' });
    $(srt1).eq(0).css({ 'border-top': '1px solid #DCDCDC', 'border-radius': '5px 0px 0px 0px' });
    sravn();
    cngGreenBut();
    cngOrangeBut2();
}

function domReady() {
    var ref138 = $('body').find('.btn_toBasket');
    (ref138).css({ 'width': '100px', 'height': '25px', 'margin-left': '0px', 'margin-top': '0px', 'padding': '.1em .1em .4em .1em' });
    ref138.find('.svg-inline--fa.fa-search.fa-w-16').remove();
    thisText1 = 'В корзину';
    ref138.text(thisText1);
    $('.goods_container').attr('id', 'chgid1');
    var ref = $('body').find('.view-switches');
    (ref).remove();
    $('body').find('.goods_sort').append('<div class="view-switchesC"><span class="view-switchC Cng0"></span><span class="view-switchC Cng1"></span><span class="view-switchC Cng2"></span></div>');
    var ref20 = $('body').find('.Cng0');
    (ref20).addClass('activeB');
    var ref21 = $('body').find('.Cng0');
    if (localStorage.setCng0 == '1' || localStorage.setCng1 == '1' || localStorage.setCng2 == '1') {
        if (localStorage.setCng0 == '1') {
            newView1();
        }
        else {
            var ref4 = $('body').find('.Cng0');
            (ref4).removeClass('activeB');
        }
        if (localStorage.setCng1 == '1') {
            newView2();
        }
        else {
            var ref6 = $('body').find('.Cng1');
            (ref6).removeClass('activeB');
        }
        if (localStorage.setCng2 == '1') {
            newView3();
        }
        else {
            var ref8 = $('body').find('.Cng2');
            (ref8).removeClass('activeB');
        }
    }
    else {
        localStorage.setCng0 == '1'
        newView1();
    }
}


$(document).ready(function () { /*код смены отображения плиток с товаром*/
    domReady();
});

$(function () { /*код смены отображения плиток с товаром (переключение в строковый вид)*/
    $('body').on('click', '.view-switchC.Cng0', function () {
        newView1();
    });
    $('body').on('click', '.view-switchC.Cng1', function () {
        newView2();
    });
    $('body').on('click', '.view-switchC.Cng2', function () {
        newView3();
    });
});

$(function () { //увеличение миниат. при наведении в каталоге
    $('body').on('mouseover', '.goods-cell.goods_view_line.goods_view_row .goods_pic', function () {
        if (localStorage.setCng2 == '1') {
            $(this).find('img').addClass('imgResVL');
            $(this).parent().css('z-index', '3');
        }
    });
    $('body').on('mouseout', '.goods-cell.goods_view_line.goods_view_row .goods_pic', function () {
        if (localStorage.setCng2 == '1') {
            $(this).find('img').removeClass('imgResVL');
            $(this).parent().css('z-index', '2');
        }
    });
});


$(document).ready(function () { //замена кнопок цена/популярность
    str = $('body').find('.goods_sort');
    $(str).append('<div class=""></div>')
    srt1 = str.children();
    ad1 = $()
});


$(document).ready(function () {
    var box = $("#container");
    if (box.size() > 0) {
        if (document.createStyleSheet) {
            document.createStyleSheet('font-awesome.min');
        }
        else {
            $("head").append($("<link rel='stylesheet' href='" + box.childen().attr('id') + "-style.css' type='text/css' media='screen' />"));
        }
    }
});


$(document).ready(function () { /*����������� �������� ��� �������*/
    if ($('div').is('.b-slider')) {
        var ref = $('body').find('.b-breadcrumb');
        (ref).css({ 'margin-top': '320px' });
    }
    else {
        var ref = $('body').find('.b-breadcrumb');
        (ref).css({ 'margin-top': '-10px' });
    }
});






$(document).ready(function () { /*���������*/
    $('head').before($('<div class="loader"><div class="loader-frame"><img class="svg-loader" src="https://kyivstore.com/wp-content/uploads/2016/09/loader.svg" alt="circle-loader"></div></div>'));
});

/*$(window).load(function () {
    $('#before-load').find('i').fadeOut().end().delay(400).fadeOut('slow');
});*/

function loadLoader() {
    $('head').before($('<div class="loader"><div class="loader-frame"><img class="svg-loader" src="https://kyivstore.com/wp-content/uploads/2016/09/loader.svg" alt="circle-loader"></div></div>'));
}

$(function () { //параметры ссылок, при которых не появляется анимация загрузки
    $('body').on('click', 'a', function () {
        var chenghref = $(this);
        var chnCls = $(this).parent();
        var thisClass = ["fancybox", "fancybox-big", "fancybox-big", "myMainImg", "nivo-prevNav", "nivo-nextNav", "linkDotted", "go_enter_byerd"];
        var parentClass = ["element_tabs_btn", "ui-state-default", "prod-file", "fast-order", "el_form-quantity"];
        var thisId = ["go-top"];
        var thisHref = ["/order?step=adress", 'undefined', '#nogo'];
        zx = 0;
        var checkItem = 0;
        checkItem == chenghref.hasClass(zx);
        x = thisClass.length;
        y = parentClass.length;
        z = thisId.length;
        v = thisHref.length;
        sum = 4;
        n = 0; r = 0; f = 0; c = 0; ch = 0;
        xy = x;
        xx = n;
        ch = 0;
        ck = 0;
        checkArr = thisClass;
        while (sum >= 0) {
            while (xy > (-1)) {
                var zx = checkArr[xx];
                if (ch == 0) { if (chenghref.hasClass(zx)) { ck = 1; break; } }
                if (ch == 0) { if (chenghref.hasClass('idCatSubcat')) { loadLoader(); break; } }
                if (ch == 1) { if (chnCls.hasClass(zx)) { ck = 1; break; } }
                if (ch == 2) { if (chenghref.attr("id") == zx) { ck = 1; break; } }
                if (ch == 3) { if (chenghref.attr("href") == zx) { ck = 1; break; } }
                --xy;
                ++xx;
            }
            --sum;
            if (sum == 3) { xy = y; xx = r; checkArr = parentClass; ch = 1; }
            if (sum == 2) { xy = z; xx = f; checkArr = thisId; ch = 2; }
            if (sum == 1) { xy = v; xx = c; checkArr = thisHref; ch = 3; }
        }
        if (ck == 0) { loadLoader(); };
        if (chenghref.hasClass('showchild')) { loadLoader(); }
    });

    $('.fancybox-big').click(function () {
        setTimeout(function () {
            $('.loader').remove();
        }, 1500);
    });
});

$(document).ready(function () {//убранно ограничение размера изоб при открытии галереи
    if ($('a').hasClass('fancybox-big')) {
        var z = $('.fancybox-big').attr('href');
        z = z.replace('-w250', '');
        $('.fancybox-big').attr('href', z);
    }
});

jQuery(document).ready(function ($) { /*время анимации прелоадера*/
    $(document).ready(function () {
        setTimeout(function () {
            $('.loader').fadeOut('slow', function () { });
        }, 10);

    });
});

function orangeButton() { //переключение стиля кнопки
    var resultNum = $('body').find('#result_num');
    var xs = resultNum.text();
    var yeResult = $('body').find('.ye_result');
    if (xs != '0') {
        yeResult.addClass('orangeButton');
    }
    else {
        yeResult.removeClass('orangeButton')
    }
}

$(function () { // сброс настроек кнопок фильтра
    $('body').on('click', '.delFilterM', function () {
        var x = $(this).parent();
        var y = x.find('.filter-group-body');
        var tt = y.find('.from');
        var s = y.find('.to');
        var v = x.find('.title');
        var t1 = y.find('.irs-from');
        var s1 = y.find('.irs-to');
        var z = y.find('.cb-checkbox');
        z.each(function () {
            if ($(this).hasClass('checked')) {
                $(this).click();
                $(this).removeClass('checked');
            }
        });
        v.removeClass('checkedFilter');
        tt.removeClass('checked');
        s.removeClass('checked');
        $(this).remove();
        tt.css({ 'left': '0%' });
        s.css({ 'left': '93.8462%' });
        t1.css({ 'left': '0%' });
        s1.css({ 'left': '83.4615%' });
        var list = new Array();
        var t = y.find('input[filter-type="price"]');
        var minMaxVal = y.find('[filter-type="price"]');
        var sumM = (minMaxVal.attr("min") + ";" + minMaxVal.attr("max"));
        t.val(sumM);
        list.push({ id: t.attr('arg'), type: 'price', value: t.val() });
        var tr = y.find('.from-price');
        var trx = tr.find('input');
        trx.val(minMaxVal.attr("min"));
        trx.keyup();
        var tz = y.find('.to-price');
        var tzx = tz.find('input');
        tzx.val(minMaxVal.attr("max"));
        tzx.keyup();
    });
});

function checkC() { // добавление кнопок сброса фильтра
    $('.category-filter-group').each(function () {
        var x = $(this).find('.cb-checkbox');
        var xy = $(this).find('.to');
        var xz = $(this).find('.from');
        if (x.hasClass('checked') == true || xy.hasClass('checked') || xz.hasClass('checked')) {
            y = $(this).children();
            v = $(this).children('.delFilterM');
            if (v.hasClass('delFilterM') == false) {
                $(this).append('<div class="delFilterM"><img src="/content/theme_main/img/delFilter.png" /></div>');
                y.css({ 'border-radius': '3px 0px 0px 3px' });
                $('.category-filter').prepend('<div class="myResetFilter"><a href="" class="disabled" id="  "><img src="/content/theme_main/img/delFilterX.png" /> сбросить фильтр</a></div>')
                $(this).find('.title').addClass('checkedFilter');
                y.css({ 'border-radius': '3px 0px 0px 3px' });
            }
        }
        else {
            y = $(this).children();
            $(this).find('.delFilterM').remove();
            $(this).find('.title').removeClass('checkedFilter');
            y.css({ 'border-radius': '3px 3px 3px 3px' });
        }
    });
}


$(document).ready(function () { /*выравнивание изображений на странице корзины*/ //не закончена страница корзины
    if (window.location.href == 'file:///E:/%D0%9F%D1%80%D0%BE%D0%B5%D0%BA%D1%82%D1%8B/%D0%92%D0%B5%D0%B1%20%D1%81%D0%B0%D0%B9%D1%82%D1%8B/%D0%91%D0%BE%D0%B9%D0%BB%D0%B5%D1%80-%D0%B3%D0%B0%D0%B7%20%D0%A2%D0%B5%D1%81%D1%82%D0%BE%D0%B2%D1%8B%D0%B5%20%D1%81%D1%82%D1%80%D0%B0%D0%BD%D0%B8%D1%86%D1%8B/38.4/%D0%9E%D1%84%D0%BE%D1%80%D0%BC%D0%BB%D0%B5%D0%BD%D0%B8%D0%B5%20%D0%B7%D0%B0%D0%BA%D0%B0%D0%B7%D0%B0.html') {
        var ref13 = $('body').find('.goods_pic');
        (ref13).css({ 'top': '0px', '': '' });
        var ref14 = $('body').find('.goods_form');
        (ref14).css({ 'top': '0px', '': '' });
        $('.container_12.goods_container').append('<div id="orderRightM"></div>');
        $('.slider_content.goods_container').prependTo('#orderRightM');
    }
});


$(window).load(function () { /*скрипт для нового дизайна фильтра, меняющий высоту фильтра*/
    var checkCss = $('body').find('.filter-group-body');
    $('.category-filter-body').prepend('<div class="mypFilter">подобрать товар</div>')
    $('.noselect').each(function () {
        if ($(this).text() === 'Мощность 90/70/20°c, вт' || $(this).text() === 'Подключение') {
            $('body').find('.category-filter').css({ 'height': '113px' });
        }
    });
    $('.noselect').each(function () {
        if ($(this).text() === 'Способ установки' || $(this).text() === 'Объём, л') {
            $('body').find('.category-filter').css({ 'height': '73px' });
        }
    });
    checkC();
    orangeButton();

});



$(function () { /*скрипт для нового дизайна фильтра*/

    function stWidCss(y) { /*стандартная ширина чекбоксов во всплывающем меню фильтра*/
        y.find('.filter-cbx').css({ 'min-width': '48%', 'max-width': '48%' });
    };
    $('body').on('click', '.title', function () {
        checkThis = $(this);
        checkCss = $(this).parent();
        checkCssC = $(this).children();
        checkCssB = checkCss.find('.filter-group-body');
        checkCssA = checkCssB.parent();
        $('div.myarrow').remove();
        $('.g-arrs').css({ 'transform': 'rotate(180deg)' });
        $('.g-darrs').css({ 'transform': 'rotate(180deg)' });
        $('body').find('.filter-group-body').css({ 'display': 'none', '': '', '': '', });
        switch (checkCssC.text()) {
            case 'Вид нагрева':
                $('body').find('.filter-group-body').addClass('rLeftBodyFiltr');
                $('body').find('.filter-group-body').removeClass('rBodyFilterRec rBodyFilterSp rBodyFilterOb rBodyFilterPow rBodyFilterPr rBodyFilterVs rBodyFilterSr rBodyFilterTp');
                stWidCss(checkCss);
                break;
            case 'Рециркуляция':
                $('body').find('.filter-group-body').addClass('rBodyFilterRec');
                $('body').find('.filter-group-body').removeClass('rBodyFilterSp rBodyFilterOb rBodyFilterPow rLeftBodyFiltr rBodyFilterPr rBodyFilterVs rBodyFilterSr rBodyFilterTp');
                stWidCss(checkCss);
                break;
            case 'Объём, л':
                $('body').find('.filter-group-body').addClass('rBodyFilterOb');
                $('body').find('.filter-group-body').removeClass('rBodyFilterRec rBodyFilterSp rBodyFilterPow rLeftBodyFiltr rBodyFilterPr rBodyFilterVs rBodyFilterSr rBodyFilterTp');
                checkCss.find('.filter-cbx').css({ 'min-width': '18%', 'max-width': '18%' });
                break;
            case 'Мощность, кВт':
                $('body').find('.filter-group-body').addClass('rBodyFilterPow');
                $('body').find('.filter-group-body').removeClass('rBodyFilterRec rBodyFilterSp rBodyFilterOb rLeftBodyFiltr rBodyFilterPr rBodyFilterVs rBodyFilterSr rBodyFilterTp');
                checkCss.find('.filter-cbx').css({ 'min-width': '18%', 'width': '18%', 'max-width': '48%' });
                break;
            case 'Способ установки':
                $('body').find('.filter-group-body').addClass('rBodyFilterSp');
                $('body').find('.filter-group-body').removeClass('rBodyFilterRec rBodyFilterOb rBodyFilterPow rLeftBodyFiltr rBodyFilterPr rBodyFilterVs rBodyFilterSr rBodyFilterTp');
                stWidCss(checkCss);
                break;
            case 'Цена':
                $('body').find('.filter-group-body').addClass('rBodyFilterPr');
                $('body').find('.filter-group-body').removeClass('rBodyFilterRec rBodyFilterOb rBodyFilterPow rLeftBodyFiltr rBodyFilterSp rBodyFilterVs rBodyFilterSr rBodyFilterTp');
                stWidCss(checkCss);
                break;
            case 'Высота, мм':
                $('body').find('.filter-group-body').addClass('rBodyFilterVs');
                $('body').find('.filter-group-body').removeClass('rBodyFilterRec rBodyFilterOb rBodyFilterPow rLeftBodyFiltr rBodyFilterSp rBodyFilterPr rBodyFilterSr rBodyFilterTp');
                stWidCss(checkCss);
                break;
            case 'Ширина, мм':
                $('body').find('.filter-group-body').addClass('rBodyFilterSr');
                $('body').find('.filter-group-body').removeClass('rBodyFilterRec rBodyFilterOb rBodyFilterPow rLeftBodyFiltr rBodyFilterSp rBodyFilterPr rBodyFilterVs rBodyFilterTp');
                stWidCss(checkCss);
                break;
            case 'Тип':
                $('body').find('.filter-group-body').addClass('rBodyFilterTp');
                $('body').find('.filter-group-body').removeClass('rBodyFilterRec rBodyFilterOb rBodyFilterPow rLeftBodyFiltr rBodyFilterSp rBodyFilterPr rBodyFilterSr rBodyFilterVs');
                stWidCss(checkCss);
                break;
            case 'Подключение':
                $('body').find('.filter-group-body').addClass('rBodyFilterTp');
                $('body').find('.filter-group-body').removeClass('rBodyFilterRec rBodyFilterOb rBodyFilterPow rLeftBodyFiltr rBodyFilterSp rBodyFilterPr rBodyFilterSr rBodyFilterVs');
                stWidCss(checkCss);
                break;
            case 'Мощность 90/70/20°c, вт':
                $('body').find('.filter-group-body').addClass('rBodyFilterTp');
                $('body').find('.filter-group-body').removeClass('rBodyFilterRec rBodyFilterOb rBodyFilterPow rLeftBodyFiltr rBodyFilterSp rBodyFilterPr rBodyFilterSr rBodyFilterVs');
                stWidCss(checkCss);
            default:
                $('body').find('.filter-group-body').removeClass('rBodyFilterRec rBodyFilterSp rBodyFilterOb rBodyFilterPow rLeftBodyFiltr rBodyFilterPr rBodyFilterVs rBodyFilterSr rBodyFilterTp');
        }
        $(checkCss).find('.filter-group-body').css({ 'display': 'block', '': '', '': '', });
        $(checkCss).append('<div class="myarrow"></div>');
        if ($(checkCss).find('.filter-group-body').css('display') == 'block' && $(checkCss).hasClass('open') == false) {
            $(".category-filter-group").removeClass('open');
            $('body').find('.category-filter-apply').css({ 'display': 'block' });
            $(checkCss).addClass('open');
            $(checkCss).find('.g-arrs').css({ 'transform': 'rotate(0deg)' });
            $(checkCss).find('.g-darrs').css({ 'transform': 'rotate(0deg)' });
            $('.category-filter-apply').appendTo(checkCssB);
        }
        else {
            $('body').find('.category-filter-apply').css({ 'display': 'none' });
            $(".category-filter-group").removeClass('open');
            $(checkCss).find('.g-arrs').css({ 'transform': 'rotate(180deg)' });
            $(checkCss).find('.g-darrs').css({ 'transform': 'rotate(180deg)' });
            $('body').find('.filter-group-body').removeClass('rBodyFilterRec rBodyFilterSp rBodyFilterOb rBodyFilterPow rLeftBodyFiltr rBodyFilterPr');
            $('div.myarrow').remove();
            checkCss.find('.filter-cbx').css({ 'min-width': '48%', 'max-width': '48%' })
        }
        var z = checkCss.find('.cb-checkbox');
        checkC();
        orangeButton();
    });
});

$(function () { //задержка смены цвета кнопки
    $('body').on('click', '.cb-checkbox', function () {
        checkC();
        setTimeout(function () {
            orangeButton();
        }, 700);
    });
});


$(function () { // смена стиля кнопки фильтра при перемещении ползунка

    $('body').on('click' || 'mouseup' || 'mousedown', '.from', function () {
        v = $(this);
        x = $(this).parent().parent('.category-filter-group');
        z = $(this).parent();
        var minMaxVal = $('body').find('#Filter_57');
        var minMaxVal2 = $('body').find('#Filter_49');
        var minMaxVal3 = $('body').find('#Filter_86');
        var minMaxVal4 = $('body').find('#Filter_46');
        var minMaxVal5 = $('body').find('#Filter_73');
        f = z.find('.from');
        var trx = $(z).find('.irs-from');
        var tzx = $(z).find('.irs-to');
        if (+ trx.text() > + minMaxVal.attr('min') || + trx.text() > + minMaxVal2.attr('min') || + trx.text() > + minMaxVal3.attr('min') || + trx.text() > + minMaxVal4.attr('min') || + trx.text() > + minMaxVal5.attr('min')) {
            y = x.find('.title');
            v.addClass('checked')
            v.addClass('c1')
            checkC();
            orangeButton();
        }
        else {
            if (f.hasClass('c2') == false) {
                v.removeClass('checked')
                checkC();
            }
            v.removeClass('c1')
        }
    });
});

$(function () { // смена стиля кнопки фильтра при перемещении ползунка
    $('body').on('click' || 'mouseup' || 'mousedown', '.to', function () {
        v = $(this);
        x = $(this).parent().parent('.category-filter-group');
        z = $(this).parent();
        var minMaxVal = $('body').find('#Filter_57');
        var minMaxVal2 = $('body').find('#Filter_49');
        var minMaxVal3 = $('body').find('#Filter_86');
        var minMaxVal4 = $('body').find('#Filter_46');
        var minMaxVal5 = $('body').find('#Filter_73');
        f = z.find('.to');
        var trx = $(z).find('.irs-from');
        var tzx = $(z).find('.irs-to');
        if (+ tzx.text() < + minMaxVal.attr('max') || + tzx.text() < + minMaxVal2.attr('max') || + tzx.text() < + minMaxVal3.attr('max') || + tzx.text() < + minMaxVal4.attr('max') || + tzx.text() < + minMaxVal5.attr('max')) {
            v.addClass('checked')
            v.addClass('c2')
            y = x.find('.title');
            checkC();
            orangeButton();
        }
        else {
            if (f.hasClass('c1') == false) {
                v.removeClass('checked')
                checkC();
            }
            v.removeClass('c2')
        }
    });
});


$(function () { // смена стиля кнопки фильтра при наведении на высплывающее окно
    $('body').on('click mouseup mousedown', '.filter-group-body', function () {
        checkC();
        orangeButton();
    });
});

$('.price-line input').keyup(function () { //индикация кнопки установленного фильтра по вводу значения в input
    x = $(this).parent('.category-filter-group');

});

$(document).ready(function () { //установка z-index для доп. меню карточки товара ("похожие товары", "рекомендуем купить", "с этим покупают") 
    x = $("a[arg='similar']");
    y = $("a[arg='recomend']");
    z = $("a[arg='related']");
    x.css({ 'z-index': '4' });
    y.css({ 'z-index': '3' });
    z.css({ 'z-index': '2' });
});

function delImg() {
    $('.myOverlay').remove();
};

/*$(function () { //загрузка картинки при клике на миниматюру в каталоге
    $('body').on('click', '.goods_pic', function () {
        $('body').prepend('<div class="myOverlay" onclick="delImg()"><img class="svg-loaderX" src="https://kyivstore.com/wp-content/uploads/2016/09/loader.svg" alt="circle-loader"><div class="myImgPrev"><div class="closeIc" onclick="delImg()"></div><div class="imgLoadS"></div></div></div>');
        $('.imgLoadS').load("" + $(this).children('a').attr('href') + " .side_left");
    });
});*/

$(document).ready(function () { //крестик для сокрытия меню в фильтре + "регистрация" и "вход" для фиксированного меню
    $('.filter-group-body').prepend('<img class="closeFMB" src="/content/theme_main/img/delFilterX.png">');
    $('#basket_fixpanel').append('<ul id="user-menu" class="menu menu_fixpanel"><li><a href="#nogo" id="reg-link" onclick="showRegPopup();">Регистрация</a></li><li><a class="go_enter_byerd" id="auth-link" onclick="showAuthPopup();" href="#nogo">Вход</a></li></ul>');
});

$(function () {
    $('body').on('click', '.mCSB_container .closeFMB', function () {
        y = $(this).parent().parent().parent().parent().find('.title');
        y.click();
    });
    $('body').on('click', '.filter-group-body .closeFMB', function () {
        x = $(this).parent().parent().find('.title');
        x.click();
    });
});

$(function () { // смена стиля кнопок "+ -"
    $('body').on('mouseover', '.small-icons .cat-item', function () {
        $(this).find('span').addClass('hovr');
    });
    $('body').on('mouseout', '.small-icons .cat-item', function () {
        $(this).find('span').removeClass('hovr');
    });
    $('body').on('click', '.btn.increase', function () {
        var i = $(this).parent().find('input');
        var x = $(this).parent().find('.icon_minus');
        var val = parseInt(i.val());
        if (val > 0) {
            x.addClass('icon_minus_hover');
        }
        else {
            x.removeClass('icon_minus_hover');
        }
    });
});


$(document).ready(function () { //
    var x = $('#basket_fixpanel').children().children();
    x.eq(1).addClass('basketMail');
    $('.basketMail a').addClass('copymail');
    x.eq(3).css('pointer-events', 'none');
    $('.basketMail').append('<div class="basketToolTip"><div class="myArrowx"></div><button class="btnBask">Скопировать</button></div>');
    $('.btnBask').removeClass('copyS');
    $('.btnBask').text('Скопировать');
    $('.myArrowx').css('border-color', 'transparent transparent #7eca75');
});

$(function () { //копирование эл. почты + всплыв кнопка "Скопировать"
    $('body').on('click', '.basketMail', function () {
        var emailLink = document.querySelector('.copymail');
        var range = document.createRange();
        range.selectNode(emailLink);
        window.getSelection().addRange(range);
        var successful = document.execCommand('copy');
        window.getSelection().removeAllRanges();
        $('.btnBask').addClass('copyS');
        $('.btnBask').text('Скопированно');
        $('.myArrowx').css('border-color', 'transparent transparent #ff7d19');
    });
    $('body').on('mouseover', '.basketMail', function () {
        $('.basketMail .basketToolTip').css('display', 'block');
    });
    $('body').on('mouseleave', '.basketMail', function () {
        $('.basketMail .basketToolTip').css({ 'transition': '2s', 'display': 'none' });
        $('.btnBask').removeClass('copyS');
        $('.btnBask').text('Скопировать');
        $('.myArrowx').css('border-color', 'transparent transparent #7eca75');
    });
    $('body').on('click', '.btnBask', function () {
        var emailLink = document.querySelector('.copymail');
        var range = document.createRange();
        range.selectNode(emailLink);
        window.getSelection().addRange(range);
        var successful = document.execCommand('copy');
        window.getSelection().removeAllRanges();
        $(this).addClass('copyS');
        $(this).text('Скопированно');
        $('.myArrowx').css('border-color', 'transparent transparent #ff7d19');
    });
    $('body').on('mouseleave', '.basketMail', function () {
        $('.basketMail .basketToolTip').css({ 'transition': '2s', 'display': 'none' });
        $('.btnBask').removeClass('copyS');
        $('.btnBask').text('Скопировать');
        $('.myArrowx').css('border-color', 'transparent transparent #7eca75');
    });
});


$(document).ready(function () { //иконка "на глав стр"
    $('#basket_fixpanel .container_12').prepend('<div title="На главную страницу" class="homeIcon"><a href="/"></a></div>');
    $("#cart-block .text").clone().appendTo("#head-count");
    $("#cart-block .total").clone().appendTo("#head-count");
    x = $("#head-count").eq(2);
    y = $("#head-count").eq(4);
    x.css('display', 'none');
    y.css('display', 'none');
    $('.page_top_control :not(script)').contents().filter(function () { //зам текста в корзине
        return this.nodeType === 3;
    }).replaceWith(function () {
        return this.nodeValue.replace('Вернуться к покупкам', 'Вернуться в каталог');
    });
    z = $('body').find('.menu-v').children('.selected'); // замена "+" на "-" в активной категории бокового меню
    zx = z.find('.close');
    zx.removeClass('collapsed');
    zx.addClass('expanded');
    str = $('body').find('.goods_item_properties');
    srtn = str.children();
    srt1 = srtn.children();
    $(srt1).eq($(srt1).length - 1).css({ 'border-bottom': '1pt solid #bfbfbf' });
});

$(function () {// перемещение главного меню
    function checkS() {
        var footwr = $(document).outerHeight(true) - $('article').outerHeight(true);
        var checkH = $('article').outerHeight(true) - footwr;
        var foot = $(document).outerHeight(true) - footwr;
        var hMain = $('article').outerHeight(true) - $('.main_navigate').outerHeight(true)
        var scrollM = $(".main_navigate").outerHeight(true);
        var topFixM = $('article').outerHeight(true) - scrollM;
        var top = $(document).scrollTop();
        if ($(window).width() > '800') {
            if (top < 100 + scrollM) {
                scrollM = $(".main_navigate").outerHeight(true);
                $(".main_navigate").css({ top: '0px', position: 'absolute' });
            }
            else $(".main_navigate").css({ top: '65px', position: 'fixed' });
            if (top > (hMain - 60)) {
                scrollM = $(".main_navigate").outerHeight(true);
                $('.grid_9.main_content').css({ 'min-height': scrollM });
                    $(".main_navigate").css({top: (topFixM - 200), position: 'absolute'});
                    return scrollM;
            }
        }
    }
    $(window).scroll(function () {
        checkS();
    });
    $('body').on('click', '.close', function () {
        setTimeout(function () {
            checkS();
            checkS();
        }, 500);
    });
    $(document).ready(function () {
        setTimeout(function () {
            checkS();
            checkS();
        }, 500);
    });
});

$(document).ready(function () { //боковая галерея-слайдер
    var x = $('.side_left');
    var y = x.children();
    var z = x.children().children().children();
    $('.fancybox-big img').addClass('myMainImg');
    if (y.eq(1).hasClass('image-gallery')) {
        $('.goods_item_control.clearfix').append('<div class="minGal"><div class="rButt"></div><div class="lButt"></div><div class="minGalSlider"></div></div>');
        $('.image-gallery').clone().appendTo('.minGalSlider');
    }
    $('.goods_item_properties span').addClass('mySpanFirst');
    $('.goods_item_properties a').replaceWith(function (index, oldHTML) {
        return $("<span class='mySpanX'>").html(oldHTML);
    });
    var t = $('.grid_9.main_content');
    if (t.find('.goods_container') == true) {
        $('.cat-item').addClass('minimizedCI');
    }
});

$(function () {//боковая галерея-слайдер
    var x = parseInt($('.minGalSlider .image-gallery').css('width'), 10);
    var xy = (x - 300) * (-1);
    var left = 0;
    $('body').on('click', '.lButt', function () {
        if ($('.minGalSlider .image-gallery').css('left') < '0px') {
            $('.lButt').css('visibility', 'visible');
            $('.rButt').css('visibility', 'visible');
            left += 77;
            $('.minGalSlider .image-gallery').css({ 'transition': '0.4s', 'left': '' + (left) + 'px' });
        }
        else {
            $('.lButt').css('visibility', 'hidden ');
        }
    });
    $('body').on('click', '.rButt', function () {
        var z = parseInt($('.minGalSlider .image-gallery').css('left'), 10);
        if (z >= xy) {
            $('.rButt').css('visibility', 'visible');
            $('.lButt').css('visibility', 'visible');
            left -= 75;
            $('.minGalSlider .image-gallery').css({ 'transition': '0.4s', 'left': '' + (left) + 'px' });
        }
        else {
            $('.rButt').css('visibility', 'hidden');
        }
    });
    $('.minGalSlider .image-gallery a img').mouseover(function () {
        var hrefImg = $(this).parent().attr('url');
        $('.side_left div:first img').attr('src', hrefImg);
        $('.side_left div:first a').attr('href', $(this).parent().attr('href'));
    });
    $('body').on('mouseover', '.gal-image', function () { //фиксация рамки на выбранной картинке
        $('.gal-image').css({ 'border': '1px solid #c4c5bd' });
        if ($(this).attr('url') == $('.myMainImg').attr('url')) {
            $(this).css({ 'border': '1px solid #ff7d19', 'border-radius': '3px' });
        }
    });
});

$(document).ready(function () { //подпись слайдера
    $('#Slider').append('<div class="slideTetxArea"><div class="thTextH2"></div><div class="thTextH4"></div></div>');
    $('.nivo-caption p h2').clone().appendTo('.thTextH2');
    $('.thTextH2 h2').attr('id', 'sss');
    $('.nivo-caption p h4').clone().appendTo('.thTextH4');
    var st = $('#Slider').find('.slideTetxArea');
    if (st.length != undefined){
        var hSt = $(st[0]).height();
        var hSl = $('#Slider').height();
        if (hSt != 0 && hSl != 0){
            hSl = (hSl + hSt) - 14;
            $('#Slider').css('height', ''+ hSl +'px');
            //$(st[0]).css('margin-top', ''+ hSt +'px');
            var breadc = $('body').find('#b-breadcrumb');
            if (breadc.length != undefined){
                var topBr = $(breadc[0]).css('margin-top');
                topBr = topBr + hSt;
                $(breadc[0]).css('margin-top', ''+ topBr +'px');
            }
        }
    }
    //доработки интерфейса
    $('footer .text-content font').attr('color', '#2c3134');
    var x = $('#navigation').find('.act');
    var y = x.parent();
    y.removeClass('mi');
    y.addClass('act1');
    if ($("div").is(".goods_sort") == true) {
        var z = $('body').find('.small-icons');
        var t = z.children();
        var s = t.children();
        var e = s.parent();;
        if (e.attr('class') == 'small-icons') {
            e.addClass('miniIcon');
            z
        }
        if (e.attr('class') == 'cat-item') {
            e.addClass('miniIcon');
            z.addClass('miniIcon');
            $('body').find('.small-icons.miniIcon').css({ 'margin-left': '25px' });
        }
        s.addClass('miniIcon');
    }
});

$(document).ready(function () { //перенос "вы смотрели" и "сравнение"
    $('.grid_12.slider').appendTo('.grid_9.main_content');
    $('.grid_12.slider').append('<div class="leftSliderAr"></div><div class="rightSliderAr"></div>')
    $('#ComparedList').appendTo('.grid_9.main_content');
    $('#basket_fixpanel').append('<a title="" class="linkcompare" href="https://boiler-gas.ru/compare"  target="_blank"></a>');
    $('#basket_fixpanel').append('<div id="wishlistIconGas"><a class="a_wishlist" title="Избранное" href="/cabinet?view=favorite"><i class="far fa-heart"></i><span class="wishIcount">0</span></a></div>');
    $('.container_12.text_12.decor_footer').append('<div id="footSocD"><a class="footSoc" title="Youtube" href=""><i class="fab fa-youtube"></i></a><a class="footSoc" title="VK" href=""><i class="fab fa-vk"></i></a><a class="footSoc" title="Facebook" href=""><i class="fab fa-facebook-f"></i></a><a class="footSoc" title="Twitter" href=""><i class="fab fa-twitter"></i></a></div>');
    $('#header').append('<a title="" class="linkcompare" href="https://boiler-gas.ru/compare" target="_blank"></a>');
    $('.item_view_title_main').prependTo('.grid_12.slider');
    text = 'Просмотренные:';
    $('.item_view_title_main').text(text);
});

$(function () { // слайдер "вы смотрели"
    var x = $('.slider_content').width();
    var left = $('.slider_content').css('left');
    var y = $('.slider_wrap').width();
    var t = ($('.slider_content').width() - $('.slider_wrap').width()) * (-1);
    var z = 0;
    var p = $('.slider_content').children();
    var c = p.length - 1;
    var n = 0;
    $('.rightSliderAr').click(function () {
        if (x > y) {
            if (z > t) {
                z = z - 197;
                var l = (z + 'px');
                $('.slider_content').css({ 'left': l });
                n++;
                $('.leftSliderAr').css('display', 'block');
            }
            else {
                $('.rightSliderAr').css('display', 'none');
            }
        }
    });
    $('.leftSliderAr').click(function () {
        if (x > y) {
            if (z < 0) {
                z = z + 197;
                var l = (z + 'px');
                $('.slider_content').css({ 'left': l });
                n--;
                $('.rightSliderAr').css('display', 'block');
            }
            else {
                $('.leftSliderAr').css('display', 'none');
            }
        }
    });
});


$(document).ready(function () { //замена "найти" на лупу
    var x = $('#bchange');
    y = $('#basket_fixpanel').find('#bchange');
    x.text("");
    y.text("");
    x.append('<svg class="svg-inline--fa fa-search fa-w-16" aria-hidden="true" data-prefix="fas" data-icon="search" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" data-fa-i2svg=""><path fill="currentColor" d="M 508.5 481.6 l -129 -129 c -2.3 -2.3 -5.3 -3.5 -8.5 -3.5 h -10.3 C 395 312 416 262.5 416 208 C 416 93.1 322.9 0 208 0 S 0 93.1 0 208 s 93.1 208 208 208 c 54.5 0 104 -21 141.1 -55.2 V 371 c 0 3.2 1.3 6.2 3.5 8.5 l 129 129 c 4.7 4.7 12.3 4.7 17 0 l 9.9 -9.9 c 4.7 -4.7 4.7 -12.3 0 -17 Z M 208 384 c -97.3 0 -176 -78.7 -176 -176 S 110.7 32 208 32 s 176 78.7 176 176 s -78.7 176 -176 176 Z"></path></svg>');
    y.append('<svg class="svg-inline--fa fa-search fa-w-16" aria-hidden="true" data-prefix="fas" data-icon="search" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" data-fa-i2svg=""><path fill="currentColor" d="M 508.5 481.6 l -129 -129 c -2.3 -2.3 -5.3 -3.5 -8.5 -3.5 h -10.3 C 395 312 416 262.5 416 208 C 416 93.1 322.9 0 208 0 S 0 93.1 0 208 s 93.1 208 208 208 c 54.5 0 104 -21 141.1 -55.2 V 371 c 0 3.2 1.3 6.2 3.5 8.5 l 129 129 c 4.7 4.7 12.3 4.7 17 0 l 9.9 -9.9 c 4.7 -4.7 4.7 -12.3 0 -17 Z M 208 384 c -97.3 0 -176 -78.7 -176 -176 S 110.7 32 208 32 s 176 78.7 176 176 s -78.7 176 -176 176 Z"></path></svg>');
    $('#bchange').css('display', 'block');
});




$(document).ready(function () {//новый дизайн страницы корзины
    var x = document.location.href;
    if (x == 'https://boiler-gas.ru/order') {
        $('.grid_2.text_11:eq(2)').css('display', 'none');
        $('.grid_2.text_11:eq(4)').css('display', 'none');
        $('.basket_remove_good.goods_remove').text('Удалить');
    }
});


$(function () {
    $('body').on('click', '.cat-item', function () {
        $(this).attr('id', 'thisClick');
        var x = $(this).parent();
        var z = x.children();
        var num = z.length - 1;
        var numC = z.length - 1;
        localStorage.setItem('catCheckNum', numC);
        var numb = 0;
        while (num > (-1)) {
            var p = z.eq(numb);
            if (p.attr('id') == 'thisClick') {
                localStorage.setItem('catItemNum', numb);
                break;
            }
            localStorage.setItem('catItemNum', (-1));
            ++numb;
            --num;
        }
    });
});

$(document).ready(function () {//добавление плитки текущей страницы каталога
    if ($('body').find('div').hasClass('goods_sort')) {
        var z = $('body').find('.page-path.b-breadcrumb__bar').children();
        //var y = x.length();
        var wlh = document.location.href;
        var rlh = 'https://boiler-gas.ru';
        var flh = wlh.replace(rlh, '');
        var e = z.length - 2;
        var y = z.eq(e);
        var t = y.find('a');
        var c = t.attr('href');
        var mi = $('.small-icons.miniIcon').children();
        var numbPr = localStorage.getItem('catItemNum');
        mi.eq(numbPr).addClass('prMiniIcon');
        var cx = localStorage.getItem('catItemNum');
        var cc = localStorage.getItem('catCheckNum');
        if (localStorage.getItem('catItemNum') >= 0) {
            /*if ((cx < cc) == true) {
                $('.prMiniIcon').before('<div class="catThisPage"></div>');
            }*/
            $('.prMiniIcon').before('<div class="catThisPage"></div>');
        }
        else {
            $('.small-icons.miniIcon').append('<div class="catThisPage"></div>');
        }
        $('.catThisPage').css('display', 'none');
        $('.catThisPage').load('' + c + ' .small-icons');
        setTimeout(function () {
            var xy = $('.catThisPage .small-icons').children();
            xy.css('display', 'none');
            var xz = xy.length - 1;
            var num = xz;
            while (num > (-1)) {
                var xv = xy.eq(num);
                var xc = xv.find('a');
                zx = xc.attr('href');
                if (xc.attr('href') == '' + flh + '') {
                    $('.catThisPage').css('display', 'block');
                    xv.css({ 'display': 'block', 'box-shadow': '0px 0px 15px rgba(0, 0, 0, 0.36)' });
                    xv.addClass('miniIcon');
                }
                --num;
            }
        }, 3000);
    }
});

$(document).ready(function () {//редизайн "В сравнении" (недоработан)
    var x = $('.compared-list').children();
    x.addClass('linkCompare');
    var z = x.length;
    var t = z;
    num = 1;
    while (t != 0) {
        y = x.eq(t - 1);
        y.addClass('')
        $('.compared-list').prepend('<div class="compareBoxm res' + num + '"></div>');

        num++;
        --t;
    }
    if ($("div").is("#ComparedList") == true) {
        var r = $('.compared-list').children();
        if (r.hasClass('compareBoxm') && r.hasClass('linkCompare')) {
            $('#header .linkcompare').addClass('checkLinkC');
            $('#basket_fixpanel .linkcompare').addClass('checkLinkC');
        }
        else {
            $('#header .linkcompare').removeClass('checkLinkC');
            $('#basket_fixpanel .linkcompare').removeClass('checkLinkC');
        }
    }
});

$(document).ready(function () {
    $('.container_12.text_12.decor_footer').html('');
    $('.container_12.text_12.decor_footer').prepend('<div class="newFootmMain"><div class="logoFoot"><div id="logFimg"><a id="aForCopy" title="Перейти на страницу" href="https://boiler-gas.ru"><span id="logFimgS"><span id="logFimgS1">Boiler</span><span id="logFimgS2">-</span><span id="logFimgS3">Gas</span><span id="logFimgS4">.ru</span></span>   Магазин качественной сантехники</a></div><div id="logFimgS5"></div><div id="lpgFimAd" onclick="parent.location=https://boiler-gas.ru/contacts">г.Москва, ул. Ростокинская, д.8</div></div><div class="contactNewF"><div class="telefonFoot"><a title="Позвонить" href="tel:+7 495 120 08 22"><i id="telIconF"><svg class="svg-inline--fa fa-user fa-w-16" aria-hidden="true" data-prefix="fas" data-icon="user" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" data-fa-i2svg="" width="23" height="23"><path fill="currentColor" d="M493.397 24.615l-104-23.997c-11.314-2.611-22.879 3.252-27.456 13.931l-48 111.997a24 24 0 0 0 6.862 28.029l60.617 49.596c-35.973 76.675-98.938 140.508-177.249 177.248l-49.596-60.616a24 24 0 0 0-28.029-6.862l-111.997 48C3.873 366.516-1.994 378.08.618 389.397l23.997 104C27.109 504.204 36.748 512 48 512c256.087 0 464-207.532 464-464 0-11.176-7.714-20.873-18.603-23.385z" class=""></path></svg></i><span id="telFoot1">+7(495)</span><span id="telFoot2">120-08-22</span></a></div><div id="timeworkFoot"><i id="timeIcon"><svg class="svg-inline--fa fa-user fa-w-16" aria-hidden="true" data-prefix="fas" data-icon="user" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" data-fa-i2svg="" width="23" height="23"><path fill="currentColor" d="M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8zm0 448c-110.5 0-200-89.5-200-200S145.5 56 256 56s200 89.5 200 200-89.5 200-200 200zm61.8-104.4l-84.9-61.7c-3.1-2.3-4.9-5.9-4.9-9.7V116c0-6.6 5.4-12 12-12h32c6.6 0 12 5.4 12 12v141.7l66.8 48.6c5.4 3.9 6.5 11.4 2.6 16.8L334.6 349c-3.9 5.3-11.4 6.5-16.8 2.6z" class=""></path></svg></i><span id="timeworkS1">ПН - ПТ:</span><span id="timeworkS2"> 8.00 - 22.00</span></div><div id="timeworkFoot"><span id="timeworkS1">СБ - ВС:</span><span id="timeworkS2"> 10.00 - 15.00</span></div><div class="footLine"></div><div class="headMail footCopyMail"><i id="mailIconT"><svg class="svg-inline--fa fa-user fa-w-16" aria-hidden="true" data-prefix="fas" data-icon="user" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" data-fa-i2svg="" width="23" height="23"><path fill="currentColor" d="M502.3 190.8c3.9-3.1 9.7-.2 9.7 4.7V400c0 26.5-21.5 48-48 48H48c-26.5 0-48-21.5-48-48V195.6c0-5 5.7-7.8 9.7-4.7 22.4 17.4 52.1 39.5 154.1 113.6 21.1 15.4 56.7 47.8 92.2 47.6 35.7.3 72-32.8 92.3-47.6 102-74.1 131.6-96.3 154-113.7zM256 320c23.2.4 56.6-29.2 73.4-41.4 132.7-96.3 142.8-104.7 173.4-128.7 5.8-4.5 9.2-11.5 9.2-18.9v-19c0-26.5-21.5-48-48-48H48C21.5 64 0 85.5 0 112v19c0 7.4 3.4 14.3 9.2 18.9 30.6 23.9 40.7 32.4 173.4 128.7 16.8 12.2 50.2 41.8 73.4 41.4z" class=""></path></svg></i><span id="MailP1">info</span><span id="MailP2">@</span><span id="MailP3">boiler-gas.ru</span></div></div><div class="footMenl"><ul><li><a href="">О компании</a></li><li><a href="">Доставка и оплата</a></li><li><a href="">Сотрудничество</a></li><li><a href="">Получение и возврат товара</a></li><li><a href="">Пользовательское соглашение</a></li><li><a href="">Дизайнерам и оптовикам</a></li><li><a href="">Контакты</a><li></div><div class=""></li></ul></div><div class="buyFootN"><span>Принимаем к оплате:</span><a href="https://boiler-gas.ru/how-to-buy" class=""><div class="imgPaFoot" id="imgFoot1"></div><div class="imgPaFoot" id="imgFoot2"></div><div class="imgPaFoot" id="imgFoot3"></div><div class="imgPaFoot" id="imgFoot4"></div><div class="imgPaFoot" id="imgFoot5"></div><div class="imgPaFoot" id="imgFoot6"></div><div class="imgPaFoot" id="imgFoot7"></div><div class="imgPaFoot" id="imgFoot8"></div></a></div><div class="footEnd"><div id="textFootN"><span>Вся информация о товарах взята из открытых источников и не является предметом договора публичной оферты. Изображения и тех. характеристики могут отличаться от реальных.</span></div><div id="minFoot"><span>© 2007 - 2017 Boiler-gas.ru</span></div></div></div>')
});

$(document).ready(function () { //копирование адрега сайта в подвале
    $('#logFimg').append('<div class="basketToolTip footB"><div class="myArrowx footB"></div><button class="btnBask footB">Скопировать</button></div>');
    $('.thisMcopy').append('<div class="basketToolTip footA"><div class="myArrowx footA"></div><button class="btnBask footA">Скопировать</button></div>');
    $('.footCopyMail').append('<div class="basketToolTip footC"><div class="myArrowx footC"></div><button class="btnBask footC">Скопировать</button></div>');
    $('.btnBask.footB').removeClass('copyS');
    $('.btnBask.footB').text('Скопировать');
    $('.myArrowx.footB').css('border-color', 'transparent transparent #7eca75');
    $('#chgid1 .goods_view_small').css({ 'border': '1px solid #ccc', 'border-radius': '6px' });
});


$(function () { //+ всплыв кнопка "Скопировать"
    $('body').on('mouseover', '#logFimg', function () {
        $('.basketToolTip.footB').css('display', 'block');
    });
    $('body').on('mouseover', '.thisMcopy', function () {
        $('.basketToolTip.footA').css('display', 'block');
    });
    $('body').on('mouseleave', '.thisMcopy', function () {
        $('.basketToolTip.footA').css({ 'transition': '2s', 'display': 'none' });
        $('.btnBask.footA').removeClass('copyS');
        $('.btnBask.footA').text('Скопировать');
        $('.myArrowx.footA').css('border-color', 'transparent transparent #7eca75');
    });

    $('body').on('mouseover', '.footCopyMail', function () {
        $('.basketToolTip.footC').css('display', 'block');
    });

    $('body').on('mouseleave', '.footCopyMail', function () {
        $('.basketToolTip.footC').css({ 'transition': '2s', 'display': 'none' });
        $('.btnBask.footC').removeClass('copyS');
        $('.btnBask.footC').text('Скопировать');
        $('.myArrowx.footC').css('border-color', 'transparent transparent #7eca75');
    });

    $('body').on('mouseleave', '#logFimg', function () {
        $('.basketToolTip.footB').css({ 'transition': '2s', 'display': 'none' });
        $('.btnBask.footB').removeClass('copyS');
        $('.btnBask.footB').text('Скопировать');
        $('.myArrowx.footB').css('border-color', 'transparent transparent #7eca75');
    });
    $('body').on('click', '.btnBask.footB', function () {
        var x = $('#aForCopy').attr('href');
        let tmp = document.createElement('INPUT'),
            focus = document.activeElement;
        tmp.value = x;
        document.body.appendChild(tmp);
        tmp.select();
        document.execCommand('copy');
        document.body.removeChild(tmp);
        focus.focus();

        $(this).addClass('copyS');
        $(this).text('Скопированно');
        $('.myArrowx.footB').css('border-color', 'transparent transparent #ff7d19');
    });
    $('body').on('click', '.btnBask.footA', function () {
        var x = $('.thisMcopy span').text();
        let tmp = document.createElement('INPUT'),
            focus = document.activeElement;
        tmp.value = x;
        document.body.appendChild(tmp);
        tmp.select();
        document.execCommand('copy');
        document.body.removeChild(tmp);
        focus.focus();

        $(this).addClass('copyS');
        $(this).text('Скопированно');
        $('.myArrowx.footA').css('border-color', 'transparent transparent #ff7d19');
    });
    $('body').on('click', '.btnBask.footC', function () {
        var x = 'info@boiler-gas.ru';
        let tmp = document.createElement('INPUT'),
            focus = document.activeElement;
        tmp.value = x;
        document.body.appendChild(tmp);
        tmp.select();
        document.execCommand('copy');
        document.body.removeChild(tmp);
        focus.focus();

        $(this).addClass('copyS');
        $(this).text('Скопированно');
        $('.myArrowx.footC').css('border-color', 'transparent transparent #ff7d19');
    });
    $('body').on('mouseleave', '.basketMail', function () {
        $('.basketToolTip.footB').css({ 'transition': '2s', 'display': 'none' });
        $('.btnBask.footB').removeClass('copyS');
        $('.btnBask.footB').text('Скопировать');
        $('.myArrowx.footA').css('border-color', 'transparent transparent #7eca75');
        $('.basketToolTip.footA').css({ 'transition': '2s', 'display': 'none' });
        $('.btnBask.footA').removeClass('copyS');
        $('.btnBask.footA').text('Скопировать');
        $('.myArrowx.footA').css('border-color', 'transparent transparent #7eca75');
    });
});

$(document).ready(function () {//изм размера картинки в карт. товара
    var x = $('body').find('div');
    if (x.hasClass('side_left')) {
        /*$('body').find('.prod-file span').css({ 'margin-top': '-19px' });*/
        var x = $('body').find('.side_left').children();
        var z = x.find('a').attr('href');
        z.replace('maxWidth=250&maxHeight=250', 'maxWidth=750&maxHeight=750');
        var replaced = z.replace('maxWidth=250&maxHeight=250', 'maxWidth=750&maxHeight=750');
        x.find('.fancybox-big').attr('href', replaced);
    }
    if ($('body').find('.b-slider')) {
        var x = $('body').find('#Slider').css('height');
        $('#b-breadcrumb').css('margin-top', '' + x + '');
    }
});

$(document).ready(function () {//смена стиля "сравнить" и корзины
    $('.grid_5 #SearchQuery').focus(function () {
        $('.ui-autocomplete.ui-front.ui-menu.ui-widget.ui-widget-content.ui-corner-all').addClass('fixedAutocomplete');
    });
    if ($('.fixpanel_basket_count ').text() != '0' && $('.fixpanel_basket_count ').text() != '00') {
        $('.cart-link').addClass('fChahgeCount');
    }
    else {
        $('.cart-link').removeClass('fChahgeCount');
    }
    $('body').on('click', '.btn_toBasket', function () {
        if ($('.fixpanel_basket_count').val() != '0') {
            $('.cart-link').addClass('fChahgeCount');
        }
    });
    $('.side_right').find('button').html('В корзину');
    //$('.grid_9.main_content .grid_12.slider').appendTo('article .container_12');
});


$(function () {//новый title для ссылок бокового меню
    $('.menu-v a').on('mouseover', function () {
        var x = $(this).attr('href');
        x = x.replace('https://boiler-gas.ru', '');
        $(this).append('<span id="noDispSp" style="display:none"></span>');
        $('#noDispSp').load('' + x + ' .cat-item a');
        var v = $('#noDispSp').find('span');
        var tc = "/ ";
        for (i = 0; i < v.length; i++) {
            tc += '' + v[i].text() + ' / ';
        }
        $(this).attr('title', tc);
    });
});









$(function () {
    var a = 0;
    var j = 0;
    var pr = window.screen.width;
    var mainNav = $('.main_navigate');

        $(window).on('load resize', function () {
            if ($(window).width() < '784') {
                if (a == 0) {
                    $('#navigation').prepend('<div class="menu800"><div class="buttonMenu600">Каталог</div><div class="minMenu600"></div></div>');
                    $('#navigation').append('<div class="menuRight"><div class="buttonMenuRight">Меню</div><div class="minMenuRight"></div></div>');
                    $('#basket_fixpanel .container_12').prepend('<div class="menu800"><div class="bascketMenu">Каталог</div><div class="minMenu600"></div></div>');
                    $('.main_navigate').prependTo('#navigation .minMenu600');
                    $('#navigation .menu').appendTo('.minMenuRight');
                    $('.minMenu600 .main_navigate').show();
                    $('#navigation .menu').show();
                    $('.container_12 .main_navigate').remove();
                    var d = $('#navigation').children();
                    $('.main_navigate').css({ 'top': '0px', 'position': 'absolute' });
                    //$(d[3]).remove();
                    a = 1;
                    j = 1;
                    $('.main_navigate').clone(true).appendTo('#basket_fixpanel .minMenu600');

                }
            }
            if ($(window).width() > '784') {
                if (a == 1) {
                    $('#basket_fixpanel .main_navigate').remove();
                    $('.minMenu600 .main_navigate').prependTo('body > [rel="main"] > article .container_12');
                    $('.minMenuRight .menu').appendTo('#navigation');
                    //$('.minMenuRight').before('.minMenuRight .menu');
                    $('.minMenuRight .menu').remove();
                    $('.menu800').remove();
                    a = 0;
                    $('.container_12.bottom_slider .main_navigate').remove();
                    //$('.menuRight').remove();
                    $('.minMenu600 .main_navigate').show();
                    $('#navigation .menu').show();
                    $('#navigation .menuRight').remove();
                }
            }
            if ($(window).width() <= '592') {
                if (a == 1 || a == 0) {
                    $('.menu800').after('<div class="searchMinim"><div class="buttonSerchM"><svg aria-hidden="true" data-prefix="fas" data-icon="search" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" class="svg-inline--fa fa-search fa-w-16" style="font-size: 18px !important"><path fill="currentColor" d="M505 442.7L405.3 343c-4.5-4.5-10.6-7-17-7H372c27.6-35.3 44-79.7 44-128C416 93.1 322.9 0 208 0S0 93.1 0 208s93.1 208 208 208c48.3 0 92.7-16.4 128-44v16.3c0 6.4 2.5 12.5 7 17l99.7 99.7c9.4 9.4 24.6 9.4 33.9 0l28.3-28.3c9.4-9.4 9.4-24.6.1-34zM208 336c-70.7 0-128-57.2-128-128 0-70.7 57.2-128 128-128 70.7 0 128 57.2 128 128 0 70.7-57.2 128-128 128z" class=""></path></svg><span>Поиск</span></div><div class="menSerchM"></div></div>');
                    $('.searchMinim').after('<div class="telMailMin"><div class="buttonTelMailM"><svg aria-hidden="true" data-prefix="fas" data-icon="phone" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" class="svg-inline--fa fa-phone fa-w-16" style="font-size: 18px !important"><path fill="currentColor" d="M493.4 24.6l-104-24c-11.3-2.6-22.9 3.3-27.5 13.9l-48 112c-4.2 9.8-1.4 21.3 6.9 28l60.6 49.6c-36 76.7-98.9 140.5-177.2 177.2l-49.6-60.6c-6.8-8.3-18.2-11.1-28-6.9l-112 48C3.9 366.5-2 378.1.6 389.4l24 104C27.1 504.2 36.7 512 48 512c256.1 0 464-207.5 464-464 0-11.2-7.7-20.9-18.6-23.4z" class=""></path></svg><span>/</span><svg aria-hidden="true" data-prefix="fas" data-icon="envelope" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512" class="svg-inline--fa fa-envelope fa-w-16" style="font-size: 18px !important"><path fill="currentColor" d="M502.3 190.8c3.9-3.1 9.7-.2 9.7 4.7V400c0 26.5-21.5 48-48 48H48c-26.5 0-48-21.5-48-48V195.6c0-5 5.7-7.8 9.7-4.7 22.4 17.4 52.1 39.5 154.1 113.6 21.1 15.4 56.7 47.8 92.2 47.6 35.7.3 72-32.8 92.3-47.6 102-74.1 131.6-96.3 154-113.7zM256 320c23.2.4 56.6-29.2 73.4-41.4 132.7-96.3 142.8-104.7 173.4-128.7 5.8-4.5 9.2-11.5 9.2-18.9v-19c0-26.5-21.5-48-48-48H48C21.5 64 0 85.5 0 112v19c0 7.4 3.4 14.3 9.2 18.9 30.6 23.9 40.7 32.4 173.4 128.7 16.8 12.2 50.2 41.8 73.4 41.4z" class=""></path></svg></div><div class="telMailM"></div></div>');
                    $('.sfy').prependTo('.menSerchM');
                    $('.phoneMail').prependTo('.telMailM');
                    $('.tdivM').remove();
                    $('#header .phoneMail').remove();
                    $('.grid_3.fixpanel_basket.icon-pack').prepend('<div id="iconBasckM"><a href="https://boiler-gas.ru/order"><svg aria-hidden="true" data-prefix="fas" data-icon="shopping-cart" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512" class="svg-inline--fa fa-shopping-cart fa-w-18" style="font-size: 18px !important"><path fill="currentColor" d="M528.12 301.319l47.273-208C578.806 78.301 567.391 64 551.99 64H159.208l-9.166-44.81C147.758 8.021 137.93 0 126.529 0H24C10.745 0 0 10.745 0 24v16c0 13.255 10.745 24 24 24h69.883l70.248 343.435C147.325 417.1 136 435.222 136 456c0 30.928 25.072 56 56 56s56-25.072 56-56c0-15.674-6.447-29.835-16.824-40h209.647C430.447 426.165 424 440.326 424 456c0 30.928 25.072 56 56 56s56-25.072 56-56c0-22.172-12.888-41.332-31.579-50.405l5.517-24.276c3.413-15.018-8.002-29.319-23.403-29.319H218.117l-6.545-32h293.145c11.206 0 20.92-7.754 23.403-18.681z" class=""></path></svg></a></div>')
                    a = 2;
                    newView1();
                    $('.goods_item_control.clearfix').prependTo('.goods_view_item.clearfix');
                    $('.side_right').find('.goods_item_control.clearfix').remove();
                }
            }
            if ($(window).width() > '592') {
                if (a == 2) {
                    $('#navigation .menu800').after('<div class="tdivM"></>');
                    $('.menSerchM .sfy').prependTo('.tdivM');
                    $('.telMailMin .phoneMail').prependTo('#header');
                    $('.searchMinim').remove();
                    $('.telMailMin').remove();
                    $('#basket_fixpanel .phoneMail').remove();
                    $('.grid_3.fixpanel_basket.icon-pack #iconBasckM').remove();
                    a = 1;
                    $('.tdivM').children().addClass('thisdel');
                    $('.sfy:first-child').removeClass('thisdel');
                    $('#header').children('.phoneMail').addClass('thisdel');
                    $('.phoneMail:first-child').removeClass('thisdel');
                    $('.thisdel').remove();
                    $('.goods_view_item.clearfix .goods_item_control.clearfix').prependTo('.side_right');
                    domReady();
                }
            }
        });



});


$(function () {//работа кнопок для мобильного меню
    a = 0;
    b = 0;
    c = 0;
    d = 0;
    i = 0;
    $('body').on('click', '.buttonMenu600', function () {
        if (a == 0) {
            $('#navigation .minMenu600').addClass('actM6');
            a = 1;
            setTimeout(function () {
                $('#navigation .minMenu600.actM6').css({ 'overflow': 'visible' });
            }, 600);
            $('#navigation .minMenuRight').removeClass('actM61');
            $('#basket_fixpanel .minMenu600').removeClass('actM6');
            $('.menSerchM').removeClass('act31');
            $('.telMailM').removeClass('act32');
            b = 0;
            c = 0;
            d = 0;
            i = 0;
            $('.buttonMenu600')
        }
        else {
            $('#navigation .minMenu600.actM6').css({ 'overflow': 'hidden' });
            $('#navigation .minMenu600').removeClass('actM6');
            $('#basket_fixpanel .minMenu600').removeClass('actM6');
            a = 0;
            c = 0;
        }
    });
    $('body').on('click', '.buttonMenuRight', function () {
        if (b == 0) {
            $('.minMenuRight').addClass('actM61');
            b = 1;
            setTimeout(function () {
                $('#navigation .container_12 .minMenuRight.actM61').css({ 'overflow': 'visible' });
            }, 600);
            $('#navigation .minMenu600.actM6').css({ 'overflow': 'hidden' });
            $('#navigation .minMenu600').removeClass('actM6');
            $('#basket_fixpanel .minMenu600').removeClass('actM6');
            $('.menSerchM').removeClass('act31');
            $('.telMailM').removeClass('act32');
            $('#navigation .minMenu600.actM6').css({ 'overflow': 'hidden' });
            a = 0;
            c = 0;
            d = 0;
            i = 0;
        }
        else {
            $('.minMenuRight').removeClass('actM61');
            $('#basket_fixpanel .minMenu600').removeClass('actM6');
            b = 0;
            c = 0;
        }
    });
    $('body').on('click', '.bascketMenu', function () {
        if (c == 0) {
            $('#basket_fixpanel .container_12 .minMenu600').addClass('actM6');
            $('#basket_fixpanel .container_12 .bascketMenu').addClass('actM6');
            setTimeout(function () {
                $('#basket_fixpanel .container_12 .minMenu600.actM6').css({ 'overflow': 'visible' });
            }, 600);
            c = 1;
            $('#basket_fixpanel .container_12 .minMenuRight').removeClass('actM61');
            $('#navigation .minMenu600.actM6').css({ 'overflow': 'hidden' });
            $('#navigation .minMenu600').removeClass('actM6');
            $('#navigation .minMenuRight').removeClass('actM61');
            $('.menSerchM').removeClass('act31');
            $('.telMailM').removeClass('act32');
            $('#navigation .minMenu600.actM6').css({ 'overflow': 'hidden' });
            b = 0;
            a = 0;
            d = 0;
            i = 0;
        }
        else {
            $('#basket_fixpanel .container_12 .minMenu600.actM6').css({ 'overflow': 'hidden' });
            $('#basket_fixpanel .container_12 .minMenu600').removeClass('actM6');
            $('#basket_fixpanel .container_12 .bascketMenu').removeClass('actM6');
            c = 0;
            $('#navigation .minMenu600.actM6').css({ 'overflow': 'hidden' });
            $('#navigation .minMenu600').removeClass('actM6');
            $('#navigation .minMenuRight').removeClass('actM61');
            b = 0;
            a = 0;

        }
    });
    $('body').on('click', '.buttonSerchM', function () {
        if (d == 0) {
            $('.menSerchM').addClass('act31');
            $('.telMailM').removeClass('act32');
            $('.minMenu600').removeClass('actM6');
            $('.minMenuRight').removeClass('actM61');
            $('#basket_fixpanel .container_12 .bascketMenu').removeClass('actM6');
            $('#navigation .minMenu600.actM6').css({ 'overflow': 'hidden' });
            d = 1;
            b = 0;
            a = 0;
            c = 0;
            i = 0;
        }
        else {
            $('.menSerchM').removeClass('act31');

            d = 0;
        }
    });
    $('body').on('click', '.buttonTelMailM', function () {
        if (i == 0) {
            $('.telMailM').addClass('act32');
            $('.minMenu600').removeClass('actM6');
            $('.minMenuRight').removeClass('actM61');
            $('.menSerchM').removeClass('act31');
            $('#basket_fixpanel .container_12 .bascketMenu').removeClass('actM6');
            $('#navigation .minMenu600.actM6').css({ 'overflow': 'hidden' });
            d = 0;
            b = 0;
            a = 0;
            c = 0;
            i = 1;
        }
        else {
            $('.telMailM').removeClass('act32');
            i = 0;
        }
    });
    $('body').on('click', '#basket_fixpanel .bascketMenu', function () {
        $('.telMailM.act32').css('top', '-8px');
    });
    $('body').on('click', '#basket_fixpanel .buttonSerchM', function () {

    });
    $('body').on('click', '#basket_fixpanel .buttonTelMailM', function () {

    });
    $(window).on('resize', function () {
        if ($(window).width() > '784') {
            $('.minMenu600').removeClass('actM6');
            $('.minMenuRight').removeClass('actM61');
            a = 0;
            b = 0;
        }
        if ($(window).width() > '590') {
            $('.telMailM').removeClass('act32');
            $('.minMenuRight').removeClass('actM61');
            d = 0;
            i = 0;
        }
    });
    $(document).mouseup(function (e) {
        if (a == 1 || b == 1 || c == 1 || d == 1 || i == 1) {
            var div = $(".minMenu600");
            var div2 = $('.buttonMenu600');
            var div3 = $('.buttonMenuRight');
            var div4 = $('.bascketMenu');
            var div5 = $('.buttonSerchM');
            var div6 = $('.buttonTelMailM');
            if (!div.is(e.target)
                && div.has(e.target).length === 0 && !div2.is(e.target) && !div3.is(e.target) && !div4.is(e.target) && !div5.is(e.target) && !div6.is(e.target)) {
                $('#navigation .minMenuRight.actM61').css({ 'overflow': 'hidden' });
                $('#navigation .minMenu600.actM6').css({ 'overflow': 'hidden' });
                $('#basket_fixpanel .minMenu600.actM6').css({ 'overflow': 'hidden' });
                $('#navigation .minMenu600').removeClass('actM6');
                $('#basket_fixpanel .minMenu600').removeClass('actM6');
                $('#navigation .minMenuRight').removeClass('actM61');
                a = 0;
                b = 0;
                c = 0;
            }
        }
    });
    $('body').on('click', '.category-filter-col', function () {
        var enem = $(this).find('.filter-group-body');
        var thisSize = $(enem).offset();
        if ($(window).width() < '784') {
            var x = $(window).width();
            x = x - 70;
            var y = thisSize.left + $(enem).width();
            if (y > x) {
                x = x + 70;
                var u = y - x;
                $(this).find('.filter-group-body').animate({ 'left': '-' + u + 'px' });
            }
        }
    });
    $('body').on('mouseover', '.telMailM .headMail', function () {
        $('.telMailM.act32').addClass('hoverMailM');
    });
    $('body').on('mouseleave', '.telMailM .headMail', function () {
        $('.telMailM.act32').removeClass('hoverMailM');
    });
    $('body').on('mouseover', '#basket_fixpanel', function () {
        $('.ui-autocomplete.ui-front.ui-menu.ui-widget.ui-widget-content.ui-corner-all').addClass('basketSearch');
    });
    $('body').on('mouseover', '#basket_fixpanel', function () {
        $('.ui-autocomplete.ui-front.ui-menu.ui-widget.ui-widget-content.ui-corner-all').removeClass('basketSearch');
    });
});

$(function () { // заголовок в виде бегущей строки для моб версии
    if ($(window).width() < '784') {
        var marquee = $("#header h1");
        marquee.css({ "overflow": "hidden", "width": "100%" });
        // оболочка для текста ввиде span (IE не любит дивы с inline-block)
        marquee.wrapInner("<span>");
        marquee.find("span").css({ "width": "50%", "display": "inline-block", "text-align": "center" });
        marquee.append(marquee.find("span").clone()); // тут у нас два span с текстом
        marquee.wrapInner("<div class='bString'>");
        //$('.bString').find('span').css('width', 'auto');
        marquee.find("div").css("width", "400%");
        var reset = function () {
            $(this).css("margin-left", "0%");
            $(this).animate({ "margin-left": "-200%" }, 16000, 'linear', reset);
        };
        reset.call(marquee.find("div"));
        if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
            $(window).scroll(function () {
                var dg = $(window).scrollTop();
                $('#basket_fixpanel').css('top', ''+ dg +'px');
            });
        }
    }
});

$(document).ready(function () { //улучшения карточки товара
    var x = $('.grid_9.main_content .goods_container .goods-cell .goods_data .goods_caption');
    var u = x.length;
    var z;
    try {
        $('.goods_item_control.clearfix').append('<div class="minChar"><div class="minChar1"><div class="minChar1o">Арт.:</div><div class="minChar1ch"></div></div><div class="minChar2"><div class="minChar2o">Бренд:</div><div class="minChar2ch"></div></div><div class="minChar3"><div class="minChar3o">Серия:</div><div class="minChar3ch"></div></div><div class="minChar5"><div class="minChar5o">Модель:</div><div class="minChar5ch"></div><div class="minChar5ch2"></div></div><div class="minChar4"><div class="minChar4o">Страна:</div><div class="minChar4ch"></div></div></div>');
        var xf = $('.goods_item_properties div').children();


		
		var xfs1 = $(xf).find('span:contains("Бренд")');
        var xfs2 = $(xfs1.eq(0)).parent();
        var xfs3 = $(xfs2).find('.mySpanX').text();
        if (xfs3.indexOf('(') != -1){
            xfs3 = xfs3.split('(');
            xfs3 = xfs3[0];
        }

		var xfss1 = $(xf).find('span:contains("Бренд (рус.)")');
        var xfss2 = $(xfss1.eq(0)).parent();
        var xfss3 = $(xfss2).find('.mySpanX').text();
        if (xfss3.indexOf('(') != -1){
            xfss3 = xfss3.split('(');
            xfss3 = xfss3[0];
        }
		
        var xf1 = $(xf).find('span:contains("Производитель")');
        var xf2 = $(xf1.eq(0)).parent();
        var xf3 = $(xf2).find('.mySpanX').text();
        if (xf3.indexOf('(') != -1){
            xf3 = xf3.split('(');
            xf3 = xf3[0];
        }

		var xfx1 = $(xf).find('span:contains("Производитель (рус.)")');
        var xfx2 = $(xfx1.eq(0)).parent();
        var xfx3 = $(xfx2).find('.mySpanX').text();
        if (xfx3.indexOf('(') != -1){
            xfx3 = xfx3.split('(');
            xfx3 = xfx3[0];
        }

        var xfg1 = $(xf).find('span:contains("Производитель (рус)")');
        var xfg2 = $(xfg1.eq(0)).parent();
        var xfg3 = $(xfg2).find('.mySpanX').text();
        if (xfg3.indexOf('(') != -1){
            xfg3 = xfg3.split('(');
            xfg3 = xfg3[0];
        }

		if(xfx3 != undefined && xfx3 != ''){
			xfx3 = ''+ xf3 +' ('+ xfx3 +')';
            if (xfx3.indexOf('()') == -1) {
                $('.minChar2ch').append('' + xfx3 + '');
            }
		}
		else if(xfg3 != undefined && xfg3 != ''){
            xfg3 = ''+ xf3 +' ('+ xfg3 +')';
            if (xfg3.indexOf('()') == -1) {
                $('.minChar2ch').append('' + xfg3 + '');
            }
        }
		else{
			 $('.minChar2o').text('Бренд:')
			xfx3 = ''+ xfs3 +' ('+ xfss3 +')';
			 if (xfx3.indexOf('()') == -1){
                 $('.minChar2ch').append('' + xfx3 + '');
             }
		}
		
        var xf12 = $(xf).find('span:contains("Серия")');
        var xf22 = $(xf12).parent();
        var xf32 = $(xf22).find('.mySpanX').text();
		if (xf32 == undefined || xf32 == ''){
		 $('body').find('.minChar3').css('display', 'none');
		}
		
		var xfx12 = $(xf).find('span:contains("Модель")');
        var xfx22 = $(xfx12).parent();
        var xfx32 = $(xfx22).find('.mySpanX').text();
        $('.minChar3ch').append('' + xf32 + '');
        var xf13 = $(xf).find('span:contains("Страна")');
        var xf23 = $(xf13).parent();
        var xf33 = $(xf23).find('.mySpanX').text();
        $('.minChar4ch').append('' + xf33 + '');
        var y = $('.signature').text();
        y = y.replace('Артикул: ', '');
        var parS = y.split(',');
        y = parS[0];
		if (xfx32 != undefined || xfx32 != ''){
            $('.minChar1ch').append('' + y + '');
            //$('.minChar5ch').append('' + xfx32 + '');
            $('.minChar5ch2').append('' + xfx32 + '');
		}
		else{
		    $('.minChar1ch').append('' + y + '');
            //$('.minChar5ch').append('' + xfx32 + '');
            $('.minChar5ch2').append('' + xfx32 + '');
		}
        // добавление кнопки печати и блока соцсетей
        $('.goods_item_control.clearfix').prepend('<div class="socseti"><div class="socIc"><i class="far fa-thumbs-up"></i></div><div class="hiddenSoc"><div class="soc s1"><i class="fab fa-vk"></i></div><div class="soc s2"><i class="fab fa-facebook-f"></i></div><div class="soc s3"><i class="fab fa-twitter"></i></div><div class="soc s4"><i class="fab fa-youtube"></i></div></div></div><div title="Отправить страницу на печать" class="printThisPage"><svg height="30" width="30" onclick="print()"><path d="M4,2h8v2h1V1H3v3h1M0,5v6h3v1l3,3h7v-4h3V5m-3,2v1H12v6H6V12H4V8H3V7m2,1h6v1H5m0,1h6v1H5"></path></svg></div>');
        var strP = $('.mySpanX.mySpanFirst').text();
        var parS = strP.split(',');
        var tyu = $('body').find('.mySpanX.mySpanFirst');
        $(tyu[0]).text(''+ parS[0] +'');
        if ($('.minChar5ch').text() == '' || $('.minChar5ch').text() == undefined) {
            var prp = $('.minChar5ch').text();
            $('.minChar5ch').css('display', 'none');
        }
    }
    catch (err) {
        return;
    }
    $('.socIc, .hiddenSoc').on('mouseover', function () {
        $('.hiddenSoc').addClass('choose');
        $('.socIc').addClass('thisAct');
    });
    $('.socIc, .hiddenSoc').on('mouseout', function () {
        $('.hiddenSoc').removeClass('choose');
        $('.socIc').removeClass('thisAct');
    });
    var pr = $('.goods_item_control.clearfix').children();

    if ($(pr).hasClass('minGal')) {
        $('.side_right').addClass('hasMinGal');
        var k1 = $('.goods_item_properties').height();
        var s1 = 300 + k;
        $('.side_right').css('min-height', '' + s1 + 'px');
    }
    else {
        try {
            var k = $('.goods_item_properties').height();
            var s = 250 + k;
            $('.side_right').css('min-height', '' + s + 'px');
            $('.minChar').addClass('dontHasMinGal');
            $('.side_right .goods_item_control .price').addClass('dontHasMinGal');
            $('.goods_view_item.clearfix .page_goods_item.product_form.product_form_item').addClass('dontHasMinGal');
            $('.goods_view_item.clearfix .charA.').addClass('dontHasMinGal');
            $('.main_content .goods_item_properties').addClass('dontHasMinGal');
        }
        catch (err) {
            return;
        }

    }

});

$(function () {
   var t = $('.grid_3.grid_3_right').find('div');
   if ($(t).is('.text-content')) {
       $('.grid_9.main_content').addClass('staticPage');
       $('.grid_9_to_6').addClass('staticPage');
       $('.grid_3_right').addClass('staticPage');
       $('.text-content').addClass('staticPage');
       $('.b-breadcrumb').addClass('staticPage');
   }
   $('body').on('mouseover', '.green_box', function () {
      $(this).find('.text').css('background-color', 'rgba(104, 190, 92, 0.61)');
       $(this).find('.background_text').css('background-color', 'rgba(104, 190, 92, 0.61)');
   });
    $('body').on('mouseleave', '.green_box', function () {
        $(this).find('.text').css('background-color', '#d8f1dc');
        $(this).find('.background_text').css('background-color', '#d8f1dc');
    });
    $('body').on('mouseover', '.blue_box', function () {
        $(this).find('.text').css('background-color', 'rgba(106, 180, 238, 0.61)');
        $(this).find('.background_text').css('background-color', 'rgba(106, 180, 238, 0.61)');
    });
    $('body').on('mouseleave', '.blue_box', function () {
        $(this).find('.text').css('background-color', '#deecf8');
        $(this).find('.background_text').css('background-color', '#deecf8');
    });
    $('body').on('mouseover', '.red_box', function () {
        $(this).find('.text').css('background-color', 'rgba(255, 122, 100, 0.6)');
        $(this).find('.background_text').css('background-color', 'rgba(255, 122, 100, 0.6)');
    });
    $('body').on('mouseleave', '.red_box', function () {
        $(this).find('.text').css('background-color', '#feded9');
        $(this).find('.background_text').css('background-color', '#feded9');
    });
});

$(function () {
    $("body").on("click", "a", function (event) {
        var y = $(this).attr("href").indexOf("#") + 1;
        var g = $(this).attr("href");
        g = g.replace('#', '');
        var i = $('[name = "' + g + '"]');
        var top = $(i).offset().top;
        if (y > 0) {
            event.preventDefault();
            $('body,html').animate({scrollTop: (top - 70)}, 1000);
        }
    });
});

$(function () {// перемещение главного меню
    function checkL() {
        var footwr = $(document).outerHeight(true) - $('article').outerHeight(true);
        var checkH = $('article').outerHeight(true) - footwr;
        var foot = $(document).outerHeight(true) - footwr;
        var hMain = $('article').outerHeight(true) - $('.grid_3.grid_3_right.staticPage').outerHeight(true)
        var scrollM = $(".grid_3.grid_3_right.staticPage").outerHeight(true);
        var topFixM = $('article').outerHeight(true) - scrollM;
        var top = $(document).scrollTop();
        if ($(window).width() > '800') {
            if (top < 100 + scrollM) {
                var hrW = $('body').width() - $('article').width();
                var hrW2 = hrW/2;
                scrollM = $(".grid_3.grid_3_right.staticPage").outerHeight(true);
                $(".grid_3.grid_3_right.staticPage").css({ top: '0px', position: 'absolute' });
                if ($('body').width() > 1024) {
                    $(".grid_3.grid_3_right.staticPage").css({right: '-21px'});
                }
                else {
                    $(".grid_3.grid_3_right.staticPage").css({right: '10px'});
                }
            }
            else {
                var hrW = $('body').width() - $('article').width();
                var hrW2 = hrW/2;
                $(".grid_3.grid_3_right.staticPage").css({top: '65px', position: 'fixed'});
                if ($('body').width() > 1024) {
                    $(".grid_3.grid_3_right.staticPage").css({right: '' + hrW2 + 'px'});
                }
                else {
                    $(".grid_3.grid_3_right.staticPage").css({right: '' + (hrW2 - 17) + 'px'});
                }
            }
            if (top > (hMain - 260)) {
                var hrW = $('body').width() - $('article').width();
                var hrW2 = hrW/2;
                scrollM = $(".grid_3.grid_3_right.staticPage").outerHeight(true);
                $('.grid_9.main_content').css({ 'min-height': scrollM });
                $(".grid_3.grid_3_right.staticPage").css({top: (topFixM - 470), position: 'absolute'});
                if ($('body').width() > 1024) {
                    $(".grid_3.grid_3_right.staticPage").css({right: '-21px'});
                }
                else {
                    $(".grid_3.grid_3_right.staticPage").css({right: '10px'});
                }
                return scrollM;
            }
        }
    }
    $(window).scroll(function () {
        checkL();
    });
    $('body').on('click', '.close', function () {
        setTimeout(function () {
            checkL();
            checkL();
        }, 500);
    });
    $('body').resize(function () {
            checkL();
    });
    $(document).ready(function () {
        setTimeout(function () {
            checkL();
            checkL();
        }, 500);
    });
});


$(function () { //добавление заголовка к видео
        var x = $('body').find('.video-name');
        var y = $('body').find('.video-description');
        for (var n = 0; n < x.length; n++) {
            var t = $(x[n]).parent();
            var g = $(x[n]).attr('value');
            $(t).find('#playerCell').attr('onmouseup', 'fullSizeV (this)');
            $(t).append('<div class="nameVideo" ><div><h3>'+ g +'</h3></div></div>');
        }
        for (var n = 0; n < x.length; n++) {
            var t = $(y[n]).parent();
            var g = $(y[n]).attr('value');
            $(t).append('<div class="descVideo" ><div><p>'+ g +'</p></div></div>');
        }
        var allV = $('body').find('.video-block');
        $(allV).wrapAll('<div class="divMinVid"></div>')
        var pAllV = $(allV).parent();
        for (var s = 0; s < allV.length; s++) {
            var img = $(allV[s]).find('.video-preview').attr('value');
            var x = $(allV[s]).find('#playerCell');
            $(x).after('<div class="minVideo" onclick="fSizeVid(this)"><img src="https://boiler-gas.ru'+ img +'"><i class="playVid"><svg aria-hidden="true" data-prefix="fas" data-icon="play" class="svg-inline--fa fa-play fa-w-14" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512"><path fill="currentColor" d="M424.4 214.7L72.4 6.6C43.8-10.3 0 6.1 0 47.9V464c0 37.5 40.7 60.1 72.4 41.3l352-208c31.4-18.5 31.5-64.1 0-82.6z"></path></svg></i> </div>');
        }
});

function fSizeVid (x) {
    var allV = $('body').find('.video-block');
    $(allV).css('width','50%');
    $(allV).find('.minVideo').css('display','block');
    $(allV).find('#playerCell').css('display','none');
    $(allV).find('.nameVideo').removeClass('vs');
    $(allV).find('.descVideo').removeClass('vs');
    var y = $(x).parent();
    $(y).prependTo('.divMinVid');
    $(y).css('width','780px');
    $(y).find('.minVideo').css('display','none');
    $(y).find('#playerCell').css('display','block');
    $(y).find('.nameVideo').addClass('vs');
    $(y).find('.descVideo').addClass('vs');
}


function TooltipText (textSpan) {
var resU = textSpan;
return resU;
}


//подсказка для характеристик
var clickNumb = 'n'


function closeTb () {
    var thJk = $('body').find('.appDivB');
    $(thJk).hide(500);
    clickNumb = 'n';
}

$(function () {
    try {
        var mainD = $('body').find('.mySpanFirst .help-tip');
        for (var n = 0; n < mainD.length; n++) {
            var parThis = $(mainD[n]).parent();
            var textSpan = $(mainD[n]).attr('content');
            var textBubble = String(textSpan);
            $(parThis).append('<div class="mainDivBubble"><span class="iconBubble"></span>' +
                    '<div class="propBubble"><span class="textBubble">' + textBubble + '</span><div class="closeBubble" onclick="closeTb();"></div></div></div>');
            $(mainD[n]).css('display', 'none');
        }
    }
    catch (err) {
    }

    var hiddenB = $('body').find('.propBubble');
    $(hiddenB).hide();



    var PBglobal;
    $('.iconBubble').click(function(){
        var hiddenB = $('body').find('.propBubble');
        $(hiddenB).hide(500);
        $('body').find('.appDivB').remove();
        var parentIB = $(this).parent();
        var PB = $(parentIB);
        PBglobal = PB;
        var posR = $(PB).offset();
        var textR = $(PB).find('.textBubble');
        textR = $(textR).text();


        if (clickNumb == 'n') {


            $('body').append('\'<div class="propBubble appDivB"><div class="arrow2"></div><span class="textBubble">'+ textR +'</span><div class="closeBubble" onclick="closeTb();"></div></div>\'')
            var thJk = $('body').find('.appDivB');
            $(thJk).hide();
            var widthElem = $(thJk).width();
            $(thJk).css({'top':''+ (posR.top + 10) +'px','left':''+ posR.left +'px'});
            $(thJk).show(500);
            clickNumb = textR;
        }
        else {
            if (clickNumb == textR) {


                var thJk = $('body').find('.appDivB');
                $(thJk).hide(500);
                clickNumb = 'n';
            }
            else {


                $('body').append('\'<div class="propBubble appDivB"><div class="arrow2"></div><span class="textBubble">'+ textR +'</span><div class="closeBubble" onclick="closeTb();"></div></div>\'')
                var thJk = $('body').find('.appDivB');
                $(thJk).hide();
                $(thJk).css({'top':''+ (posR.top + 10) +'px','left':''+ posR.left +'px'});
                $(thJk).show(500);
                clickNumb = textR;
            }
        }



    });


    $(document).mouseup(function (e){ // событие клика по веб-документу
        var div = $('.appDivB'); // тут указываем ID элемента
        if (!div.is(e.target) // если клик был не по нашему блоку
            && div.has(e.target).length === 0
            && e.target.className != 'iconBubble') { // и не по его дочерним элементам
            div.hide(500); // скрываем его
            clickNumb = 'n';

        }
    });

    var divGip = $('body').find('.goods_item_properties div');
    for (var n = 0; n < divGip.length; n++) {
        $(divGip[n]).css('z-index','0');
    }
});

$(function(){
    var checkFilterC = $('div').is('.category-filter-body');
    if (checkFilterC == false ) {
        $('.goods_sort').appendTo('.small-icons.miniIcon');
    }
    var parentCat = $('body').find('.cat-item').parent();
    var cParCat = $(parentCat).attr('class');
    if (cParCat == '' || cParCat == undefined)  {
        $(parentCat).attr('class', 'small-icons');
    }
});


$(function(){
    if ($('div').is('.goods_item_properties') == true) {
        /*setTimeout(function(){
            var inB = $('body').find('.goods_item_properties');
            var inC = $('body').find('.compare-prod.compare');
            $(inB).before(inC);
        }, 1500)*/
    }
    var numbVid = $('body').find('.video-block');
    if (numbVid.length == 1) {
        $(numbVid).css('float', 'none');
    }
});

$(function () {
    $('body').on('click', '.category-filter-group', function () {
        var y = $(this).parent();
        var z = $(y).find('.title span');
        var x = $(y).find('._mCS_1');
        var textSpan = $(z).text();
        if (textSpan.indexOf('Бренд') != -1) {
            $(x).addClass('width400');
        }
    });
});

$(function () {
    $('body').on('mouseover', '.menu-v .category ', function () {
        $(this).find('.close.collapsed').addClass('bigPlus');
    });
    $('body').on('mouseout', '.menu-v .category ', function () {
        $(this).find('.close.collapsed').removeClass('bigPlus');
    });
    $('body').on('click', 'li', function () {
        if ($(this).is('a')) {
            $('body').find('.loader').style('display', 'block');
        }
    });
});

$(function (){
   var x = $('body').find('.side_right');
   var a = $(x).find('div');
   if ($(a).is('.minGal') == false){
       $('.goods_item_control.clearfix').css('min-height','160px');
       var z = $('body').find('.printThisPage');
       $(z).css('margin-top','30px');
   }
});

$(function (){ //событие добавление кнопки "найдено" в фильтре
var compare = $('body').find('.compare-prod.compare');
compare = $(compare).find('span');
var txt = $(compare).text();
txt = txt.replace('Добавить в сравнение', 'Сравнить');
$(compare).text(txt);
var n = 0;
	    $('.category-filter-apply').clone(true).addClass('clone').appendTo('.category-filter-body');
		var x = $('body').find('.category-filter-apply.clone');
		$(x).removeClass('category-filter-apply');
	$("#result_num").bind( 'DOMSubtreeModified',function(){ // отслеживаем изменение содержимого блока 2
	    var y = $('body').find('.clone');
		var z = $(y).find('#result_num');
		var val = $('.category-filter-apply').find('#result_num');
		val = $(val).text();
		//$(z).text(''+ val +'');
		n++;
		if(n > 2){
					$(y).css('display', 'block');
					$('.category-filter').css('height','170px');
		}
		//$(y).addClass('orangeButton');
    });
});

$(function(){//перемещение описания
    var x = $('body').find('.item_description');
	var y = $('body').find('.goods_item_properties');
	var z = $('body').find('.prod-file-list');
    if (y[0] != undefined && z[0] != undefined){
        var bItem = $(y).offset();
        var hItem = $(y).height();
        bItem = Number(bItem.top);
        hItem = Number(hItem);
        bItem = bItem + hItem;
        var bProd = $(z).offset();
        var hProd = $(z).height();
        bProd = Number(bProd.top);
        hProd = Number(hProd);
        bProd = bProd + hProd;
        if (bItem > bProd){
            var downTop = bItem - bProd;
            var sideL = $('body').find('.side_left');
            $(sideL).css('margin-bottom',''+ downTop +'px');
            $(x).css({'top':''+ downTop +'px','width':'200%'});
        }
    }
})

$(function(){
    var x = $('body').find('.side_right ');
    if (x[0] != undefined){
        var y = $(x[0]).find('.btn_toBasket');
        var cloneB = $(y[0]).clone(true);
        $(y).after(cloneB);
        var y1 = $(x[0]).find('.btn_toBasket');
        y1 = $(y1[1]);
	$(y1).removeAttr('rel');
        $(y1).text('Быстрая покупка');
        $(y1).addClass('fastBuy');
        $(y1).unbind('click');

        $(y1).click(function(){
		showCalcOrder(false);
	});

        var cloneC = $('.compare-prod.compare');
        $('.compare-prod.compare').prependTo($('.goods_item_control'));
        $('.compare-prod.compare').css({'width':'170px'}); 
        $('.compare-prod.compare span').text('К сравнению');

        var y = $(x[0]).find('.btn_toBasket');
        var cloneB = $(y[0]).clone(true);
        $('.goods_item_control').prepend(cloneB);
        var y1 = $(x[0]).find('.btn_toBasket');
        y1 = $(y1[0]);
        $(y1).text('Рассчитать смету');
	$(y1).removeAttr('rel');
        $(y1).addClass('calcS');
        $(y1).prepend('<img class="newIconG" src="/content/theme_main/img/icon__calculate.png">');
        $(y1).unbind('click');
        $(y1).click(function(){
		showCalcOrder(true);
	});

        var cx = $('body').find('.minGal');
        if (cx[0] == undefined) {
           $('.minChar').css('top','30px');
           $('.page_goods_item.product_form.product_form_item').css('top','60px');
        }
		else{
		   $('.minChar').css('top','130px');
           $('.page_goods_item.product_form.product_form_item').css('top','165px');
		}

    }

    $('body').on('click', '.compare-prod.compare', function(){
        $('.compare-prod.compare span').click();
    })
});

$(function(){//вывод больш. изоб. галереи + скрытие "goods_action" на стр. каталогах
    $('.fancybox-big').unbind();
    $('.fancybox-big').click(function () {
        //debugger;
        var gal = $('.gal-image a[href="' + $(this).attr('href') + '"]');
        if (gal.length) {
            gal[0].click();
            return false;
        }
    });
    var checkPage = $('body').find('.goods_container');
    if (checkPage[0] != undefined) {
        var x = $('body').find('.goods_action');
        for (var n = 0; n < x.length; n++){
            $(x[n]).addClass('hiddenGA');
        }
    }
});
