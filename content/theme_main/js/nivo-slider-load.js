$().ready(function () {
    $('[id="Slider"]').each(function () {

        var w = $(this).width();
        $(this).find('.attachment-slideshow').each(function () {
            $(this).parent().parent().attr('title', '');
            var iw = parseInt($(this).attr('width'));
            var ih = parseInt($(this).attr('height'));

            $(this).attr('width', w);

            var h = parseInt(ih * (w / iw));
            $(this).attr('height', h);


        });
        $(this).css('visibility', 'visible');
        $(this).nivoSlider({
            effect: "random",
            pauseTime: 8000,
            slices: 10,
            pauseOnHover: true,
            captionOpacity: 0.9,
            directionNavHide: true,

            afterLoad: function () { $(".nivo-caption").animate({ left: "0" }, { easing: "easeOutBack", duration: 500 }) },
            beforeChange: function () { $(".nivo-caption").animate({ left: "-1200" }, { easing: "easeInBack", duration: 300 }) },
            afterChange: function () { $(".nivo-caption").animate({ left: "0" }, { easing: "easeOutBack", duration: 500 }) }
        });

        $(this).find('.nivo-directionNav a').css('top', parseInt($(this).height() / 2 - 20));

    });

    if ($('.inner-head').length) {
        $('.inner-head').css('margin-top', '20px');
        $('.inner-head').insertAfter($('.main_content [id="Slider"]:last'));
    }


});
