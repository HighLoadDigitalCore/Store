$(document).ready(function () {
    loadTree();
    loadTreeFilter();
    //loadEditable();
    loadSwitcher();
    loadOdds();
    disableBoxes();
    loadOrderBoxes();
    loadLangSwitcher();
    setRedirect();
    initFiltersDDL();
    initEditors();
    initCalendars();
    loadUI();
    loadUIElems();
    
    $('.customer-menu #reloadTreeData').click(function () {
        var dataValue = { "SelectedSection": null, prods: true, isReload : true};
        $.ajax({
            url: "/Master/ru/Home/getTreeData",
            type: "GET",
            data: dataValue,
            success: function () {
                alert("Дерево каталога обновлено");
                document.location.reload(true);
            }
        });
    });

    $('.show-popup').click(function() {
        $('.search.pop-up').toggleClass('pop-up-active');
    });

    $('.primary-nav>li>a i.icon-photon').css({ 'display': 'none' }, { 'padding-left': '14px !important' });
    
    
    
});

var dataLink = '/Master/ru/Home/getTreeData';
var editPageLink = '/Master/ru/Pages/Edit';
var deletePageLink = '/Master/ru/Pages/Delete';
var saveNode = '/Master/ru/Pages/saveNode';
var orderPagesLink = '/Master/ru/TableEditors/Pages?Type=List&ParentID=';
var addProdLink = '/Master/ru/TableEditors/Products?Type=Edit&ParentID=';
var viewProdLink = '/Master/ru/TableEditors/Products?Type=List&ParentID=';
var addCatLink = '/Master/ru/TableEditors/Categories?Type=Edit&ParentID=';
var editProdLink = '/Master/ru/TableEditors/Products?Type=Edit&UID=';
var deleteProdLink = '/Master/ru/TableEditors/Products?Type=Delete&UID=';
var editCatLink = '/Master/ru/TableEditors/Categories?Type=Edit&UID=';
//var viewCatLink = '/Master/ru/TableEditors/Categories?Type=List&ParentID=';
var viewCatLink = '/Master/ru/Home/Categories?ParentID=';
var deleteCatLink = '/Master/ru/TableEditors/Categories?Type=Delete&UID=';
var loading = false;
var filterDataLink = "/Master/ru/Pages/getTreeData";
var saveFieldDataLink = "/Master/ru/Catalog/SaveField";

function saveTelerikContent(col, val) {
    

    console.log('#' + col);
    console.log($('#' + col));
    console.log(val.length);
    $('#' + col).html('');
    $('#' + col).val(val);
}

function showTextEditor(obj) {
    if ($(obj).parent().find('.hidden-text-editor').is(":hidden")) {
        $(obj).parent().find('.hidden-text-editor').show();
        $(obj).find('span').not('.image-down, .image-up').html('Скрыть редактор');
        $(obj).find('span').not('.image-down, .image-up').css('margin-left', '5px');
        $(obj).find('.image-down').attr('class', 'image-up');

    } else {
        $(obj).parent().find('.hidden-text-editor').hide();
        $(obj).find('span').not('.image-down, .image-up').html('Показать редактор');
        $(obj).find('span').not('.image-down, .image-up').css('margin-left', '0px');
        $(obj).find('.image-up').attr('class', 'image-down');
    }
}

function loadByLink(obj) {
    if (loading) return false;
    loading = true;
    $.get($(obj).attr('href'), {}, function (data) {
        saveLink($(obj).attr('href'));
        $('#PageContent').replaceWith(data);
        loadTranslitter();
        ajaxComplete();
        loadUIElems();
        loadInfo(data);
        initEditors();
        loading = false;
    });
    return false;
}

function checkAndToggle(obj) {
    if ($(obj).hasClass('clicked'))
        return false;
    else {
        $(obj).addClass('clicked');
        return true;
    }
}

function clearTextEditors() {
    //for (var instance in CKEDITOR.instances) {
        
    //    CKEDITOR.instances[instance].destroy();
    //}

}

function loadContent(arg, allowRedir, callback) {
    //clearTextEditors();

    var showPreview = arg.indexOf('x') == 0 || arg.indexOf('r') == 0;

    if (!$('.title-in1 .previewsite').length && showPreview) {
        $('.title-in1').append('<a class="previewsite preview-link" target="_blank" id="pageView" href="/"><img src="/content/admin/eye-pict.png">Посмотреть на сайте</a>');
    }
    if ($('.title-in1 .previewsite').is(':hidden') && showPreview) {
        $('.title-in1 .previewsite').show();
    }
    if ($('.image-add-text').length) {
        $('.title-in1 .previewsite').hide();
    }
    if (arg.indexOf(',') > 0) {
        arg = arg.substr(0, arg.indexOf(','));
        /*$.cookie('node_selected_main', '#' + arg, {
            expires: 365,
            path: '/Master/ru'
        });*/
    }
    $.cookie('node_selected_main', '#' + arg, {
        expires: 365,
        path: '/Master/ru'
    });
    var selection = $('li[id="' + arg + '"]').find('> a').addClass('jstree-clicked');

    if (loading) return;
    loading = true;
    if ((document.location.pathname != '/Master/ru/' && document.location.pathname != '/Master/ru/Home/Index' /*&& document.location.pathname != '/Master/ru/Home/Recicle'*/) || !treeLoaded) {
        if (!allowRedir) {
            loading = false;
            return;
        } else {
            document.location.href = '/Master/ru/';
            return;
        }
    }

        if (arg.indexOf('r') == 0) {
            document.location.href = "/Master/ru/Home/Recicle";
        }

        if (arg.indexOf('x') == 0) {


            $.post('/Master/ru/Pages/UniversalModuls', { pageID: arg.substr(1), HideSettings: 1 }, function(data) {
                saveLink('/Master/ru/Pages/UniversalModuls?pageID=' + arg.substr(1) + "&HideSettings=1");
                var cnt = $('#PageContent');
                if (!cnt.length) {
                    cnt = $('#EditPage');
                }
                if (!cnt.length) {
                    cnt = $('#DeletePage');
                }

                cnt.replaceWith(data);

                initDragAndDrop();
                loading = false;
                loadInfo(data);

                if (callback)
                    callback();
            });
        }
        if (arg.indexOf('c') == 0) {
            //var target = '/Master/ru/TableEditors/Categories?Type=List&ParentID=' + arg.substr(1);
            var target = editCatLink + arg.substr(1);
            /*
                if (target != (document.location.pathname + document.location.search)) {
                    setTimeout(function () {
                        document.location.href = target;
                    }, 100);
        
                }
        */
            $.get(target, {}, function(data) {
                saveLink(target);
                var cnt = $('#PageContent');
                if (!cnt.length) {
                    cnt = $('#EditPage');
                }
                if (!cnt.length) {
                    cnt = $('#DeletePage');
                }

                cnt.replaceWith(data);
                loadTranslitter();
                ajaxComplete();
                loadUIElems();
                loadInfo(data);
                initEditors();
                loading = false;

                if (callback)
                    callback();
            });
        }
        if (arg.indexOf('p') == 0) {
            var target = '/Master/ru/TableEditors/Products?Type=Edit&UID=' + arg.substr(1);
            /*        if (target != (document.location.pathname + document.location.search)) {
                    setTimeout(function () {
                        document.location.href = target;
                    }, 100);
                }*/
            $.get(target, {}, function(data) {
                saveLink(target);
                var cnt = $('#PageContent');
                if (!cnt.length) {
                    cnt = $('#EditPage');
                }
                if (!cnt.length) {
                    cnt = $('#DeletePage');
                }

                cnt.replaceWith(data);
                loadTranslitter();
                ajaxComplete();
                loadUIElems();
                loadInfo(data);
                initEditors();
                loading = false;

                if (callback)
                    callback();
            });
        }
    //jstree-clicked
}

function resetSelectNode(node) {

    //$('#tree').jstree("refresh");
    $("#tree").hide();
    $("#tree").jstree('destroy');
    $.removeCookie('node_selected_main');
    $.cookie('node_selected_main', node, { expires: 365, path: '/Master/ru' });
    loadTree();

    loadContent(node.substr(1), false);

}

