

$().ready(function () {
    loadSwitchers();
    loadCartBlock();
    loadSearch();
    loadBlurs();
    loadCrumbs();
    loadMenu();
    loadSliders();
    loadSocialPopup();
    loadCart();
    gmapsInit();
    loadCheck();
    loadStarAndComment();
    loadAvatarEdit();
    loadAdressDelete();
    loadFilters();
    loadOrderDetails();
    loadMagnifier();
    loadFeedLinks();
    loadCatalogPath();
    loadCompare();
    loadFilterA();
    loadFilterB();
    initRadFilter();
    loadInnerFilter();
    loadSearchWords();
    loadScroller();
    loadTips();
/*
    $('body').click(function () {
        $('#dialog').hide();
    });
*/
    $('.gal-image a').click(function () {
        $('.utk_preview').attr('href', $(this).attr('href'));
        $('.utk_preview img').attr('src', '/Master/ru/Image/Resize?skiplogo=False&filePath=' + $(this).attr('href').replace('/', '%2F') + '&padding=0&maxWidth=200&maxHeight=200&vertalign=center');

        $('.large-img').attr('style', 'background:url("/Master/ru/Image/Resize?skiplogo=False&filePath=' + $(this).attr('href').replace('/', '%2F') + '&padding=0&maxWidth=300&maxHeight=300&vertalign=center") no-repeat scroll -29px -257px transparent;');
        return false;
    });

    initFilterView();
    initCatMenuV2();
    initCatMenuV3();
    initCatMenuV4();

    loadCompareToggler();
    loadGears();
    loadPopups();
});

function changeBox(obj, cnt) {
    var old = $(obj).parent().find('input').val();
    var oldnum = 0;
    try {
        oldnum = parseInt(old);
    } catch (e) {
        
    }
    if (oldnum == 0)
        oldnum = 1;
    oldnum += cnt;
    if (oldnum == 0)
        oldnum = 1;

    $(obj).parent().find('input').val(oldnum);
    return false;
}

function renewCart(id, count, obj, ischange) {

    if ($(obj).val() * 1 >= 1) {
        $.get('/Master/ru/ShopCart/PopupDialog', { id: id, count: ischange ? $(obj).val() : count, needchange: true, ischange: ischange }, function (d) {
            $('#PopupDialog').replaceWith(d);
        });
    } else return false;
}

function showCartDialog(id, count) {
    $.get('/Master/ru/ShopCart/PopupDialog', {id:id, count:count, needchange: false}, function(d) {
        $('#el_modal_window').remove();
        $('body').append(d);
    })
}

function loadPopups() {
    $('.popup-content').each(function() {
        $(this).dialog({ modal: true, closeText: "Закрыть", width: "50%" });
        return;
    });
}

function loadGears() {
    $('#zone').each(function() {
        var $this = $(this);
        var width = ($this.css('width').replace('px', '') * 1) / 2;
        $this.find('.gear').mouseover(function () {
            if ($(this).attr('tip').length) {
                var cnt = $this.find('#AnimeBlockContainer');
                cnt.find('#megaplashka').html($(this).attr('tip'));
                if ($(this).css('left').replace('px', '') * 1 > width) {
                    //right
                    cnt.css('left', $(this).css('left').replace('px', '') * 1 - cnt.outerWidth() - 15);
                    cnt.css('top', $(this).css('top').replace('px', '') * 1 +10);
                    cnt.find('#tr_right').show();
                    cnt.find('#tr_left').hide();
                } else {
                    cnt.css('left', $(this).css('left').replace('px', '') * 1 + 44);
                    cnt.css('top', $(this).css('top').replace('px', '') * 1+ 10);
                    cnt.find('#tr_left').show();
                    cnt.find('#tr_right').hide();
                }
                cnt.show();
            }
        }).mouseout(function() {
            $('#AnimeBlockContainer').hide();
        });
    });
}

function loadCompareToggler() {
    $('.compare-head').css('cursor', 'pointer');
    var show = $.cookie('ShowCompareList');
    if (!show || !show.length) {
        show = '1';
    }
    if (show == '0') {
        $('.compared-list').hide();
    }
    $('#ComparedList .compare-head').click(function () {
        if ($('.compared-list').is(':hidden')) {
            $.cookie('ShowCompareList', '1', { expires: 365, path: '/' });
        } else {
            $.cookie('ShowCompareList', '0', { expires: 365, path: '/' });
        }

        $('.compared-list').slideToggle();
        return false;
    });
}


function initCatMenuV3() {
    $('.catalog-menu-3 .close').click(function () {
        if ($(this).hasClass('collapsed'))
            $(this).next().slideDown(400, function () { $(this).prev().removeClass('collapsed').addClass('expanded'); });
        else
            $(this).next().slideUp(400, function () { $(this).prev().removeClass('expanded').addClass('collapsed'); });
    });

}


var menuV2Timer;

function initCatMenuV2() {
    $('#leaflist > div').hover(function () {
        if (menuV2Timer)
            clearTimeout(menuV2Timer);
    }, function () {
        menuV2Timer = setTimeout(function () {
            $('#catlist > div').removeClass('active');
            $('#leaflist > div').hide();

        }, 300);
    });

    $('#catlist > div').hover(function () {
        $('#catlist > div').removeClass('active');
        $(this).addClass('active');
        $('#leaflist > div').hide();
        var target = $('#leaflist #' + $(this).attr('id') + '-child');
        target.show();
        $('#leaflist').css('top', $(this).position().top + 'px');
        var cth = $('#catlist').height();
        if ($('#leaflist').position().top + $('#leaflist').height() > cth) {
            $('#leaflist').css('top', cth - $('#leaflist').height() + 'px');
        }
        if (menuV2Timer)
            clearTimeout(menuV2Timer);
    }, function () {
        menuV2Timer = setTimeout(function () {
            $('#catlist > div').removeClass('active');
            $('#leaflist > div').hide();

        }, 300);

    });

    /*
        $('body').mouseover()
    
        $('#leaflist > div').mouseout(function () {
            if ($(this).is(':visible')) {
                $('#catlist > div').removeClass('active');
                $('#leaflist > div').hide();
            }
        });
    */
}

var menuV4Timer;
function toggleCat(obj) {
    $(obj).parent().find('.childs').toggle();
    if ($(obj).parent().find('.childs').is(':visible')) {
        $(obj).addClass('closeSubCat');
    } else {
        $(obj).removeClass('closeSubCat');
    }
}
function initCatMenuV4() {
    
    /*$('#nav .menu-item').hover(function () {
        if (menuV4Timer)
            clearTimeout(menuV4Timer);
    }, function () {
        menuV4Timer = setTimeout(function () {
            $('#nav .menu-item .submenu').removeClass('active');
            $('#nav .menu-item .submenu').hide();

        }, 300);
    });*/

    $('#nav .menu-item').hover(function () {
        $('#nav .menu-item .submenu').hide();

/*
        if ($(this).find('.submenu').length) {
            $(this).find('.submenu').css('top', $(this).position().top + 'px');
            var cth = $('#nav').height();
            if ($(this).find('.submenu').position().top + $(this).find('.submenu').height() > cth) {
                //debugger;
                $(this).find('.submenu').css('top', cth - $(this).find('.submenu').height() + 'px');
            }
        }
*/
        $(this).find('.submenu').show();
        if (menuV4Timer)
            clearTimeout(menuV4Timer);
    }, function () {
        menuV4Timer = setTimeout(function () {
            $('#nav .menu-item .submenu').hide();
        }, 300);

    });

    /*
        $('body').mouseover()
    
        $('#leaflist > div').mouseout(function () {
            if ($(this).is(':visible')) {
                $('#catlist > div').removeClass('active');
                $('#leaflist > div').hide();
            }
        });
    */
}

function toggleFilter(to) {
    $.cookie('RadFilterV2', to, { expires: 365, path: '/' });
    if (to == 1) {
        $('.module_content_action_top_filter_2').fadeIn(1000);
        $('.module_content_action_top_filter_2_hidden').hide();
    } else {
        $('.module_content_action_top_filter_2').hide();
        $('.module_content_action_top_filter_2_hidden').fadeIn(1000);

    }
}

function initFilterView() {
    var fv = $.cookie('RadFilterV2');
    if (!fv || !fv.length) {
        fv = '0';
    }

    if (fv == '1') {
        $('.module_content_action_top_filter_2').show();
        $('.module_content_action_top_filter_2_hidden').hide();
    } else {
        $('.module_content_action_top_filter_2').hide();
        $('.module_content_action_top_filter_2_hidden').show();

    }
}

function changeImg(obj) {
    $(obj).parent().parent().find('.main-img').attr('src', '/Master/ru/Image/Resize?maxWidth=200&maxHeight=200&filePath=' + $(obj).attr('path') + '&padding=0');
}

function switchTab(obj) {
    var arg = $(obj).attr('arg');
    $(obj).unbind('click');
    if ($(obj).parents('.big-cell').length && $(obj).hasClass('selected')) {
        $(obj).removeClass('selected');
        $(obj).parent().parent().find('.element_tabs_cnt > div').hide();
        return;
    }
    $(obj).parent().find('a').removeClass('selected');
    $(obj).parent().find('a[arg="' + arg + '"]').addClass('selected');
    $(obj).parent().parent().find('.element_tabs_cnt > div').hide();
    $(obj).parent().parent().find('.element_tabs_cnt div[arg="' + arg + '"]').show();
}

function moveLeftMini(obj) {
    $(obj).unbind('click');
    var width = ($(obj).parent().find('.goods_view_small').outerWidth() + 20) * 3;
    var len = $(obj).parent().find('.goods_view_small').length;
    var screens = Math.floor(len / 3) + ((len % 3) > 0 ? 1 : 0);
    var max = screens * width;
    var left = parseInt($(obj).parent().find('.slider_content').css('left').replace('px', ''));
    if (left <= 0) {
        left += width;
    }
    if (left > 0)
        return;
    $(obj).parent().find('.slider_content').animate({
        left: "+=" + width
    }, 1000);

}

function moveRightMini(obj) {
    $(obj).unbind('click');
    var width = ($(obj).parent().find('.goods_view_small').outerWidth() + 20) * 3;
    var len = $(obj).parent().find('.goods_view_small').length;
    var screens = Math.floor(len / 3) + ((len % 3) > 0 ? 1 : 0);
    var max = screens * width;
    var left = parseInt($(obj).parent().find('.slider_content').css('left').replace('px', ''));
    if (left <= 0) {
        left -= width;
    }
    if (left <= max * -1) {
        //left = 0;
        $(obj).parent().find('.slider_content').animate({
            left: "-=" + (left + width)
        }, 1000);

    } else {
        $(obj).parent().find('.slider_content').animate({
            left: "-=" + width
        }, 1000);
    }
}


function toCartRelative(obj) {
    $.get('/Master/ru/ShopCart/CartEditor', { productID: $(obj).attr('arg'), count: 1 }, function (d) {
        $('#basket_content').replaceWith(d);
        loadCart();
        refreshCartHeader();
        updateHead();
    });

    return false;
}

function scrollUp() {
    var current = $('.goods_view_alternative:visible');
    var prev = current.prev();
    if (prev.length) {
        prev.show();
        current.hide();
    }
}
function scrollDown() {
    var current = $('.goods_view_alternative:visible');
    var prev = current.next();
    if (prev.length) {
        prev.show();
        current.hide();
    }
}

function updateHead() {
    $.get('/Master/ru/Cabinet/HeadCount', {}, function (d) {
        $('#head-count').replaceWith(d);
    });
}


function showOrderForm() {
    if ($(this).hasClass('disabled'))
        return false;
    $.get('/Master/ru/Forms/FastOrderPopup', {}, function (d) {
        $('#el_modal_window').remove();
        $('body').append(d);
    });
    return false;
}

function setOrderForm(arg) {
    $('div[arg]').hide();
    $('div[arg="' + arg + '"]').show();
}

