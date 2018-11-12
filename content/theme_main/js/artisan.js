

$().ready(function () {


    $('.tab').click(function () {
        $('.tab').removeClass('active');
        $(this).addClass('active');
        $('.tab-cnt').removeClass('active');
        $('.tab-cnt:eq(' + $(this).index() + ')').addClass('active');

        
    });


    var tv = $('#townlist option:contains("' + $('#townlist').attr('def') + '")').attr('value');

    $('#townlist').val(tv);

    $('#townlist').change(function () {
        var path = location.protocol + '//' + location.host + location.pathname;

        if ($(this).find('option:selected').text() != "Все мастера") {
            path += '?town=' + $(this).find('option:selected').text();
        }
        document.location.href = path;
    });

    $('.rating-cell').each(function () {
        var rating = parseInt($(this).attr('rating'));
        hoverLine($(this), rating);
    });


    loadHover($('.rating-cell:not(.voted)').find('.star-box'));

    /*
            $('.user-data-rating').mouseout(function () {
                //debugger;
                var rating = parseInt($(this).find('.rating-cell').attr('rating'));
                $(this).find('.rating-cell').removeClass('hovered');
                hoverLine($(this).find('.rating-cell'), rating);
            });
    */

    $('.rating-cell').hover(function () {

    }, function () {
        var rating = parseInt($(this).attr('rating'));
        $(this).removeClass('hovered');
        hoverLine($(this), rating);

    });


    $('.descr-cell div').not('.user-data-rating').hover(function () {
        var cell = $(this).closest('td').find('.rating-cell');
        var rating = parseInt(cell.attr('rating'));
        cell.removeClass('hovered');
        hoverLine(cell, rating);

    });

    $('.user-list td').mouseover(function () {
        var another = $('.user-list td').not($(this)).find('.rating-cell.hovered');
        if (another.length) {

            var rating = parseInt(another.attr('rating'));
            another.removeClass('hovered');
            hoverLine(another, rating);
        }
    });


});


function loadHover(obj) {
    if ($(obj).closest('.rating-cell').hasClass('voted')) {
        return;
    }
    $(obj).mouseover(function () {
        var rating = parseInt($(this).attr('arg'));
        $(this).closest('.rating-cell').addClass('hovered');
        hoverLine($(this).closest('.rating-cell'), rating);

    }).mouseout(function () {
        var cell = $(this).closest('.rating-cell');
        var rating = parseInt(cell.attr('rating'));
        hoverLine(cell, rating);
        //$(this).closest('.rating-cell').removeClass('hovered');

    });

}

function hoverLine(obj, rating) {

    if (rating == $(obj).find('.checked').length && rating > 0) {
        return;
    }

    var html = '';

    for (var i = 1; i <= 10; i++) {
        if (i <= rating) {
            html += "<i onclick='setRating(this);' class='star-box checked' arg='" + i + "'></i>";
        } else {
            html += "<i onclick='setRating(this);' class='star-box' arg='" + i + "'></i>";
        }
    }
    $(obj).html(html);
    loadHover(obj.find('.star-box'));
}

function setRating(obj) {
    var r = $(obj).attr('arg');
    $.post('/Master/ru/Artisan/RateUser', { user: $(obj).closest('.rating-cell').attr('arg'), rate: r }, function (data) {
        if (data.length) {
            alert(data);
            return;
        }
        $(obj).closest('.rating-cell').attr('rating', r);
        var cell = $(obj).closest('.rating-cell');
        hoverLine(cell, r);
        cell.addClass('voted');

    });
    /*alert($(obj).attr('arg'));*/
}