function initDragAndDrop() {


    setTimeout(function () {
        $('.cell-wrapper iframe').each(function () {
            try {
                var h = $($(this).get(0).contentWindow.document.body).find('.text-content').height() + 27; //$(this).get(0).contentWindow.document.body.offsetHeight;
                //debugger;
                if ($($(this).get(0).contentWindow.document.body).find('.text-content').length) {
                    //console.log("frame: " + h);
                    $(this).get(0).height = h;
                } else {
                    $(this).get(0).height = 27;
                }
            } catch (e) {
                
            }
        });
    }, 1500);
    $('.cell-wrapper')
        .sortable({
            appendTo: document.body,
            tolerance: 20,
            distance: 20,
            /*forceHelperSize: true,*/
            dropOnEmpty: true,
            /*containment: '.containers',*/
            connectWith: '.cell-wrapper',
            placeholder: "highlight",
            start: function (event, ui) {
                ui.item.toggleClass("highlight");
            },
            stop: function (event, ui) {
                ui.item.toggleClass("highlight");
                var cell = ui.item.parents('.cell-wrapper').attr('arg');
                var lst = '';
                ui.item.parents('.cell-wrapper').find('.modul').each(function () {
                    lst += $(this).attr('arg') + ";";
                });

                var pid = $.cookie('node_selected_main').substr(2);

                $.post('/Master/ru/Home/SaveModulOrder', { cell: cell, list: lst, pageID: pid }, function (d) {
                    $('#PageContent').replaceWith(d);
                    initDragAndDrop();
                });
            }
        }).sortable("option", "cursorAt", { left: 0, top: 0 });;
}

var needReload = false;
var treeLoaded = false;

function showBtns(obj) {

    var w = $(obj).find('a').length * 30;
    var offset = $(obj).parent().offset().left - $('#tree').offset().left;
    var maxpos = $('#tree').width() - 40 - w - offset;
    if (($(obj).parent().offset().left + $(obj).parent().width() + w > $('#tree').width()) && $(obj).css('display')!="inline-block") {
        $(obj).parent().css('position', 'relative');
        $(obj).css('display', 'block').css('width', w + "px").css('left', maxpos + 'px').css('position', 'absolute').css('top', '2px');
    } else {
        $(obj).attr('style', '');
        $(obj).parent().css('position', 'inherit');
        $(obj).css('display', 'inline-block');
        

    }
}