function loadTips() {
    $('.help-tip').each(function () {
        var t = $(this);
        t.qtip(
        {
            content: {
                text: t.attr('content')
            },
            show: {
                delay: 0,
                //event: 'click', // Show it on click...
                solo: true, // ...and hide all other tooltips...
            },
            hide: {
                //event: 'unfocus click',
                delay: 0
            },
            position: { my: 'top left', at: 'bottom right', target: t },
            /*
                        api: {
                            onRender: function () {
                                alert('test');
                            }
                        }
            */
        });
    });
}


function setField(name, obj) {
    $('input[name="' + name + '"]').val($(obj).text().trim().replace('«', '').replace('»', ''));
    return false;
}

function loadScroller() {
    $('.fixett').css('height', $(window).height() - 50);
    $('.fixett_left').css('height', $(window).height());
    $(window).resize(function () {
        $('.fixett').css('height', $(window).height() - 50);
        $('.fixett_left').css('height', $(window).height());
    });

    $('#Go_Top1').click(function () {
        $('body,html').animate({
            scrollTop: 0
        }, 800);
        return false;

    });

    $('.fixpanel_upper').click(function () {
        $('body,html').animate({
            scrollTop: 0
        }, 800);
        return false;

    });
    $('#Go_Top_Left').click(function () {
        $('body,html').animate({
            scrollTop: 0
        }, 800);
        return false;

    });

    if ($(this).scrollTop() > 100) {
        $('#fixed').fadeIn();
        //$('#fixed-left').fadeIn();
        $('#basket_fixpanel').fadeIn();
    } else {
        $('#fixed').fadeOut();
        //$('#fixed-left').fadeOut();
        $('#basket_fixpanel').fadeOut();
    }

    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('#fixed').fadeIn();
            //$('#fixed-left').fadeIn();
            $('#basket_fixpanel').fadeIn();
        } else {
            $('#fixed').fadeOut();
            //$('#fixed-left').fadeOut();
            $('#basket_fixpanel').fadeOut();
        }
    });
}

function loadSearchWords() {
    var word = getURLParameter('word');
    if (word != undefined) {
        var nice = word.toUpperCase().substr(0, 1) + word.toLowerCase().substr(1);
        $('.goods_caption').html($('.goods_caption').html().replace(word.toLowerCase(), '<mark>' + word.toLowerCase() + '</mark>').replace(word.toUpperCase(), '<mark>' + word.toUpperCase() + '</mark>').replace(nice, '<mark>' + nice + '</mark>'));
    };

}

function getURLParameter(name) {
    return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.search) || [, ""])[1].replace(/\+/g, '%20')) || null
}


function loadInnerFilter() {
    //$('.inner-filter .f-cell input').styler();

    $('.inner-filter .f-cell input').change(function () {
        $('.inner-filter .f-cell input').trigger('refresh');
        calcCountRadFilterC();


    });
}

function initRadFilter() {
    $('.rad-filter input[type="radio"], .rad-filter select').change(function () {
        calcCountRadFilterA();

    });
}


function showRadFilterResultC() {
    var text = '';
    var tpb = '';
    $('.inner-filter .f-cell').each(function () {
        var boxes = $(this).find('input:checked');
        boxes.each(function () {
            if (tpb == '') {
                tpb += $(this).attr('arg') + '::';
            }
            tpb += $(this).val() + '::';

        });
        if (tpb.length) {
            text += tpb + "^^";
        }

        tpb = '';
    });


    $(".inner-filter input[type='slider']").each(function () {
        text += $(this).attr('arg') + '::' + $(this).val() + '^^';
    });


    $.post('/Master/ru/ClientCatalog/FilterCount', { args: text, pageID: $('#PageID').val(), names: 'Тип;Ширина;Высота;Мощность;Подключение' }, function (d) {
        var link = document.location.href;
        if (link.indexOf('filter=') >= 0)
            link = link.substr(0, link.indexOf('filter=') - 1);
        if (link.indexOf('?') < 0) {
            if (d.Link.substr(0, 1) == '/') {
                link = d.Link;
            } else {
                link += '?' + d.Link;
            }
        } else link += '&' + d.Link;
        document.location.href = (d.IsInCatalog == '1' ? link : d.Link);
    });
}



function calcCountRadFilterC() {
    var text = '';

    var tpb = '';
    $('.inner-filter .f-cell').each(function () {
        var boxes = $(this).find('input:checked');
        boxes.each(function () {
            if (tpb == '') {
                tpb += $(this).attr('arg') + '::';
            }
            tpb += $(this).val() + '::';

        });
        if (tpb.length) {
            text += tpb + "^^";
        }

        tpb = '';
    });


    $(".inner-filter input[type='slider']").each(function () {
        text += $(this).attr('arg') + '::' + $(this).val() + '^^';
    });

    /*
        if (tpb.length) {
            text += tpb + "^^";
        }
    
    */

    $.post('/Master/ru/ClientCatalog/FilterCount', { args: text, pageID: $('#PageID').val(), names: 'Тип;Ширина;Высота;Мощность;Подключение' }, function (d) {
        var link = document.location.href;
        if (link.indexOf('filter=') >= 0)
            link = link.substr(0, link.indexOf('filter=') - 1);
        if (link.indexOf('?') < 0) {
            if (d.Link.substr(0, 1) == '/') {
                link = d.Link;
            } else {
                link += '?' + d.Link;
            }
        } else link += '&' + d.Link;
        $('.inner-filter .found').show();
        $('.inner-filter .found a').attr('href', (d.IsInCatalog == '1' ? link : d.Link));

        var rem = d.Count % 10;
        var g = '';
        if (rem == 1) {
            g = 'товар';
        }
        else if (rem == 0 || (rem >= 5 && rem <= 9)) {
            g = 'товаров';
        }
        else if (rem >= 2 && rem <= 4) {
            g = 'товара';
        }


        $('.inner-filter .found a').html(d.Count + ' ' + g);
    });
}


function calcCountRadFilterA() {
    var text = '';
    $(".rad-filter input[type='slider']").each(function () {
        text += $(this).attr('arg') + '::' + $(this).val() + '^^';
    });
    $('.rad-filter input[type="radio"]:checked').each(function () {
        text += $(this).attr('arg') + '::';
        var st = '';
        st += $(this).val() + '::';
        text += st + "^^";
    });
    $('.rad-filter select').each(function () {
        if ($(this).val().length) {
            text += $(this).attr('arg') + '::';
            var st = '';
            st += $(this).val() + '::';
            text += st + "^^";
        }
    });
    $.post('/Master/ru/ClientCatalog/FilterCount', { args: text, pageID: $('#PageID').val(), names: 'Тип;Ширина;Высота;Мощность;Подключение' }, function (d) {
        var link = document.location.href;
        if (link.indexOf('filter=') >= 0)
            link = link.substr(0, link.indexOf('filter=') - 1);
        if (link.indexOf('?') < 0) {
            if (d.Link.substr(0, 1) == '/') {
                link = d.Link;
            } else {
                link += '?' + d.Link;
            }
        } else link += '&' + d.Link;
        $('.rad-filter .found').show();
        $('.rad-filter .found a').attr('href', (d.IsInCatalog == '1' ? link : d.Link));

        var rem = d.Count % 10;
        var g = '';
        if (rem == 1) {
            g = 'товар';
        }
        else if (rem == 0 || (rem >= 5 && rem <= 9)) {
            g = 'товаров';
        }
        else if (rem >= 2 && rem <= 4) {
            g = 'товара';
        }


        $('.rad-filter .found a').html(d.Count + ' ' + g);
    });
}


function calcCountRadFilterB() {
    var text = '';
    $(".module_content_action_top_filter_2 input[type='hidden']").each(function () {
        text += $(this).attr('arg') + '::' + $(this).val() + '^^';
    });


    var tpb = '';
    $('.module_content_action_top_filter_2 .type-box:checked').each(function () {
        if (tpb == '') {
            tpb += $(this).attr('arg') + '::';
        }
        tpb += $(this).val() + '::';
    });
    if (tpb.length) {
        text += tpb + "^^";
    }

    tpb = '';
    $('.module_content_action_top_filter_2 .con-box:checked').each(function () {
        if (tpb == '') {
            tpb += $(this).attr('arg') + '::';
        }
        tpb += $(this).val() + '::';
    });
    if (tpb.length) {
        text += tpb + "^^";
    }


    $.post('/Master/ru/ClientCatalog/FilterCount', { args: text, pageID: $('#PageID').val(), names: 'Тип;Ширина;Высота;Мощность;Подключение' }, function (d) {
        var link = document.location.href;
        if (link.indexOf('filter=') >= 0)
            link = link.substr(0, link.indexOf('filter=') - 1);
        if (link.indexOf('?') < 0) {
            if (d.Link.substr(0, 1) == '/') {
                link = d.Link;
            } else {
                link += '?' + d.Link;
            }
        } else link += '&' + d.Link;
        $('.module_content_action_top_filter_2 .found').show();
        $('.module_content_action_top_filter_2 .found a').attr('href', (d.IsInCatalog == '1' ? link : d.Link));

        var rem = d.Count % 10;
        var g = '';
        if (rem == 1) {
            g = 'товар';
        }
        else if (rem == 0 || (rem >= 5 && rem <= 9)) {
            g = 'товаров';
        }
        else if (rem >= 2 && rem <= 4) {
            g = 'товара';
        }


        $('.module_content_action_top_filter_2 .found a').html(d.Count + ' ' + g);
    });
}

var filterBLock;
function loadFilterB() {

    $('.module_content_action_top_filter_2 input[type="checkbox"]').styler();

    var maxX = parseInt($('#Width').val()) / 100;
    var maxY = parseInt($('#Height').val()) / 100;
    if (maxX > 12) {
        maxX = 12 + (maxX - 12) / 2;
    }

    setBack(maxX, maxY);


    $('#widthVal').html($('#Width').val());
    $('#heightVal').html($('#Height').val());


    $('.fc').mouseover(function () {
        if (filterBLock)
            return;
        var x = $(this).attr('w');
        var y = $(this).attr('h');
        if (x < 400)
            x = 400;
        if (y < 300)
            y = 300;

        x = x / 100;
        y = y / 100;

        if (x > 12) {
            x = 12 + (x - 12) / 2;
        }

        setBack(x, y);


    }).click(function () {
        filterBLock = setTimeout(function () {
            filterBLock = null;
        }, 2000);
        var w = $(this).attr('w');
        if (w < 400)
            w = 400;
        $('#Width').val(w);
        $('#widthVal').html(w);
        var h = $(this).attr('h');
        if (h < 300)
            h = 300;
        $('#Height').val(h);
        $('#heightVal').html(h);
        calcCountRadFilterB();
    });
    $('.module_content_action_top_filter_2 input[type="checkbox"]').each(function () {
        if ($(this).is(':checked')) {
            $(this).parents('.rad2-type-cell').addClass('active');
        } else {
            $(this).parents('.rad2-type-cell').removeClass('active');
        }
    });
    $('.module_content_action_top_filter_2 input[type="checkbox"]').change(function () {
        calcCountRadFilterB();
        if ($(this).is(':checked')) {
            $(this).parents('.rad2-type-cell').addClass('active');
        } else {
            $(this).parents('.rad2-type-cell').removeClass('active');
        }
    });

    $('.rad-table').mouseout(function () {
        var maxX = parseInt($('#Width').val()) / 100;
        var maxY = parseInt($('#Height').val()) / 100;
        if (maxX > 12) {
            maxX = 12 + (maxX - 12) / 2;
        }

        setBack(maxX, maxY);
    });
}

