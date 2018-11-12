var bt = null;
$().ready(function () {
    $('.b-breadcrumb__item').hover(function () {
        var t = $(this);
        bt = setTimeout(function() {
            if (t.find('ul').length) {
                
                t.addClass('b-breadcrumb__item_state_opened');


                var max = 0;
                
                t.find('ul').find('li').each(function () {
                    
                    var w = $(this).find('a').textWidth() + 20;
                    if (w > max)
                        max = w;
                });
                if (max < t.find('ul').width()) {
                    t.find('ul').css('width', max + 'px');
                    t.find('ul').find('li').css('width', max + 'px');

                }
            }

        }, 800);
    }, function() {
        if (bt != null) {
            clearTimeout(bt);
            bt = null;
        }
        $('.b-breadcrumb__item_state_opened').removeClass('b-breadcrumb__item_state_opened');
    })
});

$.fn.textWidth = function () {
    var html_org = $(this).html();
    var html_calc = '<span>' + html_org + '</span>';
    $(this).html(html_calc);
    var width = $(this).find('span:first').width();
    $(this).html(html_org);
    return width;
};