function loadTree() {
    treeLoaded = false;
    $('#tree').html('<img src="/content/loader.gif" style="width:50px; height: 50px;" class="tree-loader">');
    $('#tree').show();
    $('.tree-loader').css('margin-left', ($('#tree').width() - $('.tree-loader').width()) / 2).
        css('margin-top', '150px');

    $.getJSON(dataLink, { prods: true }, function (res) {


        $('#tree').jstree({
            "plugins": [
                "themes", "json_data", "ui", "crrm", "cookies", "dnd", "search", "types", "state"
            ],
            'state': { 'key': 'subproductgroups', 'ttl': false },
            "cookies": {
                "cookie_options": {
                    expires: 365,
                    path: '/Master/ru'
                },
                "save_selected": "node_selected_main",
                "save_opened": "node_opened_main"
            },
            "crrm": {
                "move": {
                    "default_position": "first",
                    "check_move": function (m) {
                        return m.o[0].id == "x0" || (m.r[0].id == "x0" && (m.p == "before" || m.p == "after")) ? false : true;
                    }
                }
            },
            "json_data": { "data": res, "progressive_render": true, cache: false }
        }).bind('ready.jstree', function (e, data) {
            $('#tree').jstree().restore_state();


        }).bind("select_node.jstree", function (e, data) {
            /*
                        if (data.rslt.obj.attr('id') == "c0") {
                            document.location.href = '/Master/ru/Home/Recicle';
                            loading = true;
                            return false;
                        }
            */
            if (document.location.pathname != '/Master/ru/') {
                //$('#tree').jstree("deselect_all");
                //$('#tree').deselect_node(data);
            }
            if (treeLoaded) {
                $('#tree li .btns').each(function () {
                    if ($(this).parent().attr('id') != data.rslt.obj.attr('id'))
                        $(this).css('display', 'none');
                });

                loadContent(data.rslt.obj.attr('id'), 1);

            }

            if (data.rslt.obj.attr('id') && data.rslt.obj.attr('id').indexOf('x') == 0) {
                loadContent(data.rslt.obj.attr('id'));
            }


        }).bind("click.jstree", function (event, data) {
            if (!$(event.target).hasClass('jstree-icon')) {
                var node = $(event.target).closest("li");
                $('#tree li .btns').each(function () {
                    if ($(this).parent().attr('id') != node.attr('id'))
                        $(this).css('display', 'none');
                });
                loadContent(node.attr('id'), 1);
            }
            //return false;
        }).bind("move_node.jstree", function (event, data) {

            var node = data.rslt.o.attr("uid");
            var target = data.rslt.r.attr("uid");
            var rel = data.rslt.p;
            $.post(saveNode, { nodeID: node, targetID: target, type: rel }, function (data) {
            });


        }).bind("loaded.jstree", function (event, data) {
            var parent = $('#tree').find("[id='x0']");
            var newNode = {
                data: "Корзина",
                attr: {
                    id: "r0", href: "/Master/ru/Home/Recicle", uid: -1, priority: 0, class: "jstree-leaf cart-icon"
                }
            };
            $('#tree').jstree("create_node", parent, 'last', newNode, false, false);
            var depth = 1;
            data.inst.get_container().find('li').each(function (i) {
                if (data.inst.get_path($(this)).length <= depth) {
                    data.inst.open_node($(this));
                }
            });


            //$('#tree').jstree('open_all');
            $('#tree li a[href="#"]').each(function () {


                /*
                                var ins = $(this).find('ins').clone();
                                var text = '<span>' + $(this).text() + "</span>";
                                $(this).html('');
                                $(this).append(ins);
                                $(this).append(text);
                */




                if ($(this).parents('li').hasClass('cart-icon')) {
                    return;
                }

                if ($(this).parents('li').attr('allcnt') != '0' && !$(this).find('.cnt-box').length) {
                    $(this).append("<span class='cnt-box'>" + $(this).parents('li').attr('allcnt') + "</span>");
                }

                if ($(this).find('.btns').length) {
                    return;
                }
                if ($(this).parents('li').attr('id').indexOf('x') == 0 || $(this).parents('li').attr('uid') == '0') {


                    var catid = $('#CatPageID').val();
                    if (catid == $(this).parents('li').attr('uid')) {

                        var content = $('<span class="btns"></span>');


                        content.append('<a href="/" title="Удалить раздел" onclick="return deletePage(' + $(this).parents('li').attr('uid') + ')" class="delpage"></a>');
                        content.append('<a href="/" title="Добавить подкатегорию" onclick="return addCat(' + ($(this).parents('li').attr('uid').indexOf('x') >= 0 ? "1" : $(this).parents('li').attr('uid')) + ')" class="addpage"></a>');
                        content.append('<a href="/" title="Список подкатегорий" onclick="return catList(' + $(this).parents('li').attr('uid') + ')" class="ico-list"></a>');

/*
                        if ($(this).parents('li').attr('cnt') != '0') {
                            content.append('<a href="/" title="Список товаров раздела" onclick="return viewProds(' + $(this).parents('li').attr('uid') + ')" class="viewprod"></a>');
                        }
*/
                        content.append('<a href="/" title="Добавить товар" onclick="return addProd(' + $(this).parents('li').attr('uid') + ')" class="addprod"></a>');
                        /*
                                                if ($(this).parents('li').attr('uid') != '0')
                                                    content.append('<a href="/" title="Редактировать раздел" onclick="return editPage(' + $(this).parents('li').attr('uid') + ')" class="editpage"></a>');
                        */
                        if ($(this).parents('li').attr('uid') != '0')
                            content.append('<a href="/" title="Открыть сайт" target="_blank" onclick="return openPage(&quot;' + $(this).parents('li').attr('id') + '&quot;, this)" class="open-page ico-eye"></a>');

                        $(this).append(content);

                        $(this).mouseover(function () {
                            showBtns($(this).parent().find('.btns:first'));
                        }).mouseout(function () {
                            $(this).find('.btns').each(function () {
                                if (!$(this).parent().hasClass('jstree-clicked'))
                                    $(this).css('display', 'none');
                            });
                        });

                    } else {
                        var content = $('<span class="btns"></span>');
                        if ($(this).parents('li').attr('uid') != '0')
                            content.append('<a href="/" title="Удалить раздел" onclick="return deletePage(' + $(this).parents('li').attr('uid') + ')" class="delpage"></a>');
                        content.append('<a href="/" title="Открыть сайт" target="_blank" onclick="return openPage(&quot;' + $(this).parents('li').attr('id') + '&quot;, this)" class="open-page ico-eye"></a>');

                        content.append('<a href="/" title="Добавить подраздел" onclick="return addPage(' + $(this).parents('li').attr('uid') + ')" class="addpage"></a>');
                        /*
                                                if ($(this).parents('li').attr('uid') != '0')
                                                    content.append('<a href="/" title="Редактировать раздел" onclick="return editPage(' + $(this).parents('li').attr('uid') + ')" class="editpage"></a>');
                        */
                        //if ($(this).parents('li').attr('uid') != '0')

                        if ($(this).parents('li').attr('pagecnt') != '0' || $(this).parents('li').attr('id') == 'x0') {
                            content.append('<a href="/" title="Сортировка папок" onclick="return sortPage(' + $(this).parents('li').attr('uid') + ')" class="ico-list"></a>');
                        }


                        $(this).append(content);

                        $(this).mouseover(function () {
                            showBtns($(this).parent().find('.btns:first'));
                        }).mouseout(function () {
                            $(this).find('.btns').each(function () {
                                if (!$(this).parent().hasClass('jstree-clicked'))
                                    $(this).css('display', 'none');
                            });
                        });
                    }
                }
                else if ($(this).parents('li').attr('id').indexOf('c') == 0) {



                    var content = $('<span class="btns"></span>');
                    content.append('<a href="/" title="Удалить раздел" onclick="return deleteCat(' + $(this).parents('li').attr('uid') + ')" class="delpage"></a>');


                    content.append('<a href="/" title="Добавить товар" onclick="return addProd(' + $(this).parents('li').attr('uid') + ')" class="addprod"></a>');

/*
                    if ($(this).parents('li').attr('cnt') != '0') {
                        content.append('<a href="/" title="Список товаров раздела" onclick="return viewProds(' + $(this).parents('li').attr('uid') + ')" class="viewprod"></a>');
                    }
*/


                    content.append('<a href="/" title="Открыть сайт" target="_blank" onclick="return openPage(&quot;' + $(this).parents('li').attr('id') + '&quot;, this)" class="open-page ico-eye"></a>');
                    content.append('<a href="/" title="Добавить подраздел" onclick="return addCat(' + $(this).parents('li').attr('uid') + ')" class="addpage"></a>');
                    if ($(this).parents('li').attr('catcnt') != '0') {
                        content.append('<a href="/" title="Список подразделов" onclick="return catList(' + $(this).parents('li').attr('uid') + ')" class="ico-list"></a>');
                    }

                    $(this).append(content);

                    $(this).mouseover(function () {
                        showBtns($(this).parent().find('.btns:first'));
                    }).mouseout(function () {
                        $(this).find('.btns').each(function () {
                            if (!$(this).parent().hasClass('jstree-clicked'))
                                $(this).css('display', 'none');
                        });
                    });

                }
                else if ($(this).parents('li').attr('id').indexOf('p') == 0) {
                    var content = $('<span class="btns"></span>');
                    content.append('<a href="/" title="Удалить товар" onclick="return deleteProd(' + $(this).parents('li').attr('uid') + ')" class="delpage"></a>');
                    /*content.append('<a href="/" title="Редактировать товар" onclick="return editProd(' + $(this).parents('li').attr('uid') + ')" class="editpage"></a>');*/
                    content.append('<a href="/" title="Открыть сайт" target="_blank" onclick="return openPage(&quot;' + $(this).parents('li').attr('id') + '&quot;, this)" class="open-page ico-eye"></a>');

                    $(this).append(content);

                    $(this).mouseover(function () {
                        showBtns($(this).parent().find('.btns:first'));
                    }).mouseout(function () {
                        $(this).find('.btns').each(function () {
                            if (!$(this).parent().hasClass('jstree-clicked'))
                                $(this).css('display', 'none');
                        });
                    });
                }
            });

            setTimeout(function () {
                treeLoaded = true;

                if (document.location.pathname == '/Master/ru/' || document.location.pathname == '/Master/ru/Home/Index' || document.location.pathname == '/Master/ru/Image') {
                    if ($.cookie('node_selected_main')) {
                        //console.log('load by cook: ', $.cookie('node_selected_main').substr(1));
                        loadContent($.cookie('node_selected_main').substr(1), 0);
                    }else if (document.location.hash.replace('#', '').length > 0) {
                        loadByHash();
                        //console.log('load by hash');
                    } 
                    else {
                        loadContent('x0', 0);
                    }

                }
            }, 100);

            $("#tree").show();

        }).bind("open_node.jstree", function (e, data) {
            /*
                        if (data.rslt.obj.attr('id') == "r0") {
                            document.location.href = '/Master/ru/Home/Recicle';
                        }
            */
            $(data.rslt.obj).find('li a[href="#"]').each(function () {

                /*                var ins = $(this).find('ins').clone();
                                var text = '<span>' + $(this).text()+"</span>";
                                $(this).html('');
                                $(this).append(ins);
                                $(this).append(text);*/


                //$(this).append(content);

                var catid = $('#CatPageID').val();
                if ($(this).parents('li').hasClass('cart-icon')) {
                    return;
                }

                if ($(this).parents('li').attr('allcnt') != '0' && !$(this).find('.cnt-box').length) {
                    $(this).append("<span class='cnt-box'>" + $(this).parents('li').attr('allcnt') + "</span>");
                }
                if ($(this).find('.btns').length) {
                    return;
                }

                if ($(this).parents('li').attr('id').indexOf('x') == 0 || $(this).parents('li').attr('uid') == '0') {

                    if (catid == $(this).parents('li').attr('uid')) {
                        var content = $('<span class="btns"></span>');

                        content.append('<a href="/" title="Удалить раздел" onclick="return deletePage(' + $(this).parents('li').attr('uid') + ')" class="delpage"></a>');

/*
                        if ($(this).parents('li').attr('cnt') != '0') {
                            content.append('<a href="/" title="Список товаров раздела" onclick="return viewProds(' + $(this).parents('li').attr('uid') + ')" class="viewprod"></a>');
                        }
*/

                        //                        content.append('<a href="/" title="Список товаров раздела" onclick="return viewProds(' + $(this).parents('li').attr('uid') + ')" class="viewprod"></a>');
                        content.append('<a href="/" title="Добавить товар" onclick="return addProd(' + $(this).parents('li').attr('uid') + ')" class="addprod"></a>');

                        if ($(this).parents('li').attr('uid') != '0')
                            content.append('<a href="/" title="Открыть сайт" target="_blank" onclick="return openPage(&quot;' + $(this).parents('li').attr('id') + '&quot;, this)" class="open-page ico-eye"></a>');

                        /*
                                                if ($(this).parents('li').attr('uid') != '0')
                                                    content.append('<a href="/" title="Редактировать раздел" onclick="return editPage(' + $(this).parents('li').attr('uid') + ')" class="editpage"></a>');
                        */

                        content.append('<a href="/" title="Добавить подраздел" onclick="return addCat(' + $(this).parents('li').attr('uid') + ')" class="addpage"></a>');
                        content.append('<a href="/" title="Список подразделов" onclick="return catList(' + $(this).parents('li').attr('uid') + ')" class="ico-list"></a>');
                        $(this).append(content);

                        $(this).mouseover(function () {
                            showBtns($(this).parent().find('.btns:first'));

                        }).mouseout(function () {
                            $(this).find('.btns').each(function () {
                                if (!$(this).parent().hasClass('jstree-clicked'))
                                    $(this).css('display', 'none');
                            });
                        });
                    } else {
                        var content = $('<span class="btns"></span>');
                        if ($(this).parents('li').attr('uid') != '0')
                            content.append('<a onclick="return deletePage(' + $(this).parents('li').attr('uid') + ')" href="/" title="Удалить раздел" class="delpage"></a>');

                        content.append('<a href="/" title="Открыть сайт" target="_blank" onclick="return openPage(&quot;' + $(this).parents('li').attr('id') + '&quot;, this)" class="open-page ico-eye"></a>');

                        content.append('<a href="/" onclick="return addPage(' + $(this).parents('li').attr('uid') + ')" title="Добавить подраздел" class="addpage"></a>');

                        //if ($(this).parents('li').attr('uid') != '0')
                        if ($(this).parents('li').attr('pagecnt') != '0' || $(this).parents('li').attr('id') == 'x0') {
                            content.append('<a href="/" title="Сортировка папок" onclick="return sortPage(' + $(this).parents('li').attr('uid') + ')" class="ico-list"></a>');
                        }


                        /*
                                                if ($(this).parents('li').attr('uid') != '0')
                                                    content.append('<a href="/" onclick="return editPage(' + $(this).parents('li').attr('uid') + ')" title="Редактировать раздел" class="editpage"></a>');
                        */


                        $(this).append(content);
                        $(this).mouseover(function () {
                            showBtns($(this).parent().find('.btns:first'));
                        }).mouseout(function () {
                            $(this).find('.btns').each(function () {
                                if (!$(this).parent().hasClass('jstree-clicked'))
                                    $(this).css('display', 'none');
                            });
                        });
                    }
                }
                else if ($(this).parents('li').attr('id').indexOf('c') == 0) {
                    var content = $('<span class="btns"></span>');
                    content.append('<a href="/" title="Удалить раздел" onclick="return deleteCat(' + $(this).parents('li').attr('uid') + ')" class="delpage"></a>');


                    content.append('<a href="/" title="Добавить товар" onclick="return addProd(' + $(this).parents('li').attr('uid') + ')" class="addprod"></a>');
                    //content.append('<a href="/" title="Список товаров раздела" onclick="return viewProds(' + $(this).parents('li').attr('uid') + ')" class="viewprod"></a>');
/*
                    if ($(this).parents('li').attr('cnt') != '0') {
                        content.append('<a href="/" title="Список товаров раздела" onclick="return viewProds(' + $(this).parents('li').attr('uid') + ')" class="viewprod"></a>');
                    }
*/

                    content.append('<a href="/" title="Открыть сайт" target="_blank" onclick="return openPage(&quot;' + $(this).parents('li').attr('id') + '&quot;, this)" class="open-page ico-eye"></a>');

                    content.append('<a href="/" title="Добавить подраздел" onclick="return addCat(' + $(this).parents('li').attr('uid') + ')" class="addpage"></a>');

                    if ($(this).parents('li').attr('catcnt') != '0') {
                        content.append('<a href="/" title="Список подразделов" onclick="return catList(' + $(this).parents('li').attr('uid') + ')" class="ico-list"></a>');
                    }
                    $(this).append(content);

                    $(this).mouseover(function () {
                        showBtns($(this).parent().find('.btns:first'));
                    }).mouseout(function () {
                        $(this).find('.btns').each(function () {
                            if (!$(this).parent().hasClass('jstree-clicked'))
                                $(this).css('display', 'none');
                        });
                    });

                }
                else if ($(this).parents('li').attr('id').indexOf('p') == 0) {
                    var content = $('<span class="btns"></span>');
                    content.append('<a href="/" title="Удалить товар" onclick="return deleteProd(' + $(this).parents('li').attr('uid') + ')" class="delpage"></a>');

                    content.append('<a href="/" title="Открыть сайт" target="_blank" onclick="return openPage(&quot;' + $(this).parents('li').attr('id') + '&quot;, this)" class="open-page ico-eye"></a>');
                    /*content.append('<a href="/" title="Редактировать товар" onclick="return editProd(' + $(this).parents('li').attr('uid') + ')" class="editpage"></a>');*/

                    $(this).append(content);

                    $(this).mouseover(function () {
                        showBtns($(this).parent().find('.btns:first'));
                    }).mouseout(function () {
                        $(this).find('.btns').each(function () {
                            if (!$(this).parent().hasClass('jstree-clicked'))
                                $(this).css('display', 'none');
                        });
                    });
                }
            });
        });


    });
}