function setBack(maxX, maxY) {


    for (var k = 0; k <= 16 ; k++) {

        for (var l = 0; l <= 6; l++) {
            var cell = $('.rad-table tr.fr:eq(' + l + ') td.fc:eq(' + k + ')');
            cell.css({ 'background-image': 'none' });
        }
    }

    /*    for (var i = 0; i < maxX; i++) {
            for (var j = 0; j < maxY; j++) {
                var cell = $('.rad-table tr.fr:eq(' + j + ') td.fc:eq(' + i + ')');
                if (i == 0) {
                    if (j == 0) {
                        cell.css({ 'background-image': 'url(/content/utk/img/lt.png)', 'background-repeat': 'no-repeat' });
    
                    }
                    else if (j == maxY - 1) {
                        cell.css({ 'background-image': 'url(/content/utk/img/lb.png)', 'background-repeat': 'no-repeat' });
    
                    } else {
                        cell.css('background-image', 'url(/content/utk/img/lm.png)');
    
                    }
                }
                else if (i == maxX - 1) {
                    if (j == 0) {
                        cell.css({ 'background-image': 'url(/content/utk/img/rt.png)', 'background-repeat': 'no-repeat', 'background-position': 'right center' });
                    } else if (j == maxY - 1) {
                        cell.css({ 'background-image': 'url(/content/utk/img/rb.png)', 'background-repeat': 'no-repeat', 'background-position': 'right center' });
    
                    } else {
                        cell.css({ 'background-image': 'url(/content/utk/img/rc.png)', 'background-repeat': 'no-repeat', 'background-position': 'right center' });
    
    
                    }
                } else {
                    if (j == 0) {
                        cell.css({ 'background-image': 'url(/content/utk/img/ct.png)', 'background-repeat': 'no-repeat' });
                    } else if (j == maxY - 1) {
                        cell.css({ 'background-image': 'url(/content/utk/img/cb.png)', 'background-repeat': 'no-repeat' });
    
                    } else {
                        cell.css({ 'background-image': 'url(/content/utk/img/cc.png)', 'background-repeat': 'no-repeat' });
    
    
                    }
    
                }
    
            }
        }*/


    for (var i = 0; i < maxX; i++) {
        for (var j = 0; j < maxY; j++) {
            var cell = $('.rad-table tr.fr:eq(' + j + ') td.fc:eq(' + i + ')');
            if (i == 0) {
                if (j == 0) {
                    cell.css({ 'background-image': 'url(/content/utk/img/rad.png)', 'background-repeat': 'no-repeat', 'background-position': '0px 0px' });

                }
                else if (j == maxY - 1) {
                    cell.css({ 'background-image': 'url(/content/utk/img/rad.png)', 'background-repeat': 'no-repeat', 'background-position': '0px -42px' });

                } else {
                    cell.css({ 'background-image': 'url(/content/utk/img/rad.png)', 'background-repeat': 'no-repeat', 'background-position': '0px -21px' });

                }
            }
            else if (i == maxX - 1) {
                if (j == 0) {
                    cell.css({ 'background-image': 'url(/content/utk/img/rad.png)', 'background-repeat': 'no-repeat', 'background-position': 'right 0px' });
                } else if (j == maxY - 1) {
                    cell.css({ 'background-image': 'url(/content/utk/img/rad.png)', 'background-repeat': 'no-repeat', 'background-position': 'right -42px' });

                } else {
                    cell.css({ 'background-image': 'url(/content/utk/img/rad.png)', 'background-repeat': 'no-repeat', 'background-position': 'right -21px' });


                }
            } else {
                if (j == 0) {
                    cell.css({ 'background-image': 'url(/content/utk/img/rad.png)', 'background-repeat': 'no-repeat', 'background-position': '-21px 0px' });
                } else if (j == maxY - 1) {
                    cell.css({ 'background-image': 'url(/content/utk/img/rad.png)', 'background-repeat': 'no-repeat', 'background-position': '-21px -42px' });

                } else {
                    cell.css({ 'background-image': 'url(/content/utk/img/rad.png)', 'background-repeat': 'no-repeat', 'background-position': '-21px -21px' });


                }

            }

        }
    }
}

function loadFilterA() {
    $('.filter-line-top input').styler();
    $('.filter-cell select').styler();
    $('.filter-group input[type="radio"]').change(function () {
        $('.filter-line-top input').trigger('refresh');
    });
}

function loadCompare() {
    $('.compare-table-wrapper').mCustomScrollbar({ axis: "x" });
    $('.to-cart-compare').click(function () {
        $.post('/Master/ru/Shopcart/toCart', { id: $(this).attr('arg'), count: 1, spec: false }, function (data) {
            $('.basket_view-basic').replaceWith(data);
            updateHead();
        });
        $(this).addClass('active');
        return false;

    });

    $('#remove-selected').click(function () {
        var v = $.cookie('ForCompare');
        if (!v)
            v = '';
        var saved = v.split(';');
        if (!saved)
            saved = new Array();
        saved = cleanArray(saved);


        $('.command-row td input[type="checkbox"]:checked').each(function () {
            var arg = $(this).val();
            if (saved.indexOf(arg) > -1) {
                saved.splice(saved.indexOf(arg), 1);
            }
        });
        $.cookie('ForCompare', saved.join(';'), { expires: 365, path: '/' });
        $('.command-row td input[type="checkbox"]').removeAttr('checked');
        document.location.reload();
        return false;
    });

    loadCompareTable();

    $('.compare-types a').click(function () {
        $.cookie('CompareType', $(this).attr('arg'), { expires: 30, path: '/' });
        loadCompareTable();
        return false;
    });


}

function loadCompareTable() {
    var type = $.cookie('CompareType');
    if (!type) {
        type = '0';
        $.cookie('CompareType', type, { expires: 30, path: '/' });
    }
    $('.compare-types a').removeClass('active');
    $('.compare-types a[arg="' + type + '"]').addClass('active');

    if (type == '0') {
        $('.compare-tbl tr').show();
    } else {

        $('.char-line').each(function () {
            var isDif = false;
            var differ = '';
            $(this).find('.char-cell').each(function () {
                if (differ == '') {
                    differ = $(this).text().trim();
                } else {
                    if ($(this).text().trim() != differ) {
                        isDif = true;
                    }
                }
            });
            if (!isDif) {
                $(this).hide();
            }
        });


    }
}

function loadCatalogPath() {
    var rp = $('#ReviewPath');
    if (rp.length) {
        var v = $.cookie('ReviewPath');
        if (!v)
            v = '';
        var saved = v.split(';', 6);
        if (!saved)
            saved = new Array();
        saved = cleanArray(saved);
        if (saved.indexOf(rp.val()) > -1) {
            saved.splice(saved.indexOf(rp.val()), 1);
        } else if (saved.length > 5) {
            saved.splice(0, 1);
        }
        saved.unshift(rp.val());
        $.cookie('ReviewPath', saved.join(';'), { expires: 365, path: '/' });
    }
}

function cleanArray(actual) {
    var newArray = new Array();
    for (var i = 0; i < actual.length; i++) {
        if (actual[i]) {
            newArray.push(actual[i]);
        }
    }
    return newArray;
}


function loadFeedLinks() {
    $('a[href="#feedback"]').unbind('click');
    $('a[href="#feedback"]').click(function () {
        showFeedBack();
        return false;
    });

}


function loadMagnifier() {
    /*    $('.goods_pic.utk_preview').loupe();*/

    var native_width = 0;
    var native_height = 0;

    //Now the mousemove function
    $(".magnify").mousemove(function (e) {
        //When the user hovers on the image, the script will first calculate
        //the native dimensions if they don't exist. Only after the native dimensions
        //are available, the script will show the zoomed version.
        if (!native_width && !native_height) {
            //This will create a new image object with the same image as that in .small
            //We cannot directly get the dimensions from .small because of the 
            //width specified to 200px in the html. To get the actual dimensions we have
            //created this image object.
            var image_object = new Image();
            image_object.src = $(".small-img").attr("src");

            //This code is wrapped in the .load function which is important.
            //width and height of the object would return 0 if accessed before 
            //the image gets loaded.
            native_width = image_object.width;
            native_height = image_object.height;
        }
        else {
            //x/y coordinates of the mouse
            //This is the position of .magnify with respect to the document.
            var magnify_offset = $(this).offset();
            //We will deduct the positions of .magnify from the mouse positions with
            //respect to the document to get the mouse positions with respect to the 
            //container(.magnify)
            var mx = e.pageX - magnify_offset.left;
            var my = e.pageY - magnify_offset.top;

            //Finally the code to fade out the glass if the mouse is outside the container
            if (mx < $(this).width() && my < $(this).height() && mx > 0 && my > 0) {
                $(".large-img").fadeIn(100);
            }
            else {
                $(".large-img").fadeOut(100);
            }
            if ($(".large-img").is(":visible")) {
                //The background position of .large will be changed according to the position
                //of the mouse over the .small image. So we will get the ratio of the pixel
                //under the mouse pointer with respect to the image and use that to position the 
                //large image inside the magnifying glass
                var rx = Math.round(mx / $(".small-img").width() * native_width - $(".large-img").width() / 2) * -1;
                var ry = Math.round(my / $(".small-img").height() * native_height - $(".large-img").height() / 2) * -1;
                var bgp = rx + "px " + ry + "px";

                //Time to move the magnifying glass with the mouse
                var px = mx - $(".large-img").width() / 2;
                var py = my - $(".large-img").height() / 2;
                //Now the glass moves with the mouse
                //The logic is to deduct half of the glass's width and height from the 
                //mouse coordinates to place it with its center at the mouse coordinates

                //If you hover on the image now, you should see the magnifying glass in action
                $(".large-img").css({ left: px, top: py, backgroundPosition: bgp });
            }
        }
    })

}

function showFeedBack() {

    $.get('/Master/ru/Forms/FeedBackPopupV2', {}, function (d) {
        $('#el_modal_window').remove();
        $('body').append(d);
    });
    return false;

}

function showBackCall() {

    $.get('/Master/ru/Forms/BackCallPopup', {}, function (d) {
        $('#el_modal_window').remove();
        $('body').append(d);
    });
    return false;

}

function loadOrderDetails() {
    $('.check_all_goods').change(function () {
        if ($(this).is(':checked'))
            $('.goods_view_history_line .goods_check input[type="checkbox"]').attr('checked', 'checked');
        else $('.goods_view_history_line .goods_check input[type="checkbox"]').removeAttr('checked');
    });

    $('.grid_5.omega .action_to_basket').click(function () {
        var args = '';
        $('.goods_view_history_line .goods_check input[type="checkbox"]').each(function () {
            args += $(this).attr('value') + ';' + $(this).attr('count') + ";";
        });

        $.post('/Master/ru/Shopcart/toCartAll', { ids: args }, function (data) {
            $('.basket_view-basic').replaceWith(data);
            updateHead();
        });
        return false;

    });
    $('.list_check button').click(function () {
        var args = '';
        $('.goods_view_history_line .goods_check input[type="checkbox"]:checked').each(function () {
            args += $(this).attr('value') + ';' + $(this).attr('count') + ";";
        });

        $.post('/Master/ru/Shopcart/toCartAll', { ids: args }, function (data) {
            $('.basket_view-basic').replaceWith(data);
            updateHead();
        });
        return false;
    });
    $('.goods_view_history_line .goods_form button').click(function () {
        var i = $(this).parents('.goods_form').find('input');
        $.post('/Master/ru/Shopcart/toCart', { id: i.attr('arg'), count: i.val(), spec: false }, function (data) {
            $('.basket_view-basic').replaceWith(data);
            updateHead();
        });

    });
}


function loadFilters() {
    $('#btn-choose').click(function () {
        document.location.href = $(this).attr('arg');
    });
    $("input[type='slider']").each(function () {
        var t = $(this);
        var min = Math.floor(parseFloat($(this).attr('from')));
        var max = Math.ceil(parseFloat($(this).attr('to')));
        var step = (max - min) / 5;
        var scale = ['|', Math.ceil(min + step), '|', Math.ceil(min + step * 2), '|', Math.ceil(min + step * 3), '|', Math.ceil(min + step * 4), '|'];
        $(this).slider({
            from: min,
            to: max,
            skin: "blue",
            limits: true,
            format: { format: '######', locale: 'ru' },
            scale: scale,
            callback: function (value) {
                showFilteredPopup(t);
            }
        });
        $('.list.property input[type="checkbox"]').unbind('change');
        $('.list.property input[type="checkbox"]').change(function () {
            showFilteredPopup($(this));
        });
    });
}

