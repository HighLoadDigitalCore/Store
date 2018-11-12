$().ready(function() {
    loadStates();
    
});

$(window).resize(function () {
    
    loadWidth();
});

function loadStates() {
    loadDefState();
    loadDefStateVert();
    
}

function loadWidth() {
    
    $('.group-content:visible, #group-content:visible').each(function () {
        
        var max = 0;
        var label = $(this).width()* .23404255319148934;
        $(this).find('.control-label').each(function () {
            
            $(this).html('<span>' + $(this).text() + '</span>');
            if ($(this).find('span').width() > max) {
                max = $(this).find('span').width();
            }
            //console.log($(this).find('span').width());

        });

        max += 10;

        if (max > label) {
            
            $(this).find('.control-label').each(function () {
                $(this).parent().attr('style', '');
            });
            return;
        }

        $(this).find('.control-label').each(function() {
            $(this).parent().css('width', max + "px");
        });
    });
}

function loadDefStateVert() {

    
    var uid = $('#EditorUID').val();
    if (!uid)
        return;
    var selected = $.cookie('Tabs_' + uid);
    if (!selected || !selected.length) {
        selected = $('.tab-horiz .group-name:first-child').attr('gid');
    }
    if (!$('.tab-horiz .group-name[gid="' + selected + '"]').length) {
        selected = $('.tab-horiz .group-name:first-child').attr('gid');
    }

    
    if ($('#isPhotoPage').length) {
        $('.tab-horiz .group-name').click(function () {
            $('.tab-horiz .group-name').removeClass('active');


                $('.group-content:visible').html('<img src="/content/loader.gif" style="width:50px; height: 50px;" class="tree-loader">');
                $('.group-content:visible .tree-loader').css('margin-left', ($('.group-content:visible').width() - $('.tree-loader').width()) / 2).
                    css('margin-top', '100px').css('margin-bottom', '100px');

            $('.group-name[gid="' + $(this).attr('gid') + '"]').addClass('active');
            
            $.cookie('Tabs_' + $('#EditorUID').val(), $(this).attr('gid'), { expires: 365, path: '/' });
            
            //document.location.href = '/Master/ru/TableEditors/Products?Type=Edit&UID=' + $('#EditorUID').val();
            loadContent('p' + $('#UID').val(), true);

        });
        return;
    }

    $('.tab-horiz .group-name').removeClass('active');
    $('.tab-horiz .group-name[gid="' + selected + '"]').addClass('active'); //fadeTo(300, 1);

    
    $('.tab-horiz .group-content').each(function () {
        $(this).insertAfter($('.tab-horiz .group-content:last'));
    });

    if ($('.previewsite.preview-link:not(.prev-head)').length) {

        if ($('.tab-horiz .group-name:last').length > 0) {
            $('.previewsite.preview-link:not(.prev-head)').hide();
        } else if (!$('.image-add-text').length) {
            $('.previewsite.preview-link:not(.prev-head)').show();
        }
    }

/*
    setTimeout(function() {
        $('.prev-head').attr('href', $('.previewsite.preview-link:not(.prev-head)').attr('href'));
    }, 100);
*/

    
    



    $('<div class="clear"></div>').insertAfter($('.prev-head'));
    $('.tab-horiz .group-name').removeClass('last-group');
    $('.tab-horiz .group-name').wrapAll('<div class="horiz-menu"></div>');

 
    
    $('.tab-horiz .group-content[gid="' + selected + '"]').fadeTo(300, 1, function() {
        initEditors();
        //debugger;
        if ($('.group-content:visible').find('iframe').length) {
            try {
                $('.group-content:visible').find('iframe')[0].contentWindow.calcSize();
            } catch (e) {

            }
        }

    });
    loadWidth();


    $('.tab-horiz .group-name:last').after('<a class="previewsite preview-link prev-head" style="position: fixed; right: 110px; margin-top:-4px;" title="Посмотреть на сайте" target="_blank" id="pageView" href=""><img src="/content/Eye-icon.png"></a>');
    //'<input type="submit" class="btn btn-head" value="Сохранить" style="">'
    $('.tab-horiz .group-name:last').after($('#UF input[type="submit"]').css({ 'position': 'fixed' }).css("right", "10px"));

    $('.horiz-menu').clone(true, true).insertAfter($('.group-content:last'));



    $('.tab-horiz .group-name').unbind('click');
    $('.tab-horiz .group-name').click(function () {
        $('.tab-horiz .group-name').removeClass('active');
        
        if ($(this).attr('gid') == '') {
            
            $('.group-content:visible').html('<img src="/content/loader.gif" style="width:50px; height: 50px;" class="tree-loader">');
            $('.group-content:visible .tree-loader').css('margin-left', ($('.group-content:visible').width() - $('.tree-loader').width()) / 2).
                css('margin-top', '100px').css('margin-bottom', '100px');

            return;
        }
        $('.group-name[gid="' + $(this).attr('gid') + '"]').addClass('active');

        $.cookie('Tabs_' + uid, $(this).attr('gid'), { expires: 365, path: '/' });
        var g = $(this).attr('gid');
        $('.group-content:visible').fadeTo(300, 0, function () {
            $(this).hide();
            $('.tab-horiz .group-content[gid="' + g + '"]').fadeTo(300, 1, function () {
                if ($('.group-content:visible').find('iframe').length) {
                    try {
                        $('.group-content:visible').find('iframe')[0].contentWindow.calcSize();
                    } catch (e) {
                        
                    }
                }
                
                initEditors();
                
            });
            loadWidth();
            //$(this).next('.group-content').fadeTo(300, 1);
        });

    });



    setTimeout(function () {
        $('.prev-head').attr('href', $('.previewsite.preview-link:not(.prev-head)').attr('href'));
    }, 200);


}/*
function loadDefStateVert() {
    var uid = $('#EditorUID').val();
    var sum = 0;
    var len = $('#UF').width();
    var added = false;
    var di = -1;
    $('.tab-horiz .group-name').each(function (index) {
        var l = $(this).outerWidth();
        if (sum + l >= len) {
            if (!added) {
                $('.tab-horiz .group-name.last-group').removeClass('last-group');
                added = true;
                $('.tab-horiz .group-name:eq(' + (index - 1) + ')').addClass('last-group');
            }
            di = index - 1;
            var addlen = (len - sum) / (index - 1);
            $('.tab-horiz .group-name:lt(' + (index-1) + ')').each(function () {
                $(this).attr('style', 'width:' + ($(this).outerWidth() + addlen) + 'px!important');
                $(this).find('a').width($(this).width());
            });
            sum = 0;
        } else {
            sum += l;
        }
    });

    var rest = $('.tab-horiz .group-name:gt(' + di + ')');
    
    sum = 0;
    rest.each(function(ind) {
        sum += $(this).outerWidth();
    });
    var addlen = (len - sum) / (rest.length);
    rest.each(function () {
        $(this).attr('style', 'width:' + ($(this).outerWidth() + addlen) + 'px!important');
        $(this).find('a').width($(this).width());
    });

    $('.tab-horiz .group-content').each(function() {
        $(this).insertAfter($('.tab-horiz .group-content:last'));
    });

    $('<div class="clear"></div>').insertAfter($('.tab-horiz .group-name:last'));

    $('.tab-horiz .group-name').unbind('click');
    $('.tab-horiz .group-name').click(function () {
        
        $('.tab-horiz .group-name').removeClass('active');
        $(this).addClass('active');
        $.cookie('Tabs_' + uid, $(this).attr('gid'), { expires: 365, path: '/' });
        var g = $(this).attr('gid');
        $('.group-content:visible').fadeTo(300, 0, function () {
            $(this).hide();
            $('.tab-horiz .group-content[gid="' + g + '"]').fadeTo(300, 1, function() {
                initEditors();
            });
            //$(this).next('.group-content').fadeTo(300, 1);
        });
        
    });
    //debugger;
    var selected = $.cookie('Tabs_' + uid);
    if (!selected || !selected.length) {
        selected = $('.tab-horiz .group-name:first-child').attr('gid');
    }
    $('.tab-horiz .group-name[gid="' + selected + '"]').addClass('active'); //fadeTo(300, 1);
    $('.tab-horiz .group-content[gid="' + selected + '"]').fadeTo(300, 1, function() {
        initEditors();
    });

}*/

function loadDefState() {
    $('.tab-vert .group-name').unbind('click');
    
    $('.tab-vert .group-name').click(function () {
        var state = $(this).attr('state');
        state = (state == '1' ? '0' : '1');
        $(this).attr('state', state);
        $.cookie('g' + $(this).attr('gid'), state, { expires: 365 });
        if (state == '1')
            $(this).next('.group-content').fadeTo(300, 1);
        else {

            $(this).next('.group-content').fadeTo(300, 0);
            $(this).next('.group-content').fadeOut();
        }
        return false;
    });
    $('.tab-vert .group-name').each(function () {
        var state = $.cookie('g' + $(this).attr('gid'));
        if (state == null)
            state = '0';
        $(this).attr('state', state);
    });
    $('.tab-vert .group-name').each(function () {
        if ($(this).attr('state') == '1')
            $(this).next('.group-content').show();
        else $(this).next('.group-content').fadeOut();
    });

    if ($('.tab-vert .group-name').length && !$('.tab-vert .group-content:visible').length) {
        $('.tab-vert .group-name:first').click();
    }
}