function loadByHash() {
    if (document.location.hash.indexOf('pageID=') > 0) {
        $.post(document.location.hash.replace('#', ''), {}, function (data) {
            var cnt = $('#PageContent');
            if (!cnt.length) {
                cnt = $('#EditPage');
            }
            if (!cnt.length) {
                cnt = $('#DeletePage');
            }

            cnt.replaceWith(data);

            loadTranslitter();
            initDragAndDrop();
            loading = false;
            loadInfo(data);
        });
    } else {
        $.get(document.location.hash.replace('#', ''), {}, function (data) {
            var cnt = $('#PageContent');
            if (!cnt.length) {
                cnt = $('#EditPage');
            }
            if (!cnt.length) {
                cnt = $('#DeletePage');
            }

            cnt.replaceWith(data);

            loadTranslitter();
            ajaxComplete();
            loadUIElems();
            loadInfo(data);
            initEditors();
            loading = false;
        });
    }
}

function saveLink(link) {
    document.location.hash = link;
}

function viewProds(id) {
    if (id == $('#CatPageID').val())
        id = 0;

    if (loading) return;
    loading = true;
    var link = viewProdLink + id;
    $.get(link, function (data) {
        saveLink(link);
        var cnt = $('#PageContent');
        if (!cnt.length) {
            cnt = $('#EditPage');
        }
        if (!cnt.length) {
            cnt = $('#DeletePage');
        }

        cnt.replaceWith(data);
        loadTranslitter();
        ajaxComplete();
        loadUIElems();
        loadInfo(data);
        loading = false;
    });
}

function catList(id) {
    //debugger;
    if (id == $('#CatPageID').val())
        id = 1;
    if (loading) return;
    loading = true;
    var link = viewCatLink + id;
    $.get(link, function (data) {
      //  debugger;
        saveLink(link);
        var cnt = $('#PageContent');
        if (!cnt.length) {
            cnt = $('#EditPage');
        }
        if (!cnt.length) {
            cnt = $('#DeletePage');
        }

        cnt.replaceWith(data);
        loadTranslitter();
        ajaxComplete();
        loadUIElems();
        loadInfo(data);
        loading = false;
    });
}

function addProd(id) {
    if (id == $('#CatPageID').val())
        id = 0;
    if (loading) return;
    loading = true;
    var link = addProdLink + id;
    $.get(link, function (data) {
        saveLink(link);
        var cnt = $('#PageContent');
        if (!cnt.length) {
            cnt = $('#EditPage');
        }
        if (!cnt.length) {
            cnt = $('#DeletePage');
        }

        cnt.replaceWith(data);
        loadTranslitter();
        ajaxComplete();
        loadUIElems();
        loadInfo(data);
        loading = false;
    });


}

function editProd(id) {
   // debugger;
    if (loading) return;
    loading = true;
    var link = editProdLink + id;
    $.get(link, function (data) {
        saveLink(link);

        var cnt = $('#PageContent');
        if (!cnt.length) {
            cnt = $('#EditPage');
        }
        if (!cnt.length) {
            cnt = $('#DeletePage');
        }

        cnt.replaceWith(data);
        loadTranslitter();
        ajaxComplete();
        loadUIElems();
        loadInfo(data);
        loading = false;
    });


}

function deleteProd(id) {
    if (loading) return;
    loading = true;
    var link = deleteProdLink + id;
    $.get(link, function (data) {
        var cnt = $('#PageContent');
        if (!cnt.length) {
            cnt = $('#EditPage');
        }
        if (!cnt.length) {
            cnt = $('#DeletePage');
        }

        cnt.replaceWith(data);
        loadTranslitter();
        ajaxComplete();
        loadUIElems();
        loadInfo(data);
        loading = false;
    });


}

function addCat(id) {
    if (id == $('#CatPageID').val())
        id = 1;
    if (loading) return;
    loading = true;
    var link = addCatLink + id;
    $.get(link, function (data) {
        saveLink(link);
        var cnt = $('#PageContent');
        if (!cnt.length) {
            cnt = $('#EditPage');
        }
        if (!cnt.length) {
            cnt = $('#DeletePage');
        }

        cnt.replaceWith(data);
        loadTranslitter();
        ajaxComplete();
        loadUIElems();
        loadInfo(data);
        loading = false;
    });
}

function editCat(id) {
    if (loading) return;
    loading = true;
    var link = editCatLink + id;
    $.get(link, function (data) {
        saveLink(link);
        var cnt = $('#PageContent');
        if (!cnt.length) {
            cnt = $('#EditPage');
        }
        if (!cnt.length) {
            cnt = $('#DeletePage');
        }

        cnt.replaceWith(data);
        loadTranslitter();
        ajaxComplete();
        loadUIElems();
        loadInfo(data);
        loading = false;
    });
}

function deleteCat(id) {
    if (loading) return;
    loading = true;
    var link = deleteCatLink + id;
    $.get(link, function (data) {
        var cnt = $('#PageContent');
        if (!cnt.length) {
            cnt = $('#EditPage');
        }
        if (!cnt.length) {
            cnt = $('#DeletePage');
        }

        cnt.replaceWith(data);
        loadTranslitter();
        ajaxComplete();
        loadUIElems();
        loadInfo(data);
        loading = false;
    });
}


function sortPage(id) {
    if (loading) return;
    loading = true;
    var link = orderPagesLink + id;
    $.get(link, function (data) {
        saveLink(link);
        var cnt = $('#PageContent');
        if (!cnt.length) {
            cnt = $('#EditPage');
        }
        if (!cnt.length) {
            cnt = $('#DeletePage');
        }

        cnt.replaceWith(data);
        loadInfo(data);
        loadTranslitter();
        ajaxComplete();
        loadUIElems();
        loadInfo(data);
        loading = false;
    });
}

function search(e) {
    if (e.keyCode == 13) {
        loadSearch($('[name="SearchText"]').val());
        return false;
    }
}

function loadSearch(val, page) {
    if (!page)
        page = 0;
    $.get('/Master/ru/Search/Admin', { word: val, page: page }, function (data) {
        $('#pageHeader').html('Результаты поиска по сайту');
        var cnt = $('#PageContent');
        if (!cnt.length) {
            cnt = $('#EditPage');
        }
        if (!cnt.length) {
            cnt = $('#DeletePage');
        }

        cnt.replaceWith(data);

        $('.pager a').click(function() {
            var url = $(this).attr('href').split('/');
            loadSearch($('#word').val(), url[url.length - 1]);
            return false;
        });
    });
}