function showFilteredPopup(obj) {

    if (obj.parents('.rad-filter').length) {
        calcCountRadFilterA();
        return;
    }

    if (obj.parents('.inner-filter').length) {
        calcCountRadFilterC();
        return;
    }

    var text = '';
    $(obj).parents('.properties').find("input[type='slider']").each(function () {
        text += $(this).attr('arg') + '::' + $(this).val() + '^^';
    });
    $(obj).parents('.properties').find('.list.property').each(function () {
        text += $(this).attr('arg') + '::';
        var st = '';
        $(this).find('input[type="checkbox"]:checked').each(function () {
            st += $(this).val() + '::';
        });
        text += st + "^^";
    });
    var o = $(obj).parents('.properties').find('#ObjID').val();
    $.post('/Master/ru/ClientCatalog/FilterCount', { args: text, pageID: $('#PageID').val(), objID: o }, function (d) {

        var link = document.location.href;
        if (link.indexOf('filter=') >= 0)
            link = link.substr(0, link.indexOf('filter=') - 1);
        if (link.indexOf('?') < 0)
            link += '?' + d.Link;
        else link += '&' + d.Link;
        var html = '<div class="tooltip force horizontal"><span style="white-space:nowrap">Товаров найдено: ' + d.Count + '</span><br><a href="' + (d.IsInCatalog == '1' ? link : d.Link) + '">Показать</a></div>';
        $('.tooltip.force.horizontal').remove();
        $('#btn-choose').attr('arg', link);
        obj.parents('.property').append(html);
    });
}



function showRadFilterResult() {
    var text = '';
    $(".rad-filter input[type='slider']").each(function () {
        text += $(this).attr('arg') + '::' + $(this).val() + '^^';
    });
    $('.rad-filter input[type="radio"]:checked').each(function () {
        text += $(this).attr('arg') + '::';
        var st = '';
        st += $(this).val() + '::';
        text += st + "^^";
    });
    $('.rad-filter select').each(function () {
        if ($(this).val().length) {
            text += $(this).attr('arg') + '::';
            var st = '';
            st += $(this).val() + '::';
            text += st + "^^";
        }
    });
    $.post('/Master/ru/ClientCatalog/FilterCount', { args: text, pageID: $('#PageID').val(), names: 'Тип;Ширина;Высота;Мощность;Подключение' }, function (d) {
        var link = document.location.href;
        if (link.indexOf('filter=') >= 0)
            link = link.substr(0, link.indexOf('filter=') - 1);
        if (link.indexOf('?') < 0) {
            if (d.Link.substr(0, 1) == '/') {
                link = d.Link;
            } else {
                link += '?' + d.Link;
            }
        } else link += '&' + d.Link;
        document.location.href = (d.IsInCatalog == '1' ? link : d.Link);
    });
}

function showRadFilterResultB() {
    var text = '';
    $(".module_content_action_top_filter_2 input[type='hidden']").each(function () {
        text += $(this).attr('arg') + '::' + $(this).val() + '^^';
    });


    var tpb = '';
    $('.module_content_action_top_filter_2 .type-box:checked').each(function () {
        if (tpb == '') {
            tpb += $(this).attr('arg') + '::';
        }
        tpb += $(this).val() + '::';
    });
    if (tpb.length) {
        text += tpb + "^^";
    }

    tpb = '';
    $('.module_content_action_top_filter_2 .con-box:checked').each(function () {
        if (tpb == '') {
            tpb += $(this).attr('arg') + '::';
        }
        tpb += $(this).val() + '::';
    });
    if (tpb.length) {
        text += tpb + "^^";
    }

    $.post('/Master/ru/ClientCatalog/FilterCount', { args: text, pageID: $('#PageID').val(), names: 'Тип;Ширина;Высота;Мощность;Подключение' }, function (d) {
        var link = document.location.href;
        if (link.indexOf('filter=') >= 0)
            link = link.substr(0, link.indexOf('filter=') - 1);
        if (link.indexOf('?') < 0) {
            if (d.Link.substr(0, 1) == '/') {
                link = d.Link;
            } else {
                link += '?' + d.Link;
            }
        } else link += '&' + d.Link;
        document.location.href = (d.IsInCatalog == '1' ? link : d.Link);
    });
}


function loadAdressDelete() {
    $('.address-delete').click(function () {
        $.post('/Master/ru/Cabinet/ProfileAdressesDelete', { ID: $(this).attr('arg') }, function (d) {
            $('#ProfileAdresses').replaceWith(d);
        });
    });
}

function loadAvatarEdit() {
    $('.action_delete_avatar').unbind('click');
    $('.action_delete_avatar').click(function () {
        $.post('/Master/ru/Cabinet/AvatarDelete', {}, function (d) {
            if (d != "fail") {
                $('.action_delete_avatar').remove();
                var src = $('.profile_avatar .user_pic').attr('src');
                src += '&rnd=' + new Date().getTime();
                $('.profile_avatar .user_pic').attr('src', src);
            }
        });
    });
    $('#AvatarUpload').each(function () {
        var fu = $(this);
        fu.fileupload({
            autoUpload: true,
            url: '/Master/ru/Users/UploadAvatar?uid=' + $('.profile_avatar .user_pic').attr('arg'),
            dataType: 'json',
            add: function (e, data) {


                //$('#message').html('Загрузка изображения...');

                var jqXHR = data.submit()
                    .success(function (data, textStatus, jqXHR) {
                        if (data.isUploaded) {
                            //$('#message').html('Изображение загружено на сервер');
                            var src = $('.profile_avatar .user_pic').attr('src');
                            src += '&rnd=' + new Date().getTime();
                            $('.profile_avatar .user_pic').attr('src', src);
                            if (!$('.profile_avatar .action_delete_avatar').length)
                                $('.profile_avatar').append('<a href="#nogo" class="action_delete_avatar"><span>×</span><ins>Удалить аватар</ins></a>');
                            loadAvatarEdit();
                        } else {
                            //$('#message').html(data.message);
                        }

                    })
                    .error(function (data, textStatus, errorThrown) {
                        if (typeof (data) != 'undefined' || typeof (textStatus) != 'undefined' || typeof (errorThrown) != 'undefined') {
                            alert("onerror:" + textStatus + errorThrown + data);
                        }
                    });
            },
            fail: function (event, data) {
                if (data.files[0].error) {
                    alert("onfail:" + data.files[0].error);
                }
            }
        });
    });
}


function loadCheck() {
    $('.goods_view_check .btn.increase').unbind('click');
    $('.goods_view_check .btn.reduce').unbind('click');

    $('.goods_view_check .btn.increase').click(function () {
        var i = $(this).parent().find('input');
        var val = parseInt(i.val());
        if (val == 0)
            val = 1;
        val++;
        i.val(val);
        $(this).parent().find('.btn.reduce').removeClass('disabled');
        var arg = $(this).parents('.goods_view_check').attr('data-item_id');
        $.get('/Master/ru/ShopCart/CartConfirm', { productID: arg, count: val }, function (d) {
            $('#CartConfirm').replaceWith(d);
            loadCheck();
            refreshCartHeader();
        });
        return false;

    });
    $('.goods_view_check .btn.reduce').click(function () {
        var i = $(this).parent().find('input');
        var val = parseInt(i.val());
        if (val <= parseInt(i.attr('min'))) {
            return false;
        }
        val--;
        i.val(val);
        if (val == parseInt(i.attr('min'))) {
            $(this).addClass('disabled');
        }
        var arg = $(this).parents('.goods_view_check').attr('data-item_id');
        $.get('/Master/ru/ShopCart/CartConfirm', { productID: arg, count: val }, function (d) {
            $('#CartConfirm').replaceWith(d);
            loadCheck();
            refreshCartHeader();
        });
        return false;
    });

}

function loadMapsAPI() {
    if ($('#YMap_Container').length) {
        addScript('https://maps.googleapis.com/maps/api/js?key=' + $('#GoogleAPI').val() + '&sensor=false&callback=mapsApiReady');
    }
    if ($('#ymaps_container').length) {
        addScript('https://maps.googleapis.com/maps/api/js?key=' + $('#GoogleAPI').val() + '&sensor=false&callback=mapsApiReadyAdd');
    }
}

function mapsApiReady() {
    mapInit();
}
function mapsApiReadyAdd() {
    mapInitAdd();
}
function addScript(url, callback) {
    var script = document.createElement('script');
    if (callback) script.onload = callback;
    script.type = 'text/javascript';
    script.src = url;
    document.body.appendChild(script);
}
function gmapsInit() {

    loadMapsAPI();
}

function addMarker(markerMarkup, type, fromData, callback) {
    var icon = '/content/client/i/markers/';

    if (fromData)
        type = (fromData.IsRegion ? 1 : 0);


    if (type == 0)
        icon += "marker5.png";
    if (type == 1)
        icon += "marker6.png";

    var latlng = map.getCenter();


    if (fromData)
        latlng = new google.maps.LatLng(fromData.PointPosition.Lat, fromData.PointPosition.Lng);

    currentPoint = new google.maps.Marker({
        map: map,
        position: latlng,
        visible: true,
        icon: icon,
        draggable: true,
        zIndex: 2000
    });

    var ibOptions = {
        content: markerMarkup,
        alignBottom: false,
        pixelOffset: new google.maps.Size(35, -94),
        infoBoxClearance: new google.maps.Size(10, 55)
    };

    var ib = new InfoBox(ibOptions);
    ib.PointType = type;
    ib.ObjectID = 0;
    if (fromData)
        ib.ObjectID = fromData.ID;
    ib.marker = currentPoint;
    currentPoint.ib = ib;
    if (type == 1) {
        var polygone;
        if (!fromData) {
            var bounds = map.getBounds();
            var leftUp = bounds.getNorthEast();
            var bottomDown = bounds.getSouthWest();
            var distance = Math.abs(leftUp.lat() - bottomDown.lat()) / $('#map').width();

            var pixelMargin = 50;

            var magicDigit = pixelMargin * distance;

            var center = map.getCenter();
            polygone = [
                new google.maps.LatLng(center.lat() - magicDigit / 2, center.lng() - magicDigit),
                new google.maps.LatLng(center.lat() + magicDigit / 2, center.lng() - magicDigit),
                new google.maps.LatLng(center.lat() + magicDigit / 2, center.lng() + magicDigit),
                new google.maps.LatLng(center.lat() - magicDigit / 2, center.lng() + magicDigit),
                new google.maps.LatLng(center.lat() - magicDigit / 2, center.lng() - magicDigit)
            ];
        } else {
            polygone = new Array();
            for (var i = 0; i < fromData.RegionPosition.length; i++) {
                polygone.push(new google.maps.LatLng(fromData.RegionPosition[i].Lat, fromData.RegionPosition[i].Lng));
            }
        }
        var defPolygon = new google.maps.Polygon({
            paths: polygone,
            fillColor: '#000000',
            fillOpacity: 0.35,
            strokeWeight: 5,
            strokeColor: "#000000",
            strokeOpacity: 0.8,
            clickable: true,
            editable: true,
            zIndex: 1000,
            draggable: true
        });
        defPolygon.marker = currentPoint;
        currentPoint.polygon = defPolygon;

        defPolygon.setMap(map);


        google.maps.event.addListener(defPolygon, 'dragstart', function () {
            defPolygon.FirstPoint = defPolygon.getPath().getAt(0);
            closeBox();
            currentPoint.setMap(null);
        });

        google.maps.event.addListener(defPolygon, 'drag', function () {

        });

        google.maps.event.addListener(defPolygon, 'dragend', function () {

            var latChange = defPolygon.getPath().getAt(0).lat() - defPolygon.FirstPoint.lat();
            var lngChange = defPolygon.getPath().getAt(0).lng() - defPolygon.FirstPoint.lng();
            var oldPos = currentPoint.getPosition();
            currentPoint.setPosition(new google.maps.LatLng(oldPos.lat() + latChange, oldPos.lng() + lngChange));
            currentPoint.setMap(map);
            geocodePosition(currentPoint.getPosition(), function (adress) {
                if (!currentPoint.UserData)
                    currentPoint.UserData = new Object();
                currentPoint.UserData.ZoneAdress = adress;
            });
        });


    }

    google.maps.event.addListener(currentPoint, 'click', function () {
        openBoxTimeout = setTimeout('console.log("moved")', openBoxTimeoutDelay);
        toggleBox();
        //openBox();
    });

    google.maps.event.addListener(ib, 'domready', function () {
        if (ib.ObjectID > 0) {
        } else {
            loadBoxFilledValues(ib);
        }

        forms();
        select();
        initClient();
        initSave();
    });

    google.maps.event.addListener(currentPoint, 'dragstart', function () {
        closeBox();
    });

    google.maps.event.addListener(currentPoint, 'drag', function () {
        var polygon = currentPoint.polygon;
        if (polygon) {
            var pos = currentPoint.getPosition();
            var isWithinPolygon = containsLatLng(polygon, pos);
            if (!isWithinPolygon) {
                currentPoint.setDraggable(false);
                setTimeout(function () {
                    currentPoint.setDraggable(true);
                }, 200);
                currentPoint.setPosition(currentPoint.LastPointInside);
                return false;
            } else {
                currentPoint.LastPointInside = pos;
            }
        }
    });

    google.maps.event.addListener(currentPoint, 'dragend', function () {
        geocodePosition(currentPoint.getPosition(), function (adress) {
            if (!currentPoint.UserData)
                currentPoint.UserData = new Object();
            currentPoint.UserData.ZoneAdress = adress;
            openBox(currentPoint.ib);
        });
    });


    if (fromData)
        saveFieldsInMarker(ib, fromData);

    if (callback)
        callback();

    return currentPoint;
}


