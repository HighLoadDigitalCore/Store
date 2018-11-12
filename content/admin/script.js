var dragObj1 = null;
var dragObj2 = null;

var structReload = false;
var structData = '';
var showHelp;
var dockID = '';
var zoneID = '';
var dockIndex = -1;
function moveDock() {
    if (dockID == '' || zoneID == '' || dockIndex == -1)
        return;
    $find(zoneID).dock($find(dockID), dockIndex);
    dockID = '';
    zoneID = '';
    dockIndex = -1;
}

function isIE7(){
	return (navigator.userAgent.indexOf('MSIE 7.0') != -1)
}

function reloadStruct(rebuild) {
    if (structReload) {
        if (structData.length == 0 || rebuild) {
            $.get("/Helpers/PageStruct.aspx", { Act: 0 }, function(Data) {
                $("#leftContent").html(Data);
                structData = Data;
            });
        }
        else {
            if (structData.substr(0, 2) == 'ok')
                structData = structData.substring(2, structData.length);
            $("#leftContent").html(structData);
        }
    }
}

$(document).ready(function() {
    primaryNav();
    resize();
    $("#tree").treeview();
    block1();
    reloadStruct(false);
    title4();
    toggleHelp();
    showModulesInstalledMsg();
    stretchVertical();
});

function toggleHelp() {

    var help = $('.help');

    help.css({ 'top': $('.title').height() + 54 });

    help.find('.help-title').click(function() {
        help.addClass('help-hidden');
    })

    $('.help-title').click(function() {
        $('.help').addClass('help-hidden');
        showHelp = '0';
        $.cookie('showHelp', showHelp, { expires: 365, path: '/' });
    });

    $('.ico-help1').click(function() {
        $('.help').removeClass('help-hidden');
        showHelp = '1';
        $.cookie('showHelp', showHelp, { expires: 365, path: '/' });
    })

    setHelpWnd();
}

function setHelpWnd() {
    if ($.cookie('showHelp') != null && showHelp == null) {
        showHelp = $.cookie('showHelp');
    }
    if (showHelp == null)
        showHelp = '0';

    if (showHelp == '1') {
        $('.help').removeClass('help-hidden');
    }
    else {
        $('.help').addClass('help-hidden');
    }

}

var mSetRus = { "а": "a", "б": "b", "в": "v", "г": "g", "д": "d", "е": "e", "ё": "jo", "ж": "zh", "з": "z", "и": "i", "й": "j", "к": "k", "л": "l", "м": "m", "н": "n", "о": "o", "п": "p", "р": "r", "с": "s", "т": "t", "у": "u", "ф": "f", "х": "h", "ц": "ts", "ч": "ch", "ш": "sh", "щ": "shh", "ы": "y", "э": "je", "ю": "ju", "я": "ja" };
var mSetEng = { "1": "1", "2": "2", "3": "3", "4": "4", "5": "5", "6": "6", "7": "7", "8": "8", "9": "9", "0": "0", "a": "a", "b": "b", "c": "c", "d": "d", "e": "e", "f": "f", "g": "g", "h": "h", "i": "i", "j": "j", "k": "k", "l": "l", "m": "m", "n": "n", "o": "o", "p": "p", "q": "q", "r": "r", "s": "s", "t": "t", "u": "u", "v": "v", "w": "w", "x": "x", "y": "y", "z": "z" };

function getBodyScrollTop() {
    return self.pageYOffset || (document.documentElement && document.documentElement.scrollTop) || (document.body && document.body.scrollTop);
}

