$().ready(function () {
    loadFromQuery();
    loadFilterToggler();
    
    loadPriceSlider();
    loadRangeSlider();
    loadCbxScrollers();
    loadGroupTogglers();

    $('.filter-cbx').checkBo();
});


function loadFromQuery() {
    if ($('#Query').length && $('#Query').val().length) {
        var jsp = JSON.parse($('#Query').val());
        for (var i = 0; i < jsp.length; i++) {
            var item = jsp[i];
            var inp = null;
            if (item.type == 'price') {
                inp = $('input[filter-type="price"]').filter('[arg="' + item.id + '"]');
                inp.val(item.value);
                inp.attr('def', item.value);
                var vv = item.value.split(';');
                inp.parent().find('.from-price input').val(Math.round(vv[0]));
                inp.parent().find('.to-price input').val(Math.round(vv[1]));
            }
            else if (item.type == 'range') {
                inp = $('input[filter-type="range"]').filter('[arg="' + item.id + '"]');
                inp.attr('chg', '1');
                inp.val(item.value);
                inp.attr('def', item.value);
            }
            else if (item.type == 'value') {
                inp = $('input[filter-type="value"]').filter('[arg="' + item.id + '"]');
                inp.attr('chg', '1');
                inp.val(item.value);
                inp.attr('def', item.value);
            }
            else if (item.type == 'check') {
                var cell = $('.filter-cbx-group[arg="' + item.id + '"]');
                var vals = item.value.split(';');
                for (var j = 0; j < vals.length; j++) {
                    var c = cell.find('input[value="' + vals[j] + '"]');
                    c.prop('checked', true);
                    c.attr('checked', 'checked');
                }
            }
        }
    }
}

function loadCbxScrollers() {
    $('.filter-group-body-cbx').mCustomScrollbar({
        scrollInertia: 250,

    });
    $('.filter-cbx').find('input').change(function() {
        fastSearch();
    });
}

function fastSearch() {
    var list = new Array();
    $('.filter-group-body-price').each(function() {
        var t = $(this).find('input[filter-type="price"]');
        list.push({ id: t.attr('arg'), type: 'price', value: t.val() });
    });

    $('.filter-group-body-range').each(function () {
        var t = $(this).find('input[filter-type="range"]');
        if (t.length) {
            if (t.attr('chg') == '1') {
                list.push({ id: t.attr('arg'), type: 'range', value: t.val() });
            }
        } else {
            t = $(this).find('input[filter-type="value"]');
            if (t.attr('chg') == '1') {
                list.push({ id: t.attr('arg'), type: 'value', value: t.val() });
            }
        }
    });

    $('.filter-cbx-group').each(function() {
        var selection = '';
        $(this).find('input[type="checkbox"]:checked').each(function() {
            selection += $(this).val() + ";";
        });

        if (selection.length > 0) {
            selection = selection.substring(0, selection.length - 1);
        }
        if (selection.length)
            list.push({ id: $(this).attr('arg'), type: 'check', value: selection });

    });

    $('.category-filter-body').append('<div class="category-filter-overlay"></div>');

    $.post('/Master/ru/ClientCatalog/FastSearch', { query: JSON.stringify(list), CategoryID: $('#CategoryID').val() }, function (data) {
        $('.category-filter-apply').show();
        $('.showchild').find('#result_num').html(data.count);
        if (parseInt(data.count) == 0) {
            $('.showchild').find('b').hide();
            $('.showchild').attr('href', 'javascript:void(0);');
        } else {
            $('.showchild').find('b').show();
            $('.showchild').attr('href', data.link);
        }
        $('.category-filter-overlay').remove();
    });
    //console.log(list);
}