function editPage(id, type, callback) {
    if (loading) return;
    loading = true;
    if (!type)
        type = 1;
    var link = editPageLink + "/" + id + "?vtype=" + type;
    $.get(link, function (data) {
        saveLink(link);
        var cnt = $('#PageContent');
        if (!cnt.length) {
            cnt = $('#EditPage');
        }
        if (!cnt.length) {
            cnt = $('#DeletePage');
        }

        cnt.replaceWith(data);
        loadTranslitter();
        loadInfo(data);
        loading = false;
        if (callback)
            callback();
    });

}


function openInNewTab(url) {
    try {
        var win = window.open(url, '_blank');
        win.focus();
    } catch (e) {

    }
}

function openPage(id, obj) {

    $.get('/Master/ru/Pages/GetSiteUrl', { uid: id }, function (d) {
        document.location.href = d;
        //openInNewTab(d);
    });

}

function addPage(id) {
    if (loading) return;
    loading = true;

    var link = editPageLink + "?ParentID=" + id;
    $.get(link, function (data) {
        var cnt = $('#PageContent');
        if (!cnt.length) {
            cnt = $('#EditPage');
        }
        if (!cnt.length) {
            cnt = $('#DeletePage');
        }

        cnt.replaceWith(data);
        loadTranslitter();
        loadInfo(data);
        loading = false;
    });


}

function deletePage(id) {
    if (loading) return;
    loading = true;

    var link = deletePageLink + "/" + id;
    $.get(link, function (data) {
        var cnt = $('#PageContent');
        if (!cnt.length) {
            cnt = $('#EditPage');
        }
        if (!cnt.length) {
            cnt = $('#DeletePage');
        }
        cnt.replaceWith(data);
        loadTranslitter();
        loadInfo(data);
        loading = false;
    });
}

function closeDialog(obj) {
    $('#dialog-text-edit').find('#PopupText').find('*').remove();
    $('#dialog-text-edit').dialog('destroy');
}
function addModulShow() {
    $('.add-modul-content').dialog({
        "resizable": false,
        'height': 450,
        'width': 350,
        dialogClass: 'module-select',
        beforeClose: function () {
            $('.modul-list').sortable("destroy");
        },
        open: function () {
            $('.modul-list')
                .sortable({
                    appendTo: '.containers',
                    tolerance: 20,
                    distance: 20,
                    dropOnEmpty: true,
                    connectWith: '.cell-wrapper',
                    placeholder: "highlight",
                    start: function (event, ui) {
                        ui.item.toggleClass("highlight");
                        //$('.add-modul-content').dialog('close');
                        $('.ui-dialog').hide();
                    },
                    stop: function (event, ui) {
                        ui.item.toggleClass("highlight");
                        var cell = ui.item.parents('.cell-wrapper').attr('arg');
                        var lst = '';
                        ui.item.parents('.cell-wrapper').find('.modul').each(function () {
                            lst += $(this).attr('arg') + ";";
                        });

                        var pid = $.cookie('node_selected_main').substr(2);

                        $.post('/Master/ru/Home/SaveModulOrderNew', { cell: cell, list: lst, pageID: pid }, function (d) {
                            $('#PageContent').replaceWith(d);
                            initDragAndDrop();
                        });
                    }
                }).sortable("option", "cursorAt", { left: 0, top: 0 });;
        }
    });
}

function refreshText(obj) {
    var em = $('#PopupText').find('#EditorFrame_Modul');
    if (!em.length) {
        em = $('#PopupText').find('#EditorFrame_Settings');
    }
    if (em.length) {
        var iframe = document.getElementById('EditorFrame_Modul');
        if (!iframe) {
            iframe = document.getElementById('EditorFrame_Settings');
        }
        iframe.src = iframe.src;
        return;
    }
    $.get('/Master/ru/Home/GetText', { CMSPageID: $('#dialog-text-edit').attr('cmspageid'), ViewID: $('#dialog-text-edit').attr('viewid') }, function (data) {
        $('#PopupText').html('<iframe id="EditorFrame_Text" style="height: 600px!important" frameborder="0" src="' + $('#TelerikFrameDomain').val() + '?table=CMSPageTextDatas&targetcolumn=Text&searchcolumn=ID&condition=' + data.id + '" width="100%" height="600px"></iframe>' +
            '<textarea arg="Text" class="text-editor" name="Text" id="Text" style="display: none!important">' + data.value + '</textarea>');

    });

}


function showFullScreen(obj) {
    var fs = $(obj).attr('fs');
    if (fs == '1') {
        $('#dialog-text-edit').dialog("option", "width", 715);
        $('#dialog-text-edit').dialog("option", "height", 700);
        $(obj).attr('fs', '0');
        $('.ui-dialog').css('position', 'absolute').css('top', '100px');

        //tinyMCE.activeEditor.getContent(); //CKEDITOR.instances["PopupText"].resize('100%', 510);

    } else {
        var w = $(window).width();
        $('#dialog-text-edit').dialog("option", "width", w-5);

        var h = $(window).height()+0;

        $('#dialog-text-edit').dialog("option", "height", h);
        $(obj).attr('fs', '1');
        $('.ui-dialog').css('position', 'fixed').css('top', '0px');

        //tinyMCE.activeEditor.getContent(); // CKEDITOR.instances["PopupText"].resize('100%', h - 90);

    }

}

function deleteCatalogSlider(categoryid, type) {
    var res = confirm("Удалить все изображения слайдера для этой категории?");
    if (res) {
        $.get('/Master/ru/Home/DeleteCategorySlider', { CategoryID: categoryid, type:type }, function (data) {

        });
    }
    return false;
}

function showTextEdit(cmspageid, viewid) {
  //  debugger;
    $('#dialog-text-edit').dialog({
        height: 700,
        width: 715,
        modal: true,
    });
    clearEditors();
    $('#dialog-text-edit').attr('cmspageid', cmspageid);
    $('#dialog-text-edit').attr('viewid', viewid);
    $('#dialog-text-edit .dt span').html('Редактирвание текста');
    $('#dialog-text-edit').find('#btn-cancel, #btn-save').show();
    var h = $('#dialog-text-edit').height() - 80;

    //CKEDITOR.on('instanceReady', function () {
    //    CKEDITOR.instances["PopupText"].resize('100%', h);
    //});

/*
    var editor = tinyMCE.activeEditor.getContent(); //CKEDITOR.instances["PopupText"].getData(); // CKEDITOR.instances["PopupText"];
    if (editor) { editor.destroy(true); }
*/


    //CKEDITOR.replace("PopupText", {
    //    filebrowserBrowseUrl: '/Content/ckeditor/ckfinder/ckfinder.html',
    //    filebrowserImageBrowseUrl: '/Content/ckeditor/ckfinder/ckfinder.html?type=Images',
    //    filebrowserFlashBrowseUrl: '/Content/ckeditor/ckfinder/ckfinder.html?type=Flash',
    //    filebrowserUploadUrl: '/Content/ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files',
    //    filebrowserImageUploadUrl: '/Content/ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images',
    //    filebrowserFlashUploadUrl: '/Content/ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash'
    //});

/*
    $.get('/Master/ru/Home/GetText', { CMSPageID: cmspageid, ViewID: viewid }, function(data) {
        tinyMCE.activeEditor.getContent(); //CKEDITOR.instances["PopupText"].setData(data);
    });
*/

    $.get('/Master/ru/Home/GetText', { CMSPageID: cmspageid, ViewID: viewid }, function(data) {
        $('#PopupText').html('<iframe id="EditorFrame_Text" style="height: 600px!important" frameborder="0" src="' + $('#TelerikFrameDomain').val() + '?table=CMSPageTextDatas&targetcolumn=Text&searchcolumn=ID&condition='+data.id+'" width="100%" height="600px"></iframe>' +
            '<textarea arg="Text" class="text-editor" name="Text" id="Text" style="display: none!important">' + data.value + '</textarea>');

    });



    $('.ui-widget-overlay').css('height', $(document).height() + 'px').css('z-index', 1000);
    return false;
}
function showSettingsEdit(blockName, viewid) {
  //  debugger;
    $('#dialog-text-edit').dialog({
        height: 700,
        width: 715,
        modal: true,
    });
    $('#dialog-text-edit .dt span').html('Настройки модуля');
    $('#dialog-text-edit').attr('blockname', blockName);
    $('#dialog-text-edit').attr('viewid', viewid);
    $('#dialog-text-edit').find('#btn-cancel, #btn-save').hide();
    var h = $('#dialog-text-edit').height() - 80;

   
    $('#PopupText').html('<iframe id="EditorFrame_Settings" style="height: 600px!important; width:705px" frameborder="0" src="/Master/ru/Settings/BlockSettings?blockname=' + blockName + '&ViewID=' + viewid+ '" width="705px" height="600px"></iframe>');



    $('.ui-widget-overlay').css('height', $(document).height() + 'px').css('z-index', 1000);
    return false;
}