function mapInit() {
    var marker;
    var mapOptions = {
        zoom: 8,
        center: new google.maps.LatLng(55.7583574, 37.5827574),
        mapTypeId: google.maps.MapTypeId.ROADMAP,
    };
    var hasPoint = false;
    if ($('.address_list input[type="radio"]:checked').length) {
        if ($('.address_list input[type="radio"]:checked').attr('data-coordinate')) {
            var args = $('.address_list input[type="radio"]:checked').attr('data-coordinate').split(':');
            mapOptions.center = new google.maps.LatLng(parseFloat(args[0]), parseFloat(args[1]));
            mapOptions.zoom = parseInt(args[2]);
            hasPoint = true;
        }
    }

    var map = new google.maps.Map(document.getElementById('YMap_Container'), mapOptions);
    map.panTo(mapOptions.center);

    if (hasPoint) {
        marker = new google.maps.Marker({
            position: mapOptions.center,
            map: map,
            draggable: false,
            title: $('.address_list input[type="radio"]:checked').attr('text')
        });
    }

    $('.address_list input[type="radio"]').change(function () {
        if (marker)
            marker.setMap(null);
        if ($('.address_list input[type="radio"]:checked').attr('data-coordinate')) {
            var args = $('.address_list input[type="radio"]:checked').attr('data-coordinate').split(':');
            marker = new google.maps.Marker({
                position: new google.maps.LatLng(parseFloat(args[0]), parseFloat(args[1])),
                map: map,
                draggable: false,
                title: $('.address_list input[type="radio"]:checked').attr('text')
            });


            map.setCenter(new google.maps.LatLng(parseFloat(args[0]), parseFloat(args[1])));
            map.setZoom(parseInt(args[2]));
            hasPoint = true;
        } else {
            hasPoint = false;
        }
        $.post('/Master/ru/ShopCart/SaveField', { value: $('.address_list input[type="radio"]:checked').val(), name: "LastAddressID" }, function (d) {
            $('a[rel="saveaddr"]').removeClass('disabled');
        });

    });

    $('a[rel="saveaddr"]').click(function () {
        $.post('/Master/ru/ShopCart/SaveField', { value: $('.address_list input[type="radio"]:checked').val(), name: "LastAddressID" }, function (d) {

        });
    });

}

function getAdress(latlng, callback) {
    var url = "http://maps.google.com/maps/api/geocode/json?latlng=%s,%s&sensor=false";
    $.get(sprintf(url, latlng.lat(), latlng.lng()), function (responce) {

        if (responce.results.length && responce.status == google.maps.GeocoderStatus.OK) {
            return callback(responce.results[0]);
        } else {
            //return callback("");
        }
    });

}

function getAdressCoords(address, callback) {
    //http://maps.google.com/maps/api/geocode/json?address=1600+Amphitheatre+Parkway,+Mountain+View,+CA&sensor=false
    var url = "http://maps.google.com/maps/api/geocode/json?address=%s&sensor=false";
    $.get(sprintf(url, address), function (responce) {

        if (responce.results.length && responce.status == google.maps.GeocoderStatus.OK) {
            return callback(responce);
        } else {
            //return callback("");
        }
    });

}


function mapInitAdd() {

    $('#ymaps_search_address button').click(function () {
        if ($('#ymaps_search_address input').val().trim().length) {
            getAdressCoords($('#ymaps_search_address input').val().trim(), function (d) {
                if (d.results.length && d.status == google.maps.GeocoderStatus.OK) {
                    $('input[name="Lat"]').val(d.results[0].geometry.location.lat);
                    $('input[name="Lng"]').val(d.results[0].geometry.location.lng);
                    $('input[name="Zoom"]').val(13);

                    if (d.results[0].address_components.length) {
                        for (var i = 0; i < d.results[0].address_components.length; i++) {
                            if (d.results[0].address_components[i].types[0] == 'street_number') {
                                var str = d.results[0].address_components[i].long_name;
                                if (str.indexOf(' ') > 0)
                                    str = str.substr(0, str.indexOf(' ') - 1);
                                $('input[name="House"]').val(str);
                            }
                            if (d.results[0].address_components[i].types[0] == 'route') {

                                var rt = d.results[0].address_components[i].long_name.replace('улица', '').replace('Улица', '').trim();
                                if (rt != 'Unnamed Road')
                                    $('input[name="Street"]').val(rt);
                                else $('input[name="Street"]').val('');
                            }
                            if (d.results[0].address_components[i].types[0] == 'locality') {
                                $('input[name="Town"]').val(d.results[0].address_components[i].long_name);
                            }
                        }
                    }
                    placeMarker(new google.maps.LatLng(d.results[0].geometry.location.lat, d.results[0].geometry.location.lng));
                } else {
                    alert('Указанный адрес не найден');
                }
            });
        }
        return false;
    });

    var marker;
    var mapOptions = {
        zoom: 8,
        center: new google.maps.LatLng(55.7583574, 37.5827574),
        mapTypeId: google.maps.MapTypeId.ROADMAP,
    };
    var map = new google.maps.Map(document.getElementById('ymaps_container'), mapOptions);
    map.panTo(mapOptions.center);


    google.maps.event.addListener(map, 'click', function (event) {
        placeMarker(event.latLng);

    });

    function placeMarker(location) {
        if (marker) {
            marker.setMap(null);
        }
        marker = new google.maps.Marker({
            position: location,
            map: map,
            draggable: true
        });

        google.maps.event.addListener(
            marker,
            'dragend',
            function () {
                loadData(marker.position);
                map.setCenter(marker.position);
                if (map.getZoom() < 13)
                    map.setZoom(13);

            }
        );
        loadData(location);
        map.setCenter(location);
        if (map.getZoom() < 13)
            map.setZoom(13);


    }
    function loadData(location) {
        getAdress(location, function (d) {
            $('input[name="Lat"]').val(location.lat());
            $('input[name="Lng"]').val(location.lng());
            $('input[name="Zoom"]').val(map.getZoom());

            if (d.address_components.length) {
                for (var i = 0; i < d.address_components.length; i++) {
                    if (d.address_components[i].types[0] == 'street_number') {
                        var str = d.address_components[i].long_name;
                        if (str.indexOf(' ') > 0)
                            str = str.substr(0, str.indexOf(' ') - 1);
                        $('input[name="House"]').val(str);
                    }
                    if (d.address_components[i].types[0] == 'route') {
                        var rt = d.address_components[i].long_name.replace('улица', '').replace('Улица', '').trim();
                        if (rt != 'Unnamed Road')
                            $('input[name="Street"]').val(rt);
                        else $('input[name="Street"]').val('');
                    }
                    if (d.address_components[i].types[0] == 'locality') {
                        $('input[name="Town"]').val(d.address_components[i].long_name);
                    }
                }
            }
            checkForm();
        });

    }

    function checkForm() {
        if ($('input[name="House"]').val().trim().length && $('input[name="Street"]').val().trim().length && $('input[name="Town"]').val().trim().length && $('input[name="Zoom"]').val().trim().length && $('input[name="Lat"]').val().trim().length && $('input[name="Lng"]').val().trim().length) {
            $('#address_submit').removeAttr('disabled');
        } else {
            $('#address_submit').attr('disabled', 'disabled');
        }

    }

    $('#address_form input').keyup(function () {
        checkForm();
    });
    $('#address_submit').click(function () {
        var args = {
            'Town': $('input[name="Town"]').val(),
            'Street': $('input[name="Street"]').val(),
            'House': $('input[name="House"]').val(),
            'Building': $('input[name="Building"]').val(),
            'BuildingPart': $('input[name="BuildingPart"]').val(),
            'Doorway': $('input[name="Doorway"]').val(),
            'Floor': $('input[name="Floor"]').val(),
            'Flat': $('input[name="Flat"]').val(),
            'Comment': $('input[name="Comment"]').val(),
            'Lat': $('input[name="Lat"]').val().replace('.', ','),
            'Lng': $('input[name="Lng"]').val().replace('.', ','),
            'Zoom': $('input[name="Zoom"]').val(),
        };
        $.post('/Master/ru/ShopCart/SaveAddress', args, function (d) {
            if (d == 1) {

                if (document.location.href.indexOf('hidemenu=1') >= 0) {
                    document.location.href = '/cabinet?view=adresses';
                } else {
                    document.location.href = document.location.href.replace('&edit=1', '');
                }

            } else {
                alert(d);
            }
        });
    });
}



function refreshCartHeader() {
    $.get('/Master/ru/ShopCart/CartBlock', function (d) {
        $('.basket_view-basic').replaceWith(d);
    });
}

function loadCart() {
    $('#basket_all_goods .btn.increase').unbind('click');
    $('#basket_all_goods .btn.reduce').unbind('click');
    $('.basket_remove_good').unbind('click');

    $('.basket_remove_good').click(function () {
        var arg = $(this).parents('.goods_view_basket').attr('data-item_id');
        $.get('/Master/ru/ShopCart/CartEditor', { productID: arg, count: 0 }, function (d) {
            $('#basket_content').replaceWith(d);
            loadCart();
            refreshCartHeader();
            updateHead();
        });

        return false;
    });

    $('#basket_all_goods .btn.increase').click(function () {
        var i = $(this).parent().find('input');
        var val = parseInt(i.val());
        if (val == 0)
            val = 1;
        val++;
        i.val(val);
        $(this).parent().find('.btn.reduce').removeClass('disabled');
        var arg = $(this).parents('.goods_view_basket').attr('data-item_id');
        $.get('/Master/ru/ShopCart/CartEditor', { productID: arg, count: val }, function (d) {
            $('#basket_content').replaceWith(d);
            loadCart();
            refreshCartHeader();
            updateHead();
        });
        return false;

    });
    $('#basket_all_goods .btn.reduce').click(function () {
        var i = $(this).parent().find('input');
        var val = parseInt(i.val());
        if (val <= parseInt(i.attr('min'))) {
            return false;
        }
        val--;
        i.val(val);
        if (val == parseInt(i.attr('min'))) {
            $(this).addClass('disabled');
        }
        var arg = $(this).parents('.goods_view_basket').attr('data-item_id');
        $.get('/Master/ru/ShopCart/CartEditor', { productID: arg, count: val }, function (d) {
            $('#basket_content').replaceWith(d);
            loadCart();
            refreshCartHeader();
            updateHead();
        });
        return false;
    });
}