function primaryNav() {
    $('.primary-nav li').hover(
		function() {
		    $(this).find('ul:first').css({ visibility: 'visible', display: 'none' }).fadeIn(100); //показываем подменю
		    $(this).find('ul a').css('width', $(this).width()) //IE 6/7 багфикс ширины
		},
		function() {
		    $(this).find('ul:first').css({ visibility: 'hidden' }); //прячем его
		});

    $('.primary-nav li').find('li').click(
		function() {
		    $(this).parent().fadeOut(100);
		}
	);
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

function resize() { //Ресайз боковой панели
    $('body').mouseup(function() {
        if (dragObj1)
            $.cookie('drag1', $('.side').width(), { expires: 365, path: '/' });
        dragObj1 = null //Сброс при отжатии мыши
        $('.resize').removeClass('resizeHover');
    });

    $('.resize')
	.mousedown(function() {
	    dragObj1 = $(this); //Присваиваем объект ресайза
	    $(this).addClass('resizeHover');
	    document.body.onselectstart = function() { return false }; //Убираем выделение текста при перемещении ресайза в IE...
	    return false; //...и в остальных браузерах
	})

    $('body').mousemove(
		function(e) {
		    var cursor = moveEvent(e);
		    if (dragObj1) {
		        //Приравниваем ширину к х-кординате курсора и преобразовывем ее в процентную
		        $('.side').width(((cursor.x - 4) * 100 / $('.container').width()).toFixed(1) + '%');
		    }
		});
}



function block1() {
    $('.block1-title img').css('background-position', '0 -12px');
    $('.block1-title').click(function() { //Переключение
        if (getChangedCookie($('#' + $(this).attr('block')).attr('id'))) {
            $(this).find('img').css('background-position', '0 -12px'); // Стрелка указывает в сторону
        }
        else {
            $(this).find('img').css('background-position', '0 0'); // Стрелка указывает в сторону
        }
        $('#' + $(this).attr('block')).toggle(90);
        return false;
    });
    $('.block1-title').each(function() {
        if ($.cookie($(this).attr('block')) == 'hide') {
            $('#' + $(this).attr('block')).toggle(90);
            $(this).find('img').css('background-position', '0 0'); // Стрелка указывает в сторону
        }
    })

}

function inputFocus(a, b) {
    if (a.value == b) (a.value = '');
}


function inputBlur(a, b) {
    if (a.value == '') (a.value = b);
}


function title4() {
    $('.title4').click(function() {
        getChangedCookie($(this).attr('blockName'))
        $(this).next().toggle(90);
        $(this).find('.ico-minus').toggleClass('ico-plus');
        return false;
    });
    $('.title4').each(function() {
        if ($.cookie($(this).attr('blockName')) == 'hide') {
            $(this).next().toggle(90);
            $(this).find('.ico-minus').toggleClass('ico-plus');
        }
    })
}


function getChangedCookie(blockName) {
    if ($.cookie(blockName) == 'hide') {
        $.cookie(blockName, 'show', { expires: 365, path: '/' });
        return true;
    }
    else {
        $.cookie(blockName, 'hide', { expires: 365, path: '/' });
        return false;
    }
}


function stretchVertical() { //заполнение блоками до пределов контейнеров при загрузке, изменении размера и прокрутке
    if ($.cookie('drag1')) {
        $('.side').width($.cookie('drag1') + "px");
    }
	
	_stretch();
    $(window).resize(function() {
        _stretch();
    })
    $(window).scroll(function() {
        _stretch();
    })
	function _stretch() {
		$('.side1').height($(window).height() - 97 + getBodyScrollTop());
		if (!isIE7()) {
			$('.content2').height($(window).height() - $('.title').height() - 65);
		} else {
			$('.content2').height($(document).height() - $('.title').height() - 65);
		}
	}
}

function autoFill(e, source, target, copy, title, keywords, desc) {
    var keynum;
    var keychar;
    if (window.event) // IE
    {
        keynum = e.keyCode;
    }
    else if (e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which;
    }
    keychar = String.fromCharCode(keynum).toLowerCase();
    var check = /[\w\А-Яа-я\d\s\"\!\@\#\$\%\^\&\*\(\)\<\>\[\]\_\+\-\=\|\~\`\№\;\:\?\.\,\']/;

    if (check.test(keychar)) {
        if ($(target).val() == translit($(source).val())) {
            $(target).val($(target).val() + translit(keychar));
        }

    }
    else if (keynum == 8) {
        var sourceString = $(source).val();
        var prevString = sourceString.substr(0, sourceString.length - 1);
        if (prevString.length == 0 && translit(sourceString) == $(target).val().toLowerCase()) {
            $(target).val("");
        }
        else {
            var targetString = $(target).val().substr(0, $(target).val().length - charLength(sourceString.substr(sourceString.length - 1, 1))).toLowerCase();

            if (targetString == translit(prevString)) {
                $(target).val(targetString);
            }
        }
    }
    else {
        return false;
    }
    if (check.test(keychar)) {
        if ($(copy).val() == $(source).val()) {
            $(copy).val($(copy).val() + String.fromCharCode(keynum));
        }
        if ($(title).val() == $(source).val()) {
            $(title).val($(title).val() + String.fromCharCode(keynum));
        }
        if ($(keywords).val() == $(source).val()) {
            $(keywords).val($(keywords).val() + String.fromCharCode(keynum));
        }
        if ($(desc).val() == $(source).val()) {
            $(desc).val($(desc).val() + String.fromCharCode(keynum));
        }
    }
    else if (keynum == 8) {
        var sourceString = $(source).val();
        var prevString = sourceString.substr(0, sourceString.length - 1);
        if (prevString.length == 0) {

            if (sourceString == $(copy).val())
                $(copy).val("");
            if (sourceString == $(title).val())
                $(title).val("");
            if (sourceString == $(keywords).val())
                $(keywords).val("");
            if (sourceString == $(desc).val())
                $(desc).val("");
            return true;
        }

        var targetString1 = $(copy).val().substr(0, $(copy).val().length - 1);
        var targetString2 = $(title).val().substr(0, $(title).val().length - 1);
        var targetString3 = $(keywords).val().substr(0, $(keywords).val().length - 1);
        var targetString4 = $(desc).val().substr(0, $(desc).val().length - 1);

        if (targetString1 == prevString) {
            $(copy).val(targetString1);
        } 
        if (targetString2 == prevString) {
            $(title).val(targetString2);
        } 
        if (targetString3 == prevString) {
            $(keywords).val(targetString3);
        } 
        if (targetString4 == prevString) {
            $(desc).val(targetString4);
        }

    }


    return true;

}

function charLength(letter) {
    if (mSetRus[letter.toLowerCase()]) {
        return mSetRus[letter.toLowerCase()].length;
    }
    else {
        if (mSetEng[letter.toLowerCase()]) {
            return 1;
        }
    }
    return 0;
}

function translit(sourceString) {
    var translit = "";
    for (var i = 0; i < sourceString.length; i++) {
        var letter = sourceString.charAt(i).toLowerCase();
        if (mSetRus[letter]) {
            translit += mSetRus[letter];
        }
        else {
            if (mSetEng[letter]) {
                translit += mSetEng[letter];
            }
        }

    }
    return translit;
}

function refreshMain(main, active) {
    if ($(main).get(0).checked) {
        $(active).get(0).disabled = true;
        $(active).get(0).checked = true;
    }
    else {
        $(active).get(0).disabled = false;
    }
}

function updatePosPressed(event, sender, pageid) {
    if (event.keyCode == 13) {
        updatePos(sender, pageid);
    }
    return true;
}
function updatePos(sender, pageid) {
    var inputs = $("#leftContent :text");
    if (inputs.length == 1) {
        $("#leftContent").load("/Helpers/PageStruct.aspx", { Act: 0 });
        return;
    }
    if (!parseInt(sender.value)) {
        $("#leftContent").load("/Helpers/PageStruct.aspx", { Act: -1 });
        return;
    }
    var listOrder = 0;
    if (sender.value <= 1) {
        listOrder = 0;
    }
    else if (sender.value > inputs.length) {
        listOrder = inputs.length - 1;
    }
    else {
        listOrder = sender.value - 1;
    }
    $.get("/Helpers/PageStruct.aspx", { Act: 1, PageID1: sender.getAttribute("pageid"), PageID2: (inputs.get(listOrder)).getAttribute("pageid") },
        function(Data) {
            if (Data.substr(0, 2) == 'ok') {
                $("#leftContent").html(Data.substring(2, Data.length));

                doPostBackAsync('default.aspx', 'RebuildLeftMenu');
            }
            else
                $("#leftContent").html(Data);
            structData = Data;

        });
}

function deletePages(qst) {
    if (window.confirm(qst)) {
        var pageIDs = '';
        $("#leftContent :checkbox:checked").each(function() {
            pageIDs += $(this).attr('pageid') + ';'
        });
        //$("#leftContent").load("/Helpers/PageStruct.aspx", { Act: 2, PageIDS: pageIDs });
        $.get("/Helpers/PageStruct.aspx", { Act: 2, PageIDS: pageIDs },
        function(Data) {
            if (Data.substr(0, 2) == 'ok') {
                structData = Data.substring(2, Data.length);
                $("#leftContent").html(structData);
                doPostBackAsync('default.aspx', 'RebuildLeftMenu');
            }
            else {
                $("#leftContent").html(Data);
                structData = Data;
            }
        });


    }
}

function doPostBackAsync(eventName, eventArgs) {
    var prm = Sys.WebForms.PageRequestManager.getInstance();

    if (!Array.contains(prm._asyncPostBackControlIDs, eventName)) {
        prm._asyncPostBackControlIDs.push(eventName);
    }

    if (!Array.contains(prm._asyncPostBackControlClientIDs, eventName)) {
        prm._asyncPostBackControlClientIDs.push(eventName);
    }

    __doPostBack(eventName, eventArgs);
}

function ask(message) {
    return window.confirm(message);
}

function mouseOut(sender, text) {
    if (sender.value == '')
        sender.value = text;
}

function mouseIn(sender, text) {
    if (sender.value == text)
        sender.value = '';
}


//Шоу начинается! Готовим бубен 128 размера.
function positionModules() {
    if ($('.modules_installed, .module-select')) { var xPos = $('.modules_installed, .module-select').prev().position() }

    if (navigator.userAgent.indexOf('Firefox') != -1) {
        $('.modules_installed').css({ 'top': xPos.top + 206, 'left': xPos.left + 50 }).fadeIn();
        $('.module-select').css({ 'top': xPos.top + 20, 'left': xPos.left - 25 }).fadeIn();
    } else {
        $('.modules_installed').css({ 'top': xPos.top + 50, 'left': xPos.left + 50 }).fadeIn();
        $('.module-select').css({ 'top': xPos.top - 135, 'left': xPos.left - 25 }).fadeIn();
    }

    $('.modules_installed .btn4').bind('click', function() { $(this).closest('.modules_installed').hide() })
}


function positionModulesHide() {
    if ($('.modules_installed, .module-select')) { var xPos = $('.modules_installed, .module-select').prev().position() }

    if (navigator.userAgent.indexOf('Firefox') != -1) {
        $('.modules_installed').css({ 'top': xPos.top + 206, 'left': xPos.left + 50 }).fadeOut();
        $('.module-select').css({ 'top': xPos.top + 20, 'left': xPos.left - 25 }).fadeOut();
    } else {
        $('.modules_installed').css({ 'top': xPos.top + 50, 'left': xPos.left + 50 }).fadeOut();
        $('.module-select').css({ 'top': xPos.top - 135, 'left': xPos.left - 25 }).fadeOut();
    }

    $('.modules_installed .btn4').bind('click', function() { $(this).closest('.modules_installed').hide() })
}

//256 размер
function showModulesInstalledMsg(postback) {
    $('#jQAddModule').bind('click', function() {
        //if (postback)
        //    positionModulesHide();
        //else
        positionModules();
    });

    $('.module-close').bind('click', function() {
        $('.module-select').fadeOut();
    });

    $('.module-selectable li').draggable({
        helper: 'clone',
        appendTo: 'body',
        addClasses: false,
        revert: 'invalid',
        revertDuration: 30,
        scroll: false,
        zIndex: 100,
        cursorAt: { left: 0, top: 0 },
        start: function(e, ui) {
            $('.module-select').fadeOut();
            $(this).addClass('ui-selected');
        },
        stop: function(e, ui) {
            $(this).removeClass('ui-selected');
        }
    });
    $('div.RadDockZone').droppable({
        accept: '.module-selectable li',
        drop: function(e, ui) {
            var moduleID = $(ui.helper).attr('mid');
            var containerID = $(this).attr('cname');

            //positionModulesHide();
            doPostBackAsync('default.aspx', 'AddModuleToTemplate_' + moduleID + '_' + containerID);
        }
    });
}

/*******************************************************************************************
// Регистрирует RadTabStrip для исползования с хэш частью строки запроса.
// При выборе закладки, в хэш добавляется её номер. При загрузке страницы, в
// адресе которой есть хэщ, выбирается закладка с этим номером.
*******************************************************************************************/
function TabStripHashChecker(tabStrip) {
    var hash = window.location.hash.replace("#", "");
    if (hash !== "") {
        tabStrip.get_tabs().getTab(hash).select();
    }

    if (tabStrip != null) {
        tabStrip.add_tabSelected(function(sender, eventArgs) {
            window.location.replace(window.location.pathname + window.location.search + "#" + eventArgs.get_tab().get_index());
        });
    }
};