function showModulEdit(action, controller, cmspageid, viewid, categoryid, productid) {
  //  debugger;
    $('#dialog-text-edit').dialog({
        height: 655,
        width: 715,
        modal: true,
    });
    $('#dialog-text-edit .dt span').html('Редактирование модуля');
    $('#dialog-text-edit').attr('cmspageid', cmspageid);
    $('#dialog-text-edit').attr('viewid', viewid);
    $('#dialog-text-edit').attr('categoryid', categoryid);
    $('#dialog-text-edit').attr('productid', productid);

    $('#dialog-text-edit').find('#btn-cancel, #btn-save').hide();

    var h = $('#dialog-text-edit').height() - 80;


    $('#PopupText').html('<iframe id="EditorFrame_Modul" style="height: 600px!important; width:705px" frameborder="0" src="/Master/ru/' + controller + '/' + action + '?CMSPageID=' + (cmspageid == 0 ? '' : cmspageid) + '&ViewID=' + (viewid == 0 ? '' : viewid) + '&CategoryID=' + (categoryid == 0 ? '' : categoryid) + '&ProductID=' + (productid == 0 ? '' : productid) + '" width="705px" height="600px"></iframe>');
            


/*
    $.get('/Master/ru/Home/GetModulEditor', { CMSPageID: cmspageid, ViewID: viewid, CategoryID: categoryid }, function(data) {
        $('#PopupText').html('<iframe id="EditorFrame_Modul" style="height: 600px!important" frameborder="0" src="' + $('#TelerikFrameDomain').val() + '?table=CMSPageTextDatas&targetcolumn=Text&searchcolumn=ID&condition='+data.id+'" width="100%" height="600px"></iframe>' +
            '<textarea arg="Text" class="text-editor" name="Text" id="Text" style="display: none!important">' + data.value + '</textarea>');

    });
*/



    $('.ui-widget-overlay').css('height', $(document).height() + 'px').css('z-index', 1000);
    return false;
}

function saveText() {
   // debugger;
    $('#dialog-text-edit').attr('cmspageid');
    $.post('/Master/ru/Home/SetText', { CmsPageID: $('#dialog-text-edit').attr('cmspageid'), ViewID: $('#dialog-text-edit').attr('viewid'), text: $('#Text').val() }, function (data) {  //CKEDITOR.instances["PopupText"].getData()
        $('#dialog-text-edit').dialog('close');
        $('.modul[arg="' + $('#dialog-text-edit').attr('viewid') + '"]').find('iframe').attr('src', function (i, val) { return val; });

        setTimeout(function () {
            $('.modul[arg="' + $('#dialog-text-edit').attr('viewid') + '"]').find('iframe').each(function () {
                try {
                    var h = $($(this).get(0).contentWindow.document.body).find('.text-content').height() + 27;
                    if ($($(this).get(0).contentWindow.document.body).find('.text-content').length) {
                        //console.log("frame: " + h);
                        $(this).get(0).height = h;
                    } else {
                        $(this).get(0).height = 27;
                    }
                } catch (e) {

                }
            });
        }, 200);
    });
}

function deleteModul(id) {
    if (confirm('Вы уверены, что хотите удалить этот модуль?')) {
        $.post('/Master/ru/Home/DeleteModul', { id: id, pageID: $.cookie('node_selected_main').substr(2) }, function(d) {
            $('#PageContent').replaceWith(d);
            initDragAndDrop();
        });
    }
}

function loadInfo(data) {
    var link = $(data).find('#PreviewLink');
    if (!link.length || !link.val().length)
        $('#pageView').hide();
    else {
        $('#pageView').attr('href', link.val());
    }
    $('#pageHeader').html($(data).find('#Header').val());
    $('#pageDescription').html($(data).find('#Description').val());

}

var colors = new Object();

function mouseOverColor(model, hex) {
    //$('input[id="' + model + '"]').val(hex);
    $('input[id="' + model + '"]').css('background-color', hex);
    document.body.style.cursor = "pointer";
}


function inputFocus(a, b) {
    if (a.value == b) (a.value = '');
}


function inputBlur(a, b) {
    if (a.value == '') (a.value = b);
}


function hidelLoginCell(id) {
    $(id).hide();
}

function mouseOutMap(model) {

    $('input[id="' + model + '"]').css('background-color', colors[model]);
    document.body.style.cursor = "";
}

function clickColor(model, hex, seltop, selleft) {
    var xhttp, c;
    if (hex == 0) {
        c = colors[model];
    }
    else {
        c = hex;
    }
    if (c.substr(0, 1) == "#") {
        c = c.substr(1);
    }
    var colorhex = "#" + c;
    colorhex = colorhex.substr(0, 10);
    colors[model] = colorhex;
    $('input[id="' + model + '"]').val(hex);
    $('input[id="' + model + '"]').css('background-color', hex);

    $('div#chat-model input').val(hex);
    $('div#chat-model input').css('background-color', hex);

    $('.color-shades[uid="' + model + '"]').html('');

    var digs = new Array();
    var maxlen = 5;
    for (var i = 0; i < maxlen; i++) {
        var p1 = parseInt(colors[model].replace('#', '').substr(0, 2), 16);
        var p2 = parseInt(colors[model].replace('#', '').substr(2, 2), 16);
        var p3 = parseInt(colors[model].replace('#', '').substr(4, 2), 16);

        var sub1 = Math.round(p1 / maxlen);
        var sub2 = Math.round(p2 / maxlen);
        var sub3 = Math.round(p3 / maxlen);

        var o1 = (p1 - sub1 * (maxlen - i - 1));
        var o2 = (p2 - sub2 * (maxlen - i - 1));
        var o3 = (p3 - sub3 * (maxlen - i - 1));
        digs.push('#' + decimalToHex(o1, 2) + decimalToHex(o2, 2) + decimalToHex(o3, 2));

    }
    digs.push(hex);
    for (var i = 0; i < maxlen; i++) {
        var g1 = parseInt(colors[model].replace('#', '').substr(0, 2), 16);
        var g2 = parseInt(colors[model].replace('#', '').substr(2, 2), 16);
        var g3 = parseInt(colors[model].replace('#', '').substr(4, 2), 16);
        var add1 = Math.round((255 - g1) / maxlen);
        var add2 = Math.round((255 - g2) / maxlen);
        var add3 = Math.round((255 - g3) / maxlen);

        var o1 = (g1 + add1 * (i + 1));
        var o2 = (g2 + add2 * (i + 1));
        var o3 = (g3 + add3 * (i + 1));


        digs.push('#' + decimalToHex(o1, 2) + decimalToHex(o2, 2) + decimalToHex(o3, 2));
    }


    for (var j = 0; j < digs.length; j++) {
        var h = '<div class="dig-shade" style="background-color:' + digs[j] + '" onclick="clickColor(\'' + model + '\', \'' + digs[j] + '\')"></div>';

        $('.color-shades[uid="' + model + '"]').append(h);
    }

    //document.getElementById("colorhex").value = colorhex;
    /*
        if (window.XMLHttpRequest) {
            xhttp = new XMLHttpRequest();
        }
        else {
            xhttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
        xhttp.open("GET", "http_colorshades.asp?colorhex=" + c + "&r=" + Math.random(), false);
        xhttp.send("");
        document.getElementById("colorshades").innerHTML = xhttp.responseText;
        if (seltop > -1 && selleft > -1) {
            document.getElementById("selectedColor").style.top = seltop + "px";
            document.getElementById("selectedColor").style.left = selleft + "px";
            document.getElementById("selectedColor").style.visibility = "visible";
        }
        else {
            document.getElementById("divpreview").style.backgroundColor = colorhex;
            document.getElementById("divpreviewtxt").innerHTML = colorhex;
            document.getElementById("selectedColor").style.visibility = "hidden";
        }
    */
    //refreshleader()
}
function decimalToHex(d, padding) {
    if (d > 255)
        d = 255;
    var hex = Number(d).toString(16);
    padding = typeof (padding) === "undefined" || padding === null ? padding = 2 : padding;

    while (hex.length < padding) {
        hex = "0" + hex;
    }

    return hex;
}
function loadUIElems() {
    initAutoFileUpload();
    $('[rel="ht"]').each(function () {
        var $this = $(this);
        var link = $(this).attr('data-link');
        $.get(link, {}, function (data) {
            var html = '<table class="inline-edit">';
            for (var i = 0; i < data.length; i++) {
                html += '<tr>';
                for (var j = 0; j < data[i].length; j++) {
                    html += '<td>';
                    if (i == 0) {
                        html += '<b>' + data[i][j] + '</b>';
                    } else {
                        html += '<input type="text" value="' + data[i][j] + '">';
                    }

                    html += '</td>';

                }
                if (i > 0)
                    html += '<td><a id="del-line" href="#nogo">удалить</a></td>';
                else {
                    html += '<td></td>';
                }
                html += '</tr>';
            }
            html += '<tr class="last-line"><td style="text-align:right" colspan="' + (data[0].length + 1) + '"><a href="#nogo" id="add-line">Добавить характеристику</a></td></tr>';
            $this.html(html);
            loadTableEdit();
            loadTableDelete();
        });
    });

    function loadTableDelete() {
        $('.inline-edit #del-line').unbind('click');
        $('.inline-edit #del-line').click(function () {
            $(this).parent().parent().remove();
            return false;
        });
    }

    function loadTableEdit() {
        $('.inline-edit #add-line').unbind('click');
        $('.inline-edit #add-line').click(function () {
            $(this).parents('table').find('.last-line').before('<tr><td><input type="text" value=""></td><td><input type="text" value=""></td><td><a href="#nogo" id="del-line">удалить</a></td></tr>');
            loadTableDelete();

        });
    }

    $(".controls select").select2({
        dropdownCssClass: 'noSearch'
    });
    $('.controls input[type="checkbox"]').each(function () {
        if (!$(this).parent().parent().hasClass('checker')) {
            $(this).uniform();
        }
    });

    $('.controls .time-picker').timeEntry({ show24Hours: true, showSeconds: true }).change();

    $('input[id^="Color"]').each(function () {
        colors[$(this).attr('id')] = $(this).val();
    });
    /*
        $('div[rel="ColorPicker"]').each(function () {
            $(this).farbtastic('#Color_' + $(this).attr('arg'));
        });
    */
    $('.tagsinput').remove();
    $('input[rel="tags"]').each(function () {
        var t = $(this);
        t.tagsInput({
            'width': '100%',
            'placeholderColor': '#fff',
            onAddTag: function (d) {
                var elems = t.parent().find('.tagsinput .tag span');
                var text = '';
                elems.each(function () {
                    text += $(this).text().trim() + ',';
                });
                if (text.length)
                    text = text.substr(0, text.length - 1);
                t.val(text);
            },
            onRemoveTag: function (d) {
                var elems = t.parent().find('.tagsinput .tag span');
                var text = '';
                elems.each(function () {
                    text += $(this).text().trim() + ',';
                });
                if (text.length)
                    text = text.substr(0, text.length - 1);
                t.val(text);
            },
            onChange: function (d) {
                var elems = t.parent().find('.tagsinput .tag span');
                var text = '';
                elems.each(function () {
                    text += $(this).text().trim() + ',';
                });
                if (text.length)
                    text = text.substr(0, text.length - 1);
                t.val(text);
            }
        });
    })

}