function loadSocialPopup() {
    var h = $('.auth-result-hidden');
    if (h.length) {
        if (h.attr('redirect').length) {
            document.location.href = h.attr('redirect');
        }
        if (h.attr('arg').length) {
            if (h.attr('arg') == 'Login') {
                showAuthPopup(h.val());
            }
            else if (h.attr('arg') == 'Register') {
                showRegPopup(h.val());
            }

        }
    }
}

function closeDialog(btn) {
    $($(btn).parents('form').attr('data-ajax-update')).remove();
    return false;

}
function closeDialogNative(btn) {
    $(btn).parents('#el_modal_window').parent().remove();
    return false;

}

function showAuthPopup(msg) {
    if ($('#el_modal_window').length)
        $('#el_modal_window').parent().parent().remove();

    $.get('/Master/ru/Account/SimpleLogin', { PageURL: $('PageURL').val() }, function (d) {
        $('body').append(d);
        if (msg && msg.length) {
            $('<div class="grid_8 messages_alert"><span>' + msg + '</span></div>').insertAfter($('#el_modal_window h2'));
        }
        closeDialog();
    });
    return false;
}

function showRegPopup(msg) {
    if ($('#el_modal_window').length)
        $('#el_modal_window').parent().parent().remove();

    $.get('/Master/ru/Account/SimpleRegister', { PageURL: $('PageURL').val() }, function (d) {
        $('body').append(d);
        if (msg && msg.length) {
            $('<div class="grid_8 messages_alert"><span>' + msg + '</span></div>').insertAfter($('#el_modal_window h2'));
        }
        closeDialog();
    });
    return false;
}
function showRestorePopup(msg) {
    if ($('#el_modal_window').length)
        $('#el_modal_window').parent().parent().remove();

    $.get('/Master/ru/Account/SimpleRestore', { PageURL: $('PageURL').val() }, function (d) {
        $('body').append(d);
        if (msg && msg.length) {
            $('<div class="grid_8 messages_alert"><span>' + msg + '</span></div>').insertAfter($('#el_modal_window h2'));
        }
        closeDialog();
    });
    return false;
}



function DoAjaxPostAndMore(btnClicked) {
    var $form = $(btnClicked).parents('form');

    $.ajax({
        type: "POST",
        url: $form.attr('action'),
        data: $form.serialize(),
        error: function (xhr, status, error) {
            //do something about the error
        },
        success: function (response) {
            //do something with response
            $($form.attr('data-ajax-update')).replaceWith(response);

        }
    });

    return false;// if it's a link to prevent post

}
/*

function submitForm(selector) {
    //    console.log($(selector).find('form'));

    $('form[data-ajax-update="' + selector + '"]').unbind('submit').trigger('submit');

    //    $(selector).find('form').trigger('submit');
    return false;
}

*/

function loadSliders() {
    $('#page_login_slider').each(function () {
        var timer;
        var parent = $(this);
        var bigs = parent.find('.page_slider_content');
        var smalls = parent.find('.page_slider_preview');
        if (!bigs.find('.selected').length) {
            big.find('a:first-child').addClass('selected');
            smalls.find('a').removeClass('selected');
            smalls.find('a:first-child').addClass('selected');
        }
        var cnt = smalls.find('a').length;

        var time = 4000;
        var anim = 300;
        var index = 0;
        timer = setImage();

        function setImage() {
            timer = setTimeout(function () {
                index++;
                if (index >= cnt) {
                    index = 0;
                }
                bigs.find('a.selected').fadeTo(anim, 0, function () {
                    $(this).removeClass('selected');
                });
                $(bigs.find('a').get(index)).fadeTo(anim, 1, function () {
                    $(this).addClass('selected');
                });
                //smalls.find('a.selected').fadeTo(anim, 0, function() {
                smalls.find('a.selected').removeClass('selected');
                //});
                //$(smalls.find('a').get(index)).fadeTo(anim, 1, function () {
                $(smalls.find('a').get(index)).addClass('selected');
                //});
                setImage();
            }, time);
            return timer;
        }

        bigs.mouseover(function () {
            clearTimeout(timer);
            timer = null;
        }).mouseout(function () {
            if (timer) {
                clearTimeout(timer);
                timer = null;
            }
            timer = setImage();
        }
        );

        smalls.mouseover(function () {

            clearTimeout(timer);
            timer = null;
        }).mouseout(function () {
            if (timer) {
                clearTimeout(timer);
                timer = null;
            }
            timer = setImage();
        });
        smalls.find('a').click(function () {
            index = smalls.find('a').index($(this));
            console.log(index);
            bigs.find('a.selected').fadeTo(anim, 0, function () {
                $(this).removeClass('selected');
            });
            $(bigs.find('a').get(index)).fadeTo(anim, 1, function () {
                $(this).addClass('selected');
            });
            smalls.find('a.selected').removeClass('selected');
            $(this).addClass('selected');
            clearTimeout(timer);
            timer = null;
            return false;
        });
    });

}

function loadCrumbs() {
    var catItem = $('.bread_crumbs a[href="/catalog"]');
    if (catItem && catItem.length) {
        var links = $('.navigate_left .catalogue_menu_wrapper').find('a[data-level="1"]').clone().removeClass('active');
        $('#bread_with_catalogue .catalogue_menu_wrapper').append(links);

        var mm_content = $('.navigate_left .megamenu_content');
        if (mm_content && mm_content.length) {
            $('#bread_with_catalogue .megamenu_content').replaceWith(mm_content.clone());
        } else {
            $.get('/Master/ru/CommonBlocks/CatalogMenuChildren', { url: document.location.href }, function (d) {
                $('#bread_with_catalogue .megamenu_content').replaceWith(d);
            });

        }

    }
    catItem.mouseover(function () {
        if ($('.navigate_left .catalogue_menu_wrapper').html().trim().length) {
            $('.bread_crumbs').addClass('zTop');
            $('#bread_with_catalogue').addClass('active');
        }
    });
}



function loadBlurs() {
    $('.blur-box').focus(function () {
        if ($(this).val() == $(this).attr('def'))
            $(this).val('');
    }).blur(function () {
        if ($.trim($(this).val()) == '')
            $(this).val($(this).attr('def'));

    });
    $('.blur-box[req="1"]').parents('form').find('button').click(function () {
        var reqs = $(this).parents('form').find('input[req="1"]');
        var error = false;
        reqs.each(function () {
            if ($.trim($(this).val()) == '' || $.trim($(this).val()) == $(this).attr('def'))
                error = true;
        });
        return !error;
    });
}


function loadSearch() {

    $('.btn-xs.dropdown-toggle').click(function () {
        $(this).parent().toggleClass('open');
        //        $('#search-cat-dropdown-inner').toggleClass('open');
        return false;
    });
    $('body').click(function () {
        //$('#search-cat-dropdown').removeClass('open');
        $('.dropdown.b-form-search__dropdown').removeClass('open');

    });

    $('.this_cat_only').find('#this_cat_only').change(function () {
        if ($(this).is(':checked')) {

            $(this).parents('form').find('#search-cat-t').html($(this).attr('cn'));
            $(this).parents('form').find('#search-cat-t').parent().attr('title', $(this).attr('cn'));
        } else {

            $(this).parents('form').find('#search-cat-t').html($(this).parents('form').find('#search-cat-menu .selected a').html());
            $(this).parents('form').find('#search-cat-t').parent().attr('title', $(this).parents('form').find('#search-cat-menu .selected a').html());
        }
    });

    $('.search_field_text').each(function () {
        var exist = $(this).find('#current-section').val();
        if (exist.length) {
            var $this = $(this).find('#search-cat-menu li a[arg="' + exist + '"]');
            if ($this.length) {
                $(this).find('#search-cat-menu li').removeClass('selected');
                $this.parent().addClass('selected');
                $(this).find('#search-cat-t').html($this.html());
                $(this).find('#search-cat-t').parent().attr('title', $this.html());
                $(this).find('#search-cat-dropdown').removeClass('open');
                if ($(this).find('#this_cat_only').attr('arg') != $(this).find('#search-cat-menu .selected a').attr('arg')) {
                    $(this).find('#this_cat_only').removeAttr('checked');
                }

            }
        }


    })
    $('.dropdown-menu.b-form-search__dropdown-menu li a').click(function () {
        $(this).parents('.dropdown-menu.b-form-search__dropdown-menu').find('li').removeClass('selected');
        $(this).parent().addClass('selected');
        $(this).parents('form').find('#search-cat-t').html($(this).html());
        //$('#search-cat-t').html($(this).html());
        $(this).parents('form').find('#search-cat-t').parent().attr('title', $(this).html());
        //$('#search-cat-t').parent().attr('title', $(this).html());
        $(this).parents('form').find('#search-cat-dropdown').removeClass('open');
        //$('#search-cat-dropdown').removeClass('open');

        var thiscat = $(this).parents('form').find('#this_cat_only');

        if (thiscat.attr('arg') != $(this).attr('arg')) {
            thiscat.removeAttr('checked');
        }
        /*
                if ($('#this_cat_only').attr('arg') != $('#search-cat-menu .selected a').attr('arg')) {
                    $('#this_cat_only').removeAttr('checked');
                }
        */
        return false;
    });

    $(".search input[type='submit']").click(function () {
        if ($(".search input[type='text']").val().length && $(".search input[type='text']").val() != $(".search input[type='text']").attr('def')) {
            document.location.href = '/search?word=' + encodeURIComponent($('.search input[type="text"]').val());
        }
        return false;
    });

    function split(val) {
        return val.split(/,\s*/);
    }
    function extractLast(term) {
        return split(term).pop();
    }

    function getTargetCat(searchForm) {
        if (!searchForm) return "";
        if (searchForm.find("#this_cat_only").length && searchForm.find("#this_cat_only").is(':checked') && searchForm.find("#this_cat_only").attr('arg') != '0') {
            return "&section=" + searchForm.find("#this_cat_only").attr('arg');
        }
        else if (searchForm.find('#search-cat-menu .selected').length && searchForm.find('#search-cat-menu .selected a').attr('arg') != '0') {
            return "&section=" + searchForm.find('#search-cat-menu .selected a').attr('arg');
        } else {
            return "";
        }
    }

    var searchForm;
    $('.search').parents('form').submit(function () {
        searchForm = $(this);
        var location = '/search?word=' + encodeURIComponent(searchForm.find('input[type="text"]').val());
        location += getTargetCat(searchForm);
        /*
                if (searchForm.find("#this_cat_only") && searchForm.find("#this_cat_only").is(':checked') && searchForm.find("#this_cat_only").attr('arg') != '0') {
                    location += "&section=" + searchForm.find("#this_cat_only").attr('arg');
                }
        */
        document.location.href = location;
        return false;
    });
    $(".search input[type='text']")
        // don't navigate away from the field on tab when selecting an item
        .bind("keydown", function (event, ui) {
            //console.log(ui.item.value)
            searchForm = $(this).parents('form');
            if (event.keyCode === $.ui.keyCode.TAB &&
                $(this).data("ui-autocomplete").menu.active) {
                event.preventDefault();
            }
            if (event.keyCode === $.ui.keyCode.ENTER ||
                event.keyCode === $.ui.keyCode.ENTER) {
                var location = '/search?word=' + encodeURIComponent($(this).val());
                /*
                                if (searchForm.find("#this_cat_only") && searchForm.find("#this_cat_only").is(':checked') && searchForm.find("#this_cat_only").attr('arg') != '0') {
                                    location += "&section=" + searchForm.find("#this_cat_only").attr('arg');
                                }
                */
                location += getTargetCat(searchForm);
                document.location.href = location;
            }
        })
        .autocomplete({
            source: function (request, response) {
                var section = '0';
                /*
                                if (searchForm.find("#this_cat_only") && searchForm.find("#this_cat_only").is(':checked') && searchForm.find("#this_cat_only").attr('arg') != '0') {
                                    section = searchForm.find("#this_cat_only").attr('arg');
                                }
                */
                var add = getTargetCat(searchForm);
                if (add.length > 0) {
                    section = add.replace('&section=', '');
                }

                $.getJSON("/Master/ru/Search/FastList", {
                    term: extractLast(request.term),
                    section: section
                }, response);
            },
            search: function () {
                // custom minLength
                var term = extractLast(this.value);
                if (term.length < 1) {
                    return false;
                }
            },
            focus: function (event, ui) {
                searchForm.find(".search input[type='text']").val(ui.item.value);
            },
            select: function (event, ui) {
                var location = '/search?word=' + encodeURIComponent(ui.item.value);//$(".search input[type='text']").val()
                /*
                                if (searchForm.find("#this_cat_only") && searchForm.find("#this_cat_only").is(':checked') && searchForm.find("#this_cat_only").attr('arg') != '0') {
                                    location += "&section=" + searchForm.find("#this_cat_only").attr('arg');
                                }
                */
                location += getTargetCat(searchForm);
                document.location.href = location;
                return false;
            }
        });
}