function loadRangeSlider() {
    $('input[filter-type="range"]').each(function () {
        var o = $(this);
        var arr = o.attr('values').split(';');
        $(this).ionRangeSlider({
            type: "double",
            force_edges: true,
            grid: arr.length <= 30,
            values: arr,
            prettify_enabled: false,
            onFinish: function (data) {
                o.attr('chg', '1');
                var value = o.prop("value").split(';');
                var undef = false;
                if (value[0] == 'undefined') {
                    value[0] = data.input.attr('values').split(';')[Math.round(data.from)];
                    undef = true;
                }
                if (value[1] == 'undefined') {
                    value[1] = data.input.attr('values').split(';')[Math.round(data.to)];
                    undef = true;
                }
                if (undef) {
                    data.input.val(value[0]+";"+value[1]);
                }
                o.parent().find('.irs-from').text(value[0]);
                o.parent().find('.irs-to').text(value[1]);
                o.parent().find('.irs-single').text(value[0]+" - "+value[1]);
                fastSearch();
            },
            onChange: function (data) {
                o.attr('chg', '1');
                var value = o.prop("value").split(';');
                var undef = false;
                if (value[0] == 'undefined') {
                    value[0] = data.input.attr('values').split(';')[Math.round(data.from)];
                    undef = true;
                }
                if (value[1] == 'undefined') {
                    value[1] = data.input.attr('values').split(';')[Math.round(data.to)];
                    undef = true;
                }
                if (undef) {
                    data.input.val(value[0]+";"+value[1]);
                }
                o.parent().find('.irs-from').text(value[0]);
                o.parent().find('.irs-to').text(value[1]);
                o.parent().find('.irs-single').text(value[0] + " - " + value[1]);
                
            },
        });

        if (o.attr('def')) {
            var slider = o.data("ionRangeSlider");
            var values = o.attr('values').split(';');
            
            var vv = o.attr('def').split(';');
            slider.update({
                from: values.indexOf(vv[0]),
                to: values.indexOf(vv[1])
            });

            var slv = o.prop("value").split(';');
            var sundef = false;
            if (slv[0] == 'undefined') {
                slv[0] = vv[0];
                sundef = true;
            }
            if (slv[1] == 'undefined') {
                slv[1] = vv[1];
                sundef = true;
            }
            if (sundef) {
                o.prop("value", slv[0] + ";" + slv[1]);
                
            }
            o.parent().find('.irs-from').text(slv[0]);
            o.parent().find('.irs-to').text(slv[1]);
            o.parent().find('.irs-single').text(slv[0] + " - " + slv[1]);

            console.log(o.prop("value"));
        }
    

    });

    $('input[filter-type="value"]').each(function () {
        var o = $(this);
        var arr = o.attr('values').split(';');
        $(this).ionRangeSlider({
            type: "single",
            force_edges: true,
            grid: arr.length < 20,
            values: arr,
            prettify_enabled: false,
            onFinish: function (data) {
                o.attr('chg', '1');
                var value = o.prop("value");
                if (value == 'undefined') {
                    value = data.input.attr('values').split(';')[Math.round(data.from)];
                    data.input.val(value);
                }

                o.parent().find('.irs-single').text(value);
                fastSearch();
            },
            onChange: function (data) {
                o.attr('chg', '1');
                var value = o.prop("value");
                if (value == 'undefined') {
                    value = data.input.attr('values').split(';')[Math.round(data.from)];
                    data.input.val(value);
                }
                o.parent().find('.irs-single').text(value);
                
            },
        });

        $(this).on("change", function () {
            var $this = $(this),
                value = $this.prop("value");

            $this.parent().find('.irs-single').text(value);
        });

        if (o.attr('def')) {
            var slider = o.data("ionRangeSlider");
            var vv = o.attr('def');
            var values = o.attr('values').split(';');
            slider.update({
                from: values.indexOf(vv)
        });
        }
    });
}


function loadGroupTogglers() {
/*
    $('.category-filter-group .title').click(function(e) {
        $(this).find('.group-arrow').trigger('click');
        e.stopPropagation();
    });
*/
    $('.category-filter-body .title').click(function () {
        var target = $(this).parent().find('.filter-group-body');
        if (target.hasClass('filter-group-collapsed')) {
            target.removeClass('filter-group-collapsed');
            $(this).find('.group-arrow').removeClass('g-darrs');
            $(this).find('.group-arrow').removeClass('g-arrs');
            $(this).find('.group-arrow').addClass('g-arrs');

        } else {
            target.addClass('filter-group-collapsed');
            $(this).find('.group-arrow').removeClass('g-arrs');
            $(this).find('.group-arrow').removeClass('g-darrs');
            $(this).find('.group-arrow').addClass('g-darrs');

        }
    });
}

function loadPriceSlider() {
    $('input[filter-type="price"]').each(function () {
        $(this).ionRangeSlider({
            type: "double",
            step: 1,
            min: parseInt($(this).attr("min")),
            max: parseInt($(this).attr("max")),
            grid: true,
            force_edges: true,
            prettify: function(num) {
                return Math.round(num);
            },
            onFinish: function(data) {
                fastSearch();
            },
        });

        $(this).on("change", function () {
            var $this = $(this),
                value = $this.prop("value").split(";");

            var arg = $(this).attr('arg');
            $('#FromPrice_' + arg).val(Math.round(value[0]));
            $('#ToPrice_' + arg).val(Math.round(value[1]));

        });


    });

    $('.price-line input').keyup(function () {
        var num = parseInt($(this).val());
        var slider = $(this).parent().parent().parent().find('[filter-type="price"]');
        if (isNaN(num)) {
            return;
            if ($(this).parent().hasClass('from-price')) {
                num = parseInt(slider.attr("min"));
            } else {
                num = parseInt(slider.attr("max"));
            }
        }

        var range = slider.prop('value').split(";");

        if (parseInt(slider.attr("min")) > num) {
            return;
        }
        if (parseInt(slider.attr("max")) < num) {
            return;
        }

        if ($(this).parent().hasClass('from-price')) {

            if (num > parseInt(range[1]))
                return;

            slider.data("ionRangeSlider").update({
                from: num
            });
            fastSearch();
        } else {

            if (num < parseInt(range[0]))
                return;


            slider.data("ionRangeSlider").update({
                to: num
            });
            fastSearch();
        }

    });
}

function loadFilterToggler() {
    var ft = $.cookie('FilterToggler');
    if (ft != '0') {
        $('.category-filter-body').show();

    } else {
        $('.category-filter-header').find('.order-uarr, .order-darr').attr('class', 'order-darr');
    }

    $('.category-filter .category-filter-header-toggler').click(function () {
        if ($('.category-filter-body').is(':visible')) {
            $('.category-filter-body').slideUp(500);
            $.cookie('FilterToggler', '0', { expires: 365, path: '/' });
            $('.category-filter-header').find('.order-uarr, .order-darr').attr('class', 'order-darr');
        } else {
            $('.category-filter-body').slideDown(500);
            $.cookie('FilterToggler', '1', { expires: 365, path: '/' });
            $('.category-filter-header').find('.order-uarr, .order-darr').attr('class', 'order-uarr');

        }

    });
}