function submitThis(obj) {
    var form = obj.parents('form');
    $.ajax({
        cache: false,
        async: true,
        type: "POST",
        url: form.attr('action'),
        data: form.serialize(),
        success: function (data) {
            var script = form.attr('data-ajax-complete');
            $(form.attr('data-ajax-update')).replaceWith(data);
            setTimeout(script, 100);
        }
    });
    //document.forms[obj.parents('form').attr('name')].submit();
}

function loadUI() {

    setTimeout(function () {
        $('.nav-fixed-topright').removeAttr('style');
    }, 300);

    $(window).scroll(function () {
        if ($('.breadcrumb-container').length) {
            var scrollState = $(window).scrollTop();
            if (scrollState > 0) $('.nav-fixed-topright').addClass('nav-released');
            else $('.nav-fixed-topright').removeClass('nav-released');
        }
    });
    $('.user-sub-menu-container').on('click', function () {
        $(this).toggleClass('active-user-menu');
    });
    $('.user-sub-menu .light').on('click', function () {
        if ($('body').is('.light-version')) return;
        $('body').addClass('light-version');
        setTimeout(function () {
            $.cookie('themeColor', 'light', {
                expires: 365,
                path: '/'
            });
        }, 500);
    });
    $('.user-sub-menu .dark').on('click', function () {
        if ($('body').is('.light-version')) {
            $('body').removeClass('light-version');
            $.cookie('themeColor', 'dark', {
                expires: 365,
                path: '/'
            });
        }
    });

}


function loadEditor(id) {
    //var instance = CKEDITOR.instances[id];
    //if (instance) {
    //    return;
    //    instance.destroy();
    //    CKEDITOR.remove(instance);
        
    //}
    //CKEDITOR.replace(id, {
    //    filebrowserBrowseUrl: '/Content/ckeditor/ckfinder/ckfinder.html',
    //    filebrowserImageBrowseUrl: '/Content/ckeditor/ckfinder/ckfinder.html?type=Images',
    //    filebrowserFlashBrowseUrl: '/Content/ckeditor/ckfinder/ckfinder.html?type=Flash',
    //    filebrowserUploadUrl: '/Content/ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files',
    //    filebrowserImageUploadUrl: '/Content/ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images',
    //    filebrowserFlashUploadUrl: '/Content/ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash'
    //});
}

function initEditors() {
    $('.text-editor').each(function () {
        
        if ($(this).parent().parent().is(':visible')) {
            loadEditor($(this).attr('ID'));
            /*
                CKEDITOR.replace($(this).attr('ID'), {
                    filebrowserBrowseUrl: '/Content/ckeditor/ckfinder/ckfinder.html',
                    filebrowserImageBrowseUrl: '/Content/ckeditor/ckfinder/ckfinder.html?type=Images',
                    filebrowserFlashBrowseUrl: '/Content/ckeditor/ckfinder/ckfinder.html?type=Flash',
                    filebrowserUploadUrl: '/Content/ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files',
                    filebrowserImageUploadUrl: '/Content/ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images',
                    filebrowserFlashUploadUrl: '/Content/ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash'
                });
        */
        }

    });
    loadOrderBoxes();
}

/*
function initUploadify() {
    $('#file_upload').uploadify({
        'formData': {
            'SyndicateID': $('#SyndicateID').val()
        },
        'buttonText': 'Выберите файл',
        'swf': '/Scripts/Uploadify/uploadify.swf',
        'uploader': '/Master/ru/MainPage/SyndicateTicketUpload?SyndicateID=' + $('#SyndicateID').val(),
        'onUploadComplete': function (file) {
            setTimeout(function () {
                $.get('/Master/ru/MainPage/SyndicateTicket', { SyndicateID: $('#SyndicateID').val() }, function (data) {
                    $('#UploadForm').html(data);
                    initUploadify();
                });
            }, 500);
        }
    });

    $('.deleter').click(function () {
        $.get('/Master/ru/MainPage/SyndicateTicketDelete', { SyndicateID: $('#SyndicateID').val() }, function (data) {
            $('#UploadForm').html(data);
            initUploadify();
        });
        return false;
    });
}
*/

function clearEditors() {
    //for (name in CKEDITOR.instances) {
    //    CKEDITOR.instances[name].destroy(true);
    //}
}

function ajaxComplete() {
    clearEditors();
    setRedirect();
    initEditors();
    initCalendars();
    loadStates();
    initTinymce();
}

function initTinymce() {
    return;
    tinyMCE.init({
        selector: '.text-editor',
        height: 500,
        language: "ru",
        theme: 'modern',
        plugins: [
            'advlist autolink lists link image charmap print preview hr anchor pagebreak',
            'searchreplace wordcount visualblocks visualchars code fullscreen',
            'insertdatetime media nonbreaking save table contextmenu directionality',
            'emoticons template paste textcolor colorpicker textpattern imagetools codesample toc'
        ],
        toolbar1: 'undo redo | insert | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
        toolbar2: 'print preview media | forecolor backcolor emoticons | codesample',
        image_advtab: true,
        templates: [
            { title: 'Test template 1', content: 'Test 1' },
            { title: 'Test template 2', content: 'Test 2' }
        ]
    });
}