function loadStarAndComment() {
    $('.page_stars_large span').unbind('click');
    $('.comment_rate').unbind('click');

    $('.comment_rate').click(function () {
        var $this = $(this);
        $.post('/Master/ru/CommonBlocks/CommentRating', { CommentID: $this.parents('.rate_wrapper').attr('data-commentid'), Useful: $this.hasClass('rate_up') ? '1' : '0' }, function (d) {
            if (d.length) {
                var args = d.split(';');
                $this.parents('.rate_wrapper').find('.rate_up').html('<i class="icon_c_like"></i><b>Да</b> (<span>' + args[0] + '</span>)');
                $this.parents('.rate_wrapper').find('.rate_down').html('<i class="icon_c_dislike"></i><b>Нет</b> (<span>' + args[1] + '</span>)');
                if ($this.hasClass('rate_up')) {

                } else {

                }
            }
        });
    });

    $('.page_stars_large span').click(function () {
        var $this = $(this);
        if ($this.parent().attr('auth-user') == '1') {
            $.post('/Master/ru/ClientCatalog/SaveMark', { mark: $this.attr('data-ratingpos'), book: $this.parent().attr('data-itemid') }, function (d) {
                $('.comment_control').replaceWith(d);
                $('.goods_view_item .rating_star').replaceWith($('.comment_control .rating_star').not('.large').clone());
                $('.goods_view_item .goods_property.signature').html($('.comment_control .number_votes_text').text());
                loadStarAndComment();
            });
        } else {
            showAuthPopup();
        }
    });
}

function moveLeft() {
    var width = ($('.bottom_slider .goods_view_small').outerWidth() + 20) * 6;
    var len = $('.bottom_slider .goods_view_small').length;
    var screens = Math.floor(len / 6) + ((len % 6) > 0 ? 1 : 0);
    var max = screens * width;
    var left = parseInt($('.bottom_slider .slider_content').css('left').replace('px', ''));
    if (left <= 0) {
        left += width;
    }
    if (left > 0)
        return;
    $('.bottom_slider .slider_content').animate({
        left: "+=" + width
    }, 1000);
}

function moveRight() {
    var width = ($('.bottom_slider .goods_view_small').outerWidth() + 20) * 6;
    var len = $('.bottom_slider .goods_view_small').length;
    var screens = Math.floor(len / 6) + ((len % 6) > 0 ? 1 : 0);
    var max = screens * width;
    var left = parseInt($('.bottom_slider .slider_content').css('left').replace('px', ''));
    if (left <= 0) {
        left -= width;
    }
    if (left <= max * -1) {
        //left = 0;
        $('.bottom_slider .slider_content').animate({
            left: "-=" + (left + width)
        }, 1000);

    } else {
        $('.bottom_slider .slider_content').animate({
            left: "-=" + width
        }, 1000);
    }
}

function loadCartBlock() {

    $('.btn.increase').unbind('click');
    $('.btn.reduce').unbind('click');
    $('[rel="to-cart"]').unbind('click');
    $('.goods_favorite').unbind('click');
    $('.icon_list_delete').unbind('click');
    $('.favorite_assign_controls input[type="checkbox"]').unbind('change');
    $('.favorite_page .in_product_control input[type="checkbox"]').unbind('change');
    $('.favorite_assign_controls .to_basket').unbind('click');
    $('.goods-cell .goods_pic a').unbind('click');
    $('.slider_content .goods_pic a').unbind('click');
    $('.slider_content .btn_toBasket').unbind('click');
    $('.element_tabs_btn.related-items a').unbind('click');
    /*$('.compare a').unbind('click');*/

    $('.compare input').change(function () {
        var arg = $(this).attr('arg');
        var v = $.cookie('ForCompare');
        if (!v)
            v = '';
        var saved = v.split(';');
        if (!saved)
            saved = new Array();
        saved = cleanArray(saved);

        if (saved.indexOf(arg) > -1) {
            saved.splice(saved.indexOf(arg), 1);
            $(this).parent().removeClass('active');
            $(this).attr('title', 'Добавить в сравнение');
        } else {
            $(this).parent().addClass('active');
            $(this).attr('title', 'Убрать из сравнения');

            saved.push(arg);
        }

        $.cookie('ForCompare', saved.join(';'), { expires: 365, path: '/' });

        $.get('/Master/ru/ClientCatalog/ComparedProductsList', function (d) {
            $('#ComparedList').replaceWith(d);
        });
        return false;

    });

    $('.element_tabs_btn.related-items a').click(function () {
        debugger;
        $('.element_tabs_cnt .slider_content').css('left', '0px');
        $('.element_tabs_btn a').removeClass('selected');
        $(this).addClass('selected');
        var source = $('.page_item_crosslink.selected');
        source.removeClass('selected');
        source.addClass('hidden');
        var target = $('.page_item_crosslink[arg="' + $(this).attr('arg') + '"]');
        target.removeClass('hidden');
        target.addClass('selected');

        var left = $('.icon_slide-green-left');
        var right = $('.icon_slide-green-right');

        if (target.find('.goods_view_small').length > 4) {
            left.show();
            right.show();
        } else {
            left.hide();
            right.hide();
        }
        $(this).parents('.grid_9.slider .slider_content').css('left', '0px');
    });

    var target = $('.page_item_crosslink.selected');
    var left = $('.element_tabs_cnt.related-items .icon_slide-green-left');
    var right = $('.element_tabs_cnt.related-items .icon_slide-green-right');

    if (left.parents('.bottom_slider').length == 0) {

        if (target.find('.goods_view_small').length > 4) {
            left.show();
            right.show();
        } else {
            left.hide();
            right.hide();
        }

    }



    $('.element_tabs_cnt .icon_slide-green-left').click(function () {



        var width = ($('.goods_view_small').outerWidth() + 20) * 4;
        var len = $('.page_item_crosslink.selected .goods_view_small').length;
        var screens = Math.floor(len / 4) + ((len % 4) > 0 ? 1 : 0);
        var max = screens * width;
        var left = parseInt($('.slider_content').css('left').replace('px', ''));
        if (left <= 0) {
            left += width;
        }
        if (left > 0)
            return;
        $('.grid_9.slider .slider_content').animate({
            left: "+=" + width
        }, 1000);
        //$('.slider_content').css('left', left + 'px');
    });

    $('.element_tabs_cnt .icon_slide-green-right').click(function () {
        var width = ($('.goods_view_small').outerWidth() + 20) * 4;
        var len = $('.page_item_crosslink.selected .goods_view_small').length;
        var screens = Math.floor(len / 4) + ((len % 4) > 0 ? 1 : 0);
        var max = screens * width;
        var left = parseInt($('.slider_content').css('left').replace('px', ''));
        if (left <= 0) {
            left -= width;
        }
        if (left <= max * -1) {
            //left = 0;
            $('.grid_9.slider .slider_content').animate({
                left: "-=" + (left + width)
            }, 1000);

        } else {
            $('.grid_9.slider .slider_content').animate({
                left: "-=" + width
            }, 1000);
        }
        //$('.slider_content').css('left', left + 'px');
    });

    $('.grid_9.slider .slider_content .btn_toBasket').click(function (e) {
        var b = $(this);
        $.post('/Master/ru/Shopcart/toCart', { id: $(this).attr('arg'), count: 1, spec: false }, function (data) {
            $('.basket_view-basic').replaceWith(data);
            updateHead();

            b.html('В корзине');
            b.removeClass('btn_green');
            b.addClass('btn_orange');

            showCartDialog($(this).attr('arg'), 1);
/*
            $('#dialog').css('left', (e.pageX) + 'px').css('top', (e.pageY) + 'px');
            $('#dialog').show();
*/

        });
    });



    $('.goods_view_item .goods_item_favorite').click(function () {
        var $this = $(this);
        $.post('/Master/ru/ClientCatalog/ToFavoriteSimple', { ProductID: $(this).attr('data-goodsid') }, function (d) {
            if (d == '1')
                $this.addClass('active');
            else
                $this.removeClass('active');
        });

    });

    $('.btn.increase').click(function () {
        var i = $(this).parent().find('input');
        var val = parseInt(i.val());
        if (val == 0)
            val = 1;
        val++;
        i.val(val);
        $(this).parent().find('.btn.reduce').removeClass('disabled');
        return false;

    });
    $('.btn.reduce').click(function () {
        var i = $(this).parent().find('input');
        var val = parseInt(i.val());
        if (val <= parseInt(i.attr('min'))) {
            return false;
        }
        val--;
        i.val(val);
        if (val == parseInt(i.attr('min'))) {
            $(this).addClass('disabled');
        }
    });

    $('.goods_favorite').click(function () {
        var $this = $(this);
        $.post('/Master/ru/ClientCatalog/ToFavorite', { ProductID: $(this).attr('data-goodsid') }, function (d) {
            if ($('.favorite_page').length) {
                $this.parent().remove();
            } else {
                $this.parent().replaceWith(d);
            }
            loadCartBlock();
        });
    });


    $('.icon_list_delete').click(function () {
        var $this = $(this);
        $.post('/Master/ru/ClientCatalog/ToFavorite', { ProductID: $(this).attr('arg') }, function (d) {
            if ($('.favorite_page').length) {
                $this.parents('.goods-cell').remove();
            }
            loadCartBlock();
        });
    });

    $('.favorite_assign_controls input[type="checkbox"]').change(function () {
        if ($(this).is(':checked')) {
            $('.favorite_page .in_product_control input[type="checkbox"]').attr('checked', 'checked');
        } else {
            $('.favorite_page .in_product_control input[type="checkbox"]').removeAttr('checked');
        }
        var len = $('.favorite_page .in_product_control input[type="checkbox"]:checked').length;
        if (len) {
            $('.favorite_assign_controls .to_basket').removeAttr('disabled');
        } else {
            $('.favorite_assign_controls .to_basket').attr('disabled', 'disabled');
        }
    });

    $('.favorite_page .in_product_control input[type="checkbox"]').change(function () {
        var len = $('.favorite_page .in_product_control input[type="checkbox"]:checked').length;
        if (len) {
            $('.favorite_assign_controls .to_basket').removeAttr('disabled');
        } else {
            $('.favorite_assign_controls .to_basket').attr('disabled', 'disabled');
        }
    });


    $('.favorite_assign_controls .to_basket').click(function () {
        var args = '';
        $('.favorite_page .in_product_control input[type="checkbox"]:checked').each(function () {
            var cnt = parseInt($(this).parents('.goods-cell').find('.goods_form .el_form-quantity input[type="text"]').val());
            if (cnt < 1 || cnt == NaN)
                cnt = 1;
            args += $(this).attr('value') + ';' + cnt + ";";
        });
        $.post('/Master/ru/Shopcart/toCartAll', { ids: args }, function (data) {
            $('.basket_view-basic').replaceWith(data);
            updateHead();

        });
    });

    $('[rel="to-cart"]').click(function (e) {
        var arg = $(this).attr('arg');
        var spec = $(this).attr('spec');
        var count = 1;
        var pb = $(this).parents('.goods_form');
        if (pb != null && pb.length) {
            try {
                count = parseInt(pb.find('.el_form-quantity input').val());
            }
            catch (e) {
                count = 1;
            }
        }
        var btn = $(this);
        $.post('/Master/ru/Shopcart/toCart', { id: arg, count: count, spec: spec == '1' }, function (data) {
            $('.basket_view-basic').replaceWith(data);
            updateHead();
            var url = '/Master/ru/ClientCatalog/CatalogListProdictTile';
            if (btn.parents('.goods-cell').hasClass('goods_view_line'))
                url = '/Master/ru/ClientCatalog/CatalogListProdictLine';
            if (btn.parents('.goods-cell').hasClass('goods_view_row'))
                url = '/Master/ru/ClientCatalog/CatalogListProdictRow';
            $.get(url, { ProductID: arg }, function (d) {
                btn.parents('.goods-cell').replaceWith(d);
                loadCartBlock();
            });

        });
        showCartDialog(arg, count);
/*
        $('#dialog').css('left', (e.pageX) + 'px').css('top', (e.pageY) + 'px');
        $('#dialog').show();
*/

        return false;

        /*     var t = $(this);
             t.qtip(
             {
                 content: {
                     text: 'QTIP'
                 },
                 show: {
                     delay: 0,
                     event: 'click', // Show it on click...
                     solo: true, // ...and hide all other tooltips...
                 },
                 hide: {
                     event: 'unfocus click',
                     delay: 0
                 },
                 /*position: { viewport: $('.main_content'), my: 'top left', at: 'bottom right', target: t },#1#
                 events: {
                    /* show: function(event, api) { $('.selector').addClass('show'); },#1#
                     show: function () {
                         //alert('test');
     
                         
     
                         var arg = t.attr('arg');
                         var spec = t.attr('spec');
                         var count = 1;
                         var pb = t.parents('.goods_form');
                         if (pb != null && pb.length) {
                             try {
                                 count = parseInt(pb.find('.el_form-quantity input').val());
                             }
                             catch (e) {
                                 count = 1;
                             }
                         }
                         var btn = t;
                         $.post('/Master/ru/Shopcart/toCart', { id: arg, count: count, spec: spec == '1' }, function (data) {
                             $('.basket_view-basic').replaceWith(data);
                             var url = '/Master/ru/ClientCatalog/CatalogListProdictTile';
                             if (btn.parents('.goods-cell').hasClass('goods_view_line'))
                                 url = '/Master/ru/ClientCatalog/CatalogListProdictLine';
                             if (btn.parents('.goods-cell').hasClass('goods_view_row'))
                                 url = '/Master/ru/ClientCatalog/CatalogListProdictRow';
                             $.get(url, { ProductID: arg }, function (d) {
                                 btn.parents('.goods-cell').replaceWith(d);
                                 loadCartBlock();
                             });
     
                         });
     
                         return false;
     
                     }
                 }
             });*/
    });
    /*
            $('[rel="to-cart"]').click(function () {
                var arg = $(this).attr('arg');
                var spec = $(this).attr('spec');
                var count = 1;
                var pb = $(this).parents('.goods_form');
                if (pb != null && pb.length) {
                    try {
                        count = parseInt(pb.find('.el_form-quantity input').val());
                    }
                    catch (e) {
                        count = 1;
                    }
                }
                var btn = $(this);
                $.post('/Master/ru/Shopcart/toCart', { id: arg, count: count, spec: spec == '1' }, function (data) {
                    $('.basket_view-basic').replaceWith(data);
                    var url = '/Master/ru/ClientCatalog/CatalogListProdictTile';
                    if (btn.parents('.goods-cell').hasClass('goods_view_line'))
                        url = '/Master/ru/ClientCatalog/CatalogListProdictLine';
                    if (btn.parents('.goods-cell').hasClass('goods_view_row'))
                        url = '/Master/ru/ClientCatalog/CatalogListProdictRow';
                    $.get(url, { ProductID: arg }, function (d) {
                        btn.parents('.goods-cell').replaceWith(d);
                        loadCartBlock();
                    });
        
                });
               
                return false;
            });*/

    $('.goods_view_item .btn_toBasket').click(function () {
        var arg = $(this).attr('arg');
        var spec = $(this).attr('spec');
        var count = 1;
        var pb = $(this).parent();
        if (pb != null && pb.length) {
            try {
                count = parseInt(pb.find('.el_form-quantity input').val());
            }
            catch (e) {
                count = 1;
            }
        }
        $.post('/Master/ru/Shopcart/toCart', { id: arg, count: count, spec: spec == '1' }, function (data) {
            $('.basket_view-basic').replaceWith(data);
            updateHead();
            var cell = $('.goods_view_item .goods_in_basket');
            if (cell.length) {
                var cnt = parseInt($.trim(cell.text())) + count;
                cell.html(cnt);
            } else {
                $('div.goods_pic').prepend('<div class="icon_goods goods_in_basket">' + count + '</div>');
            }
        });
        return false;
    });

    $('.goods-cell .goods_pic a').not('.no-hover').click(function () {

        $.get('/Master/ru/ClientCatalog/PopupDescription', { ProductID: $(this).parents('.goods-cell').attr('data-item_id') }, function (d) {
            $('#el_modal_window').remove();
            $('body').append(d);
            loadPopupUI();
        });
        return false;
    });
    $('.slider_content .goods_pic a').not('.no-hover').click(function () {

        $.get('/Master/ru/ClientCatalog/PopupDescription', { ProductID: $(this).parents('.goods_view_small').attr('data-item_id') }, function (d) {
            $('#el_modal_window').remove();
            $('body').append(d);
            loadPopupUI();
        });
        return false;
    });
}

