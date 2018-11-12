var notranslit = false;

function loadTranslitter() {

    notranslit = $.cookie('translit') == '0';
    if (notranslit) {
        $('.icon-translit').addClass('icon-translit-fade');
    }
    $('.icon-translit').unbind('click');
    $('.icon-translit').click(function () {

        if (notranslit) {
            $.cookie('translit', '1', { expires: 365, path: '/atilekt' });
            $.cookie('translit', '1', { expires: 365, path: '/atilektcms' });
            $(this).removeClass('icon-translit-fade');
            $('input.tl').unbind('keyup');
            $('input.tl')
                .keyup(function () {
                    if (notranslit)
                        return;
                    $(this).val(tl($(this).val()));
                });

        } else {
            $.cookie('translit', '0', { expires: 365, path: '/atilekt' });
            $.cookie('translit', '0', { expires: 365, path: '/atilektcms' });
            $(this).addClass('icon-translit-fade');
        }
        notranslit = !notranslit;
    });
    $('input.tl, #URL, #Slug').unbind('keyup');
    $('input.tl, #URL, #Slug')
        .keyup(function(e) {
        //console.log(e);
            if (notranslit || (e.ctrlKey && e.which != 86))
                return;
            for (var i = 0; i < cck.length; i++) {
                if (cck[i][0] == e.which) {
                    return;
                }
            }
            var elem = $(this).get(0);
            var startPos = elem.selectionStart;
            var prevLen = $(this).val().length;
            //var endPos = ctl.selectionEnd;

            $(this).val(tl($(this).val()).toLowerCase());

            if ($(this).val().length > prevLen) {
                startPos++;
            }

            if (elem.createTextRange) {
                var range = elem.createTextRange();
                range.move('character', startPos);
                range.select();
            } else {
                if (elem.selectionStart) {
                    elem.focus();
                    elem.setSelectionRange(startPos, startPos);
                } else
                    elem.focus();
            }
        });


}

var cck = [[13, 'Enter'], [38, 'Up'], [40, 'Down'], [39, 'Right'], [37, 'Left'], [27, 'Esc'], /*[32, 'SpaceBar'],*/ [17, 'Ctrl'], [18, 'Alt'], [16, 'Shift']];

function tl(str) {
    var alphabetMap = { а: 'a', б: 'b', в: 'v', г: 'g', д: 'd', е: 'e', ё: 'yo', ж: 'zh', з: 'z', и: 'i', й: 'y', к: 'k', л: 'l', м: 'm', н: 'n', о: 'o', п: 'p', р: 'r', с: 's', т: 't', у: 'u', ф: 'f', х: 'kh', ц: 'ts', ч: 'ch', ш: 'sh', щ: 'sch', ы: 'i', э: 'e', ю: 'yu', я: 'ya' };

    var result = $.map(
        str.split(''),
        function (el, idx) {
            if (el == '-')
                return el;
            if (/\w/i.test(el)) {
                return el;
            }
            if (/\s/.test(el)) {
                return '-';
            }
            if (alphabetMap[el.toLowerCase()]) {
                return alphabetMap[el.toLowerCase()];
            }
            return '';
        });
    return result.join('');
}