function initCalendars() {
    $.datepicker.regional['ru'] = {
        closeText: 'Закрыть',
        prevText: '<Пред',
        nextText: 'След>',
        currentText: 'Сегодня',
        monthNames: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь',
        'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
        monthNamesShort: ['Янв', 'Фев', 'Мар', 'Апр', 'Май', 'Июн',
        'Июл', 'Авг', 'Сен', 'Окт', 'Ноя', 'Дек'],
        dayNames: ['воскресенье', 'понедельник', 'вторник', 'среда', 'четверг', 'пятница', 'суббота'],
        dayNamesShort: ['вск', 'пнд', 'втр', 'срд', 'чтв', 'птн', 'сбт'],
        dayNamesMin: ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
        weekHeader: 'Не',
        dateFormat: 'dd.mm.yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    };
    $.datepicker.setDefaults($.datepicker.regional['ru']);


    $.timepicker.regional['ru'] = {
        timeOnlyTitle: 'Выберите время',
        timeText: 'Время',
        hourText: 'Часы',
        minuteText: 'Минуты',
        secondText: 'Секунды',
        millisecText: 'Миллисекунды',
        timezoneText: 'Часовой пояс',
        currentText: 'Текущее',
        closeText: 'Закрыть',
        timeFormat: 'HH:mm',
        amNames: ['AM', 'A'],
        pmNames: ['PM', 'P'],
        isRTL: false
    };
    $.timepicker.setDefaults($.timepicker.regional['ru']);
    $('input[rel="calendar"]').datetimepicker({
        dateFormat: "dd.mm.yy",
        timeFormat: "HH:mm",
        showOn: "button",
        buttonImage: "/Content/photon/images/date_picker1.gif",
        buttonImageOnly: true
    });

    $('input[rel="calendar"], input[type="datetime"]').css('width', '200px').css('margin-right', '10px').attr('tabindex', '-1');

    $('input[type="datetime"]').datepicker({
        dateFormat: "dd.mm.yy",
        showOn: "button",
        buttonImage: "/Content/photon/images/date_picker1.gif",
        buttonImageOnly: true
    });
}

function initFiltersDDL() {
    var cells = $('#AutoFilterTable select, #AutoFilterTable input');

    cells.change(function () {
        var base = $(this).attr('base');
        var params = "";
        if ($(this).val().length) {
            params += $(this).attr('id') + "=" + $(this).val();
        }

        if (params) {
            if (base.indexOf("?") >= 0)
                base = base + "&" + params;
            else base = base + "?" + params;

            document.location.href = base;
        }

    });

    $('.container-select td').click(function () {
        var tbl = $(this).parents('table');
        var base = tbl.attr('base');
        var params = tbl.attr('name') + "=" + $(this).attr('arg');
        if (base.indexOf("?") >= 0)
            base = base + "&" + params;
        else base = base + "?" + params;

        document.location.href = base;
    });
}

function loadLangSwitcher() {
    $('#MasterLang').change(function () {
        $.cookie('MasterLang', $('#MasterLang').val(), { expires: 365, path: '/' });
        document.location.href = '/Master/' + $('#MasterLang').val() + '/' + $('#MasterLangURL').val();
    });
}

function loadOrderBoxes() {
    $('input[box="orderbox"]').change(function () {
        var box = $(this);

        var link = '/Master/ru/' + $(this).attr('target') + "/" + $(this).attr('action');


        var query = $('#Request').val().substr(1);
        if ($(this).attr('oldval') != $(this).val()) {
            $.post(link, { id: $(this).attr('arg'), value: $(this).val(), page: $(this).attr('page'), tablename: $(this).attr('tablename'), uidname: $(this).attr('uidname'), ordername: $(this).attr('ordername'), cc: $(this).attr('cc'), ca: $(this).attr('ca'), addqs: $(this).attr('addqs'), query: query }, function (d) {
                if (d.length) {
                    var table = box.parent().parent().parent().parent();
                    table.find('tr').each(function () {
                        if (!$(this).find('th').length)
                            $(this).remove();
                    });
                    table.append(d);
                    loadOrderBoxes();
                }
            }, "html");
        }
    });

    $('input[box="orderboxComplex"]').change(function () {
        var box = $(this);

        var link = '/Master/ru/' + $(this).attr('target') + "/" + $(this).attr('action');


        var query = $('#Request').val().substr(1);
        if ($(this).attr('oldval') != $(this).val()) {
            $.post(link, { id1: $(this).attr('arg1'), id2: $(this).attr('arg2'), uid1: $(this).attr('uid1'), uid2: $(this).attr('uid2'), value: $(this).val(), page: $(this).attr('page'), tablename: $(this).attr('tablename'), ordername: $(this).attr('ordername'), cc: $(this).attr('cc'), ca: $(this).attr('ca'), addqs: $(this).attr('addqs'), query: query }, function (d) {
                if (d.length) {
                    var table = box.parent().parent().parent().parent();
                    table.find('tr').each(function () {
                        if (!$(this).find('th').length)
                            $(this).remove();
                    });
                    table.append(d);
                    loadOrderBoxes();
                }
            }, "html");
        }
    });
}


function setRedirect() {

    var cell = $('input[type="hidden"]').filter('#RedirectURL');
    var part = $('input[type="hidden"]').filter('#IsPartial');
    var node = $('input[type="hidden"]').filter('#NewNode');
    if (cell.length && part.length) {
        var val = cell.val();
        if (val.length && part.val() == '0') {
            document.location.href = val;
        }
        else if (val.length && part.val() == '1') {
            if (node.length && node.val().length) {
                resetSelectNode(node.val());
            } else {
                loading = true;
                $.get(val, {}, function (data) {
                    $('#PageContent').replaceWith(data);
                    loading = false;
                    loadInfo(data);

                });
            }
        }
    }
}

function disableBoxes() {
    $('input[inactive="1"], select[inactive="1"]').attr('disabled', 'disabled');
}

function loadOdds() {
    $('.odd-grid tr:odd').addClass('odd');
}

function loadSwitcher() {
    $('.switcher').click(function () {
        $('.switcher-content').toggle();
        var currentSwitch = $.cookie('switcher');
        if (currentSwitch == null)
            currentSwitch = '1';

        if (currentSwitch == '0')
            currentSwitch = '1';
        else currentSwitch = '0';
        $.cookie('switcher', currentSwitch);
        return false;
    });

    var currentSwitch = $.cookie('switcher');
    if (currentSwitch == null)
        currentSwitch = '1';

    if (currentSwitch == '0')
        $('.switcher-content').hide();
    else $('.switcher-content').show();

}

function loadEditable() {
    $('.editable').click(function () {
        if ($(this).hasClass('editing')) return false;
        $('.editable').filter('.editing').each(function () {
            $(this).html($(this).attr('val'));
            $(this).removeClass('editing');
        });
        $(this).html('<div class="cell"><input type="text" value="' + $(this).attr('val') + '"></div><div class="btns"><a class="accept" href="/"/><a class="cancel" href="/"></a></div>');
        $(this).addClass('editing');
        $('.editing .cancel').click(function () {
            var cell = $(this).parents('.editing');
            cell.html(cell.attr('val'));
            cell.removeClass('editing');
            return false;
        });
        $('.editing .accept').click(function () {
            var cell = $(this).parents('.editing');
            $.post(saveFieldDataLink, { field: cell.attr('target'), id: cell.attr('targetId'), value: cell.find('input').val() }, function (d) {
                if (d.length) {
                    cell.attr('val', d);
                    cell.html(cell.attr('val'));
                    cell.removeClass('editing');
                }
            });
            return false;
        });
    });
}


var changing = false;
function loadTreeFilter() {
    try {

        if ($('#tree-filter').length) {
            var argList = $('#PageListPlain').val().split(';');
            $.getJSON(filterDataLink, {}, function (res) {


                $('#tree-filter').jstree({
                    "plugins": [
                        "themes", "json_data", "ui", "cookies", "dnd", "search", "types", "checkbox"
                    ],

                    "cookies": {
                        "save_opened": "js_tree_catalog_filter",
                        "cookie_options": { expires: 365 }
                    },

                    "checkbox": {
                        "two_state": false
                    },

                    /*
                                        "themes": {
                                            "theme": "apple",
                                            "url": "/Content/themes/apple/style.css"
                                        },
                    */

                    "json_data": {
                        "data": res,
                        "progressive_render": true
                    }
                }).bind("change_state.jstree", function (e, d) {
                    if (changing) return false;
                    var single = false;
                    try {
                        single = singleSelection;
                    } catch (e) {
                    }
                    var sections = $('#tree-filter').jstree("get_checked", null, true);

                    if (single) {
                        //console.log(d);
                        changing = true;
                        var current = d.rslt[0];
                        $('#tree-filter').jstree('uncheck_all');
                        $('#tree-filter').jstree('check_node', current);
                        changing = false;
                        sections = $('#tree-filter').jstree("get_checked", null, true);
                        //console.log(e);

                    }

                    var sectionPlain = '';
                    sections.each(function () {
                        sectionPlain += $(this).attr('id').replace('x', '') + ";";
                    });
                    $('#PageListPlain').val(sectionPlain);
                }).bind("loaded.jstree", function (event, data) {

                    for (var i = 0; i < argList.length; i++) {
                        $('#tree-filter').jstree("check_node", 'x' + argList[i]);
                    }

                }).bind("open_node.jstree", function (e, data) {
                    for (var i = 0; i < argList.length; i++) {
                        $('#tree-filter').jstree("check_node", '#x' + argList[i]);
                    }
                });
            });
        }
    }
    catch (exc) {

    }
}