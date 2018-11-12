
$().ready(function () {
    loadChat();
});

var chatTimeout;
var chatRefresh = 2000;
function loadChat() {
    if (document.location.href.indexOf('#openchat') >= 0) {
        setTimeout(function() {

            $('#jivo_chat_widget').hide();
            $('#jivo-chat').show();

            $('#jivo-chat').animate({
                height: "+=" + 397
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
        $('#jivo-chat').show();

        $('#jivo-chat').animate({
            height: "+=" + 397
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
    $('#jivo_close_button').click(function () {
        $('#jivo-chat').animate({
            height: "-=" + 397
        }, 500, function () {
            $('#jivo-chat').hide();
            $('#jivo_chat_widget').show();
            if (chatTimeout) {
                clearTimeout(chatTimeout);
            }
        });



    });




    $('#chat-body #input-field').keypress(function (e) {
        if (e.which == 13 && $('#chat-body #input-field').val().length) {
            var un = '';
            var um = '';
            var uf = '';
            
            if ($(this).parent().hasClass('unauth')) {
                un = $(this).parent().find('#ChatUserName').val();
                um = $(this).parent().find('#ChatEmail').val();
                uf = $(this).parent().find('#ChatPhone').val();
                var err = false;
                if (!un.length) {
                    $(this).parent().find('#ChatUserName').addClass('err');
                    err = true;
                } else {
                    $(this).parent().find('#ChatUserName').removeClass('err');
                }
                if (!um.length) {
                    $(this).parent().find('#ChatEmail').addClass('err');
                    err = true;
                } else {
                    $(this).parent().find('#ChatEmail').removeClass('err');
                }
                if (!uf.length) {
                    $(this).parent().find('#ChatPhone').addClass('err');
                    err = true;
                } else {
                    $(this).parent().find('#ChatPhone').removeClass('err');
                }
                if (err)
                    return false;
            }
            loadMessages(false, $('#chat-body #input-field').val(), un, um, uf);
        }
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
        if (um && un && uf && um.length && un.length && uf.length) {
            document.location.href = document.location.href + '#openchat';
            document.location.reload();
            return;
        }

        if ($('#input-div').hasClass('unauth')) {
            return;
        }
        for (var i = 0; i < d.length; i++) {
            var tmpl = '<div class="new-msg-container clientMessage" arg="{id}"><div class="pip"></div><div class="new-msg-body clientMessage"><div class="new-msg-body-inner"><div class="new-msg-text">{msg}</div></div></div><div class="new-time">{time}</div></div>';
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
