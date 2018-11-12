$.fn.stTruncate = function (options) {

    var defaults = {
        addBlockCSS: {},
        addBlockStyleClass: '',
        maxHeight: 0, // Ð ÑšÐ Â°Ð Ñ”Ð¡ÐƒÐ Ñ‘Ð Ñ˜Ð Â°Ð Â»Ð¡ÐŠÐ Ð…Ð Â°Ð¡Ð Ð Ð†Ð¡â€¹Ð¡ÐƒÐ Ñ•Ð¡â€šÐ Â°
        maxHeightCut: 0, // Ð Ñ›Ð Â±Ð¡Ð‚Ð ÂµÐ Â·Ð Â°Ð ÂµÐ Ñ˜ Ð Ò‘Ð Ñ• .. px
        moreText: "Развернуть",
        lessText: "Свернуть",
        speedHide: 1000,
        speedShow: 1000,
        ShowOnOpen: false,
        hide: false,
        showRealHeight: false,
        customOonShow: false,
        backHeight: $(this).height() + 30,
        onBeforeHide: function () { },
        isHref: true
    };

    var options = $.extend(defaults, options);
    var $element = $(this);

    var isAdditionalProducts = $element.hasClass('complgreenchek-prod');



    if ($.isNumeric(options.maxHeight)) {
        if ($element.height() >= options.maxHeight || options.hide) {

            $element.css(options.addBlockCSS);
            $element.addClass(options.addBlockStyleClass);

            if (options.ShowOnOpen) {

                if ($.isFunction(options.customOonShow))
                    options.customOonShow($element, options);
                if (options.isHref) {
                    $element.addClass('stTruncate stTruncate-close').animate({
                        height: options.maxHeightCut - (isAdditionalProducts ? 40 : 0)
                    },
                        options.speedHide
                    ).append('<div class="str-button"><a href="#">' + options.moreText + '</a></div>');
                } else {
                    $element.addClass('stTruncate stTruncate-close').animate({
                        height: options.maxHeightCut - (isAdditionalProducts ? 40 : 0)
                    },
                        options.speedHide
                    ).append('<div class="str-button"><a>' + options.moreText + '</a></div>');
                }
            } else {
                if (options.isHref) {
                    $element.addClass('stTruncate stTruncate-close').animate({
                        height: options.maxHeightCut - (isAdditionalProducts ? 40 : 0)
                    },
                        options.speedHide
                    ).append('<div class="str-button"><a href="#">' + options.moreText + '</a></div>');
                } else {
                    $element.addClass('stTruncate stTruncate-close').animate({
                        height: options.maxHeightCut - (isAdditionalProducts ? 40 : 0)
                    },
                        options.speedHide
                    ).append('<div class="str-button"><a>' + options.moreText + '</a></div>');
                }

            }

            var EventClick = 'Y';
            $element.find('.str-button a').click(function () {
                if (EventClick == 'Y') {
                    //ga('send', 'event', 'pokazat_vse_in_kpk', 'click');
                    EventClick = 'N';
                }


                if ($element.hasClass('stTruncate-close')) {

                    if ($.isFunction(options.customOonShow) && options.customOonShow($element, options)) {
                    }
                    else {
                        if (options.showRealHeight) {

                            options.backHeight = 35;

                            $.each($element.find('>div:visible'), function (index, value) {

                                if (!$(value).hasClass('str-button')) {
                                    options.backHeight = options.backHeight + $(value).height();
                                }

                            });
                        }

                        $element.find('.str-button a').html(options.lessText);

                        var timer = setInterval(function () {
                            $.event.trigger({
                                type: "changeLimit",
                                limit: function () {
                                    return $('#footer').offset().top;
                                }
                            });
                        }, 50);
                        $element.addClass('stTruncate-open').animate({
                            height: options.backHeight
                        },
                            options.speedShow,
                            false,
                            function () {
                                $.event.trigger({
                                    type: "changeLimit",
                                    limit: function () {
                                        return $('#footer').offset().top;
                                    }
                                });
                                clearInterval(timer);
                                $element.removeClass('stTruncate-close');
                            }
                        );
                    }
                }
                else if ($element.hasClass('stTruncate-open')) {

                    if (typeof options.onBeforeHide == 'function') {
                        options.onBeforeHide();
                    }

                    $element.removeClass('stTruncate-open').addClass('stTruncate-close');

                    $element.find('.str-button a').html(options.moreText);



                    var timer2 = setInterval(function () {
                        $.event.trigger({
                            type: "changeLimit",
                            limit: function () {
                                return $('#footer').offset().top;
                            }
                        });
                    }, 50);

                    $element.animate({
                        height: options.maxHeightCut - (isAdditionalProducts ? 40 : 0)
                    },
                        options.speedHide,
                        function () {
                            clearInterval(timer2);
                            $.event.trigger({
                                type: "changeLimit",
                                limit: function () {
                                    return $('#footer').offset().top;
                                }
                            });

                        }
                    );
                }

                return false;
            });
        }
    }
};