function loadPopupUI() {
    var inModal = false;
    $('.modal_wrap_content').mouseover(function () {
        inModal = true;
    }).mouseout(function () {
        inModal = false;
    });

    $('#el_modal_window').click(function () {
        if (!inModal) {
            $('#el_modal_window').remove();
        }
        return false;
    });

    $('.modal_window_close').click(function () {
        $('#el_modal_window').remove();
        return false;
    })

    $('#el_modal_window .goods_item_favorite').click(function () {
        var $this = $(this);
        $.post('/Master/ru/ClientCatalog/ToFavorite', { ProductID: $(this).attr('data-goodsid'), InPopup: true }, function (d) {
            $('#el_modal_window').replaceWith(d);
            loadPopupUI();
        });

    });
    $('.show_all_commnts').click(function () {
        document.location.href = $(this).attr('href');
    });
    $('#el_modal_window .quick_tabs a[data-tabs]').click(function () {

        $('.quick_tabs > a').removeClass('selected');
        $('#el_modal_window .quick_tabs_content').removeClass('selected');
        $(this).addClass('selected');
        $('#el_modal_window .quick_tabs_content[data-tabs="' + $(this).attr('data-tabs') + '"]').addClass('selected');
        return false;
    });


    $('#el_modal_window .el_form-quantity .increase').click(function () {
        var i = $(this).parent().find('input');
        var val = parseInt(i.val());
        if (val == 0)
            val = 1;
        val++;
        i.val(val);
        $(this).parent().find('.btn.reduce').removeClass('disabled');
        return false;

    });
    $('#el_modal_window .el_form-quantity .reduce').click(function () {
        var i = $(this).parent().find('input');
        var val = parseInt(i.val());
        if (val <= parseInt(i.attr('min'))) {
            return false;
        }
        val--;
        i.val(val);
        if (val == parseInt(i.attr('min'))) {
            $(this).addClass('disabled');
        }
    });
    $('#el_modal_window .btn_toBasket').click(function () {
        var arg = $(this).attr('arg');
        var spec = $(this).attr('spec');
        var count = 1;
        var pb = $(this).parent();
        if (pb != null && pb.length) {
            try {
                count = parseInt(pb.find('.el_form-quantity input').val());
            }
            catch (e) {
                count = 1;
            }
        }
        $('.modal_wrap_content .quick_view').fadeTo(200, 0.2);
        $.post('/Master/ru/Shopcart/toCart', { id: arg, count: count, spec: spec == '1' }, function (data) {
            $('.basket_view-basic').replaceWith(data);
            updateHead();
            $.get('/Master/ru/ClientCatalog/PopupDescription', { ProductID: arg }, function (d) {
                $('#el_modal_window').replaceWith(d);
                loadPopupUI();
            });

        });
        return false;
    });

    $('#el_modal_window .move_control').click(function () {
        var arg = $(this).attr('arg');
        if (arg != '0') {
            $.get('/Master/ru/ClientCatalog/PopupDescription', { ProductID: arg }, function (d) {
                $('#el_modal_window').replaceWith(d);
                loadPopupUI();
            });
        }
    });
    loadMagnifier();
    //$('#el_modal_window .goods_pic.utk_preview').loupe();
    $('#el_modal_window h2 a').click(function () {
        document.location.href = $(this).attr('href');
    })
}

function loadSwitchers() {
    $('.goods_sort .el_select select').change(function () {
        $.cookie($(this).attr('id'), $(this).val(), { expires: 30, path: '/' });
        document.location.reload();
    });

    $('.goods_sort .el_switcher a').click(function () {
        /*
                var c = $(this).find('input[type="checkbox"]');
                var nv = $.cookie(c.attr('id')) == '1' ? '0' : '1';
                $.cookie(c.attr('id'), nv, { expires: 30, path: '/' });
        */
        $.cookie('ProductList', $(this).attr('arg'), { expires: 30, path: '/' });
        document.location.reload();
        return false;
    });
}


function loadMenu() {

    var mt;
    var ct;
    var opened_id = 0;
    if ($('.navigate_left').attr('data-megamenu') == '1') {
        $.get('/Master/ru/CommonBlocks/CatalogMenuChildren', { url: document.location.href }, function (d) {
            $('.navigate_left').prepend(d);
        });
    }

    $('.navigate_left .catalogue_menu_wrapper a.switch').click(function () {
        var cook = $.cookie('MenuHidden');
        if (cook != '0' && cook != '1') {
            cook = '0';
        }
        cook = (cook == '1' ? '0' : '1');
        $('.navigate_left').attr('data-hidesection', cook);
        $.cookie('MenuHidden', cook);
        return false;
    });

    $('.navigate_left[data-megamenu="1"]').each(function () {
        var $parent = $(this);
        $parent.find('.catalogue_menu_wrapper a').not('.switch').mouseover(function () {

            var $this = $(this);
            $('.menu-back').attr('data-promo', $this.attr('data-catid'));
            clearTimeout(ct);
            mt = setTimeout(function () {
                var oc = $parent.find('.megamenu_content div[data-catalogueparent="' + $this.attr('data-catid') + '"]');
                if (oc.length) {
                    $parent.addClass('hovered');

                    $parent.find('.megamenu_content div[data-catalogueparent]').removeClass('opened');
                    oc.addClass('opened');
                    opened_id = $this.attr('data-catid');
                } else {
                    ct = setTimeout(function () {
                        if (opened_id > 0) {
                            $parent.removeClass('hovered');
                            opened_id = 0;
                        }

                    }, 200);

                }
            }, opened_id > 0 ? 100 : 1000);
        }).mouseout(function () {
            clearTimeout(mt);
        });

        $parent.mouseover(function () {
            clearTimeout(ct);
        });
        $parent.mouseout(function () {
            ct = setTimeout(function () {
                if (opened_id > 0) {
                    $parent.removeClass('hovered');
                    opened_id = 0;
                }

            }, 200);
        });

    });

    $('#bread_with_catalogue .fade_area').mouseover(function () {
        $('.bread_crumbs').removeClass('zTop');
        $('#bread_with_catalogue').removeClass('active');
        opened_id = 0;
    })
}