
$().ready(function () {
    loadChat();
});

var HEIGHT = 460;

var chatTimeout;
var chatRefresh = 2000;
function loadChat() {
    //return;
    if (document.location.href.indexOf('#openchat') >= 0) {
        setTimeout(function() {

            $('#jivo_chat_widget').hide();
            $('#jivo-chat').css('height', '0px');
            $('#jivo-chat').show();

            $('#jivo-chat').animate({
                height: "+=" + HEIGHT
            }, 500, function() {
                if (chatTimeout) {
                    clearTimeout(chatTimeout);
                }
                chatTimeout = setTimeout(function() {
                    loadMessages(true);
                }, chatRefresh);
                loadMessages(false);

            });
        }, 100);
    }

    $('#jivo_chat_widget').click(function () {
        $(this).hide();
        $('#jivo-chat').css('height', '0px');
        $('#jivo-chat').show();
        

        $('#jivo-chat').animate({
            height: "+=" + HEIGHT
        }, 500, function () {
            if (chatTimeout) {
                clearTimeout(chatTimeout);
            }
            chatTimeout = setTimeout(function () {
                loadMessages(true);
            }, chatRefresh);
            loadMessages(false);

        });

    });

    $('#jivo-chat #messages-div-inner').mCustomScrollbar({
        scrollButtons: {
            enable: true
        },
        height: 200
    });
    $('#jivo-chat #messages-div-inner').mCustomScrollbar("scrollTo", "bottom");
    $('#jivo_close_button, #chat-close').click(function () {
        $.cookie('chat-closed', '1');
        $('#jivo-chat').css('height', HEIGHT + 'px');
        $('#jivo-chat').animate({

            height: "-=" + HEIGHT
        }, 500, function () {
            $('#jivo-chat').hide();
            $('#jivo_chat_widget').show();
            if (chatTimeout) {
                clearTimeout(chatTimeout);
            }
            $('#chat-send-offline-button').prev().find('#ChatUserName').removeClass('err');
            $('#chat-send-offline-button').prev().find('#ChatEmail').removeClass('err');
            $('#chat-send-offline-button').prev().find('#ChatPhone').removeClass('err');
        });
    });

    //$('#chat-body #input-field').keypress(function (e) 
    $('#chat-send-offline-button').click(function (e) {
        //if ($('#chat-body #input-field').val().length) {
            var un = '';
            var um = '';
            var uf = '';
            if ($(this).prev().hasClass('unauth')) {
            un = $('#chat-send-offline-button').prev().find('#ChatUserName').val();
            um = $('#chat-send-offline-button').prev().find('#ChatEmail').val();
            uf = $('#chat-send-offline-button').prev().find('#ChatPhone').val();
                var err = false;
              /*  if (!un.length) {
                    $('#chat-send-offline-button').prev().find('#ChatUserName').addClass('err');
                    err = true;
                } else {
                    $('#chat-send-offline-button').prev().find('#ChatUserName').removeClass('err');
                }
                if (!um.length) {
                    $('#chat-send-offline-button').prev().find('#ChatEmail').addClass('err');
                    err = true;
                } else {
                    $('#chat-send-offline-button').prev().find('#ChatEmail').removeClass('err');
                }
                if (!uf.length) {
                    $('#chat-send-offline-button').prev().find('#ChatPhone').addClass('err');
                    err = true;
                } else {
                    $('#chat-send-offline-button').prev().find('#ChatPhone').removeClass('err');
                }*/
                if (err)
                    return false;
            }
            loadMessages(false, $('#chat-body #input-field').val(), un, um, uf);
       // }
    });
}

function s4() {
    return Math.floor((1 + Math.random()) * 0x10000)
               .toString(16)
               .substring(1);
}
function guid() {

    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
        s4() + '-' + s4() + s4() + s4();
}

function loadMessages(cicle, msg, un, um, uf) {
    var getLocation = function (href) {
        var l = document.createElement("a");
        l.href = href;
        return l;
    };
    var getSession = function () {
        var s = $.cookie('chat');
        if (s)
            return s;
        else {
            s = guid();
            $.cookie('chat', s, { expires: 1 });
            return s;
        }
    };
    $.post('/Master/ru/Chat/PostMessage', { host: getLocation(document.location.href).hostname, session: getSession(), message: msg, un: un, um: um, uf: uf }, function (d) {
        if (um && un && uf && um.length && un.length && uf.length && document.location.href.indexOf('#openchat')<0) {
            document.location.href = document.location.href + '#openchat';
            document.location.reload();
            return;
        }

        
        if (/*$('#input-div').hasClass('unauth') || */d.length > 0) {
            $('#input-div').removeClass('unauth');
            //$('#messages-div').css('height', '230px');
            $('#input-div *').not('textarea').hide();
            $('#input-div').css('height', '65px');
            $('#input-div textarea').css('height', '52px');
            //return;

        }
        else if ($('#input-div').hasClass('unauth')) {
            $('#input-div textarea').css('height', '150px');
        }

        for (var i = 0; i < d.length; i++) {
            var tmpl = '<div class="new-msg-container clientMessage" arg="{id}"><div class="new-msg-body clientMessage"><div class="new-msg-body-inner"><div class="new-msg-text">{msg}</div></div></div><div class="new-time">{time}</div></div>';
            if (!$('.new-msg-container[arg="' + d[i].ID + '"]').length) {
                var line = tmpl.replace('{id}', d[i].ID).replace('{msg}', d[i].Message).replace('{time}', d[i].Time);
                $('#messages-div-inner-clear').before(line);
                $('#jivo-chat #messages-div-inner').each(function () {
                    $(this).mCustomScrollbar('update');
                    $(this).mCustomScrollbar("scrollTo", "bottom");
                });

            } else {
                $('.new-msg-container[arg="' + d[i].ID + '"] .new-msg-text').html(d[i].Message);
            }
        }


        if (msg)
            $('#chat-body #input-field').val('');
    });
    if (cicle) {
        chatTimeout = setTimeout(function () {
            loadMessages(cicle);
        }, chatRefresh);
    }
};
